using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Xamarin.Forms;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using System.Collections;
using System.Collections.Specialized;

namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionSource : UICollectionViewDataSource
    {
        //private Random rnd = new Random();
        //public List<nfloat> Heights { get; set; } = new List<nfloat>();
        internal event NotifyCollectionChangedEventHandler CollectionViewUpdating;
        internal event NotifyCollectionChangedEventHandler CollectionViewUpdated;

        public DataTemplate ItemTemplate { get; set; }

        readonly IEnumerable _itemsSource;
        private nint _section = 0;

        #region Computed Properties
        public UICollectionView CollectionView { get; set; }
        public List<object> ItemSource { get; set; } = new List<object>();
        #endregion

        #region Constructors
        public WaterfallCollectionSource(UICollectionView collectionView, IEnumerable itemsource)
        {
            // Initialize
            CollectionView = collectionView;
            _itemsSource = itemsource;
            // Init numbers collection
            foreach (var item in itemsource)
            {
                ItemSource.Add(item);
            }
            try
            {
                ((INotifyCollectionChanged)itemsource).CollectionChanged += CollectionChanged;

            }
            catch (Exception ex)
            {

            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (Device.IsInvokeRequired)
            {
                Device.BeginInvokeOnMainThread(() => CollectionChanged(args));
            }
            else
            {
                CollectionChanged(args);
            }
        }

        void CollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            // Force UICollectionView to get the internal accounting straight
            if (!CollectionView.Hidden)
                CollectionView.NumberOfItemsInSection(_section);

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Add(args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove(args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Replace(args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    Move(args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reload();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        internal int IndexOf(object item)
        {
            if (_itemsSource is IList list)
                return list.IndexOf(item);

            int count = 0;
            foreach (var i in _itemsSource)
            {
                if (i == item)
                    return count;
                count++;
            }

            return -1;
        }

        protected virtual NSIndexPath[] CreateIndexesFrom(int startIndex, int count)
        {
            return IndexPathHelpers.GenerateIndexPathRange((int)_section, startIndex, count);
        }
        void Update(Action update, NotifyCollectionChangedEventArgs args)
        {
            if (CollectionView.Hidden)
            {
                return;
            }

            OnCollectionViewUpdating(args);
            update();
            OnCollectionViewUpdated(args);
        }

        void OnCollectionViewUpdating(NotifyCollectionChangedEventArgs args)
        {
            CollectionViewUpdating?.Invoke(this, args);
        }

        void OnCollectionViewUpdated(NotifyCollectionChangedEventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CollectionViewUpdated?.Invoke(this, args);
            });
        }


        void Add(NotifyCollectionChangedEventArgs args)
        {
            var count = args.NewItems.Count;
            //Count += count;
            var startIndex = args.NewStartingIndex > -1 ? args.NewStartingIndex : IndexOf(args.NewItems[0]);
            foreach (object item in args.NewItems)
            {
                ItemSource.Add(item);
            }

            // Queue up the updates to the UICollectionView
            try
            {
                var inpath = CreateIndexesFrom(startIndex, count);
                Update(() => CollectionView.InsertItems(inpath), args);

            }
            catch (Exception ex)
            {

            }
        }

        void Remove(NotifyCollectionChangedEventArgs args)
        {
            var startIndex = args.OldStartingIndex;

            foreach (object item in args.OldItems)
            {
                ItemSource.Remove(item);
            }

            if (startIndex < 0)
            {
                // INCC implementation isn't giving us enough information to know where the removed items were in the
                // collection. So the best we can do is a ReloadData()
                Reload();
                return;
            }

            // If we have a start index, we can be more clever about removing the item(s) (and get the nifty animations)
            var count = args.OldItems.Count;
            //Count -= count;

            Update(() => CollectionView.DeleteItems(CreateIndexesFrom(startIndex, count)), args);
        }

        void Replace(NotifyCollectionChangedEventArgs args)
        {
            var newCount = args.NewItems.Count;

            if (newCount == args.OldItems.Count)
            {
                var startIndex = args.NewStartingIndex > -1 ? args.NewStartingIndex : IndexOf(args.NewItems[0]);

                // We are replacing one set of items with a set of equal size; we can do a simple item range update

                Update(() => CollectionView.ReloadItems(CreateIndexesFrom(startIndex, newCount)), args);
                return;
            }

            // The original and replacement sets are of unequal size; this means that everything currently in view will 
            // have to be updated. So we just have to use ReloadData and let the UICollectionView update everything
            Reload();
        }

        void Move(NotifyCollectionChangedEventArgs args)
        {
            var count = args.NewItems.Count;

            if (count == 1)
            {
                // For a single item, we can use MoveItem and get the animation
                var oldPath = NSIndexPath.Create(_section, args.OldStartingIndex);
                var newPath = NSIndexPath.Create(_section, args.NewStartingIndex);

                Update(() => CollectionView.MoveItem(oldPath, newPath), args);
                return;
            }

            var start = Math.Min(args.OldStartingIndex, args.NewStartingIndex);
            var end = Math.Max(args.OldStartingIndex, args.NewStartingIndex) + count;

            Update(() => CollectionView.ReloadItems(CreateIndexesFrom(start, end)), args);
        }

        void Reload()
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

           // Count = ItemsCount();

            OnCollectionViewUpdating(args);

            CollectionView.ReloadData();
            CollectionView.CollectionViewLayout.InvalidateLayout();

            OnCollectionViewUpdated(args);
        }

        #endregion





        #region Override Methods
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            // We only have one section
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            // Return the number of items
            return ItemSource.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            var cell = collectionView.DequeueReusableCell(TextCollectionViewCell.CELL_ID, indexPath) as TextCollectionViewCell;
            if (cell.RendererView == null)
            {
                View formsView;
                //if (isHeader)
                //    formsView = (View)HeaderTemplate.CreateContent();
                //else
                formsView = (View)ItemTemplate.CreateContent();
                var nativeView = formsView.ToUIView(new CGRect(0, 0, cell.Frame.Width, cell.Frame.Height));
                cell.RendererView = nativeView;
            }

            var subview = cell.ContentView.Subviews[0];
            var frame = subview.Frame;
            //force height to the cell height in case the reusable cell subview have the old height
            subview.Frame = new CGRect(frame.X, frame.Y, cell.Frame.Width, cell.Frame.Height);

            var element = (cell.RendererView as VisualElementRenderer<VisualElement>).Element;
            element.BindingContext = ItemSource[(int)indexPath.Item];
            element.Layout(new Xamarin.Forms.Rectangle(0, 0, cell.Frame.Width, cell.Frame.Height));



            return cell;
        }











        //public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // We can always move items
        //    return false;
        //}

        //public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        //{
        //    // Reorder our list of items
        //    var item = Numbers[(int)sourceIndexPath.Item];
        //    Numbers.RemoveAt((int)sourceIndexPath.Item);
        //    Numbers.Insert((int)destinationIndexPath.Item, item);
        //}
        #endregion
    }
}
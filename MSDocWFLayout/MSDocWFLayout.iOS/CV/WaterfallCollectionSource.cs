using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Xamarin.Forms;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;

namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionSource : UICollectionViewDataSource
    {
        private Random rnd = new Random();
        public List<nfloat> Heights { get; set; } = new List<nfloat>();


        public DataTemplate ItemTemplate { get; set; }



        #region Computed Properties
        public UICollectionView CollectionView { get; set; }
        public List<int> Numbers { get; set; } = new List<int>();
        #endregion

        #region Constructors
        public WaterfallCollectionSource(UICollectionView collectionView)
        {
            // Initialize
            CollectionView = collectionView;

            // Init numbers collection
            for (int n = 0; n < 1000; ++n)
            {
                Numbers.Add(n);
                Heights.Add(rnd.Next(100, 300) + 0.0f);
            }
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
            return Numbers.Count;
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
            element.BindingContext = Numbers[indexPath.Row];
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
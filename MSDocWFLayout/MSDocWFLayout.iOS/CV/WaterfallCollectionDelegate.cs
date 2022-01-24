using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using CoreGraphics;

namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionDelegate : UICollectionViewDelegate
    {
        #region Computed Properties
        public UICollectionView CollectionView { get; set; }
        public WaterfallCollectionSource ItemsSource { get; }
        protected float PreviousHorizontalOffset, PreviousVerticalOffset;
        #endregion

        ItemsView ItemsView;

        #region Constructors
        public WaterfallCollectionDelegate(UICollectionView collectionView, ItemsView itemsView, WaterfallCollectionSource source)
        {

            // Initialize
            CollectionView = collectionView;
            ItemsView = itemsView;
            ItemsSource = source;
        }



        public override void Scrolled(UIScrollView scrollView)
        {
            var (visibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex) = GetVisibleItemsIndex();

            if (!visibleItems)
                return;

            var contentInset = scrollView.ContentInset;
            var contentOffsetX = scrollView.ContentOffset.X + contentInset.Left;
            var contentOffsetY = scrollView.ContentOffset.Y + contentInset.Top;

            var itemsViewScrolledEventArgs = new ItemsViewScrolledEventArgs
            {
                HorizontalDelta = contentOffsetX - PreviousHorizontalOffset,
                VerticalDelta = contentOffsetY - PreviousVerticalOffset,
                HorizontalOffset = contentOffsetX,
                VerticalOffset = contentOffsetY,
                FirstVisibleItemIndex = firstVisibleItemIndex,
                CenterItemIndex = centerItemIndex,
                LastVisibleItemIndex = lastVisibleItemIndex
            };

            var itemsView = ItemsView;
            var source = ItemsSource;
            itemsView.SendScrolled(itemsViewScrolledEventArgs);

            PreviousHorizontalOffset = (float)contentOffsetX;
            PreviousVerticalOffset = (float)contentOffsetY;

            switch (itemsView.RemainingItemsThreshold)
            {
                case -1:
                    return;
                case 0:
                    if (lastVisibleItemIndex == ItemsSource.ItemSource.Count - 1)
                        itemsView.SendRemainingItemsThresholdReached();
                    break;
                default:
                    if (ItemsSource.ItemSource.Count - 1 - lastVisibleItemIndex <= itemsView.RemainingItemsThreshold)
                        itemsView.SendRemainingItemsThresholdReached();
                    break;
            }
        }

        protected virtual (bool VisibleItems, NSIndexPath First, NSIndexPath Center, NSIndexPath Last) GetVisibleItemsIndexPath()
        {
            var indexPathsForVisibleItems = CollectionView.IndexPathsForVisibleItems.OrderBy(x => x.Row).ToList();

            var visibleItems = indexPathsForVisibleItems.Count > 0;

            NSIndexPath firstVisibleItemIndex = null, centerItemIndex = null, lastVisibleItemIndex = null;

            if (visibleItems)
            {
                NSIndexPath firstVisibleItem = indexPathsForVisibleItems.First();

                UICollectionView collectionView = CollectionView;
                NSIndexPath centerIndexPath = GetCenteredIndexPath(collectionView);
                NSIndexPath centerItem = centerIndexPath ?? firstVisibleItem;

                NSIndexPath lastVisibleItem = indexPathsForVisibleItems.Last();

                firstVisibleItemIndex = indexPathsForVisibleItems.First();
                centerItemIndex = centerItem;
                lastVisibleItemIndex = lastVisibleItem;
            }

            return (visibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex);
        }

        static NSIndexPath GetCenteredIndexPath(UICollectionView collectionView)
        {
            NSIndexPath centerItemIndex = null;

            var indexPathsForVisibleItems = collectionView.IndexPathsForVisibleItems.OrderBy(x => x.Row).ToList();

            if (indexPathsForVisibleItems.Count == 0)
                return centerItemIndex;

            var firstVisibleItemIndex = indexPathsForVisibleItems.First();

            var centerPoint = new CGPoint(collectionView.Center.X + collectionView.ContentOffset.X, collectionView.Center.Y + collectionView.ContentOffset.Y);
            var centerIndexPath = collectionView.IndexPathForItemAtPoint(centerPoint);
            centerItemIndex = centerIndexPath ?? firstVisibleItemIndex;

            return centerItemIndex;
        }
        protected virtual (bool VisibleItems, int First, int Center, int Last) GetVisibleItemsIndex()
        {
            var (VisibleItems, First, Center, Last) = GetVisibleItemsIndexPath();
            int firstVisibleItemIndex = -1, centerItemIndex = -1, lastVisibleItemIndex = -1;

            if (VisibleItems)
            {
                WaterfallCollectionSource source = ItemsSource;

                firstVisibleItemIndex = GetItemIndex(First, source);
                centerItemIndex = GetItemIndex(Center, source);
                lastVisibleItemIndex = GetItemIndex(Last, source);
            }

            return (VisibleItems, firstVisibleItemIndex, centerItemIndex, lastVisibleItemIndex);
        }

        static int GetItemIndex(NSIndexPath indexPath, WaterfallCollectionSource itemSource)
        {
            int index = (int)indexPath.Item;

            if (indexPath.Section > 0)
            {
                for (int i = 0; i < indexPath.Section; i++)
                {
                    index += itemSource.ItemSource.Count;
                }
            }

            return index;
        }
        //public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    base.ItemSelected(collectionView, indexPath);
        //}


        #endregion

        #region Overrides Methods
        //public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Always allow for highlighting
        //    return false;
        //}

        //public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Get cell and change to green background
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = UIColor.FromRGB(183, 208, 57);
        //}

        //public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Get cell and return to blue background
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = UIColor.FromRGB(164, 205, 255);
        //}
        #endregion
    }
}
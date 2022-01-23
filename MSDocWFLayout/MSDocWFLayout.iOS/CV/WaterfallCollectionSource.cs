using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionSource : UICollectionViewDataSource
    {
        private Random rnd = new Random();
        public List<nfloat> Heights { get; set; } = new List<nfloat>();





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
            for (int n = 0; n < 100; ++n)
            {
                Numbers.Add(n);
                Heights.Add(rnd.Next(100, 200) + 40.0f);
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
            cell.Title = Numbers[(int)indexPath.Item].ToString();

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return false;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            // Reorder our list of items
            var item = Numbers[(int)sourceIndexPath.Item];
            Numbers.RemoveAt((int)sourceIndexPath.Item);
            Numbers.Insert((int)destinationIndexPath.Item, item);
        }
        #endregion
    }
}
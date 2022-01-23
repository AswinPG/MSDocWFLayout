﻿using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionDelegate : UICollectionViewDelegate
    {
        #region Computed Properties
        public UICollectionView CollectionView { get; set; }
        #endregion

        #region Constructors
        public WaterfallCollectionDelegate(UICollectionView collectionView)
        {

            // Initialize
            CollectionView = collectionView;

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
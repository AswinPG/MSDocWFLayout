using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using Xamarin.Forms;

namespace MSDocWFLayout.iOS.CV
{
    [Register("WaterfallCollectionView")]
    public class WaterfallCollectionView : UICollectionView
    {
        #region Constructors
        public WaterfallCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        //public override void AwakeFromNib()
        //{
        //    base.AwakeFromNib();

            

        //    // Initialize
        //    DataSource = new WaterfallCollectionSource(this);
        //    Delegate = new WaterfallCollectionDelegate(this);

        //}
        #endregion
        public WaterfallCollectionSource Source
        {
            get { return (WaterfallCollectionSource)DataSource; }
        }
    }
}
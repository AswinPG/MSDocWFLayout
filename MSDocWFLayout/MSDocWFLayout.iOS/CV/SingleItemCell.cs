using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;

namespace MSDocWFLayout.iOS.CV
{
    public class SingleItemCell : UICollectionViewCell
    {
        public const string CELL_ID = "CELL_ID";

        [Export("initWithFrame:")]
        public SingleItemCell(RectangleF frame) : base(frame)
        {

        }

        UIView _rendererView;
        public UIView RendererView
        {
            get { return _rendererView; }
            set
            {
                _rendererView = value;
                if (ContentView.Subviews.Length > 0)
                {
                    ContentView.Subviews[0].RemoveFromSuperview();
                }

                var subview = new UIView(new CGRect(0, 0, Frame.Width, Frame.Height));
                _rendererView.Frame = subview.Frame;
                subview.Add(_rendererView);

                ContentView.AddSubview(subview);
            }
        }
    }
}
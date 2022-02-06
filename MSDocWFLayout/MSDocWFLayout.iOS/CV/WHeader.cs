using CoreGraphics;
using Foundation;
using System.Drawing;
using UIKit;

namespace MSDocWFLayout.iOS.CV
{
    internal class WHeader : UICollectionReusableView
    {
        public const string CELL_ID = "HEADER_ID";

        [Export("initWithFrame:")]
        public WHeader(RectangleF frame) : base(frame)
        {

        }

        UIView _rendererView;
        public UIView RendererView
        {
            get { return _rendererView; }
            set
            {
                _rendererView = value;
                if (Subviews.Length > 0)
                {
                    Subviews[0].RemoveFromSuperview();
                }

                var subview = new UIView(new CGRect(0, 0, Frame.Width, Frame.Height));
                _rendererView.Frame = subview.Frame;
                subview.Add(_rendererView);

                AddSubview(subview);
            }
        }
    }


}
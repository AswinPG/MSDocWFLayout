using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MSDocWFLayout.iOS.CV
{
    public partial class TextCollectionViewCell : UICollectionViewCell
    {
        //public string Title
        //{
        //    get { return TextLabel.Text; }
        //    set { TextLabel.Text = value; }
        //}
        //#endregion

        //public UILabel TextLabel
        //{
        //    get; set;
        //}
        public const string CELL_ID = "CELL_ID";

        [Export("initWithFrame:")]
        public TextCollectionViewCell(RectangleF handle) : base(handle)
        {
            //TextLabel = new UILabel();
            //BackgroundColor = UIColor.White;
            //var subview = new UIView(new CGRect(0, 0, Frame.Width, Frame.Height));
            //TextLabel.Frame = subview.Frame;
            //subview.Add(TextLabel);

            //ContentView.AddSubview(subview);
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
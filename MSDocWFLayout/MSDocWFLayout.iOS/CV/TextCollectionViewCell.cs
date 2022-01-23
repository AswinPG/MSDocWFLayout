using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MSDocWFLayout.iOS.CV
{
    public partial class TextCollectionViewCell : UICollectionViewCell
    {
        #region Computed Properties
        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }
        #endregion

        public UILabel TextLabel
        {
            get; set;
        }
        public const string CELL_ID = "CELL_ID";

        #region Constructors
        public TextCollectionViewCell(IntPtr handle) : base(handle)
        {
            TextLabel = new UILabel();
            BackgroundColor = UIColor.White;
            var subview = new UIView(new CGRect(0, 0, Frame.Width, Frame.Height));
            TextLabel.Frame = subview.Frame;
            subview.Add(TextLabel);

            ContentView.AddSubview(subview);
        }
        #endregion
    }
}
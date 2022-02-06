using CoreGraphics;
using MSDocWFLayout;
using MSDocWFLayout.iOS.CV;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WFCollectionView), typeof(WaterfallCollectionViewRenderer))]
namespace MSDocWFLayout.iOS.CV
{
    public class WaterfallCollectionViewRenderer : ViewRenderer<WFCollectionView, UICollectionView>
    {

        public WaterfallCollectionViewRenderer()
        {

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Element.Width) && Control == null)
            {
                base.OnElementPropertyChanged(sender, e);
                //_events = new EventTracker(this);
                CreateView();
            }
        }
        public WaterfallCollectionSource Source
        {
            get { return (WaterfallCollectionSource)collectionView.DataSource; }
        }
        UICollectionView collectionView;
        //private EventTracker _events;

        void CreateView()
        {

            var _layout = new WaterfallCollectionLayout();
            _layout.Header = Element.Header;
            //_layout.Header.PropertyChanged += _layout.Header_PropertyChanged;
            //_layout.ItemsSource = SourceList;

            // _layout.GetHeightForCellDelegate = Element.GetHeightForCellDelegate;
            _layout.ColumnCount = 3;
            //_layout.MinCellHeight = Element.MinCellHeight;
            //_layout.MaxCellHeight = Element.MaxCellHeight;
            // _layout.Header = Element.Header;

            var frame = new CGRect(0, 0, Element.Width, Element.Height);
            collectionView = new UICollectionView(frame, _layout);
            _layout.SizeForItem = Element.GetHeightForCellDelegate;
            //var WSource = new WCollectionViewSource(Element, collectionView);
            //WSource.ItemTemplate = Element.ItemTemplate;
            ////source.ItemsSource = SourceList;
            //WSource.Header = Element.Header;
            //var source = new ObservableItemsSource(Element.ItemsSource, collectionView, WSource, this);
            var wfsouce = new WaterfallCollectionSource(collectionView, Element.ItemsSource,Element.Header);
            wfsouce.ItemTemplate = Element.ItemTemplate;
            collectionView.DataSource = wfsouce;
            collectionView.Delegate = new WaterfallCollectionDelegate(collectionView,Element,wfsouce);

            collectionView.RegisterClassForCell(typeof(TextCollectionViewCell), TextCollectionViewCell.CELL_ID);
            if (Element.Header != null)
                collectionView.RegisterClassForSupplementaryView(typeof(WHeader), UICollectionElementKindSection.Header, WHeader.CELL_ID);
            //collectionView.RegisterClassForCell(typeof(WHeaderCell), WHeaderCell.CELL_ID);

            //collectionView.Source = source;
            collectionView.BackgroundColor = Element.BackgroundColor.ToUIColor();
            collectionView.AlwaysBounceVertical = true;

            //if (Element.Header != null)
             //   collectionView.RegisterClassForSupplementaryView(typeof(CVHeader), UICollectionElementKindSection.Header, CVHeader.CELL_ID);

            SetNativeControl(collectionView);
        }



        //public override void AwakeFromNib()
        //{
        //    base.AwakeFromNib();
        //    var waterfallLayout = new WaterfallCollectionLayout();

        //    waterfallLayout.SizeForItem += (collectionView, layout, indexPath) => {
        //        var collection = collectionView as WaterfallCollectionView;
        //        return new CGSize(180, collection.Source.Heights[(int)indexPath.Item]);
        //    };
        //}

    }
    

}
using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MSDocWFLayout.iOS.CV
{
    [Register("WaterfallCollectionLayout")]
    public class WaterfallCollectionLayout : UICollectionViewLayout
    {
        #region Private Variables
        private int columnCount = 3;
        private nfloat minimumColumnSpacing = 10;
        private nfloat minimumInterItemSpacing = 10;
        //private nfloat headerHeight = 0.0f;

        public View Header;


        private nfloat footerHeight = 0.0f;
        private UIEdgeInsets sectionInset = new UIEdgeInsets(10, 10, 10, 10);
        private WaterfallCollectionRenderDirection itemRenderDirection = WaterfallCollectionRenderDirection.ShortestFirst;
        private Dictionary<nint, UICollectionViewLayoutAttributes> headersAttributes = new Dictionary<nint, UICollectionViewLayoutAttributes>();
        private Dictionary<nint, UICollectionViewLayoutAttributes> footersAttributes = new Dictionary<nint, UICollectionViewLayoutAttributes>();



        private List<CGRect> unionRects = new List<CGRect>();
        private List<nfloat> columnHeights = new List<nfloat>();
        private List<UICollectionViewLayoutAttributes> allItemAttributes = new List<UICollectionViewLayoutAttributes>();
        private List<List<UICollectionViewLayoutAttributes>> sectionItemAttributes = new List<List<UICollectionViewLayoutAttributes>>();
        private nfloat unionSize = 20;
        #endregion

        #region Computed Properties
        [Export("ColumnCount")]
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                WillChangeValue("ColumnCount");
                columnCount = value;
                DidChangeValue("ColumnCount");

                InvalidateLayout();
            }
        }

        [Export("MinimumColumnSpacing")]
        public nfloat MinimumColumnSpacing
        {
            get { return minimumColumnSpacing; }
            set
            {
                WillChangeValue("MinimumColumnSpacing");
                minimumColumnSpacing = value;
                DidChangeValue("MinimumColumnSpacing");

                InvalidateLayout();
            }
        }

        [Export("MinimumInterItemSpacing")]
        public nfloat MinimumInterItemSpacing
        {
            get { return minimumInterItemSpacing; }
            set
            {
                WillChangeValue("MinimumInterItemSpacing");
                minimumInterItemSpacing = value;
                DidChangeValue("MinimumInterItemSpacing");

                InvalidateLayout();
            }
        }

        //[Export("HeaderHeight")]
        //public nfloat HeaderHeight
        //{
        //    get { return headerHeight; }
        //    set
        //    {
        //        WillChangeValue("HeaderHeight");
        //        headerHeight = value;
        //        DidChangeValue("HeaderHeight");

        //        InvalidateLayout();
        //    }
        //}

        [Export("FooterHeight")]
        public nfloat FooterHeight
        {
            get { return footerHeight; }
            set
            {
                WillChangeValue("FooterHeight");
                footerHeight = value;
                DidChangeValue("FooterHeight");

                InvalidateLayout();
            }
        }

        [Export("SectionInset")]
        public UIEdgeInsets SectionInset
        {
            get { return sectionInset; }
            set
            {
                WillChangeValue("SectionInset");
                sectionInset = value;
                DidChangeValue("SectionInset");

                InvalidateLayout();
            }
        }

        [Export("ItemRenderDirection")]
        public WaterfallCollectionRenderDirection ItemRenderDirection
        {
            get { return itemRenderDirection; }
            set
            {
                WillChangeValue("ItemRenderDirection");
                itemRenderDirection = value;
                DidChangeValue("ItemRenderDirection");

                InvalidateLayout();
            }
        }
        #endregion

        #region Constructors
        public WaterfallCollectionLayout()
        {
            //Header.PropertyChanged += Header_PropertyChanged;
        }

        //public void Header_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if(e.PropertyName == nameof(Header.Height))
        //    {
        //        InvalidateLayout();
        //    }
            
        //}

        //public WaterfallCollectionLayout(NSCoder coder) : base(coder)
        //{

        //}
        #endregion

        #region Public Methods
        public nfloat ItemWidthInSectionAtIndex(int section)
        {
            var width = CollectionView.Bounds.Width - SectionInset.Left - SectionInset.Right;
            return (nfloat)Math.Floor((width - ((ColumnCount - 1) * MinimumColumnSpacing)) / ColumnCount);
        }
        #endregion

        #region Override Methods
        public override void PrepareLayout()
        {
            base.PrepareLayout();

            // Get the number of sections
            var numberofSections = CollectionView.NumberOfSections();
            if (numberofSections == 0)
                return;

            // Reset collections
            headersAttributes.Clear();
            footersAttributes.Clear();
            unionRects.Clear();
            columnHeights.Clear();
            allItemAttributes.Clear();
            sectionItemAttributes.Clear();

            // Initialize column heights
            for (int n = 0; n < ColumnCount; n++)
            {
                columnHeights.Add((nfloat)0);
            }

            // Process all sections
            nfloat top = 0.0f;
            var attributes = new UICollectionViewLayoutAttributes();
            var columnIndex = 0;
            for (nint section = 0; section < numberofSections; ++section)
            {
                // Calculate section specific metrics
                var minimumInterItemSpacing = (MinimumInterItemSpacingForSection == null) ? MinimumColumnSpacing :
                  MinimumInterItemSpacingForSection(CollectionView, this, section);

                // Calculate widths
                var width = CollectionView.Bounds.Width - SectionInset.Left - SectionInset.Right;
                var itemWidth = (nfloat)Math.Floor((width - ((ColumnCount - 1) * MinimumColumnSpacing)) / ColumnCount);

                // Calculate section header
                //var heightHeader = (HeightForHeader == null) ? HeaderHeight :
                //  HeightForHeader(CollectionView, this, section);

                if (Header != null)
                {
                    attributes = UICollectionViewLayoutAttributes.CreateForSupplementaryView(UICollectionElementKindSection.Header, NSIndexPath.FromRowSection(0, section));
                    var frame = new CGRect(0, 0, CollectionView.Frame.Width, Header.Height);
                    attributes.Frame = frame;
                    headersAttributes.Add(section, attributes);
                    allItemAttributes.Add(attributes);

                    top = attributes.Frame.GetMaxY();
                }

                top += SectionInset.Top;
                for (int n = 0; n < ColumnCount; n++)
                {
                    columnHeights[n] = top;
                }

                // Calculate Section Items
                var itemCount = CollectionView.NumberOfItemsInSection(section);
                List<UICollectionViewLayoutAttributes> itemAttributes = new List<UICollectionViewLayoutAttributes>();

                for (nint n = 0; n < itemCount; n++)
                {
                    var indexPath = NSIndexPath.FromRowSection(n, section);
                    string c = "hj";
                    columnIndex = NextColumnIndexForItem(n);
                    var xOffset = SectionInset.Left + (itemWidth + MinimumColumnSpacing) * (nfloat)columnIndex;
                    var yOffset = columnHeights[columnIndex];
                    var itemSize = (SizeForItem == null) ? new CGSize(0, 0) : CalculateFromSize(SizeForItem((int)indexPath.Item));
                    nfloat itemHeight = 0.0f;

                    if (itemSize.Height > 0.0f && itemSize.Width > 0.0f)
                    {
                        itemHeight = (nfloat)Math.Floor(itemSize.Height * itemWidth / itemSize.Width);
                    }

                    attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                    attributes.Frame = new CGRect(xOffset, yOffset, itemWidth, itemHeight);
                    itemAttributes.Add(attributes);
                    allItemAttributes.Add(attributes);
                    columnHeights[columnIndex] = attributes.Frame.GetMaxY() + MinimumInterItemSpacing;
                }
                sectionItemAttributes.Add(itemAttributes);

                // Calculate Section Footer
                nfloat footerHeight = 0.0f;
                columnIndex = LongestColumnIndex();
                top = columnHeights[columnIndex] - MinimumInterItemSpacing + SectionInset.Bottom;
                footerHeight = (HeightForFooter == null) ? FooterHeight : HeightForFooter(CollectionView, this, section);

                if (footerHeight > 0)
                {
                    attributes = UICollectionViewLayoutAttributes.CreateForSupplementaryView(UICollectionElementKindSection.Footer, NSIndexPath.FromRowSection(0, section));
                    attributes.Frame = new CGRect(0, top, CollectionView.Bounds.Width, footerHeight);
                    footersAttributes.Add(section, attributes);
                    allItemAttributes.Add(attributes);
                    top = attributes.Frame.GetMaxY();
                }

                for (int n = 0; n < ColumnCount; n++)
                {
                    columnHeights[n] = top;
                }
            }

            var i = 0;
            var attrs = allItemAttributes.Count;
            while (i < attrs)
            {
                var rect1 = allItemAttributes[i].Frame;
                i = (int)Math.Min(i + unionSize, attrs) - 1;
                var rect2 = allItemAttributes[i].Frame;
                unionRects.Add(CGRect.Union(rect1, rect2));
                i++;
            }

        }

        public override CGSize CollectionViewContentSize
        {
            get
            {
                if (CollectionView.NumberOfSections() == 0)
                {
                    return new CGSize(0, 0);
                }

                var contentSize = CollectionView.Bounds.Size;
                contentSize.Height = columnHeights[0];
                return contentSize;
            }
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            if (indexPath.Section >= sectionItemAttributes.Count)
            {
                return null;
            }

            if (indexPath.Item >= sectionItemAttributes[indexPath.Section].Count)
            {
                return null;
            }

            var list = sectionItemAttributes[indexPath.Section];
            return list[(int)indexPath.Item];
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForSupplementaryView(NSString kind, NSIndexPath indexPath)
        {
            var attributes = new UICollectionViewLayoutAttributes();

            switch (kind)
            {
                case "header":
                    attributes = headersAttributes[indexPath.Section];
                    break;
                case "footer":
                    attributes = footersAttributes[indexPath.Section];
                    break;
            }
            attributes = headersAttributes[indexPath.Section];
            return attributes;
            //var attrs = UICollectionViewLayoutAttributes.CreateForSupplementaryView(kind, indexPath);
            //attrs.Frame = new CGRect(0, 0, CollectionView.Frame.Width, Header.HeightRequest);
            //return attrs;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var begin = 0;
            var end = unionRects.Count;
            List<UICollectionViewLayoutAttributes> attrs = new List<UICollectionViewLayoutAttributes>();

            for (int i = 0; i < end; i++)
            {
                if (rect.IntersectsWith(unionRects[i]))
                {
                    begin = i * (int)unionSize;
                    break;
                }
            }

            for (int i = end - 1; i >= 0; i--)
            {
                if (rect.IntersectsWith(unionRects[i]))
                {
                    end = (int)Math.Min((i + 1) * (int)unionSize, allItemAttributes.Count);
                    break;
                }
            }
            int counter = 0;

            //for (int i = 0; i < allItemAttributes.Count; i++)
            //{
            //    try
            //    {
            //        Console.WriteLine(i);
            //        var data = allItemAttributes[i].Frame.ToString();
            //        if (i == 99)
            //        {

            //        }
            //        Console.WriteLine(data);
            //        Console.WriteLine("End");
            //    }
            //    catch (Exception g)
            //    {
            //    }
                

            //}

            for (int i = begin; i < end; i++)
            {

                var attr = allItemAttributes[i];
                if (rect.IntersectsWith(attr.Frame))
                {
                    attrs.Add(attr);
                    //Console.WriteLine();
                    //Console.WriteLine(attr.ToString());
                    //Console.WriteLine();
                    counter++;
                }
            }
            Console.WriteLine("Items Count : " + counter);
            return attrs.ToArray();
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            var oldBounds = CollectionView.Bounds;
            return (newBounds.Width != oldBounds.Width);
        }
        #endregion

        #region Private Methods
        private int ShortestColumnIndex()
        {
            var index = 0;
            var shortestHeight = nfloat.MaxValue;
            var n = 0;

            // Scan each column for the shortest height
            foreach (nfloat height in columnHeights)
            {
                if (height < shortestHeight)
                {
                    shortestHeight = height;
                    index = n;
                }
                ++n;
            }

            return index;
        }

        private int LongestColumnIndex()
        {
            var index = 0;
            var longestHeight = nfloat.MinValue;
            var n = 0;

            // Scan each column for the shortest height
            foreach (nfloat height in columnHeights)
            {
                if (height > longestHeight)
                {
                    longestHeight = height;
                    index = n;
                }
                ++n;
            }

            return index;
        }

        private int NextColumnIndexForItem(nint item)
        {
            var index = 0;

            switch (ItemRenderDirection)
            {
                case WaterfallCollectionRenderDirection.ShortestFirst:
                    index = ShortestColumnIndex();
                    break;
                case WaterfallCollectionRenderDirection.LeftToRight:
                    index = GetIndexLeftToRght(item);
                    break;
                case WaterfallCollectionRenderDirection.RightToLeft:
                    index = (ColumnCount - 1) - ((int)item / ColumnCount);
                    break;
            }

            return index;
        }

        public int GetIndexLeftToRght(nint item)
        {
            return (int)(item % columnCount);
        }

        #endregion

        #region Events
        public delegate nfloat WaterfallCollectionFloatDelegate(UICollectionView collectionView, WaterfallCollectionLayout layout, nint section);
        public delegate UIEdgeInsets WaterfallCollectionEdgeInsetsDelegate(UICollectionView collectionView, WaterfallCollectionLayout layout, nint section);

        //public GetHeightForItemDelegate GetHeightForItemDelegate { get; set; }


        public WaterfallCollectionSizeDelegate SizeForItem;

        public CGSize CalculateFromSize(Size size)
        {
            return new CGSize(size.Width, size.Height);
        }

        public event WaterfallCollectionFloatDelegate HeightForHeader;
        public event WaterfallCollectionFloatDelegate HeightForFooter;
        public event WaterfallCollectionEdgeInsetsDelegate InsetForSection;
        public event WaterfallCollectionFloatDelegate MinimumInterItemSpacingForSection;
        #endregion
    }
}
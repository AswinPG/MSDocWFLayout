using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MSDocWFLayout
{
    //public delegate double GetHeightForItemDelegate(object item);

    public delegate CGSize WaterfallCollectionSizeDelegate(int index);

    public class WFCollectionView : ItemsView
    {
        public WaterfallCollectionSizeDelegate GetHeightForCellDelegate { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSDocWFLayout
{
    //public delegate double GetHeightForItemDelegate(object item);

    public delegate Size WaterfallCollectionSizeDelegate(int index);

    public class WFCollectionView : View
    {
        public event EventHandler<ItemsViewScrolledEventArgs> Scrolled;
        public event EventHandler RemainingItemsThresholdReached;
        public WaterfallCollectionSizeDelegate GetHeightForCellDelegate { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(WFCollectionView), null);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(WFCollectionView));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }


        public void SendScrolled(ItemsViewScrolledEventArgs e)
        {
            Scrolled?.Invoke(this, e);

            OnScrolled(e);
        }
        protected virtual void OnScrolled(ItemsViewScrolledEventArgs e)
        {

        }




        public static readonly BindableProperty RemainingItemsThresholdProperty =
            BindableProperty.Create(nameof(RemainingItemsThreshold), typeof(int), typeof(WFCollectionView), -1, validateValue: (bindable, value) => (int)value >= -1);

        public int RemainingItemsThreshold
        {
            get => (int)GetValue(RemainingItemsThresholdProperty);
            set => SetValue(RemainingItemsThresholdProperty, value);
        }

        public static readonly BindableProperty RemainingItemsThresholdReachedCommandProperty =
            BindableProperty.Create(nameof(RemainingItemsThresholdReachedCommand), typeof(ICommand), typeof(WFCollectionView), null);

        public ICommand RemainingItemsThresholdReachedCommand
        {
            get => (ICommand)GetValue(RemainingItemsThresholdReachedCommandProperty);
            set => SetValue(RemainingItemsThresholdReachedCommandProperty, value);
        }

        public static readonly BindableProperty RemainingItemsThresholdReachedCommandParameterProperty = BindableProperty.Create(nameof(RemainingItemsThresholdReachedCommandParameter), typeof(object), typeof(WFCollectionView), default(object));

        public object RemainingItemsThresholdReachedCommandParameter
        {
            get => GetValue(RemainingItemsThresholdReachedCommandParameterProperty);
            set => SetValue(RemainingItemsThresholdReachedCommandParameterProperty, value);
        }
        public void SendRemainingItemsThresholdReached()
        {
            RemainingItemsThresholdReached?.Invoke(this, EventArgs.Empty);

            if (RemainingItemsThresholdReachedCommand?.CanExecute(RemainingItemsThresholdReachedCommandParameter) == true)
                RemainingItemsThresholdReachedCommand?.Execute(RemainingItemsThresholdReachedCommandParameter);

            OnRemainingItemsThresholdReached();
        }
        protected virtual void OnRemainingItemsThresholdReached()
        {

        }




        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(View), typeof(WFCollectionView), null);
        public View Header
        {
            get => (View)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}
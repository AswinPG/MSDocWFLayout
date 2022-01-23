using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSDocWFLayout
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Tapped", ((Grid)sender).BindingContext.ToString(), "ok");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Tapped", ((VisualElement)sender).BindingContext.ToString(), "ok");
        }

        private async void TouchEffect_LongPressCompleted(object sender, Xamarin.CommunityToolkit.Effects.LongPressCompletedEventArgs e)
        {
            await DisplayAlert("Long Tapped", "e.Parameter.ToString()", "ok");
        }

        private async void TouchEffect_Completed(object sender, Xamarin.CommunityToolkit.Effects.TouchCompletedEventArgs e)
        {
            await DisplayAlert("Tapped", "blah", "ok");
        }

        private void TouchEffect_StatusChanged(object sender, Xamarin.CommunityToolkit.Effects.TouchStatusChangedEventArgs e)
        {

        }

        private void TouchEffect_StateChanged(object sender, Xamarin.CommunityToolkit.Effects.TouchStateChangedEventArgs e)
        {

        }
    }
}

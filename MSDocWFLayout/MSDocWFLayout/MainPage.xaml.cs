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
    }
}

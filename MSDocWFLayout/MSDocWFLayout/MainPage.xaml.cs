using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSDocWFLayout
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<TestClass> data;
        Random rand;
        HttpClient Http;
        public MainPage()
        {
            rand = new Random();
            Http = new HttpClient();
            InitializeComponent();
            Cv.GetHeightForCellDelegate = (cell) =>
            {
                return new CoreGraphics.CGSize(data[cell].width, data[cell].height);
            };
            data = new ObservableCollection<TestClass>()
            {
            };
            AddData();
            Cv.ItemsSource = data;
        }
        public async void AddData()
        {
            try
            {
                string url = "https://picsum.photos/v2/list?page=" + data.Count + "&limit=20";
                var response = await Http.GetAsync(url);
                List<TestClass> data2 = await response.Content.ReadAsAsync<List<TestClass>>();

                string fg = response.Content.ToString();
                for (int i = 0; i < data2.Count; i++)
                {
                    //double height = random.Next(200, 400);
                    if (data.Where(c => c.download_url == data2[i].download_url).FirstOrDefault() == null)
                        data.Add(data2[i]);
                }
                MainImage.Source = data[0].download_url;
                MainImage2.Source = data[0].url;
            }
            catch (Exception k)
            {

            }
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
            //await DisplayAlert("Tapped", "blah", "ok");
            var data = ((sender as VisualElement).BindingContext as TestClass);
            //data.Name = "TestPassed" + data.Name;
        }

        private void TouchEffect_StatusChanged(object sender, Xamarin.CommunityToolkit.Effects.TouchStatusChangedEventArgs e)
        {

        }

        private void TouchEffect_StateChanged(object sender, Xamarin.CommunityToolkit.Effects.TouchStateChangedEventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //data.Add(new TestClass("Name " + data.Count));
        }
        bool updating = false;
        private async void Cv_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (!updating)
            {
                updating = true;
                try
                {
                    string url = "https://picsum.photos/v2/list?page=" + data.Count + "&limit=20";
                    List<TestClass> data2 = await (await Http.GetAsync(url)).Content.ReadAsAsync<List<TestClass>>();

                    string fg = data2.ToString();
                    for (int i = 0; i < data2.Count; i++)
                    {
                        //double height = random.Next(200, 400);
                        if (data.Where(c => c.download_url == data2[i].download_url).FirstOrDefault() == null)
                            data.Add(data2[i]);
                    }
                }
                catch (Exception k)
                {

                }
                updating = false;
            }
        }
    }
}

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
                return new Size(data[cell].width, data[cell].height);
            };
            data = new ObservableCollection<TestClass>()
            {
            };
            AddData();
            Cv.ItemsSource = data;
        }
        public async void AddData()
        {
            /*try
            {
                string url = "https://api.unsplash.com/photos/random/?count=20&client_id=VqNyjK6u67FbxI9uHuQnkYGbKq3PAeIR20i-6nGyR4M";
                var response = await Http.GetAsync(url);
                List<Splash> data2 = await response.Content.ReadAsAsync<List<Splash>>();

                string fg = response.Content.ToString();
                for (int i = 0; i < data2.Count; i++)
                {
                    //double height = random.Next(200, 400);
                    if (data.Where(c => c.download_url == data2[i].urls.full).FirstOrDefault() == null)
                        data.Add(new TestClass()
                        {
                            download_url = data2[i].urls.regular,
                            author = "blah",
                            width = data2[i].width,
                            height = data2[i].height,
                            id = data.Count.ToString(),
                            url = data2[i].links.download_location
                        }
                            );
                }
                //MainImage.Source = data[0].download_url;
                //MainImage2.Source = "https://images.unsplash.com/photo-1640940226895-28204c68ef74?crop=entropy&cs=srgb&fm=jpg&ixid=MnwyOTQyOTF8MHwxfHJhbmRvbXx8fHx8fHx8fDE2NDMwNTczMTg&ixlib=rb-1.2.1&q=85";
            }
            catch (Exception k)
            {

            }*/

            try
            {
                string url = "https://picsum.photos/v2/list?page=" + data.Count + "&limit=20";
                //List<TestClass> data2 = await (await Http.GetAsync(url)).Content.ReadAsAsync<List<TestClass>>();

                //string fg = data2.ToString();

                for (int i = 0; i < 20; i++)
                {
                    //double height = random.Next(200, 400);
                    //if (data.Where(c => c.download_url == data2[i].download_url).FirstOrDefault() == null)
                    //data.Add(data2[i]);
                    var height2 = rand.Next(500, 1000);
                    data.Add(new TestClass()
                    {
                        download_url = "https://picsum.photos/id/" + data.Count + "/" + "500" + "/" + height2,
                        author = "blah",
                        width = 500,
                        height = height2,
                        id = data.Count.ToString(),
                        url = ""
                    });
                }
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

                await Task.Delay(500);
                AddData();

                updating = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AppsZoom
{
    public partial class Other : PhoneApplicationPage
    {
        public string FromPage = "";
        public string CategoryId = "-1";
        public string PublisherId = "-1";
        public string fromReviews = "-1";
        public string toReviews = "-1";
        public string searchText = "";
        private NavigationEventArgs Args;

        public Other()
        {
            InitializeComponent();
            DataContext = App.ApplicationViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs args) {
            Args = args;
            App.ApplicationViewModel.IsDataLoaded = false;

            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("fromPage"))
            {
                FromPage = parameters["fromPage"];
            }
            if (parameters.ContainsKey("categoryId") && parameters.ContainsKey("categoryName"))
            {
                CategoryId = parameters["categoryId"];
                string cName = parameters["categoryName"];
                piv.Text = cName;
            }
            if (parameters.ContainsKey("publisherId") && parameters.ContainsKey("publisherName"))
            {
                PublisherId = parameters["publisherId"];
                string cName = parameters["publisherName"];
                piv.Text = cName;
            }
            if (parameters.ContainsKey("from") && parameters.ContainsKey("to") &&parameters.ContainsKey("text"))
            {
                fromReviews = parameters["from"];
                toReviews = parameters["to"];
                string piv1 = parameters["text"];
                piv.Text= piv1;
            }
            if(parameters.ContainsKey("searchText")){
                searchText = parameters["searchText"];
                piv.Text = searchText;
            }

            if (!App.ApplicationViewModel.IsDataLoaded)
            {
                if(FromPage.Equals("categories"))
                    App.ApplicationViewModel.LoadData(FromPage, CategoryId);
                else if(FromPage.Equals("publishers"))
                    App.ApplicationViewModel.LoadData(FromPage, PublisherId);
                else if (FromPage.Equals("reviews"))
                    App.ApplicationViewModel.LoadData(FromPage, fromReviews, toReviews);
                else if (FromPage.Equals("search"))
                    App.ApplicationViewModel.LoadData(FromPage, searchText);
                else if (FromPage.Equals("recommendation"))
                {
                    piv.Text = "Recommendation";
                    App.ApplicationViewModel.LoadData(FromPage, "");

                }

            }

           
        }


        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = sender as TextBlock;
            string applicationId = txt.Tag.ToString();
            string destination = "/AppDetails.xaml?applicationId=" + applicationId;
            this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
        }

        private void imgLike_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image img = sender as Image;
            string appId = img.Tag.ToString();

            string userId = (Application.Current as App).UserId; 

            MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
            client.setLikeAsync(userId, appId);
            client.setLikeCompleted += new EventHandler<MyService.setLikeCompletedEventArgs>(client_setLikeCompleted);
        }

        private void client_setLikeCompleted(object sender, MyService.setLikeCompletedEventArgs e)
        {
            this.OnNavigatedTo(Args);
        }

        
    }
}
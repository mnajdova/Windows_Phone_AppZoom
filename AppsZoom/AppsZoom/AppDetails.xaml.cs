using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;

namespace AppsZoom
{
    public partial class AppDetails : PhoneApplicationPage
    {
        public AppDetails()
        {
            InitializeComponent();
            DataContext = App.AppDetailViewModel;
            
        }


        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            
            if (parameters.ContainsKey("applicationId"))
            {
                string appId = parameters["applicationId"];
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
                client.getApplicationByIdAsync(Int32.Parse(appId));
                client.getApplicationByIdCompleted += new EventHandler<MyService.getApplicationByIdCompletedEventArgs>(client_getApplicationByIdCompleted);
                App.AppDetailViewModel.LoadData(appId);
            
            }



        }

        private void client_getApplicationByIdCompleted(object sender, MyService.getApplicationByIdCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject jobj = JObject.Parse(jsonString);
            jobj = jobj["Application"] as JObject;

            string Id = jobj["Id"].ToString();
            string Name = jobj["Name"].ToString();
            float Price = float.Parse(jobj["Price"].ToString());
            float Rating = float.Parse(jobj["Rating"].ToString());
            int Reviews = Int32.Parse(jobj["Reviews"].ToString());
            DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
            string PublisherName = jobj["PublisherName"].ToString();
            string ImageUrl = jobj["ImageUrl"].ToString();

            pivFirstItem.Text = Name;


            BitmapImage myBitmapImage = new BitmapImage();
            
            myBitmapImage.UriSource = new Uri(ImageUrl);
            myBitmapImage.DecodePixelWidth = 300;
            myBitmapImage.DecodePixelWidth = 300;
            myBitmapImage.DecodePixelType = DecodePixelType.Logical;
            img.Source = myBitmapImage;
            txtDatePublished.Text = "Date published: "+DatePublished.ToShortDateString();
            txtPublisher.Text = "Publisher: "+PublisherName;
            txtRating.Text = "Rating: "+Rating.ToString();
            txtReviews.Text = "Number of reviews: "+Reviews.ToString();
            if (Price > 0)
                txtPrice.Text = "Price: $ " + Price.ToString();
            else
                txtPrice.Text = "This application is free";
        }

    }
}
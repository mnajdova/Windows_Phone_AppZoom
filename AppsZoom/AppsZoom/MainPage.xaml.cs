using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppsZoom.Resources;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using AppsZoom.ViewModels;
using Newtonsoft.Json.Linq;

namespace AppsZoom
{
    

    public partial class MainPage : PhoneApplicationPage
    {



        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }


        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LongListSelector_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            SubcategoryViewModel item = e.AddedItems[0] as SubcategoryViewModel;
            string Id = item.Id;
            //string destination = "/Other.xaml?categoryId="+Id;
            string destination = "/Other.xaml?fromPage=categories&categoryId=" + Id+"&categoryName="+item.SubcategoryName.ToString();
            this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PublisherViewModel item = e.AddedItems[0] as PublisherViewModel;
            string Id = item.Id;
            //string destination = "/Other.xaml?categoryId="+Id;
            string destination = "/Other.xaml?fromPage=publishers&publisherId=" + Id + "&publisherName=" + item.Name.ToString();
            this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
        }

        private void LongListSelector_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            NumberOfReviewsViewModel item = e.AddedItems[0] as NumberOfReviewsViewModel;
            string from = item.FromReviews.ToString();
            string to = item.ToReviews.ToString();
            //string destination = "/Other.xaml?categoryId="+Id;
            string destination = "/Other.xaml?fromPage=reviews&from=" + from + "&to=" + to + "&text=" + item.Text;
            this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (txtSearc.Visibility == System.Windows.Visibility.Collapsed)
            {
                txtSearc.Visibility = System.Windows.Visibility.Visible;
            }
            else {
                string searchText = txtSearc.Text;
                string destination = "/Other.xaml?fromPage=search&searchText=" + searchText;
                this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
                txtSearc.Visibility = System.Windows.Visibility.Collapsed;
                txtSearc.Text = "";
            }

        }

        private void btnRec_Click(object sender, RoutedEventArgs e)
        {
            string destination = "/Other.xaml?fromPage=recommendation";
            this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
        }

        private void txtRegister_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LogIn.Visibility = System.Windows.Visibility.Collapsed;
            Register.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
            client.registerUserAsync(txtFirstName.Text, txtSecondName.Text, txtUsername1.Text, txtPassword1.Password);
            client.registerUserCompleted += new EventHandler<MyService.registerUserCompletedEventArgs>(client_registerUserCompleted);
        }

        private void client_registerUserCompleted(object sender, MyService.registerUserCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            json = json["User"] as JObject;
            if (json["valid"].ToString().Equals("1"))
            {
                (Application.Current as App).UserId = json["Id"].ToString();
                (Application.Current as App).Username = json["Username"].ToString();
                SeeRec.Visibility = System.Windows.Visibility.Visible;
                Register.Visibility = System.Windows.Visibility.Collapsed;
                txtLogInError.Text = "";
            }
            else
            {
                txtRegError.Text = "The username alredy exist";
                txtUsername1.Text = "";
                txtPassword1.Password = "";
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
            client.getUserAsync(txtUsername.Text, txtPassword.Password);
            client.getUserCompleted += new EventHandler<MyService.getUserCompletedEventArgs>(client_getUserCompleted);
        }

        private void client_getUserCompleted(object sender, MyService.getUserCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            json = json["User"] as JObject;
            if (json["valid"].ToString().Equals("1"))
            {
                (Application.Current as App).UserId = json["Id"].ToString();
                (Application.Current as App).Username = json["Username"].ToString();
                SeeRec.Visibility = System.Windows.Visibility.Visible;
                LogIn.Visibility = System.Windows.Visibility.Collapsed;
                txtLogInError.Text = "";
            }
            else {
                txtLogInError.Text = "Not valid username or password";
                txtUsername.Text = "";
                txtPassword.Password = "";
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

    }


    
}
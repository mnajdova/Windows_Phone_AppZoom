using AppsZoom.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppsZoom.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        //private List<CategoryViewModel> categories;

        public ApplicationViewModel()
        {
            this.Items = new ObservableCollection<AppViewModel>();
            this.Sorry = new ObservableCollection<string>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<AppViewModel> Items { get; private set; }
        public ObservableCollection<string> Sorry { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            set;
        }

        private string searchText;

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData(string fromPage, string cpId, string toR="")
        {
            this.Items.Clear();
            this.Sorry.Clear();
            string userId = (Application.Current as App).UserId; 
            // Sample data; replace with real data
            if (fromPage.Equals("categories"))
            {
                int cId = Int32.Parse(cpId);
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
               
                client.getApplicationsByCategoryAsync(cId, userId);
                client.getApplicationsByCategoryCompleted += new EventHandler<MyService.getApplicationsByCategoryCompletedEventArgs>(client_getApplicationsByCategoryCompleted);
            }
            else if (fromPage.Equals("publishers")) {
                int pId = Int32.Parse(cpId);
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
                client.getApplicationsByPublisherAsync(pId, userId);
                client.getApplicationsByPublisherCompleted += new EventHandler<MyService.getApplicationsByPublisherCompletedEventArgs>(client_getApplicationsByPublisherCompleted);
            }
            else if (fromPage.Equals("reviews"))
            {
                int from = Int32.Parse(cpId);
                int to = Int32.Parse(toR);
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
                client.getApplicationsByReviewsAsync(from, to, userId);
                client.getApplicationsByReviewsCompleted += new EventHandler<MyService.getApplicationsByReviewsCompletedEventArgs>(client_getApplicationsByReviewCompleted);
            }
            else if (fromPage.Equals("search")) {
                searchText = cpId;
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
                client.getApplicationsByNameAsync(searchText, userId);
                client.getApplicationsByNameCompleted += new EventHandler<MyService.getApplicationsByNameCompletedEventArgs>(getApplicationsByNameCompleted);
            }
            else if (fromPage.Equals("recommendation"))
            {
                MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
                client.getApplicationsByRecommendationAsync((Application.Current as App).UserId);
                client.getApplicationsByRecommendationCompleted += new EventHandler<MyService.getApplicationsByRecommendationCompletedEventArgs>(client_getApplicationsByReccomendationCompleted);
            }

            //this.Items.Add(new AppViewModel() { Id = "3", CategoryName = "Themes" });
            this.IsDataLoaded = true;
        }

        private void client_getApplicationsByReccomendationCompleted(object sender, MyService.getApplicationsByRecommendationCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Applications"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                float Price = float.Parse(jobj["Price"].ToString());
                float Rating = float.Parse(jobj["Rating"].ToString());
                int Reviews = Int32.Parse(jobj["Reviews"].ToString());
                DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
                string PublisherName = jobj["PublisherName"].ToString();
                string ImageUrl = jobj["ImageUrl"].ToString();
                int ShowLike = Int32.Parse(jobj["ShowLike"].ToString());
                this.Items.Add(new AppViewModel() { Id = Id, Name = Name, Price = Price, Rating = Rating, ImageUrl = ImageUrl, Reviews = Reviews, DatePublished = DatePublished, PublisherName = PublisherName, ShowLike = ShowLike });
            }
            if (Items.Count == 0)
            {
                this.Sorry.Add("No results");
            }
        }

        private void getApplicationsByNameCompleted(object sender, MyService.getApplicationsByNameCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Applications"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                float Price = float.Parse(jobj["Price"].ToString());
                float Rating = float.Parse(jobj["Rating"].ToString());
                int Reviews = Int32.Parse(jobj["Reviews"].ToString());
                DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
                string PublisherName = jobj["PublisherName"].ToString();
                string ImageUrl = jobj["ImageUrl"].ToString();
                int ShowLike = Int32.Parse(jobj["ShowLike"].ToString());
                this.Items.Add(new AppViewModel() { Id = Id, Name = Name, Price = Price, Rating = Rating, ImageUrl = ImageUrl, Reviews = Reviews, DatePublished = DatePublished, PublisherName = PublisherName, ShowLike = ShowLike });
            }
            if (Items.Count == 0)
            {
                this.Sorry.Add("No results");
            }
            
        }

        private void client_getApplicationsByReviewCompleted(object sender, MyService.getApplicationsByReviewsCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Applications"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                float Price = float.Parse(jobj["Price"].ToString());
                float Rating = float.Parse(jobj["Rating"].ToString());
                int Reviews = Int32.Parse(jobj["Reviews"].ToString());
                DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
                string PublisherName = jobj["PublisherName"].ToString();
                string ImageUrl = jobj["ImageUrl"].ToString();
                int ShowLike = Int32.Parse(jobj["ShowLike"].ToString());
                this.Items.Add(new AppViewModel() { Id = Id, Name = Name, Price = Price, Rating = Rating, ImageUrl = ImageUrl, Reviews = Reviews, DatePublished = DatePublished, PublisherName = PublisherName, ShowLike=ShowLike });
            }
            if (Items.Count == 0)
            {
                this.Sorry.Add("No results");
            }
        }

        private void client_getApplicationsByPublisherCompleted(object sender, MyService.getApplicationsByPublisherCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Applications"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                float Price = float.Parse(jobj["Price"].ToString());
                float Rating = float.Parse(jobj["Rating"].ToString());
                int Reviews = Int32.Parse(jobj["Reviews"].ToString());
                DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
                string PublisherName = jobj["PublisherName"].ToString();
                string ImageUrl = jobj["ImageUrl"].ToString();
                int ShowLike = Int32.Parse(jobj["ShowLike"].ToString());
                this.Items.Add(new AppViewModel() { Id = Id, Name = Name, Price = Price, Rating = Rating, ImageUrl = ImageUrl, Reviews = Reviews, DatePublished = DatePublished, PublisherName = PublisherName, ShowLike = ShowLike });
            }
            if (Items.Count == 0) {
                this.Sorry.Add("No results");
            }

        }

        private void client_getApplicationsByCategoryCompleted(object sender, MyService.getApplicationsByCategoryCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Applications"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                float Price = float.Parse(jobj["Price"].ToString());
                float Rating = float.Parse(jobj["Rating"].ToString());
                int Reviews = Int32.Parse(jobj["Reviews"].ToString());
                DateTime DatePublished = DateTime.Parse(jobj["DatePublished"].ToString());
                string PublisherName = jobj["PublisherName"].ToString();
                string ImageUrl = jobj["ImageUrl"].ToString();
                int ShowLike = Int32.Parse(jobj["ShowLike"].ToString());
                this.Items.Add(new AppViewModel() { Id = Id, Name = Name, Price = Price, Rating = Rating, ImageUrl = ImageUrl, Reviews = Reviews, DatePublished = DatePublished, PublisherName = PublisherName, ShowLike = ShowLike });
            }
            if (Items.Count == 0)
            {
                this.Sorry.Add("No results");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AppsZoom.Resources;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppsZoom.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<CategoryViewModel> categories;

        public MainViewModel()
        {
            this.Items = new ObservableCollection<CategoryViewModel>();
            this.Publishers = new ObservableCollection<PublisherViewModel>();
            this.Reviews = new ObservableCollection<NumberOfReviewsViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<CategoryViewModel> Items { get; private set; }
        public ObservableCollection<PublisherViewModel> Publishers { get; private set; }
        public ObservableCollection<NumberOfReviewsViewModel> Reviews { get; private set; }

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
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
           
            // Sample data; replace with real data

            categories = new List<CategoryViewModel>();
            MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
            client.getMainCategoriesAsync();
            client.getMainCategoriesCompleted += new EventHandler<MyService.getMainCategoriesCompletedEventArgs>(client_getMainCategoriesCompleted);





            MyService.WPServiceSoapClient client1 = new MyService.WPServiceSoapClient();
            client1.getPublishersAsync();
            client1.getPublishersCompleted += new EventHandler<MyService.getPublishersCompletedEventArgs>(client_getPublishersCompleted);



            this.Reviews.Add(new NumberOfReviewsViewModel() {Text="less than 100 reviews", FromReviews=0, ToReviews=100 });
            this.Reviews.Add(new NumberOfReviewsViewModel() { Text = "from 101 to 1000", FromReviews = 101, ToReviews = 1000 });
            this.Reviews.Add(new NumberOfReviewsViewModel() { Text = "from 1001 to 5000", FromReviews = 1001, ToReviews = 5000 });
            this.Reviews.Add(new NumberOfReviewsViewModel() { Text = "from 5001 to 10000", FromReviews = 5001, ToReviews = 10000 });
            this.Reviews.Add(new NumberOfReviewsViewModel() { Text = "more than 10001", FromReviews = 10001, ToReviews = 1000000 });







            //foreach (CategoryViewModel c in categories) {
            //    subcategories = new List<SubcategoryViewModel>();
            //    //MyService.WPServiceSoapClient client1 = new MyService.WPServiceSoapClient();
            //    //client1.getCategoriesAsync(Int32.Parse(c.Id));
            //    //client1.getCategoriesCompleted += new EventHandler<MyService.getCategoriesCompletedEventArgs>(client_getCategoriesCompleted);
            //    //c.Subcategories = subcategories;
            //    this.Items.Add(c);
           
            //}

            //this.Items.Add(new CategoryViewModel() { Id = "3", CategoryName = "Themes" });
            this.IsDataLoaded = true;
        }

        private void client_getPublishersCompleted(object sender, MyService.getPublishersCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Publishers"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                this.Publishers.Add(new PublisherViewModel() { Id = Id, Name = Name });
            }
        }

        private void client_getMainCategoriesCompleted(object sender, MyService.getMainCategoriesCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString); 
            JArray array = (JArray) json["MainCategories"];

            foreach(JObject jobj in array){
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                this.Items.Add(new CategoryViewModel() { Id = Id, CategoryName = Name });
            }

            for (int i = 0; i < this.Items.Count; i++)
            {
                MyService.WPServiceSoapClient client1 = new MyService.WPServiceSoapClient();
                client1.getCategoriesAsync(Int32.Parse(this.Items[i].Id));
                client1.getCategoriesCompleted += new EventHandler<MyService.getCategoriesCompletedEventArgs>(client_getCategoriesCompleted);
            }


        }

        private void client_getCategoriesCompleted(object sender, MyService.getCategoriesCompletedEventArgs e)
        {
            
            List<SubcategoryViewModel> subcategories = new List<SubcategoryViewModel>();
            
            string jsonString = e.Result.ToString();
            
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Categories"];
            string mainCategory = "";
            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Name = jobj["Name"].ToString();
                mainCategory = jobj["MainCategoryId"].ToString();
                subcategories.Add(new SubcategoryViewModel() { Id = Id, SubcategoryName = Name });
            }


            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Id.Equals(mainCategory))
                    this.Items[i].Subcategories = subcategories;
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
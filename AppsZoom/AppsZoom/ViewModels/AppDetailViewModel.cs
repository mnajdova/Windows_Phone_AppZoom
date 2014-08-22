using AppsZoom.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsZoom.ViewModels
{
    public class AppDetailViewModel: INotifyPropertyChanged
    {
        //private List<CategoryViewModel> categories;

        public AppDetailViewModel()
        {
            this.Comments = new ObservableCollection<CommentsViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<CommentsViewModel> Comments { get; private set; }

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
        public void LoadData(string appId)
        {
            this.Comments.Clear();
            MyService.WPServiceSoapClient client = new MyService.WPServiceSoapClient();
            client.getCommentsAsync(appId);
            client.getCommentsCompleted += new EventHandler<MyService.getCommentsCompletedEventArgs>(client_getCommentsCompleted);
            this.IsDataLoaded = true;
        }

        private void client_getCommentsCompleted(object sender, MyService.getCommentsCompletedEventArgs e)
        {
            string jsonString = e.Result.ToString();
            JObject json = JObject.Parse(jsonString);
            JArray array = (JArray)json["Comments"];

            foreach (JObject jobj in array)
            {
                string Id = jobj["Id"].ToString();
                string Username = jobj["Username"].ToString();
                DateTime DateAdded = DateTime.Parse(jobj["DateAdded"].ToString());
                string Text = jobj["Text"].ToString();
                this.Comments.Add(new CommentsViewModel() {Username=Username, DateAddedD=DateAdded, Text=Text});
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


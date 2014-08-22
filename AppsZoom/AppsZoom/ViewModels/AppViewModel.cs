using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsZoom.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private float _price;

        public float Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        private float _rating;

        public float Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                if (value != _rating)
                {
                    _rating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }

        private int _reviews;

        public int Reviews
        {
            get
            {
                return _reviews;
            }
            set
            {
                if (value != _reviews)
                {
                    _reviews = value;
                    NotifyPropertyChanged("Reviews");
                }
            }
        }

        private DateTime _datePublished;

        public DateTime DatePublished
        {
            get
            {
                return _datePublished;
            }
            set
            {
                if (value != _datePublished)
                {
                    _datePublished = value;
                    NotifyPropertyChanged("DatePublished");
                }
            }
        }

        private string _publisherName;

        public string PublisherName
        {
            get
            {
                return _publisherName;
            }
            set
            {
                if (value != _publisherName)
                {
                    _publisherName = value;
                    NotifyPropertyChanged("PublisherName");
                }
            }
        }

        private string _imageUrl;

        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                if (value != _imageUrl)
                {
                    _imageUrl = value;
                    NotifyPropertyChanged("ImageUrl");
                }
            }
        }

        private int _ShowLike;



        public int ShowLike {  get
            {
                return _ShowLike;
            }
            set
            {
                if (value != _ShowLike)
                {
                    _ShowLike = value;
                    NotifyPropertyChanged("ShowLike");
                }
            } }


        public System.Windows.Visibility LikeVisibility { get {

            if (_ShowLike == 0)
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        
        } set { } }




        public string DisplayRating { get { return "Rating " + Rating.ToString() + " out of " + Reviews.ToString(); } set { } }
        public string DisplayDatePublished { get { return DatePublished.ToString(); } set { } }
        public string DisplayReviews { get { return Reviews.ToString(); } set { } }
        public string DisplayPrice { get { 
            if(Price == 0) 
                return "Free"; 
            else 
            return "$"+Price.ToString(); 
        } 
            set { } }

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

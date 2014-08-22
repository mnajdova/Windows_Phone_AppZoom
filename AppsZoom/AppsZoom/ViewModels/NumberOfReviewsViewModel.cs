using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsZoom.ViewModels
{
    public class NumberOfReviewsViewModel : INotifyPropertyChanged
    {

        private string _name;

        public string Text
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
                    NotifyPropertyChanged("Text");
                }
            }
        }

        
        private int _fromReviews;

        public int FromReviews
        {
            get
            {
                return _fromReviews;
            }
            set
            {
                if (value != _fromReviews)
                {
                    _fromReviews = value;
                    NotifyPropertyChanged("FromReviews");
                }
            }
        }

        private int _toReviews;

        public int ToReviews
        {
            get
            {
                return _toReviews;
            }
            set
            {
                if (value != _toReviews)
                {
                    _toReviews = value;
                    NotifyPropertyChanged("FromReviews");
                }
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

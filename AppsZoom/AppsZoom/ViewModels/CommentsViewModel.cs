using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsZoom.ViewModels
{
    public class CommentsViewModel : INotifyPropertyChanged
    {
        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                NotifyPropertyChanged("Username");
            }
        }


        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyPropertyChanged("Text");
                }
            }
        }


        private DateTime _dateAdded;

        public DateTime DateAddedD
        {
            get
            {
                return _dateAdded;
            }
            set
            {
                if (value != _dateAdded)
                {
                    _dateAdded = value;
                    NotifyPropertyChanged("DateAdded");
                }
            }
        }

        public string DateAdded { get { return DateAddedD.ToShortDateString(); } set { } }


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

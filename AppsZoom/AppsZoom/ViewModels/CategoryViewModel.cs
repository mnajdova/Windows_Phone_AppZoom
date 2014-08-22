using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsZoom.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
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
        
        public string CategoryName
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
                    NotifyPropertyChanged("CategoryName");
                }
            }
        }

        private List<SubcategoryViewModel> _subcategories;

        public List<SubcategoryViewModel> Subcategories
        {
            get
            {
                return _subcategories;
            }
            set
            {
                if (value != _subcategories)
                {
                    _subcategories = value;
                    NotifyPropertyChanged("Subcategories");
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

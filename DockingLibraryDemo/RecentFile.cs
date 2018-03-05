using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace DockingLibraryDemo
{
    class RecentFile : INotifyPropertyChanged
    {

        List<PropertyChangedEventHandler> list = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add 
            {
                list.Add(value);
            }
            remove
            {
                list.Remove(value);
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            foreach (PropertyChangedEventHandler h in list)
                h(this, new PropertyChangedEventArgs(info));
        }

        public RecentFile(string fileName, string path, int size)
        {
            _fn = fileName;
            _path = path;
            _size = size;
        }

        string _fn;

        public string FileName
        {
            get { return _fn; }
            set
            {
                _fn = value;
                NotifyPropertyChanged("FileName");
            }
        }        

        string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                NotifyPropertyChanged("Path");
            }
        }    

        int _size;

        public int Age
        {
            get { return _size; }
            set
            {
                _size = value;
                NotifyPropertyChanged("Size");
            }
        }

        public override string ToString()
        {
            return FileName;
        }
    
    }
}

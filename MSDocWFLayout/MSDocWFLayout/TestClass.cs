using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MSDocWFLayout
{
    public class TestClass : INotifyPropertyChanged
    {
        public string id { get; set; }
        public string author { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string url { get; set; }
        public string download_url { get; set; }
        public TestClass()
        {
            //Name = "blah";
            //height = new Random().Next(80, 200);
        }
        //public TestClass(string nam)
        //{
        //    Name = nam;
        //    //height = new Random().Next(80, 200);
        //}
        //string name;
        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }
        //    set
        //    {
        //        name = value;
        //        NotifyPropertyChanged();
        //    }
        //}
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlarmClockApp.Models
{
    public class Alarm : INotifyPropertyChanged
    {
        private int hour;
        private int minute;
        private string days;
        public int Hours
        {
            get { return hour; }
            set
            {
                if (hour == value)
                    return;
                hour = value;
                OnPropertyChanged("Hours");
            }
        }
        public int Minutes
        {
            get { return minute; }
            set
            {
                if (minute == value)
                    return;
                minute = value;
                OnPropertyChanged("Minutes");
            }
        }
        public string Days
        {
            get { return days; }
            set
            {
                if (days == value)
                    return;
                days = value;
                OnPropertyChanged("Days");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}

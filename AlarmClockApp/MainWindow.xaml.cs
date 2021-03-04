using AlarmClockApp.Models;
using AlarmClockApp.Services;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AlarmClockApp
{
    public partial class MainWindow : UserControl
    {
        private static readonly string path = $"{Environment.CurrentDirectory}\\alarmsList.json";
        public MainWindow()
        {
            InitializeComponent();
            Binding bindingHours = new Binding();

            bindingHours.ElementName = "MainWindow";
            bindingHours.Path = new PropertyPath("hours");
            Hours.SetBinding(TextBlock.TextProperty, bindingHours);

            Binding bindingMinutes = new Binding();
            bindingMinutes.ElementName = "MainWindow";
            bindingMinutes.Path = new PropertyPath("minutes");
            Minutes.SetBinding(TextBlock.TextProperty, bindingHours);

            Timer.Tick += new EventHandler(Timer_Click);

            Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Start();
        }

        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public int hours = 4;
        public int minutes = 20;
        private string alarmSongUrl = "";
        private MediaPlayer mediaPlayer = new MediaPlayer();
        FileIOService fileIOService = new FileIOService(path);
        private void btnHoursUp_Click(object sender, RoutedEventArgs e)
        {
            var buff = int.Parse(this.Hours.Text);

            buff++;
            if (buff > 23)
            {
                buff = 0;
            }

            this.Hours.Text = buff.ToString();
            this.hours = buff;
        }

        private void btnHoursDown_Click(object sender, RoutedEventArgs e)
        {
            var buff = int.Parse(this.Hours.Text);
            buff--;
            if (buff < 0)
            {
                buff = 23;
            }

            this.Hours.Text = buff.ToString();
            this.hours = buff;
        }

        private void btnMinUp_Click(object sender, RoutedEventArgs e)
        {
            var buff = int.Parse(this.Minutes.Text);
            buff++;
            if (buff > 59)
            {
                buff = 0;
            }

            this.Minutes.Text = buff.ToString();
            this.minutes = buff;
        }

        private void btnMinDown_Click(object sender, RoutedEventArgs e)
        {
            var buff = int.Parse(this.Minutes.Text);
            buff--;
            if (buff < 0)
            {
                buff = 59;
            }

            this.Minutes.Text = buff.ToString();
            this.minutes = buff;
        }

        private void SetTheAllarm_Click(object sender, RoutedEventArgs e)
        {
            var alarmList = fileIOService.LoadData();
            alarmList.Add(new Alarm { Days = SelectedDays, Hours = hours, Minutes = minutes });
            fileIOService.SaveData(alarmList);
            MessageBox.Show("New alarm clock added!");
        }
        private void AlarmList_Click(object sender, RoutedEventArgs e)
        {

            var alarmList = new AlarmList();
            alarmList.Show();
        }

        // Sets all toggle buttons to "off"

        public void resetControl()
        {
            tbSun.IsChecked = false;
            tbMon.IsChecked = false;
            tbTue.IsChecked = false;
            tbWed.IsChecked = false;
            tbThu.IsChecked = false;
            tbFri.IsChecked = false;
            tbSat.IsChecked = false;
        }

        // Used to get and set selected days, is returned as comma separated string

        public string SelectedDays
        {
            get
            {
                string selected = "";
                int totalDaysSelected = 0;

                if ((bool)tbSun.IsChecked)
                {
                    selected += "Sun,";
                    totalDaysSelected++;
                }
                if ((bool)tbMon.IsChecked)
                {
                    selected += "Mon,";
                    totalDaysSelected++;
                }
                if ((bool)tbTue.IsChecked)
                {
                    selected += "Tue,";
                    totalDaysSelected++;
                }
                if ((bool)tbWed.IsChecked)
                {
                    selected += "Wed,";
                    totalDaysSelected++;
                }
                if ((bool)tbThu.IsChecked)
                {
                    selected += "Thu,";
                    totalDaysSelected++;
                }
                if ((bool)tbFri.IsChecked)
                {
                    selected += "Fri,";
                    totalDaysSelected++;
                }
                if ((bool)tbSat.IsChecked)
                {
                    selected += "Sat,";
                    totalDaysSelected++;
                }

                if (totalDaysSelected == 7)
                {
                    selected = "Every Day";
                }
                else if (totalDaysSelected == 2 && (bool)tbSun.IsChecked && (bool)tbSat.IsChecked)
                {
                    selected = "Weekend";
                }
                else if (totalDaysSelected == 5 && !(bool)tbSun.IsChecked && !(bool)tbSat.IsChecked)
                {
                    selected = "Weekdays";
                }
                return selected;
            }

            set
            {
                bool everyday = value.Contains("Every Day");
                bool weekend = value.Contains("Weekend");
                bool weekdays = value.Contains("Weekdays");
                bool sun = everyday || weekend || value.Contains("Sun");
                bool mon = everyday || weekdays || value.Contains("Mon");
                bool tue = everyday || weekdays || value.Contains("Tue");
                bool wed = everyday || weekdays || value.Contains("Wed");
                bool thu = everyday || weekdays || value.Contains("Thu");
                bool fri = everyday || weekdays || value.Contains("Fri");
                bool sat = everyday || weekend || value.Contains("Sat");

                resetControl();

                if (sun)
                {
                    tbSun.IsChecked = true;
                }
                if (mon)
                {
                    tbMon.IsChecked = true;
                }
                if (tue)
                {
                    tbTue.IsChecked = true;
                }
                if (wed)
                {
                    tbWed.IsChecked = true;
                }
                if (thu)
                {
                    tbThu.IsChecked = true;
                }
                if (fri)
                {
                    tbFri.IsChecked = true;
                    tbFri.Background = Brushes.Blue;
                }
                if (sat)
                {
                    tbSat.IsChecked = true;
                    tbSat.Background = Brushes.Blue;
                }
            }
        }

        private void Timer_Click(object sender, EventArgs e)
        {

            DateTime d;

            d = DateTime.Now;
            var alarmList = fileIOService.LoadData();
            label1.Content = d.Hour + " : " + d.Minute + " : " + d.Second;
            this.MyAlarm.Text = "Day: " + d.Day + " " + d.DayOfWeek + " Month: " + d.Month;

            foreach (var al in alarmList)
            {
                if (d.Hour == al.Hours && d.Second < 1)
                {
                    if (d.Minute == al.Minutes)
                    {
                        AlarmOnSound();
                    }

                }
            }

        }

        public void AlarmOnSound()
        {

            SystemSounds.Asterisk.Play();
            if (alarmSongUrl != "")
            {
                mediaPlayer.Open(new Uri(alarmSongUrl));
                mediaPlayer.Play();

            }
            else
            {
                ChooseSong();
            }

            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0.5;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            this.stop.BeginAnimation(OpacityProperty, da);



        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = VolumeSlider.Value;
        }

        private void ChooseSong()
        {
            if (MediaName.Text != "No Media")
            {

            }
            else if (alarmSongUrl == "")
            {
                MessageBox.Show("Choose song .");


            }
            else
            {

                mediaPlayer.Open(new Uri(alarmSongUrl));
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new OpenFileDialog();
            dialog.Title = "Choose Media";
            if (dialog.ShowDialog() == true)
            {
                alarmSongUrl = dialog.FileName;
                MediaName.Text = dialog.FileName;
            }

        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            AlarmOnSound();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

            mediaPlayer.Stop();
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            //da.RepeatBehavior=new RepeatBehavior(3);
            this.stop.BeginAnimation(OpacityProperty, da);
            this.stop.Opacity = 0;
        }

        private void ListAlarms_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

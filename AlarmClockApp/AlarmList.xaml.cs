using AlarmClockApp.Models;
using AlarmClockApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlarmClockApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AlarmList : Window
    {
        private readonly string path = $"{Environment.CurrentDirectory}\\alarmsList.json";
        public BindingList<Alarm> _alarmsList = new BindingList<Alarm>();
        private FileIOService _fileIOService;
        public AlarmList()
        {
            InitializeComponent();
        }

        private void alarmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void alarmList_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(path);
            try
            {
                _alarmsList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
            dgalarmsList.ItemsSource = _alarmsList;
            _alarmsList.ListChanged += AlarmsList_ListChanged;
        }

        private void AlarmsList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CUWFalcons.Models;
using CUWFalcons.Views;
using CUWFalcons.ViewModels;
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace CUWFalcons.Views
{
    public partial class CalendarPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public CalendarPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }


        private DateTime? _date;
        public DateTime? Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private ObservableCollection<XamForms.Controls.SpecialDate> attendances;
        public ObservableCollection<XamForms.Controls.SpecialDate> Attendances
        {
            get
            {
                return attendances;
            }
            set
            {
                attendances = value;
                OnPropertyChanged(nameof(Attendances));
            }
        }

        public Command DateChosen
        {
            get
            {
                return new Command((obj) =>
                {
                    System.Diagnostics.Debug.WriteLine(obj as DateTime?);
                });
            }
        }
    }

}



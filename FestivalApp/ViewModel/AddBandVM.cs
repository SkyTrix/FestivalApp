using FestivalApp.Model;
using FestivalApp.Model.DAL;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class AddBandVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private Band _band = new Band();
        public Band Band
        {
            get { return _band; }
            set { _band = value; OnPropertyChanged("Band"); }
        }
        
        private GenreManager _genreManager;
        public GenreManager GenreManager
        {
            get
            {
                if (_genreManager == null)
                    _genreManager = GenreManager.Instance;

                return _genreManager;
            }
            set { _genreManager = value; OnPropertyChanged("GenreManager"); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            try
            {
                
                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}

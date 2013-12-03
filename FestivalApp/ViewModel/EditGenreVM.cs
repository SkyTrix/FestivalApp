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
    class EditGenreVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private Genre _genre;
        public Genre Genre
        {
            get { return _genre; }
            set { _genre = value; OnPropertyChanged("Genre"); }
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
                GenreManager.Instance.EditGenre(Genre);

                // We also have to refresh the bands for the changes to be visible
                BandManager.Instance.RefreshData();

                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}

using FestivalApp.Model;
using FestivalApp.Model.DAL;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class EditBandVM : ObservableObject
    {
        private ObservableCollection<CheckBox> _genreCheckBoxes;
        public ObservableCollection<CheckBox> GenreCheckBoxes
        {
            get
            {
                if (_genreCheckBoxes == null)
                {
                    _genreCheckBoxes = new ObservableCollection<CheckBox>();
                    ObservableCollection<Genre> genres = GenreManager.GetGenresForBand(Band);
                    foreach(Genre genre in GenreManager.Genres)
                    {
                        CheckBox box = new CheckBox();
                        box.Content = genre.Name;

                        Genre g = genres.ToList().Find(x => x.ID == genre.ID);
                        if (g != null)
                        {
                            box.IsChecked = true;
                        }

                        _genreCheckBoxes.Add(box);
                    }
                }
                return _genreCheckBoxes;
            }
            set { _genreCheckBoxes = value; OnPropertyChanged(""); }
        }
        

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
                foreach (CheckBox box in GenreCheckBoxes)
                {
                    if (box.IsChecked == true)
                        Console.WriteLine(box.Content);
                }
                //DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}

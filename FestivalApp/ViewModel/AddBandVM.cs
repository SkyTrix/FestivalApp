using DAL;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private ObservableCollection<CheckBox> _genreCheckBoxes;
        public ObservableCollection<CheckBox> GenreCheckBoxes
        {
            get
            {
                if (_genreCheckBoxes == null)
                {
                    _genreCheckBoxes = new ObservableCollection<CheckBox>();

                    foreach (Genre genre in GenreManager.Genres)
                    {
                        CheckBox box = new CheckBox() { Content = genre.Name };
                        _genreCheckBoxes.Add(box);
                    }
                }

                return _genreCheckBoxes;
            }
            set { _genreCheckBoxes = value; OnPropertyChanged("GenreCheckBoxes"); }
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
                BandManager.Instance.AddBand(Band);
                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }

        public ICommand DropCommand
        {
            get { return new RelayCommand<DragEventArgs>(AddImage); }
        }

        private void AddImage(DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data.ContainsFileDropList())
            {
                var files = data.GetFileDropList();
                Band.Picture = GetPhoto(files[0]);
                OnPropertyChanged("Band");
            }
        }

        private byte[] GetPhoto(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            return data;
        }
    }
}

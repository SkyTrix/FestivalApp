using DAL;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
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

                    // Create a checkbox for each genre, check it if band is linked to genre
                    ObservableCollection<Genre> genres = GenreManager.GetGenresForBand(Band);
                    foreach (Genre genre in GenreManager.Genres)
                    {
                        CheckBox box = new CheckBox();
                        box.Content = genre;

                        Genre bandGenre = genres.ToList().Find(x => x.ID == genre.ID);
                        if (bandGenre != null)
                        {
                            box.IsChecked = true;
                        }

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
            get { return new RelayCommand(Save, Band.IsValid); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            try
            {
                // Create a list of checked checkboxes
                List<Genre> genres = new List<Genre>();
                foreach (CheckBox box in GenreCheckBoxes)
                {
                    if (box.IsChecked == true)
                    {
                        genres.Add((Genre)box.Content);
                    }
                }

                GenreManager.Instance.SetGenresForBand(Band, genres);

                BandManager.Instance.EditBand(Band);

                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }

        public ICommand ChooseImageCommand
        {
            get { return new RelayCommand(ChooseImage); }
        }

        private void ChooseImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            string allPictureExtensions = "";

            // Create a filter for all extensions
            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";

                allPictureExtensions += c.FilenameExtension + ";";
            }

            ofd.Filter = String.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, "All Picture Files", allPictureExtensions);
            ofd.FilterIndex = 6;

            ofd.DefaultExt = ".png";

            if (ofd.ShowDialog() == true)
            {
                Band.Picture = GetPhoto(ofd.FileName);
                OnPropertyChanged("Band");
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

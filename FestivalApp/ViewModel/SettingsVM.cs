using DAL;
using FestivalApp.View;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class SettingsVM : ObservableObject, IPage
    {
        private FestivalManager _festivalManager;
        public FestivalManager FestivalManager
        {
            get
            {
                if (_festivalManager == null)
                    _festivalManager = FestivalManager.Instance;
                return _festivalManager;
            }
            set { _festivalManager = value; OnPropertyChanged("FestivalManager"); }
        }

        private StageManager _stageManager;
        public StageManager StageManager
        {
            get
            {
                if (_stageManager == null)
                    _stageManager = StageManager.Instance;

                return _stageManager;
            }
            set { _stageManager = value; OnPropertyChanged("StageManager"); }
        }

        private Stage _selectedStage;
        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set
            {
                _selectedStage = value;
                _selectedStageUsedInLineUp = false;

                if (_selectedStage != null)
                {
                    _selectedStageUsedInLineUp = StageManager.StageUsedInLineup(_selectedStage);
                }

                OnPropertyChanged("SelectedStage");
            }
        }

        private bool _selectedStageUsedInLineUp = false;

        private string _stage = string.Empty;
        public string Stage
        {
            get { return _stage; }
            set { _stage = value; OnPropertyChanged("Stage"); }
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

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; OnPropertyChanged("SelectedGenre"); }
        }

        private string _genre = string.Empty;
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; OnPropertyChanged("Genre"); }
        }

        public string Name
        {
            get { return "Instellingen"; }
        }

        public ICommand LoadedCommand
        {
            get { return new RelayCommand(Loaded); }
        }

        private void Loaded()
        {
            // Selected stage could have been used in lineup since we last opened settings
            if (SelectedStage != null)
            {
                _selectedStageUsedInLineUp = StageManager.StageUsedInLineup(SelectedStage);
            }
        }

        public ICommand AddStageCommand
        {
            get { return new RelayCommand(AddStage, CanAddStage); }
        }

        private bool CanAddStage()
        {
            return Stage != null && Stage.Length > 2;
        }

        private void AddStage()
        {
            try
            {
                Stage stage = new Stage();
                stage.Name = Stage;
                StageManager.Instance.AddStage(stage);
                Stage = string.Empty;
            }
            catch (Exception)
            {
            }
        }

        public ICommand EditStageCommand
        {
            get { return new RelayCommand(EditStage, CanEditStage); }
        }

        private bool CanEditStage()
        {
            return SelectedStage != null;
        }

        private void EditStage()
        {
            EditStageWindow window = new EditStageWindow();
            ((EditStageVM)window.DataContext).Stage = SelectedStage.Copy();
            window.ShowDialog();
        }

        public ICommand DeleteStageCommand
        {
            get { return new RelayCommand(DeleteStage, CanDeleteStage); }
        }

        private bool CanDeleteStage()
        {
            return SelectedStage != null && !_selectedStageUsedInLineUp;
        }

        private void DeleteStage()
        {
            try
            {
                StageManager.DeleteStage(SelectedStage);
            }
            catch (Exception)
            {
            }
        }

        public ICommand AddGenreCommand
        {
            get { return new RelayCommand(AddGenre, CanAddGenre); }
        }

        private bool CanAddGenre()
        {
            return Genre != null && Genre.Length > 2;
        }

        private void AddGenre()
        {
            try
            {
                Genre genre = new Genre();
                genre.Name = Genre;
                GenreManager.Instance.AddGenre(genre);
                Genre = string.Empty;
            }
            catch (Exception)
            {
            }
        }

        public ICommand EditGenreCommand
        {
            get { return new RelayCommand(EditGenre, CanEditGenre); }
        }

        private bool CanEditGenre()
        {
            return SelectedGenre != null;
        }

        private void EditGenre()
        {
            EditGenreWindow window = new EditGenreWindow();
            ((EditGenreVM)window.DataContext).Genre = SelectedGenre.Copy();
            window.ShowDialog();
        }

        public ICommand DeleteGenreCommand
        {
            get { return new RelayCommand(DeleteGenre, CanDeleteGenre); }
        }

        private bool CanDeleteGenre()
        {
            return SelectedGenre != null;
        }

        private void DeleteGenre()
        {
            GenreManager.Instance.DeleteGenre(SelectedGenre);

            // We also have to refresh the bands for the changes to be visible
            BandManager.Instance.RefreshData();
        }
    }
}

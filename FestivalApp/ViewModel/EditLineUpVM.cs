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
    class EditLineUpVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }
        
        private LineUpItem _lineUpItem;
        public LineUpItem LineUpItem
        {
            get { return _lineUpItem; }
            set
            {
                _lineUpItem = value;
                OnPropertyChanged("LineUpItem");
            }
        }

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

        private BandManager _bandManager;
        public BandManager BandManager
        {
            get
            {
                if (_bandManager == null)
                    _bandManager = BandManager.Instance;

                return _bandManager;
            }
            set { _bandManager = value; OnPropertyChanged("BandManager"); }
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
                LineUpManager.Instance.EditLineUpItem(LineUpItem);
                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een fout opgetreden tijdens het toevoegen van een lineup item.", "Fout bij toevoegen");
            }
        }
    }
}

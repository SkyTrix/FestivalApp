using DAL;
using GalaSoft.MvvmLight.Command;
using Models;
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
        
        private LineUpItem _lineUpItem = new LineUpItem();
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

        private string _lineUpError;
        public string LineUpError
        {
            get { return _lineUpError; }
            set { _lineUpError = value; OnPropertyChanged("LineUpError"); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, LineUpItem.IsValid); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            // Check if end time is later than start time
            DateTime startTime = LineUpItem.DateAndTimeStringToDateTime(LineUpItem.Date, LineUpItem.StartTime);
            DateTime endTime = LineUpItem.DateAndTimeStringToDateTime(LineUpItem.Date, LineUpItem.EndTime);
            if (endTime <= startTime)
            {
                LineUpError = "Fout bij wijzigen: eindtijd moet later zijn dan starttijd.";
                return;
            }

            // Check if lineupitem overlaps with existing ones on the same stage
            if (LineUpManager.Instance.LineUpItemOverlapsWithExistingItems(LineUpItem))
            {
                LineUpError = "Fout bij wijzigen: tijdsslot overlapt met een reeds toegevoegd tijdsslot op deze stage.";
                return;
            }

            // Remove possible error message
            LineUpError = null;

            try
            {
                LineUpManager.Instance.EditLineUpItem(LineUpItem);
                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}

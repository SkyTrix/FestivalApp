using DAL;
using FestivalApp.View;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class LineUpVM : ObservableObject, IPage
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

        private LineUpManager _lineUpManager;
        public LineUpManager LineUpManager
        {
            get
            {
                if (_lineUpManager == null)
                    _lineUpManager = LineUpManager.Instance;

                return _lineUpManager;
            }
            set { _lineUpManager = value; OnPropertyChanged("LineUpManager"); }
        }

        private LineUpItem _selectedLineUpItem;
        public LineUpItem SelectedLineUpItem
        {
            get { return _selectedLineUpItem; }
            set { _selectedLineUpItem = value; OnPropertyChanged("SelectedLineUpItem"); }
        }

        private DateTime _selectedFestivalDate;
        public DateTime SelectedFestivalDate
        {
            get { return _selectedFestivalDate; }
            set { _selectedFestivalDate = value; OnPropertyChanged("SelectedFestivalDate"); }
        }

        private DateTime _selectedFilterFestivalDate;
        public DateTime SelectedFilterFestivalDate
        {
            get { return _selectedFilterFestivalDate; }
            set { _selectedFilterFestivalDate = value; OnPropertyChanged("SelectedFilterFestivalDate"); }
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
            set { _selectedStage = value; OnPropertyChanged("SelectedStage"); }
        }

        private Stage _selectedFilterStage;
        public Stage SelectedFilterStage
        {
            get { return _selectedFilterStage; }
            set { _selectedFilterStage = value; OnPropertyChanged("SelectedFilterStage"); }
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

        private Band _selectedBand;
        public Band SelectedBand
        {
            get { return _selectedBand; }
            set
            { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
        }

        private string _startTime = string.Empty;
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged("StartTime"); }
        }

        private string _endTime = string.Empty;
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged("EndTime"); }
        }

        private bool _addingBand = false;

        public LineUpVM()
        {
            try
            {
                // Startup with some default selected values
                SelectedFestivalDate = FestivalManager.Instance.Festival.FestivalDates[0];
                SelectedStage = StageManager.Instance.Stages[0];
                SelectedBand = BandManager.Instance.Bands[0];

                // Observe changes to bands so we can keep the selected item selected after updating
                BandManager.Instance.PropertyChanged += BandManager_PropertyChanged;
            }
            catch (Exception)
            {
            }
        }

        void BandManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Bands")
            {
                // show new band if we added one, previously selected one if we didn't add one
                var selected = _addingBand ? BandManager.Bands.Last() : SelectedBand;
                OnPropertyChanged("BandManager");
                SelectedBand = selected;
                _addingBand = false;
            }
        }

        public string Name
        {
            get { return "Line-up"; }
        }

        public ICommand AddLineUpItemCommand
        {
            get { return new RelayCommand(AddLineUpItem, CanAddLineUpItem); }
        }

        private bool CanAddLineUpItem()
        {
            return SelectedFestivalDate != null && SelectedStage != null && SelectedBand != null && StartTime.Length == 5 && EndTime.Length == 5;
        }

        private void AddLineUpItem()
        {
            LineUpItem item = new LineUpItem();
            item.Date = SelectedFestivalDate;
            item.StartTime = StartTime;
            item.EndTime = EndTime;
            item.Band = SelectedBand;
            item.Stage = SelectedStage;

            try
            {
                LineUpManager.Instance.AddLineUpItem(item);
            }
            catch (Exception)
            {
            }
        }

        public ICommand EditLineUpItemCommand
        {
            get { return new RelayCommand(EditLineUpItem, CanEditOrDeleteLineUpItem); }
        }

        public ICommand DeleteLineUpItemCommand
        {
            get { return new RelayCommand(DeleteLineUpItem, CanEditOrDeleteLineUpItem); }
        }

        private bool CanEditOrDeleteLineUpItem()
        {
            return SelectedLineUpItem != null;
        }

        private void EditLineUpItem()
        {
            EditLineUpWindow window = new EditLineUpWindow();
            ((EditLineUpVM)window.DataContext).LineUpItem = SelectedLineUpItem.Copy();
            window.ShowDialog();
        }

        private void DeleteLineUpItem()
        {
            try
            {
                LineUpManager.Instance.DeleteLineUpItem(SelectedLineUpItem);
            }
            catch (Exception)
            {
            }
        }

        public ICommand AddBandCommand
        {
            get { return new RelayCommand(AddBand); }
        }

        private void AddBand()
        {
            _addingBand = true;

            BandWindow window = new BandWindow();
            window.DataContext = new AddBandVM();
            window.ShowDialog();
        }

        public ICommand EditBandCommand
        {
            get { return new RelayCommand(EditBand, CanEditBand); }
        }

        private bool CanEditBand()
        {
            return SelectedBand != null;
        }

        private void EditBand()
        {
            BandWindow window = new BandWindow();
            EditBandVM viewModel = new EditBandVM();
            viewModel.Band = SelectedBand.Copy();
            window.DataContext = viewModel;
            window.Title = "Band wijzigen";
            window.ShowDialog();
        }
    }
}

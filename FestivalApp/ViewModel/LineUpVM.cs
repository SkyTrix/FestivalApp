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

        private ObservableCollection<LineUpItem> _filteredLineUpItems;
        public ObservableCollection<LineUpItem> FilteredLineUpItems
        {
            get
            {
                if (_filteredLineUpItems == null)
                {
                    UpdateLineUpFilter();
                }

                return _filteredLineUpItems;
            }
            set { _filteredLineUpItems = value; OnPropertyChanged("FilteredLineUpItems"); }
        }

        private LineUpItem _lineUpItem = new LineUpItem();
        public LineUpItem LineUpItem
        {
            get { return _lineUpItem; }
            set { _lineUpItem = value; OnPropertyChanged("LineUpItem"); }
        }

        private LineUpItem _selectedLineUpItem;
        public LineUpItem SelectedLineUpItem
        {
            get { return _selectedLineUpItem; }
            set { _selectedLineUpItem = value; OnPropertyChanged("SelectedLineUpItem"); }
        }

        public ObservableCollection<object> FilterFestivalDates
        {
            get
            {
                // Create a collection of all festival dates
                object[] dates = Array.ConvertAll(FestivalManager.Festival.FestivalDates.ToArray(), x => (object)x);
                ObservableCollection<object> festivalDates = new ObservableCollection<object>(dates);

                festivalDates.Insert(0, "All");

                return festivalDates;
            }
        }

        private object _selectedFilterFestivalDate;
        public object SelectedFilterFestivalDate
        {
            get { return _selectedFilterFestivalDate; }
            set { _selectedFilterFestivalDate = value; OnPropertyChanged("SelectedFilterFestivalDate"); UpdateLineUpFilter(); }
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

        private ObservableCollection<Stage> _filterStages;
        public ObservableCollection<Stage> FilterStages
        {
            get
            {
                if (_filterStages == null)
                {
                    _filterStages = new ObservableCollection<Stage>(StageManager.Instance.Stages);
                    _filterStages.Insert(0, new Stage { ID = -2, Name = "All" });
                }

                return _filterStages;
            }
            set { _filterStages = value; OnPropertyChanged("FilterStages"); }
        }

        private Stage _selectedFilterStage;
        public Stage SelectedFilterStage
        {
            get { return _selectedFilterStage; }
            set { _selectedFilterStage = value; OnPropertyChanged("SelectedFilterStage"); UpdateLineUpFilter(); }
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

        private string _lineUpError;
        public string LineUpError
        {
            get { return _lineUpError; }
            set { _lineUpError = value; OnPropertyChanged("LineUpError"); }
        }

        private bool _addingBand = false;
        private bool _addingLineUpItem = false;

        public LineUpVM()
        {
            try
            {
                // Startup with some default selected values
                LineUpItem.Date = FestivalManager.Instance.Festival.FestivalDates.FirstOrDefault();
                LineUpItem.Stage = StageManager.Instance.Stages.FirstOrDefault();
                LineUpItem.Band = BandManager.Instance.Bands.FirstOrDefault();
                LineUpItem.StartTime = "18:00";
                LineUpItem.EndTime = "18:45";
                SelectedFilterFestivalDate = "All";
                SelectedFilterStage = FilterStages.ToList().Find(x => x.ID == -2);

                // Observe changes to bands so we can keep the selected item selected after updating
                BandManager.Instance.PropertyChanged += BandManager_PropertyChanged;

                // Observe lineup changes
                LineUpManager.Instance.PropertyChanged += LineUpManager_PropertyChanged;

                // Observe stage changes
                StageManager.Instance.PropertyChanged += StageManager_PropertyChanged;
            }
            catch (Exception)
            {
            }
        }

        void BandManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Bands")
            {
                // Show new band if we added one, previously selected one if we didn't add one
                var selected = _addingBand ? BandManager.Bands.Last() : LineUpItem.Band;
                OnPropertyChanged("BandManager");
                LineUpItem.Band = selected;
                OnPropertyChanged("LineUpItem");
                _addingBand = false;
            }
        }

        void LineUpManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LineUpItems")
            {
                UpdateLineUpFilter();

                if (_addingLineUpItem)
                {
                    LineUpItem newItem = LineUpManager.LineUpItems.Last();

                    if(SelectedFilterStage != null)
                    {
                        // Change stage filter of newly added lineup item doesn't conform to it
                        if (newItem.Stage.ID != SelectedFilterStage.ID && SelectedFilterStage.ID != -2)
                        {
                            SelectedFilterStage = FilterStages.ToList().Find(x => x.ID == newItem.Stage.ID);
                        }
                    }

                    if (SelectedFilterFestivalDate != null && !SelectedFilterFestivalDate.Equals("All"))
                    {
                        // Change date filter of newly added lineup item doesn't conform to it
                        if (newItem.Date != (DateTime)SelectedFilterFestivalDate)
                        {
                            SelectedFilterFestivalDate = newItem.Date;
                        }
                    }

                    _addingLineUpItem = false;
                }
            }
        }

        void StageManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Stages")
            {
                // Force refresh filter stages
                FilterStages = null;

                // If selected stage no longer exists, select first one
                if (LineUpItem.Stage == null || StageManager.Instance.Stages.ToList().Find(x => x.ID == LineUpItem.Stage.ID) == null)
                    LineUpItem.Stage = StageManager.Instance.Stages.FirstOrDefault();

                // If selected filter stage no longer exists, select "All"
                if (SelectedFilterStage != null && StageManager.Instance.Stages.ToList().Find(x => x.ID == SelectedFilterStage.ID) == null)
                    SelectedFilterStage = FilterStages.ToList().Find(x => x.ID == -2);
            }
        }

        private void UpdateLineUpFilter()
        {
            DateTime? filterDate = SelectedFilterFestivalDate as DateTime?;

            if (filterDate != null && SelectedFilterStage != null && SelectedFilterStage.ID != -2)
            {
                FilteredLineUpItems = new ObservableCollection<LineUpItem>(LineUpManager.LineUpItems.ToList().FindAll(x => x.Stage.ID == SelectedFilterStage.ID && x.Date.Equals(SelectedFilterFestivalDate)));
            }
            else if (filterDate != null)
            {
                FilteredLineUpItems = new ObservableCollection<LineUpItem>(LineUpManager.LineUpItems.ToList().FindAll(x => x.Date.Equals(SelectedFilterFestivalDate)));
            }
            else if (SelectedFilterStage != null && SelectedFilterStage.ID != -2)
            {
                FilteredLineUpItems = new ObservableCollection<LineUpItem>(LineUpManager.LineUpItems.ToList().FindAll(x => x.Stage.ID == SelectedFilterStage.ID));
            }
            else
            {
                FilteredLineUpItems = new ObservableCollection<LineUpItem>(LineUpManager.LineUpItems);
            }
        }

        public string Name
        {
            get { return "Line-up"; }
        }

        public ICommand AddLineUpItemCommand
        {
            get { return new RelayCommand(AddLineUpItem, LineUpItem.IsValid); }
        }

        private void AddLineUpItem()
        {
            // Check if end time is later than start time
            DateTime startTime = LineUpItem.DateAndTimeStringToDateTime(LineUpItem.Date, LineUpItem.StartTime);
            DateTime endTime = LineUpItem.DateAndTimeStringToDateTime(LineUpItem.Date, LineUpItem.EndTime);
            if (endTime <= startTime)
            {
                LineUpError = "Fout bij toevoegen: eindtijd moet later zijn dan starttijd.";
                return;
            }

            // Check if lineupitem overlaps with existing ones on the same stage
            if (LineUpManager.LineUpItemOverlapsWithExistingItems(LineUpItem))
            {
                LineUpError = "Fout bij toevoegen: tijdsslot overlapt met een reeds toegevoegd tijdsslot op deze stage.";
                return;
            }

            // Remove possible error message
            LineUpError = string.Empty;

            _addingLineUpItem = true;

            try
            {
                LineUpManager.Instance.AddLineUpItem(LineUpItem);
            }
            catch (Exception)
            {
                _addingLineUpItem = false;
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
            EditLineUpVM viewModel = new EditLineUpVM();
            viewModel.LineUpItem = SelectedLineUpItem.Copy();
            window.DataContext = viewModel;
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
            if (window.ShowDialog() == false)
            {
                _addingBand = false;
            }
        }

        public ICommand EditBandCommand
        {
            get { return new RelayCommand(EditBand, CanEditBand); }
        }

        private bool CanEditBand()
        {
            return LineUpItem.Band != null;
        }

        private void EditBand()
        {
            BandWindow window = new BandWindow();
            EditBandVM viewModel = new EditBandVM();
            viewModel.Band = LineUpItem.Band.Copy();
            window.DataContext = viewModel;
            window.Title = "Band wijzigen";
            window.ShowDialog();
        }
    }
}

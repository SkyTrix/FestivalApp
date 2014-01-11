﻿using DAL;
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

        public ObservableCollection<object> FilterFestivalDates
        {
            get
            {
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

        private Band _selectedBand;
        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
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
        private bool _addingLineUpItem = false;

        public LineUpVM()
        {
            try
            {
                // Startup with some default selected values
                SelectedFestivalDate = FestivalManager.Instance.Festival.FestivalDates.FirstOrDefault();
                SelectedStage = StageManager.Instance.Stages.FirstOrDefault();
                SelectedBand = BandManager.Instance.Bands.FirstOrDefault();
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
                var selected = _addingBand ? BandManager.Bands.Last() : SelectedBand;
                OnPropertyChanged("BandManager");
                SelectedBand = selected;
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
                        if (newItem.Stage.ID != SelectedFilterStage.ID && SelectedFilterStage.ID != -2)
                        {
                            SelectedFilterStage = FilterStages.ToList().Find(x => x.ID == newItem.Stage.ID);
                        }
                    }

                    if (SelectedFilterFestivalDate != null && !SelectedFilterFestivalDate.Equals("All"))
                    {
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
                if (StageManager.Instance.Stages.ToList().Find(x => x.ID == SelectedStage.ID) == null)
                    SelectedStage = StageManager.Instance.Stages.FirstOrDefault();

                // If selected filter stage no longer exists, select "All"
                if (StageManager.Instance.Stages.ToList().Find(x => x.ID == SelectedFilterStage.ID) == null)
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
            get { return new RelayCommand(AddLineUpItem, CanAddLineUpItem); }
        }

        private bool CanAddLineUpItem()
        {
            return SelectedFestivalDate != null && SelectedStage != null && SelectedBand != null && StartTime.Length == 5 && EndTime.Length == 5;
        }

        private void AddLineUpItem()
        {
            _addingLineUpItem = true;

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

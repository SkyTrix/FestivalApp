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
    class EditStageVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private Stage _stage;
        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; OnPropertyChanged("Stage"); }
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
                StageManager.Instance.EditStage(Stage);

                // We also have to refresh the lineup for the changes to be visible
                LineUpManager.Instance.RefreshData();

                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}

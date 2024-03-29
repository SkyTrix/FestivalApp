﻿using DAL;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class AddTicketTypeVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private TicketType _ticketType = new TicketType();
        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; OnPropertyChanged("TicketType"); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, TicketType.IsValid); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            try
            {
                TicketTypeManager.Instance.AddTicketType(TicketType);

                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}

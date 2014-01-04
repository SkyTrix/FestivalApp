using DAL;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            _pages = new ObservableCollection<IPage>();

            // Add our IPage compliant VM's
            _pages.Add(new LineUpVM());
            _pages.Add(new ContactsVM());
            _pages.Add(new TicketingVM());
            _pages.Add(new SettingsVM());

            // Select first page
            _currentPage = Pages[0];
        }

        private IPage _currentPage;
        public IPage CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private ObservableCollection<IPage> _pages;
        public ObservableCollection<IPage> Pages
        {
            get { return _pages; }
            set { _pages = value; OnPropertyChanged("Pages"); }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private void ChangePage(IPage page)
        {
            // Save festival settings when changing from settings page
            if (CurrentPage.GetType() == typeof(SettingsVM))
            {
                SaveFestival();
            }

            CurrentPage = page;
        }

        public ICommand ClosedCommand
        {
            get { return new RelayCommand(SaveFestival); }
        }

        private void SaveFestival()
        {
            try
            {
                FestivalManager.Instance.SaveFestival();
            }
            catch (Exception)
            {
            }
        }
    }
}

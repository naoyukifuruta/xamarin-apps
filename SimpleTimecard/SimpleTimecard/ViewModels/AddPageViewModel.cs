using System;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class AddPageViewModel : ViewModelBase
    {
        private DateTime _selectedEntryDate;
        public DateTime SelectedEntryDate
        {
            get { return _selectedEntryDate; }
            set
            {
                SetProperty(ref _selectedEntryDate, value);
            }
        }

        private TimeSpan _selectedStartTime;
        public TimeSpan SelectedStartTime
        {
            get { return _selectedStartTime; }
            set
            {
                SetProperty(ref _selectedStartTime, value);
            }
        }

        private TimeSpan _selectedEndTime;
        public TimeSpan SelectedEndTime
        {
            get { return _selectedEndTime; }
            set
            {
                SetProperty(ref _selectedEndTime, value);
            }
        }

        public DelegateCommand EntryButtonCommand { get; set; }
        public DelegateCommand CancelButtonCommand { get; set; }

        public AddPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "追加";

            SelectedEntryDate = DateTime.Now;
            SelectedStartTime = new TimeSpan();
            SelectedEndTime = new TimeSpan();

            EntryButtonCommand = new DelegateCommand(Entry);
            CancelButtonCommand = new DelegateCommand(Cancel);
        }

        private async void Entry()
        {
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                var addedTimecard = realm.Add<Timecard>(new Timecard()
                {
                    StartTime = SelectedEntryDate,
                    StartTimeString = string.Format($"{SelectedStartTime.Hours.ToString("D2")}:{SelectedStartTime.Minutes.ToString("D2")}"),
                    EndTime = SelectedEntryDate,
                    EndTimeString = string.Format($"{SelectedEndTime.Hours.ToString("D2")}:{SelectedEndTime.Minutes.ToString("D2")}"),
                });
            });

            await base.NavigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            await base.NavigationService.GoBackAsync();
        }
    }
}

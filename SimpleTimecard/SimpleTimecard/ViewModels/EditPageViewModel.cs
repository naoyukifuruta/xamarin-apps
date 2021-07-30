using System;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        private DateTime _selectedEntryDate;
        public DateTime SelectedEntryDate
        {
            get { return _selectedEntryDate; }
            set { SetProperty(ref _selectedEntryDate, value); }
        }

        private TimeSpan _selectedStartTime;
        public TimeSpan SelectedStartTime
        {
            get { return _selectedStartTime; }
            set { SetProperty(ref _selectedStartTime, value); }
        }

        private TimeSpan _selectedEndTime;
        public TimeSpan SelectedEndTime
        {
            get { return _selectedEndTime; }
            set { SetProperty(ref _selectedEndTime, value); }
        }

        public DelegateCommand UpdateButtonCommand { get; set; }
        public DelegateCommand CancelButtonCommand { get; set; }

        private string _timecardId = string.Empty;

        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectedEntryDate = DateTime.Now;
            SelectedStartTime = new TimeSpan();
            SelectedEndTime = new TimeSpan();

            UpdateButtonCommand = new DelegateCommand(Update);
            CancelButtonCommand = new DelegateCommand(Cancel);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var val = parameters[nameof(Timecard)] as Timecard;
            if (val == null)
            {
                throw new ArgumentException(nameof(Timecard));
            }

            _timecardId = val.TimecardId;

            Title = val.StartTime.Value.DateTime.ToString("yyyy/MM/dd ddd");
            SelectedStartTime = TimeSpan.Parse(val.StartTimeString);
            SelectedEndTime = TimeSpan.Parse(val.EndTimeString);
        }

        private async void Update()
        {
            var realm = Realm.GetInstance();

            // 更新
            var timecard = realm.Find<Timecard>(_timecardId);
            realm.Write(() => {
                timecard.StartTime = SelectedEntryDate;
                timecard.StartTimeString = string.Format($"{SelectedStartTime.Hours.ToString("D2")}:{SelectedStartTime.Minutes.ToString("D2")}");
                timecard.EndTime = SelectedEntryDate;
                timecard.EndTimeString = string.Format($"{SelectedEndTime.Hours.ToString("D2")}:{SelectedEndTime.Minutes.ToString("D2")}");
                realm.Add<Timecard>(timecard, update: true);
            });

            await base.NavigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            await base.NavigationService.GoBackAsync();
        }
    }
}

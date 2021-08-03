using System;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        private string _timecardId = string.Empty;

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

        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectedEntryDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            SelectedStartTime = new TimeSpan();
            SelectedEndTime = new TimeSpan();
        }

        // 画面遷移時
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var val = parameters[nameof(Timecard)] as Timecard;
            if (val == null)
            {
                throw new ArgumentException(nameof(Timecard));
            } 

            _timecardId = val.TimecardId;

            Title = val.EntryDate.HasValue ? val.EntryDate.Value.DateTime.ToString("yyyy/MM/dd ddd") : string.Empty;
            SelectedStartTime = TimeSpan.Parse(val.StartTimeString);
            SelectedEndTime = TimeSpan.Parse(val.EndTimeString);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public Command OnClickUpdate => new Command(async () =>
        {
            var realm = Realm.GetInstance();

            var timecard = realm.Find<Timecard>(_timecardId);
            realm.Write(() => {
                timecard.EntryDate = DateTime.SpecifyKind(SelectedEntryDate, DateTimeKind.Local);
                timecard.StartTimeString = GetInputStartTime();
                timecard.EndTimeString = GetInputEndTime();
                realm.Add<Timecard>(timecard, update: true);
            });

            await base.NavigationService.GoBackAsync();
        });

        /// <summary>
        /// キャンセル（前の画面に戻る）
        /// </summary>
        public Command OnClickCancel => new Command(async () =>
        {
            await base.NavigationService.GoBackAsync();
        });

        // 出勤時間を取得(HH:mm)
        private string GetInputStartTime()
        {
            return string.Format($"{SelectedStartTime.Hours.ToString("D2")}:{SelectedStartTime.Minutes.ToString("D2")}");
        }

        // 退勤時間を取得(HH:mm)
        private string GetInputEndTime()
        {
            return string.Format($"{SelectedEndTime.Hours.ToString("D2")}:{SelectedEndTime.Minutes.ToString("D2")}");
        }
    }
}

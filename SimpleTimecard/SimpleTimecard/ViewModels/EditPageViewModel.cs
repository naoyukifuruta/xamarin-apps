using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Reactive.Bindings;
using Realms;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        private string _timecardId = string.Empty;

        public ReactiveProperty<DateTime> SelectedEntryDate { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<TimeSpan> SelectedStartTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> SelectedEndTime { get; } = new ReactiveProperty<TimeSpan>();

        public AsyncReactiveCommand UpdateCommand { get; } = new AsyncReactiveCommand();

        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectedEntryDate.Value = DateTime.Now;
            SelectedStartTime.Value = new TimeSpan();
            SelectedEndTime.Value = new TimeSpan();

            UpdateCommand.Subscribe(_ => Update());
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

            Title = val.EntryDate.HasValue ? val.EntryDate.Value.ToLocalTime().DateTime.ToString("yyyy/MM/dd ddd") : string.Empty;
            SelectedStartTime.Value = TimeSpan.Parse(val.StartTimeString);
            SelectedEndTime.Value = TimeSpan.Parse(val.EndTimeString);
        }

        public async Task Update()
        {
            var realm = Realm.GetInstance();

            var timecard = realm.Find<Timecard>(_timecardId);
            realm.Write(() => {
                timecard.StartTimeString = GetInputStartTime();
                timecard.EndTimeString = GetInputEndTime();
                realm.Add<Timecard>(timecard, update: true);
            });

            await base.NavigationService.GoBackAsync();
        }

        // 出勤時間を取得(HH:mm)
        private string GetInputStartTime()
        {
            return string.Format($"{SelectedStartTime.Value.Hours.ToString("D2")}:{SelectedStartTime.Value.Minutes.ToString("D2")}");
        }

        // 退勤時間を取得(HH:mm)
        private string GetInputEndTime()
        {
            return string.Format($"{SelectedEndTime.Value.Hours.ToString("D2")}:{SelectedEndTime.Value.Minutes.ToString("D2")}");
        }
    }
}

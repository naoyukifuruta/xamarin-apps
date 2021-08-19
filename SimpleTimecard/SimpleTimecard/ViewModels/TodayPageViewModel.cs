using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Realms;
using SimpleTimecard.Common;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class TodayPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;

        private string _todayTimecardId;

        public ReactiveProperty<string> NowDateTime { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> StampingStartTime { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> StampingEndTime { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand RegisterStartTimeCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand RegisterEndTimeCommand { get; } = new AsyncReactiveCommand();

        public TodayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Today";
            _pageDialogService = pageDialogService;

            RegisterStartTimeCommand.Subscribe(_ => RegisterStartTime());
            RegisterEndTimeCommand.Subscribe(_ => RegisterEndTime());

            // 現在日時
            new Task(async () =>
            {
                while (true)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        NowDateTime.Value = DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss")
                    );
                    await Task.Delay(1000);
                }
            }).Start();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Logger.Trace();
            base.Initialize(parameters);
        }

        public override void OnAppearing()
        {
            Logger.Trace();
            base.OnAppearing();

            var todayTimecard = FetchTodayTimecard();
            _todayTimecardId = todayTimecard.TimecardId;

            StampingStartTime.Value = todayTimecard.StartTimeString;
            StampingEndTime.Value = todayTimecard.EndTimeString;
        }

        // 出勤時間登録
        public async Task RegisterStartTime()
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "出勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var timecard = RegisteredTimecard(RegistType.StartTime);
            StampingStartTime.Value = timecard.StartTimeString;
        }

        // 退勤時間登録
        public async Task RegisterEndTime()
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "退勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var timecard = RegisteredTimecard(RegistType.EndTime);
            StampingEndTime.Value = timecard.EndTimeString;
        }

        // 今日の出退勤を取得
        private Timecard FetchTodayTimecard()
        {
            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            Func<Timecard, bool> predicate = timecard => timecard.EntryDate.Value.ToLocalTime().ToString("yyyyMMdd") == DateTime.Now.ToLocalTime().ToString("yyyyMMdd");

            if (allTimecards.Where(predicate).Any())
            {
                return allTimecards.Where(predicate).First();
            }

            return new Timecard()
            {
                TimecardId = string.Empty,
            };
        }

        private enum RegistType
        {
            StartTime = 0,
            EndTime,
        }

        // 出退勤時間の登録
        private Timecard RegisteredTimecard(RegistType registType)
        {
            var entryDateTime = DateTime.Now.ToLocalTime();
            var realm = Realm.GetInstance();

            if (string.IsNullOrEmpty(_todayTimecardId))
            {
                // 新規登録
                var addTimecard = new Timecard()
                {
                    EntryDate = entryDateTime,
                };
                if (registType == RegistType.StartTime)
                {
                    addTimecard.StartTimeString = entryDateTime.ToString("HH:mm");
                }
                else
                {
                    addTimecard.EndTimeString = entryDateTime.ToString("HH:mm");
                }

                realm.Write(() =>
                {
                    var addedTimecard = realm.Add<Timecard>(addTimecard);
                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                if (registType == RegistType.StartTime)
                {
                    
                    realm.Write(() => {
                        timecard.StartTimeString = entryDateTime.ToString("HH:mm");
                        realm.Add<Timecard>(timecard, update: true);
                    });
                }
                else
                {
                    realm.Write(() => {
                        timecard.EndTimeString = entryDateTime.ToString("HH:mm");
                        realm.Add<Timecard>(timecard, update: true);
                    });
                }
            }

            return realm.Find<Timecard>(_todayTimecardId);
        }
    }
}

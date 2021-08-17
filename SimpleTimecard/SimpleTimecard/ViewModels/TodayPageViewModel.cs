using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Realms;
using SimpleTimecard.Common;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class TodayPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;

        private string _todayTimecardId;

        private string _stampingStartTimeLabelText = string.Empty;
        public string StampingStartTimeLabelText
        {
            get { return _stampingStartTimeLabelText; }
            set { SetProperty(ref _stampingStartTimeLabelText, value); }
        }

        private string _stampingEndTimeLabelText = string.Empty;
        public string StampingEndTimeLabelText
        {
            get { return _stampingEndTimeLabelText; }
            set { SetProperty(ref _stampingEndTimeLabelText, value); }
        }

        private string _nowDateTimeString;
        public string NowDateTimeString
        {
            get { return _nowDateTimeString; }
            set { SetProperty(ref _nowDateTimeString, value); }
        }

        public TodayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Today";
            _pageDialogService = pageDialogService;

            // 現在日時
            new Task(async () =>
            {
                while (true)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        NowDateTimeString = DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss")
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

            // 今日分の表示
            var todayTimecard = FetchTodayTimecard();
            _todayTimecardId = todayTimecard.TimecardId;
            StampingStartTimeLabelText = todayTimecard.StartTimeString;
            StampingEndTimeLabelText = todayTimecard.EndTimeString;
        }

        /// <summary>
        /// 出勤登録
        /// </summary>
        public Command OnClickStartTimeEntry => new Command(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "出勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var timecard = RegisteredTimecard(RegistType.StartTime);
            StampingStartTimeLabelText = timecard.StartTimeString;
        });

        /// <summary>
        /// 退勤登録
        /// </summary>
        public Command OnClickEndTimeEntry => new Command(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "退勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var timecard = RegisteredTimecard(RegistType.EndTime);
            StampingEndTimeLabelText = timecard.EndTimeString;
        });

        /// <summary>
        /// 今日の出退勤時間を取得
        /// </summary>
        /// <returns>今日のタイムカード</returns>
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

        /// <summary>
        /// 出退勤時間の登録
        /// </summary>
        /// <param name="registType">登録種別</param>
        /// <returns>タイムカード</returns>
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

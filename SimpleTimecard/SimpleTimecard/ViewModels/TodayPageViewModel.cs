using System;
using System.Linq;
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

        public TodayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Today";
            _pageDialogService = pageDialogService;
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

            // 当日分の取得
            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            // TODO: ここの処理はあとで見直したい
            if (allTimecards.Where(x => x.EntryDate.Value.ToLocalTime().ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).Any())
            {
                var todayTimecard = allTimecards.Where(x => x.EntryDate.Value.ToLocalTime().ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).First();
                _todayTimecardId = todayTimecard.TimecardId;

                if (!string.IsNullOrEmpty(todayTimecard.StartTimeString))
                {
                    StampingStartTimeLabelText = todayTimecard.StartTimeString;
                }

                if (!string.IsNullOrEmpty(todayTimecard.EndTimeString))
                {
                    StampingEndTimeLabelText = todayTimecard.EndTimeString;
                }
            }
            else
            {
                _todayTimecardId = string.Empty;
                StampingStartTimeLabelText = string.Empty;
                StampingEndTimeLabelText = string.Empty;
            }
        }

        public Command OnClickStartTimeEntry => new Command(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "出勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            // TODO: あとでリファクタリングする

            var entryDateTime = DateTime.Now.ToLocalTime();
            var realm = Realm.GetInstance();

            if (string.IsNullOrEmpty(_todayTimecardId))
            {
                // 新規登録
                realm.Write(() =>
                {
                    var addedTimecard = realm.Add<Timecard>(new Timecard()
                    {
                        EntryDate = entryDateTime,
                        StartTimeString = entryDateTime.ToString("HH:mm"),
                    });

                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.StartTimeString = entryDateTime.ToString("HH:mm");
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            StampingStartTimeLabelText = entryDateTime.ToString("HH:mm");
        });

        public Command OnClickEndTimeEntry => new Command(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "退勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var entryDateTime = DateTime.Now.ToLocalTime();
            var realm = Realm.GetInstance();

            if (string.IsNullOrEmpty(_todayTimecardId))
            {
                // 新規登録
                realm.Write(() =>
                {
                    var addedTimecard = realm.Add<Timecard>(new Timecard()
                    {
                        EntryDate = entryDateTime,
                        EndTimeString = entryDateTime.ToString("HH:mm"),
                    });
                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.EndTimeString = entryDateTime.ToString("HH:mm");
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            StampingEndTimeLabelText = entryDateTime.ToString("HH:mm");
        });
    }
}

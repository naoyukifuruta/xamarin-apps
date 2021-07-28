using System;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using SimpleTimecard.Common;
using SimpleTimecard.Models;

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
            Logger.Trace();

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
            if (allTimecards.Where(x => x.StartTime.Value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).Any())
            {
                var todayTimecard = allTimecards.Where(x => x.StartTime.Value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).First();
                _todayTimecardId = todayTimecard.TimecardId;

                // 登録がある場合は画面に表示
                if (todayTimecard.StartTime.HasValue)
                {
                    StampingStartTimeLabelText = todayTimecard.StartTime.Value.ToLocalTime().ToString("HH:mm");
                }
                if (todayTimecard.EndTime.HasValue)
                {
                    StampingEndTimeLabelText = todayTimecard.EndTime.Value.ToLocalTime().ToString("HH:mm");
                }
            }
            else
            {
                _todayTimecardId = string.Empty;
                StampingStartTimeLabelText = string.Empty;
                StampingEndTimeLabelText = string.Empty;
            }
        }

        public ICommand OnClickStartTimeEntry => new DelegateCommand(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "出勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            // TODO: 更新

            var entryDateTime = DateTime.Now;
            var realm = Realm.GetInstance();

            if (string.IsNullOrEmpty(_todayTimecardId))
            {
                // 新規登録
                realm.Write(() =>
                {
                    var addedTimecard = realm.Add<Timecard>(new Timecard()
                    {
                        StartTime = entryDateTime,
                        StartTimeString = entryDateTime.ToLocalTime().ToString("HH:mm"),
                    });

                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.StartTime = entryDateTime;
                    timecard.StartTimeString = entryDateTime.ToLocalTime().ToString("HH:mm");
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            StampingStartTimeLabelText = entryDateTime.ToString("HH:mm");
        });

        public ICommand OnClickEndTimeEntry => new DelegateCommand(async () =>
        {
            var result = await _pageDialogService.DisplayAlertAsync("確認", "退勤時間の登録を行いますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var entryDateTime = DateTime.Now;
            var realm = Realm.GetInstance();

            if (string.IsNullOrEmpty(_todayTimecardId))
            {
                // 新規登録
                realm.Write(() =>
                {
                    var addedTimecard = realm.Add<Timecard>(new Timecard()
                    {
                        EndTime = entryDateTime,
                        EndTimeString = entryDateTime.ToLocalTime().ToString("HH:mm"),
                    });
                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.EndTime = entryDateTime;
                    timecard.EndTimeString = entryDateTime.ToLocalTime().ToString("HH:mm");
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            StampingEndTimeLabelText = entryDateTime.ToString("HH:mm");
        });
    }
}

using System;
using System.Diagnostics;
using System.Linq;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class TodayPageViewModel : ViewModelBase, IActiveAware
    {
        private readonly IPageDialogService _pageDialogService;

        // タブがアクティブかどうか
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (value)
                {
                    Debug.WriteLine($"{typeof(TodayPageViewModel).Name} is Active!");
                }
                SetProperty(ref this._isActive, value);
            }
        }
        public event EventHandler IsActiveChanged;

        // 出勤時間
        private string _startTimeLabelText = "--:--";
        public string StartTimeLabelText
        {
            get { return _startTimeLabelText; }
            set
            {
                SetProperty(ref _startTimeLabelText, value);
            }
        }

        // 退勤時間
        private string _endTimeLabelText = "--:--";
        public string EndTimeLabelText
        {
            get { return _endTimeLabelText; }
            set
            {
                SetProperty(ref _endTimeLabelText, value);
            }
        }

        public DelegateCommand EntryStartTimeCommand { get; set; }
        public DelegateCommand EntryEndTimeCommand { get; set; }

        private string _todayTimecardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="pageDialogService"></param>
        public TodayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Today";

            EntryStartTimeCommand = new DelegateCommand(EntryStartTime);
            EntryEndTimeCommand = new DelegateCommand(EntryEndTime);

            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            if (allTimecards.Where(x => x.StartTime.Value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).Any())
            {
                var todayTimecard = allTimecards.Where(x => x.StartTime.Value.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).First();
                _todayTimecardId = todayTimecard.TimecardId;

                if (todayTimecard.StartTime.HasValue)
                {
                    StartTimeLabelText = todayTimecard.StartTime.Value.ToLocalTime().ToString("HH:mm");
                }
                if (todayTimecard.EndTime.HasValue)
                {
                    EndTimeLabelText = todayTimecard.EndTime.Value.ToLocalTime().ToString("HH:mm");
                }
            }
            else
            {
                _todayTimecardId = string.Empty;
            }
        }

        /// <summary>
        /// 出勤時間の登録
        /// </summary>
        private async void EntryStartTime()
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
                    var addedTimecard = realm.Add<Timecard>(new Timecard() { StartTime = entryDateTime });
                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.StartTime = entryDateTime;
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            StartTimeLabelText = entryDateTime.ToString("HH:mm");
        }

        /// <summary>
        /// 退勤時間の登録
        /// </summary>
        private async void EntryEndTime()
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
                    var addedTimecard = realm.Add<Timecard>(new Timecard() { EndTime = entryDateTime });
                    _todayTimecardId = addedTimecard.TimecardId;
                });
            }
            else
            {
                // 更新
                var timecard = realm.Find<Timecard>(_todayTimecardId);
                realm.Write(() => {
                    timecard.EndTime = entryDateTime;
                    realm.Add<Timecard>(timecard, update: true);
                });
            }

            EndTimeLabelText = entryDateTime.ToString("HH:mm");
        }
    }
}

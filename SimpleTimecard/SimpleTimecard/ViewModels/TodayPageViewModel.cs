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

        public string StartTimeLabelText { get; set; }
        public string EndTimeLabelText { get; set; }
        public string StartTimeRegistButtonText { get; set; }
        public string EndTimeRegistButtonText { get; set; }

        // 出勤時間
        private string _stampingStartTimeLabelText = string.Empty;
        public string StampingStartTimeLabelText
        {
            get { return _stampingStartTimeLabelText; }
            set
            {
                SetProperty(ref _stampingStartTimeLabelText, value);
            }
        }

        // 退勤時間
        private string _stampingEndTimeLabelText = string.Empty;
        public string StampingEndTimeLabelText
        {
            get { return _stampingEndTimeLabelText; }
            set
            {
                SetProperty(ref _stampingEndTimeLabelText, value);
            }
        }

        public DelegateCommand StartTimeRegistButtonCommand { get; set; }
        public DelegateCommand EndTimeRegistButtonCommand { get; set; }

        private string _todayTimecardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="pageDialogService"></param>
        public TodayPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            // 画面テキスト
            Title = "Today";
            StartTimeLabelText = "出勤時間：";
            EndTimeLabelText = "退勤時間：";
            StartTimeRegistButtonText = "出勤登録";
            EndTimeRegistButtonText = "退勤登録";

            // ボタンイベント
            StartTimeRegistButtonCommand = new DelegateCommand(RegisterStartTime);
            EndTimeRegistButtonCommand = new DelegateCommand(RegisterEndTime);

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
            }
        }

        /// <summary>
        /// 出勤時間の登録
        /// </summary>
        private async void RegisterStartTime()
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

            StampingStartTimeLabelText = entryDateTime.ToString("HH:mm");
        }

        /// <summary>
        /// 退勤時間の登録
        /// </summary>
        private async void RegisterEndTime()
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

            StampingEndTimeLabelText = entryDateTime.ToString("HH:mm");
        }
    }
}

using System;
using System.Diagnostics;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

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

            // TODO:
            StartTimeLabelText = DateTime.Now.ToString("HH:mm");
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

            // TODO:
            EndTimeLabelText = DateTime.Now.ToString("HH:mm");
        }
    }
}

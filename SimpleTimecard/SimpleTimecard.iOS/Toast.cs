using SimpleTimecard.Interfaces;
using SimpleTimecard.iOS;
using UIKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Toast))]
namespace SimpleTimecard.iOS
{
    internal class Toast : IToast
    {
        private NSTimer alertDelay;
        private UIAlertController alert;

        public void Show(string message)
        {
            alertDelay = NSTimer.CreateScheduledTimer(1.5, (obj) =>
            {
                dismissMessage();
            });

            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        public void Show(string message, int length)
        {
            // iOSは時間指定に未対応にしておく
            Show(message);
        }

        private void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}

using System;
using Android.Widget;
using SimpleTimecard.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SimpleTimecard.Droid.Toast))]
namespace SimpleTimecard.Droid
{
    internal class Toast : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public void Show(string message, int length)
        {
            if (length == 0 || length == 1)
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, message, (ToastLength)length).Show();
                return;
            }

            Show(message);
        }
    }
}

using System;
using System.Collections.Generic;
using SimpleTimecard.Models;
using SimpleTimecard.ViewModels;
using Xamarin.Forms;

namespace SimpleTimecard.Views
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var timecard = mi.CommandParameter as Timecard;

            ((HistoryPageViewModel)BindingContext).DeleteTimecard(timecard.TimecardId);
        }
    }
}

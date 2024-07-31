using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT115L_Oracle_App
{
    [Activity(Label = "Details", Theme = "@style/AppTheme")]
    public class Details : Activity
    {
        private Button participantBtn, attendantBtn, withdrawBtn, returnBtn;
        private LinearLayout eventDetailsLL;
        private TextView txt_eventName, txt_day, txt_date, txt_venue, txt_time;
        private Bundle extras;
        private string eventName;
        private string[] eventDetailsArray;
        private int studNum, dayNum;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.details);

            participantBtn = FindViewById<Button>(Resource.Id.btn_participant);
            attendantBtn = FindViewById<Button>(Resource.Id.btn_attendant);
            withdrawBtn = FindViewById<Button>(Resource.Id.btn_withdraw);
            returnBtn = FindViewById<Button>(Resource.Id.btn_return);
            eventDetailsLL = FindViewById<LinearLayout>(Resource.Id.eventdetailsLinearLayout);
            txt_eventName = eventDetailsLL.FindViewById<TextView>(Resource.Id.txt_eventName);
            txt_day = eventDetailsLL.FindViewById<TextView>(Resource.Id.txt_day);
            txt_date = eventDetailsLL.FindViewById<TextView>(Resource.Id.txt_date);
            txt_venue = eventDetailsLL.FindViewById<TextView>(Resource.Id.txt_venue);
            txt_time = eventDetailsLL.FindViewById<TextView>(Resource.Id.txt_time);

            functions = new Functions(this, typeof(EventList));

            extras = Intent.Extras;

            if (extras != null)
            {
                eventName = extras.GetString("eventName");
                eventDetailsArray = extras.GetStringArray("eventDetails_array");
                studNum = extras.GetInt("studNum");
                dayNum = extras.GetInt("dayNum");
            }
            else 
            {
                Toast.MakeText(this, "Failed to Load Event Details.", ToastLength.Long).Show(); 
            }

            functions.DisplayEventDetails(eventDetailsArray, txt_eventName, txt_day, txt_date, txt_venue, txt_time);

            participantBtn.Click += delegate
            {
                functions.EnlistData("Participant", eventName, studNum);
            };

            attendantBtn.Click += delegate
            {
                functions.EnlistData("Audience", eventName, studNum);
            };

            withdrawBtn.Click += delegate
            {
                functions.WithdrawData(eventName, studNum);
            };

            returnBtn.Click += delegate
            {
                extras.PutInt("dayNum", dayNum); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };
        }
    }
}
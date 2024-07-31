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
    [Activity(Label = "Events", Theme = "@style/AppTheme")]
    public class Events : Activity
    {
        private Button day1Btn, day2Btn, day3Btn, day4Btn, day5Btn, profileBtn, signOutBtn;
        private Functions functions;
        private Bundle extras;
        private int studNum;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.events);

            day1Btn = FindViewById<Button>(Resource.Id.day1Btn);
            day2Btn = FindViewById<Button>(Resource.Id.day2Btn);
            day3Btn = FindViewById<Button>(Resource.Id.day3Btn);
            day4Btn = FindViewById<Button>(Resource.Id.day4Btn);
            day5Btn = FindViewById<Button>(Resource.Id.day5Btn);
            profileBtn = FindViewById<Button>(Resource.Id.profileBtn);
            signOutBtn = FindViewById<Button>(Resource.Id.signOutBtn);

            functions = new Functions(this, typeof(EventList));

            extras = Intent.Extras;

            if (extras != null)
            {
                studNum = extras.GetInt("studNum", -1);
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
            }

            day1Btn.Click += delegate
            {
                extras.PutInt("eventDay", 1); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            day2Btn.Click += delegate
            {
                extras.PutInt("eventDay", 2); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            day3Btn.Click += delegate
            {
                extras.PutInt("eventDay", 3); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            day4Btn.Click += delegate
            {
                extras.PutInt("eventDay", 4); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            day5Btn.Click += delegate
            {
                extras.PutInt("eventDay", 5); extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            profileBtn.Click += delegate
            {
                functions = new Functions(this, typeof(Profile));
                extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };

            signOutBtn.Click += delegate
            {
                studNum = 0;
                functions = new Functions(this, typeof(Login));
                functions.NextActivity();
                Toast.MakeText(this, "Log Out Successfuly.", ToastLength.Long).Show();
            };
        }
    }
}
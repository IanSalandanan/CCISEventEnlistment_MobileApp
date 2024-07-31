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
using System.Text.Json.Serialization;

namespace IT115L_Oracle_App
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme")]
    public class Profile : Activity
    {
        private EditText passwordUpdate, programUpdate;
        private EditText studNumET, fNameET, lNameET, yearLevelET, houseET;
        private Button submitButton, returnBtn;
        private Bundle extras;
        private Functions functions;
        private int studNum;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.profile);

            passwordUpdate = FindViewById<EditText>(Resource.Id.passwordInput);
            programUpdate = FindViewById<EditText>(Resource.Id.programInput);
            studNumET = FindViewById<EditText>(Resource.Id.studNumET);
            fNameET = FindViewById<EditText>(Resource.Id.fNameET);
            lNameET = FindViewById<EditText>(Resource.Id.lNameET);
            yearLevelET = FindViewById<EditText>(Resource.Id.yearLevelET);
            houseET = FindViewById<EditText>(Resource.Id.houseET);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            submitButton = FindViewById<Button>(Resource.Id.submitBtn);

            functions = new Functions(this, typeof(Events));

            extras = Intent.Extras;

            if (extras != null)
            {
                studNum = extras.GetInt("studNum", -1);
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show(); 
            }

            if (studNum != 0)
            {
                functions.DisplayProfileAsync(studNum, studNumET, passwordUpdate, fNameET, lNameET, yearLevelET, programUpdate, houseET);
            }
            else
            {
                Toast.MakeText(this, "Unable to Display User Profile.", ToastLength.Long).Show();
            }

            submitButton.Click += delegate
            {
                functions.UpdateProfileAsync(studNum, passwordUpdate.Text, programUpdate.Text, yearLevelET.Text);
            };

            returnBtn.Click += delegate
            {
                extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };
        }
    }

    public class UserProfile
    {
        [JsonPropertyName("STUD_ID")]
        public string Stud_id { get; set; }
        [JsonPropertyName("PASSWORD")]
        public string Password { get; set; }
        [JsonPropertyName("FIRST_NAME")]
        public string First_name { get; set; }
        [JsonPropertyName("LAST_NAME")]
        public string Last_name { get; set; }
        [JsonPropertyName("YEAR_LVL")]
        public string Year_lvl { get; set; }
        [JsonPropertyName("PROGRAM")]
        public string Program { get; set; }
        [JsonPropertyName("HOUSE_NAME")]
        public string House_name { get; set; }
    }
}
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
    [Activity(Label = "Login", Theme = "@style/AppTheme")]
    public class Login : Activity
    {
        private EditText studNumEt, passWordEt;
        private Button logInBtn, returnBtn;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.login);

            studNumEt = FindViewById<EditText>(Resource.Id.studNumEt);
            passWordEt = FindViewById<EditText>(Resource.Id.passWordEt);
            logInBtn = FindViewById<Button>(Resource.Id.logInBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(Events));

            logInBtn.Click += delegate
            {
                if (!string.IsNullOrEmpty(studNumEt.Text) && int.TryParse(studNumEt.Text, out int studNum))
                {
                    studNum = int.Parse(studNumEt.Text);
                    functions.Login("studNum", studNum, passWordEt.Text);
                }
                else
                {
                    Toast.MakeText(this, "Please enter a valid Student No.", ToastLength.Long).Show();
                }
            };

            returnBtn.Click += delegate
            {
                functions = new Functions(this, typeof(MainActivity));
                functions.NextActivity();
            };
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio.FileNio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static Android.Resource;

namespace IT115L_Oracle_App
{
    [Activity(Label = "Register", Theme = "@style/AppTheme")]
    public class Register : Activity
    {
        private EditText studNumEt, fNameEt, lNameEt, programEt, yearLvlEt, houseEt, passWordEt;
        private Button registerBtn, returnBtn;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.register);

            studNumEt = FindViewById<EditText>(Resource.Id.studNumEt);
            fNameEt = FindViewById<EditText>(Resource.Id.fNameEt);
            lNameEt = FindViewById<EditText>(Resource.Id.lNameEt);
            programEt = FindViewById<EditText>(Resource.Id.programEt);
            yearLvlEt = FindViewById<EditText>(Resource.Id.yearLvlEt);
            houseEt = FindViewById<EditText>(Resource.Id.houseEt);
            passWordEt = FindViewById<EditText>(Resource.Id.passWordEt);
            registerBtn = FindViewById<Button>(Resource.Id.registerBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(Events));

            registerBtn.Click += delegate
            {
                if (!string.IsNullOrEmpty(studNumEt.Text) && int.TryParse(studNumEt.Text, out int studNum))
                {
                    functions.Register("studNum", studNum, fNameEt.Text, lNameEt.Text, programEt.Text, yearLvlEt.Text, houseEt.Text.ToUpper(), passWordEt.Text);
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
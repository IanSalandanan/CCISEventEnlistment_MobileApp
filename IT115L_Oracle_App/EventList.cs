using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio.FileNio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IT115L_Oracle_App
{
    [Activity(Label = "EventList", Theme = "@style/AppTheme")]
    public class EventList : Activity
    {
        private Button returnBtn;
        private ListView listView;
        private List<string> eventList;
        private Bundle extras;
        private int studNum;
        private int dayNum;
        private Functions functions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.eventlist);

            functions = new Functions(this, typeof(Details));
            eventList = new List<string>();

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            listView = FindViewById<ListView>(Resource.Id.eventList);

            extras = Intent.Extras;

            if (extras != null)
            {
                studNum = extras.GetInt("studNum", -1);
                dayNum = extras.GetInt("eventDay", -1);
            }
            else 
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
            }

            eventList = functions.RetrieveEvents(dayNum, eventList);

            if (eventList != null && eventList.Count > 0)
            {
                listView.Adapter = functions.DisplayEvents(eventList);
            }
            else
            {
                Toast.MakeText(this, "No Listed Events for this Day.", ToastLength.Long).Show();
            }

            listView.ItemClick += (sender, e) =>
            {
                string selectedEvent = functions.GetSelectedEvent(eventList, e.Position); ;
                functions.GoToEventDescription(selectedEvent, studNum, dayNum);
            };

            returnBtn.Click += delegate
            {
                functions = new Functions(this, typeof(Events));
                extras.PutInt("studNum", studNum);
                functions.NextExtraActivity(extras);
            };
        }
    }
}
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
using System.Threading.Tasks;
using static Android.Content.ClipData;

namespace IT115L_Oracle_App
{
    class Functions
    {
        private readonly Activity currentAct;
        private readonly Type nextAct;

        private Intent intent;
        private Transactions transactions;

        private string res;

        public Functions() { }

        public Functions(Activity currentAct, Type nextAct)
        {
            this.currentAct = currentAct;
            this.nextAct = nextAct;
        }

        public void NextActivity()
        {
            intent = new Intent(currentAct, nextAct);
            currentAct.StartActivity(intent);
        }

        public void NextExtraActivity(Bundle bundle)
        {
            intent = new Intent(currentAct, nextAct);
            intent.PutExtras(bundle);
            currentAct.StartActivity(intent);
        }

        public void Register(string studNumKey, int studNum, string fName, string lName, string program, string yearLvl, string house, string passWord)
        {
            Bundle extras;

            transactions = new Transactions(studNum, "stringFiller");
            res = transactions.RegisterRequest().Trim();

            if (res.Equals("Failed!"))
            {
                Toast.MakeText(currentAct, "Account Already Exists.", ToastLength.Long).Show();
            }
            else if (res.Equals("OK!"))
            {
                transactions = new Transactions(studNum, fName, lName, program, yearLvl, house, passWord);
                res = transactions.AddRequest();

                if (res.Contains("ORA-01400"))
                {
                    Toast.MakeText(currentAct, "Please fill out all fields before submitting.", ToastLength.Long).Show();
                }
                else
                {
                    extras = new Bundle();
                    extras.PutInt(studNumKey, studNum);
                    NextExtraActivity(extras);
                }
            }
        }

        public void Login(string studNumKey, int studNum, string passWord)
        {
            Bundle extras;

            transactions = new Transactions(studNum, passWord);
            res = transactions.CheckRequest().Trim();

            if (res.Equals("OK!"))
            {
                extras = new Bundle();
                extras.PutInt(studNumKey, studNum);
                NextExtraActivity(extras);
            }
            else if (res.Equals("Failed!"))
            {
                Toast.MakeText(currentAct, "Invalid User Credentials", ToastLength.Long).Show();
            }
        }

        public List<string> RetrieveEvents(int dayNum, List<string> eventNames)
        {
            try
            {
                transactions = new Transactions(dayNum);
                res = transactions.GetEventsRequest();

                //Parsing of res API result into a more workable json data structure
                using JsonDocument doc = JsonDocument.Parse(res);
                JsonElement root = doc.RootElement;

                //check whether the parsed json result root has an array 
                if (root.ValueKind == JsonValueKind.Array)
                {
                    //Inside the array are list of event names, so iteration is needed
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        string eventName = element.GetString();
                        eventNames.Add(eventName);
                        //after every iteration within the array, each array element will be added to the list
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unexpected JSON format.");
                }

                return eventNames;
                //returning the populated list
            }
            catch (Exception ex)
            {
                Toast.MakeText(currentAct, ex.Message, ToastLength.Long).Show();
                return null;
            }
        }

        public ArrayAdapter<string> DisplayEvents(List<string> items)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(currentAct, Android.Resource.Layout.SimpleListItem1, items);
            return adapter;
        }

        public string GetSelectedEvent(List<string> eventList, int textPosition)
        {
            return eventList[textPosition];
        }

        public void GoToEventDescription(string getEventName, int studNum, int dayNum)
        {
            Bundle extras;

            String[] eventDetails_array = RequestEventDetails(getEventName);

            extras = new Bundle();
            extras.PutString("eventName", getEventName);
            extras.PutStringArray("eventDetails_array", eventDetails_array);
            extras.PutInt("studNum", studNum);
            extras.PutInt("eventDay", dayNum);
            NextExtraActivity(extras);
        }

        public String[] RequestEventDetails(string getEventName)
        {
            try
            {
                transactions = new Transactions(getEventName);
                res = transactions.GetEventDetailsRequest();

                List<string> eventDetailsList = new List<string>();

                using (JsonDocument doc = JsonDocument.Parse(res))
                {
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            foreach (JsonProperty property in element.EnumerateObject())
                            {
                                eventDetailsList.Add($"{property.Name}: {property.Value}");
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected JSON format.");
                    }
                }

                return eventDetailsList.ToArray();
            }
            catch (Exception ex)
            {
                Toast.MakeText(currentAct, ex.Message, ToastLength.Long).Show();
                return null;
            }
        }
        public void DisplayEventDetails(String[] event_details, TextView txt_eventName, TextView txt_day, TextView txt_venue, TextView txt_time, TextView txt_date)
        {
            // eventDetailsArr[0] = "EVENT_NAME: Jeopardy" for example
            txt_eventName.Text = RemovePrefix(event_details[0]);
            txt_time.Text = "Time: " + RemovePrefix(event_details[1]);
            txt_venue.Text = "Venue: " + RemovePrefix(event_details[2]);
            txt_day.Text = RemovePrefix(event_details[3]);
            txt_date.Text = "Date: " + RemovePrefix(event_details[4]);
        }

        private static string RemovePrefix(string text)
        {
            return text.Substring(text.IndexOf(':') + 2);
        }

        public void EnlistData(string attendant_type, string eventName, int studNum)
        {
            transactions = new Transactions(eventName, studNum);
            res = transactions.EnlistDataRequest(attendant_type);
            Toast.MakeText(currentAct, $"Enlisted as {attendant_type}.", ToastLength.Long).Show();
        }

        public void WithdrawData(string eventName, int studNum)
        {
            transactions = new Transactions(eventName, studNum);
            res = transactions.WithdrawDataRequest();
            Toast.MakeText(currentAct, "Withdrawn Succesfully.", ToastLength.Long).Show();
        }

        public async Task DisplayProfileAsync(int studNum, EditText studNumET, EditText passwordUpdate, EditText fNameET, EditText lNameET, EditText yearLevelET, EditText programUpdate, EditText houseET)
        {
            try
            {
                transactions = new Transactions(studNum, "stringFiller");
                res = await transactions.DisplayRequestAsync();

                if (!string.IsNullOrEmpty(res))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var profile = JsonSerializer.Deserialize<UserProfile>(res, options);

                    if (profile != null)
                    {
                        currentAct.RunOnUiThread(() =>
                        {
                            studNumET.Text = profile.Stud_id;
                            passwordUpdate.Text = profile.Password;
                            fNameET.Text = profile.First_name;
                            lNameET.Text = profile.Last_name;
                            yearLevelET.Text = profile.Year_lvl;
                            programUpdate.Text = profile.Program;
                            houseET.Text = profile.House_name;
                        });
                    }
                    else
                    {
                        Toast.MakeText(currentAct, "No profile data found", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(currentAct, "Empty response received", ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(currentAct, $"Error: {ex.Message}", ToastLength.Long).Show();
            }
        }

        public async Task UpdateProfileAsync(int studNum, string passwordUpdate, string programUpdate, string yearLevelET)
        {
            var profileData = new
            {
                Stud_id = studNum,
                Password = passwordUpdate,
                Program = programUpdate,
                Year_lvl = yearLevelET
            };

            var json = JsonSerializer.Serialize(profileData);

            transactions = new Transactions();
            res = await transactions.UpdateRequestAsync(json);

            string successMessage = "Profile updated successfully";

            if (res.Contains(successMessage))
            {
                Toast.MakeText(currentAct, successMessage, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(currentAct, "Failed to update profile", ToastLength.Long).Show();
            }
        }
    }
}


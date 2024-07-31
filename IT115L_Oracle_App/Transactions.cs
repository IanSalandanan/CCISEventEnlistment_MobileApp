using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT115L_Oracle_App
{
    internal class Transactions
    {
        private readonly string ipAdd = "";
        private readonly string filePath = "IT115L";

        private int studNum, dayNum;
        private string fName, lName, program, yearLvl, house, passWord, eventName, res;
        private HttpWebRequest request;
        private HttpWebResponse response;
        private StreamReader reader;

        public Transactions(int studNum, string fName, string lName, string program, string yearLvl, string house, string passWord)
        {
            this.studNum = studNum;
            this.fName = fName;
            this.lName = lName;
            this.program = program;
            this.yearLvl = yearLvl;
            this.house = house;
            this.passWord = passWord;
        }

        public Transactions(int studNum, string passWord)
        {
            this.studNum = studNum;
            this.passWord = passWord;
        }

        public Transactions(int dayNum)
        {
            this.dayNum = dayNum;
        }

        public Transactions(string eventName)
        {
            this.eventName = eventName;
        }

        public Transactions(string eventName, int studNum)
        {
            this.eventName = eventName;
            this.studNum = studNum;
        }

        public Transactions() { }

        public string AddRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/add_oraxampp.php?id={studNum}&passWord={passWord}&fName={fName}&lName={lName}&yearLvl={yearLvl}&program={program}&house={house}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string RegisterRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/register_oraxampp.php?id={studNum}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string CheckRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/login_oraxampp.php?id={studNum}&password={passWord}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string GetEventsRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/readEvents_oraxampp.php?day_num={dayNum}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string GetEventDetailsRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/read_eventDetails.php?event_name={eventName}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string EnlistDataRequest(String attendantType)
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/enlist_oraxampp.php?attendant_type={attendantType}&event_name={eventName}&stud_id={studNum}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public string WithdrawDataRequest()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/unenlist_oraxampp.php?event_name={eventName}&stud_id={studNum}");
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            response.Dispose();
            reader.Dispose();
            return res;
        }

        public async Task<string> DisplayRequestAsync()
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/display_profile.php?stud_id={studNum}");
            request.Method = "GET";
            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            return await reader.ReadToEndAsync();
        }

        public async Task<string> UpdateRequestAsync(string json)
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{ipAdd}/{filePath}/update_profile.php");
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}


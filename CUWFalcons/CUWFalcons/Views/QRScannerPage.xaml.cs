using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using CUWFalcons.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CUWFalcons.Views
{
    public partial class QRScannerPage : ContentPage
    {

        CalendarEventModel theEvent; // event that the user is trying to check into

        
        public QRScannerPage(CalendarEventModel anEvent)
        {
            InitializeComponent();
            theEvent = anEvent; // event that the user is trying to check into
            doScan(); // completes QR Scanning
        }

        // scans the qrcode and checks in the user if the correct qr code is scanned
        async void doScan()
        {
            var Scanner = new ZXing.Mobile.MobileBarcodeScanner(); // starts scanner
            var result = await Scanner.Scan(); // gets the value from the qr code

            if (result != null) // something was scanned
            {
                string qrcode = result.Text + " (type: " + result.BarcodeFormat.ToString() + ")";
                string correctCode = "falcons" + theEvent.id + " (type: QR_CODE)";


                bool newCheckIn = checkUser(theEvent.attendance, App.getUser()); // returns false if user is already checked into this event
                string _attendance = theEvent.attendance + "," + App.getUser(); // string with a user added to the attendance


                // correct scan
                if (qrcode == correctCode)
                {
                    // already checked in
                    if (newCheckIn == false)
                    {
                        await DisplayAlert("Success", "You already checked into this game!", "Close"); 
                    }
                    // not checked in
                    else
                    {
                        getResult(_attendance); // updates the attendance in the database
                        await DisplayAlert("Success", "You are now checked in!\nEnjoy the game!", "Close");

                    }

                }

                // wrong qr code scanned
                else
                {
                    await DisplayAlert("Error", "You were not able to be checked in, please try again", "Close");
                   
                }
                await Navigation.PopAsync();
            }
            
        }


        // updates the attendance in the database
        public void getResult(string _attendance)
        {

            string cmdString = $"UPDATE calendar_cuw SET attendance=@val1  WHERE id = @val2;";

            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand comm = new SqlCommand(cmdString, conn))
                {
                    comm.Parameters.AddWithValue("@val1", _attendance);
                    comm.Parameters.AddWithValue("@val2", theEvent.id);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }


            }
            

        }

        // sees if the user has already checked into the event
        public bool checkUser(string listOfPeople, string newPerson)
        {
            // sees if anyone is checked in
            if (listOfPeople == null)
            {
                return true;
            }

            string name = "";
            ArrayList attendances = new ArrayList();

            

            // separates people's names
            for (int i = 0; i < listOfPeople.Length; i++)
            {
                if (listOfPeople[i].ToString() != ",")
                {
                    name = name + listOfPeople[i].ToString();
                }
                else
                {
                    attendances.Add(name);
                    name = "";
                }
            }
            attendances.Add(name);


            // sees if the user already checked in
            for (int i = 0; i < attendances.Count; i++)
            {
                if (attendances[i].ToString() == newPerson)
                {
                    return false; // already checked in
                }
            }
            
            return true; // not already checked in

        }

  
    }
}



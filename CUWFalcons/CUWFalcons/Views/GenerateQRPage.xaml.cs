using System;
using System.Collections;
using CUWFalcons.Models;
using System.Data.SqlClient;
using Xamarin.Forms;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Drawing;

namespace CUWFalcons.Views
{
    public partial class GenerateQRPage : ContentPage
    {


        public GenerateQRPage()
        {
            InitializeComponent();
        }

        //gets the QR Code so admin can print and display it at the given event
        async void viewQR_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var theEvent = item.CommandParameter as CalendarEventModel;
            string qrVal = "falcons" + theEvent.id; // the QR Codes Value that will be scanned
            displayQR(qrVal);
            
        }

        //goes to the see who has checked in for the specified event
        async void viewAttendance_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var theEvent = item.CommandParameter as CalendarEventModel;
            await Navigation.PushAsync(new ViewAttendancePage(theEvent));
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                displayHomeEvents(); // fills both grids with the home events
            }
            catch
            {

            }
        }

        // makes a qrcode with the value passed through and display a pdf of the barcode
        async void displayQR(string qrVal)
        {
            //Creates a QR Code for a pdf
            PdfQRBarcode qrcode = new PdfQRBarcode();
            qrcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.High;
            qrcode.XDimension = 3;
            qrcode.Text = qrVal; // qrVal is a unique string to the event the pdf will correspond too
            qrcode.Size = new SizeF(550, 500);


            PdfDocument pdf = new PdfDocument(); // creates a pdf
            PdfPage page = pdf.Pages.Add(); // adds a page to pdf
            qrcode.Draw(page, new PointF(25, 70)); // adds qrcode on the page


            // saves and shows the pdf with the qr code
            MemoryStream stream = new MemoryStream();
            pdf.Save(stream);
            pdf.Close(true);
            await DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);
        }

        

        // sets what events are going to be displayed on the page
        public void displayHomeEvents()
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();


            //gets the upcoming home events within a week out 
            ArrayList homeEvents = new ArrayList();
            string commandText = $"SELECT * FROM calendar_cuw where date between dateadd(day, -0, convert(date, getdate())) \nand dateadd(day, +7, convert(date, getdate())) AND (home='Concordia Wisconsin' OR home='CUW' or home='cuw' or home='Concordia University Wisconsin');";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                        homeEvents.Add(theEvent);
                    }

            }
            homeGameView.ItemsSource = homeEvents; // sets the grid to display all upcoming events

            //gets the past months home events
            ArrayList pastHomeEvents = new ArrayList();
            commandText = "SELECT * FROM calendar_cuw where date between dateadd(month, -1, convert(date, getdate())) \nand dateadd(day, -0, convert(date, getdate()))AND (home='Concordia Wisconsin' OR home='CUW' or home='cuw' or home='Concordia University Wisconsin') ORDER BY date DESC ;";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                        pastHomeEvents.Add(theEvent);
                    }

            }
            pastHomeGameView.ItemsSource = pastHomeEvents; // sets the grid to display all the past events

            connection.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NewsBytes.Resources;
using System.Net.Http;
using Newtonsoft.Json;

namespace NewsBytes
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            textblock1.Text = "";

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string myurl = "";
            textblock1.Text = "";
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    int index = int.Parse(selectedIndex);
                }

                myurl = NavigationContext.QueryString["itemUrl"];
            }
            string baseurl2 = "http://api.aylien.com/api/v1/summarize";
            myurl = myurl.Replace("/", "%2F");
            myurl = myurl.Replace(":", "%3A");
            string finalurl = baseurl2 + "?url=" + myurl + "&mode=default&sentences_number=5";
            //string finalurl = "http://api.aylien.com/api/v1/summarize?url=http%3A%2F%2Fwww.nasa.gov%2Fpress%2F2015%2Ffebruary%2Fmedia-invited-to-boeing-commercial-crew-access-tower-groundbreaking%2F&mode=default&sentences_number=5";
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, finalurl);
            textblock1.Text = finalurl;

            // Add our custom headers
            requestMessage.Headers.Add("X-AYLIEN-TextAPI-Application-Key", "69cb0e7d3e052e470ae130922fd025dd");
            requestMessage.Headers.Add("X-AYLIEN-TextAPI-Application-ID", "bfbb168d");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            string responseAsString = await response.Content.ReadAsStringAsync();
            RootObject2 data2 = JsonConvert.DeserializeObject<RootObject2>(responseAsString);
            if (data2.sentences == null) { textblock1.Text = "empty"; }
            string total = "";
            foreach (String sentence in data2.sentences)
            {
                total = total + sentence;
            }
            textblock1.Text = total;

        }

    }

    public class RootObject2
    {
        public string text { get; set; }
        public List<string> sentences { get; set; }
    }
}
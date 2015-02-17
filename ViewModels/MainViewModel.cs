using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NewsBytes.Resources;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

namespace NewsBytes.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            HttpClient client = new HttpClient();
            string baseurl = "http://content.guardianapis.com/search?q=debate&tag=politics/politics&from-date=2015-01-01&api-key=test";
            string result = await client.GetStringAsync(baseurl);
            
            
            
            // Sample data; replace with real data
           /* HttpClient client = new HttpClient();
            string baseurl = "http://api.nytimes.com/svc/mostpopular/v2/mostviewed/technology/7.json?api-key=sample-key";
            string result = await client.GetStringAsync(baseurl);
            //string myurl = "";
            */
            RootObject data = JsonConvert.DeserializeObject<RootObject>(result);

            foreach (Result item in data.response.results)
            {
                int i = 0;
                this.Items.Add(new ItemViewModel() { ID = i.ToString(), LineOne = item.webTitle, LineTwo = item.sectionName, LineThree = item.webUrl });
                i++;
            }
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /*
        public class Result
        {
            public string url { get; set; }
            public string adx_keywords { get; set; }
            public string column { get; set; }
            public string section { get; set; }
            public string byline { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string @abstract { get; set; }
            public string published_date { get; set; }
            public string source { get; set; }
            public object id { get; set; }
            public object asset_id { get; set; }
            public int views { get; set; }
            public List<string> des_facet { get; set; }
            public List<string> org_facet { get; set; }
            public object per_facet { get; set; }
            public object geo_facet { get; set; }
            public object media { get; set; }
        }

        public class RootObject
        {
            public string status { get; set; }
            public string copyright { get; set; }
            public int num_results { get; set; }
            public List<Result> results { get; set; }
        }
        */


        public class Result
        {
            public string webTitle { get; set; }
            public string webPublicationDate { get; set; }
            public string sectionId { get; set; }
            public string id { get; set; }
            public string webUrl { get; set; }
            public string apiUrl { get; set; }
            public string sectionName { get; set; }
        }
/// <summary>
/// //////////////////////////////////////////////////////////////////////////////////////
/// </summary>
        public class Response
        {
            public string status { get; set; }
            public string userTier { get; set; }
            public int total { get; set; }
            public int startIndex { get; set; }
            public int pageSize { get; set; }
            public int currentPage { get; set; }
            public int pages { get; set; }
            public string orderBy { get; set; }
            public List<Result> results { get; set; }
        }

        public class RootObject
        {
            public Response response { get; set; }
        }
    }
}
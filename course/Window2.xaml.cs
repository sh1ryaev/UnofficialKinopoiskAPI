using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace course
{
    public class MoreFilm
    {
        public Film data { get; set; }
        public Rating rating { get; set; }
        public Budget budget { get; set; }
        public Review review { get; set; }
        public ExternalId externalId { get; set; }
        public Images images { get; set; }

    }
    public class Images
    {
        public Images_posters[] posters { get; set; }
        public Images_posters[] backdrops { get; set; }

    }
    public class Images_posters
    {
        public string language { get; set; }
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
    public class Rating
    {
        public float rating { get; set; }
        public int ratingVoteCount { get; set; }
        public float ratingImdb { get; set; }
        public int ratingImdbVoteCount { get; set; }
        public string ratingFilmCritics { get; set; }
        public int ratingFilmCriticsVoteCount { get; set; }
        public string ratingAwait { get; set; }
        public int ratingAwaitCount { get; set; }
        public string ratingRfCritics { get; set; }
        public int ratingRfCriticsVoteCount { get; set; }
    }
    public class Budget
    {
        public int grossRu { get; set; }
        public int grossUsa { get; set; }
        public int grossWorld { get; set; }
        public string budget { get; set; }
        public int marketing { get; set; }
    }
    public class Review
    {
        public int reviewsCount { get; set; }
        public string ratingGoodReview { get; set; }
        public int ratingGoodReviewVoteCount { get; set; }
    }
    public class ExternalId
    {
        public string imdbId { get; set; }
    }
    public class Trailer
    {
        public string url { get; set; }
        public string name { get; set; }
        public string site { get; set; }
        public string size { get; set; }
        public string type { get; set; }
    }
    public class Videos
    {
        public Trailer[] trailers { get; set; }
        public Trailer[] teasers { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            Window1 window1 = new Window1();
            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/" + id.chosenId + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<MoreFilm>(response.Content);
            nameRu.Text = top.data.nameRu;
            slogan.Text = top.data.slogan;
            description.Text = top.data.description;
            client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/"+id.chosenId+"/videos");
            response = client.Execute(request);
            var top1 = JsonConvert.DeserializeObject<Videos>(response.Content);
            var source = top1.trailers[0].url;
            var newSource = source.Split('=');

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

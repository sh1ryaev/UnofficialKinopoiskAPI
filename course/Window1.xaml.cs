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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using IMDbApiLib;
using RestSharp;
using RestSharp.Serialization.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;

namespace course
{

    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public class Film
    {
        public int filmId { get; set; }
        public string nameRu { get; set; }
        public string nameEn { get; set; }
        public string year { get; set; }
        public string filmLenght { get; set; }
        public Country[] countries { get; set; }
        public Genre[] genres { get; set; }
        public string rating { get; set; }
        public int ratingVoteCount { get; set; }
        public string posterUrl { get; set; }
        public string posterUrlPreview { get; set; }
        public string picHolder { get; set; } 
        public string AllGenres { get; set; }
        public string description { get; set; }
        public string slogan { get; set; }
    }
    public class Country
    {
        public string country { get; set; }
    }
    public class Genre
    {
        public string genre { get; set; }
    }
    public class Search
    {
        public int pagesCount { get; set; }
        public Film[] films { get; set; }
    }
    public class KeyWordSearch
    {
        public string keyword { get; set; }
        public int pagesCount { get; set; }
        public int searchFilmsCountResult { get; set; }
        public Film[] films { get; set; }

    }
    public class SearchLike
    {
        public Search search { get; set; }
        public string picHolder { get; set; }
    }
    public class KeyWordSearchLike
    {
        public KeyWordSearch KeyWordSearch { get; set; }
        public string picHoder { get; set; }
    }
    public class FilterResponse
    {
        public FilterResponseGenre[] genres { get; set; }
        public FilterResponseCountry[] countries { get; set; }
    }
    public class FilterResponseCountry
    {
        public int id { get; set; }
        public string country { get; set; }
    }
    public class FilterResponseGenre
    {
        public int id { get; set; }
        public string genre { get; set; }
    }
    public class Similar
    {
        public int total { get; set; }
        public Film[] items { get; set; }
    }
    public static class id
    {
        public static string chosenId { get; set; }
    }
    public partial class Window1 : Window
    {
        int page = 1;
        int pages = 0;
        string choseGenre = "";
        public List<FilterResponseGenre> filterGenres;
        int mode;
        public List<string> idLike = new List<string>();
        public string chosenId;
        public Window1()
        {
            InitializeComponent();
            idLike.Add("0");

            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/filters");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<FilterResponse>(response.Content);
            filterGenres = top.genres.ToList();
            List<string> genres = new List<string>();
            foreach(var g in top.genres)
            {
                if(g.id != 20)
                genres.Add(g.genre);
            }
            genresCombo.ItemsSource = genres;
            using (StreamReader sr = new StreamReader(@"like.txt", System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    idLike.Add(line);
                }
            }
            search();

        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Button_Like(object sender, RoutedEventArgs e)
        {

            var z = (Button)e.OriginalSource;
            var p = (Film)z.DataContext;

           if (p != null)
            {
                if (idLike.Contains(p.filmId.ToString()) == false)
                {
                    using (StreamWriter writer = new StreamWriter(@"like.txt", true))
                    {
                        writer.WriteLine(p.filmId.ToString());
                    }
                    idLike.Add(p.filmId.ToString());
                    p.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";
                }
                else
                {
                    idLike.Remove(p.filmId.ToString());
                    using (StreamWriter writer = new StreamWriter(@"like.txt"))
                    {
                        foreach (var id in idLike)
                            writer.WriteLine(id);
                    }
                }
            }
            if (mode == 1)
            {
                search();
            }
            if (mode == 2)
            {
                strSearch();
            }
            if (mode == 3)
            {
                GenreSearch();
            }
            if (mode == 4)
            {
                Similar(p);
            }
            if (mode == 5)
            {
                LikeList();
            }
        }
        private void Button_More(object sender, RoutedEventArgs e)
        {
            var z = (Button)e.OriginalSource;
            var q = (Film)z.DataContext;
            chosenId = q.filmId.ToString();
            id.chosenId = q.filmId.ToString();

            Window2 window2 = new Window2();
            window2.Show();
        }
        private void Button_Similar(object sender, RoutedEventArgs e)
        {
            mode = 4;
            var c = e.Source.ToString();
            var z = (Button)e.OriginalSource;
            var q = (Film)z.DataContext;
            Similar(q);

        }

        public void Similar(Film q)
        {
            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.2/films/" + q.filmId + "/similars");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<Similar>(response.Content);
            foreach (var t in top.items)
            {
                t.picHolder = "F:\\хлам с++\\course\\course\\img\\white.png";
            }
            foreach (var t in top.items)
            {
                foreach (var f in idLike)
                {
                    var x = t.filmId.ToString();
                    if (x == f)
                        t.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";
                }
            }
            filmList.ClearValue(ItemsControl.ItemsSourceProperty);
            filmList.ItemsSource = top.items;
            title.Text = "Похожие на " + q.nameRu;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mode = 2;
            page = 1;
            strSearch();
        }
        public Search search()
        {
            mode = 1;
            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_250_BEST_FILMS&page=" + page + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<Search>(response.Content);
            foreach (var t in top.films)
            {
                t.picHolder = "F:\\хлам с++\\course\\course\\img\\white.png";
            }
            foreach (var t in top.films)
            {
                foreach(var f in idLike)
                {
                    var x = t.filmId.ToString();
                    if (x == f)
                        t.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";
                }
                foreach (var v in t.genres)
                    t.AllGenres += v.genre + " ";
            }
            filmList.ClearValue(ItemsControl.ItemsSourceProperty);
            filmList.ItemsSource = top.films;
            pages = top.pagesCount;
            title.Text = "Топ 250 фильмов";
            return top;
        }
        public void GenreSearch()
        {
            var id = filterGenres.FirstOrDefault(x => x.genre == choseGenre).id;
            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-filters?genre=" + id + "&order=RATING&type=ALL&ratingFrom=0&ratingTo=10&yearFrom=1888&yearTo=2020&page="+page+"");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<Search>(response.Content);
            foreach (var t in top.films)
            {
                t.picHolder = "F:\\хлам с++\\course\\course\\img\\white.png";
            }
            foreach (var t in top.films)
            {
                foreach (var f in idLike)
                {
                    var x = t.filmId.ToString();
                    if (x == f)
                        t.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";
                }
                foreach (var v in t.genres)
                    t.AllGenres += v.genre + " ";
            }
            filmList.ClearValue(ItemsControl.ItemsSourceProperty);
            filmList.ItemsSource = top.films;
            pages = top.pagesCount;
            title.Text = "Поиск по жанру: "+choseGenre+"";
        }
        public void strSearch()
        {
            var str = HttpUtility.UrlEncode(searchBox.Text);
            var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-keyword?keyword=" + str + "&page=" + page + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
            request.AddHeader("accept", "application/json");
            IRestResponse response = client.Execute(request);
            var top = JsonConvert.DeserializeObject<KeyWordSearch>(response.Content);
            foreach (var t in top.films)
            {
                t.picHolder = "F:\\хлам с++\\course\\course\\img\\white.png";
            }
            foreach (var t in top.films)
            {
                foreach (var f in idLike)
                {
                    var x = t.filmId.ToString();
                    if (x == f)
                        t.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";
                }
                foreach (var v in t.genres)
                    t.AllGenres += v.genre + " ";
            }
            filmList.ClearValue(ItemsControl.ItemsSourceProperty);
            filmList.ItemsSource = top.films;
            pages = top.pagesCount;
            title.Text = "По запросу: '"+searchBox.Text+"' найдено "+top.searchFilmsCountResult+" результатов";
        }
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (page < pages && mode == 1)
            {
                page++;
                search();
            }
            if (page < pages && mode == 2)
            {
                page++;
                strSearch();
            }
            if (page < pages && mode == 3)
            {
                page++;
                GenreSearch();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (page > 1 && mode == 1)
            {
                page--;
                search();
            }
            if (page > 1 && mode == 2)
            {
                page--;
                strSearch();
            }
            if (page > 1 && mode == 3)
            {
                page--;
                GenreSearch();
            }

        }

        private void genresCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mode = 3;
            page = 1;
            choseGenre = genresCombo.SelectedItem.ToString();
            GenreSearch();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            mode = 5;
            LikeList();
            title.Text = "Понравившиеся фильмы ";
        }
        public void LikeList()
        {
            List<Film> moreFilms = new List<Film>();
            foreach (var p in idLike)
            {
                var client = new RestClient("https://kinopoiskapiunofficial.tech/api/v2.1/films/" + p + "");
                var request = new RestRequest(Method.GET);
                request.AddHeader("X-API-KEY", "2bb18085-cbba-4f47-9e61-8338511cc3ac");
                request.AddHeader("accept", "application/json");
                IRestResponse response = client.Execute(request);
                var top = JsonConvert.DeserializeObject<MoreFilm>(response.Content);
                if (top.data != null)
                {
                    foreach (var v in top.data.genres)
                        top.data.AllGenres += v.genre + " ";
                    top.data.picHolder = "F:\\хлам с++\\course\\course\\img\\red.png";

                    moreFilms.Add(top.data);

                }
            }
            filmList.ClearValue(ItemsControl.ItemsSourceProperty);
            filmList.ItemsSource = moreFilms;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            search();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }
    }
}

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CinemaSearch.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CinemaSearch.Controllers
{
    public class HomeController : Controller
    {
        public string EditText;

        public string Title_Movie;
        public string URL_Poster;
        public string Overview_Movie;
        public string Vote_Movie;

        public IActionResult Index()
        {
            return View();
        }

        /*
         *Search function
         */
        [HttpPost]
        public IActionResult Search()
        {
            string resp = "";
            EditText = Request.Form["moviename"];
            EditText = EditText.Replace(' ', '+');
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.themoviedb.org/3/search/movie?api_key=a3bdaae66f8cf705750820e17c0e9471&query=" + EditText);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resp = reader.ReadToEnd();
                dynamic data = JObject.Parse(resp);

                /*
                * Parsing JSON Response
                */

                try
                {
                    Title_Movie = data.results[0].title;
                    URL_Poster = Url.Content("https://image.tmdb.org/t/p/w300" +data.results[0].poster_path);
                    Vote_Movie = data.results[0].vote_average;
                    Overview_Movie = data.results[0].overview;

                }catch(Exception e)
                {
                    //JSON Parsing ERROR
                }

            }
            ViewData["Title"] = Title_Movie;
            ViewData["URL"] = URL_Poster;
            ViewData["Overview"] = Overview_Movie;
            ViewData["Vote"] = Vote_Movie;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

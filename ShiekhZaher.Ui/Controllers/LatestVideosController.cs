using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Zaher.Ui.Models.ViewModels.LatestVideosViewModels;

namespace Zaher.Ui.Controllers
{
    public class LatestVideosController : Controller
    {
        public ActionResult Index()
        {
            var model = Videos;

            return View(model);
        }

        private List<ListLatestVideo> Videos
        {
            get
            {
                var json = new WebClient().DownloadString("https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Fwww.youtube.com%2Ffeeds%2Fvideos.xml%3Fchannel_id%3DUCECu8pY7fhH3-orl-VIWyrA");
                var listOfVideosItem = new List<ListLatestVideo>();
                var itemsObjects = AllChildren(JObject.Parse(json))
                           .FirstOrDefault(c => c.Type == JTokenType.Array && c.Path.Contains("items"))
                           .Children<JObject>();

                foreach (JObject result in itemsObjects)
                {
                    var videoToBeAddedToTheList = new ListLatestVideo();
                    foreach (JProperty property in result.Properties())
                    {
                        var name = property.Name;
                        var val = property.Value.ToString();

                        switch (name)
                        {
                            case "title":
                                videoToBeAddedToTheList.Title = val;
                                break;

                            case "pubDate":
                                videoToBeAddedToTheList.PubDate = DateTime.ParseExact(val, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                break;

                            case "author":
                                videoToBeAddedToTheList.Author = val;
                                break;

                            case "link":
                                videoToBeAddedToTheList.Link = val;
                                break;

                            case "thumbnail":
                                videoToBeAddedToTheList.Thumbnail = val;
                                break;

                            default:
                                break;
                        }
                    }
                    listOfVideosItem.Add(videoToBeAddedToTheList);
                }
                return listOfVideosItem;
            }
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}

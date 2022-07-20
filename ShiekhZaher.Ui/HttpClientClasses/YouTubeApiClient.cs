using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Zaher.Ui.Extensions;
using Zaher.Ui.HttpClientClasses;

namespace Zaher.Ui.HttpClientClasses
{
    public class YouTubeApiClient
    {
        private HttpClient client;
        //private string channelID = "UCECu8pY7fhH3-orl-VIWyrA";
        //private string reqURL = "https://www.youtube.com/feeds/videos.xml?channel_id=";

        public YouTubeApiClient(HttpClient client)
        {
            this.client = client;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public YouTubeApiClient(HttpClient client, Uri uri) : this(client)
        {
            this.client.BaseAddress = uri;
        }

        public void Test()
        {
            var reqUrl = "https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Fwww.youtube.com%2Ffeeds%2Fvideos.xml%3Fchannel_id%3DUCECu8pY7fhH3-orl-VIWyrA";

            var json = new WebClient().DownloadString("https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Fwww.youtube.com%2Ffeeds%2Fvideos.xml%3Fchannel_id%3DUCECu8pY7fhH3-orl-VIWyrA");

            var listOfObjects = new List<object>();

            var listOfVideosItem = new List<VideoItems>();

            var itemsObjects = AllChildren(JObject.Parse(json))
            .FirstOrDefault(c => c.Type == JTokenType.Array && c.Path.Contains("items"))
            .Children<JObject>();

            foreach (JObject result in itemsObjects)
            {
                var videoToBeAddedToTHeList = new VideoItems();
                foreach (JProperty property in result.Properties())
                {
                    var name = property.Name;
                    var val = property.Value.ToString();

                    switch (name)
                    {
                        case "title":
                            videoToBeAddedToTHeList.title = val;
                            break;

                        case "pubDate":
                            videoToBeAddedToTHeList.pubDate = DateTime.ParseExact(val, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            break;

                        case "author":
                            videoToBeAddedToTHeList.author = val;
                            break;

                        case "link":
                            videoToBeAddedToTHeList.link = val;
                            break;

                        case "thumbnail":
                            videoToBeAddedToTHeList.thumbnail = val;
                            break;

                        default:
                            break;
                    }

                    // do something with the property belonging to result
                }
                listOfVideosItem.Add(videoToBeAddedToTHeList);
            }

            foreach (var item2 in itemsObjects.Properties())
            {
                var title = item2.Name;
                var val = item2.Value.ToString();
            }

            foreach (var item3 in itemsObjects)
            {
                var videoItem3 = new VideoItems()
                {
                };
            }

            foreach (var jsonobject in itemsObjects)
            {
                var deserilizedObject = JsonConvert.DeserializeObject(jsonobject.ToString());
                listOfObjects.Add(deserilizedObject);

                var childrenToken = deserilizedObject.GetType().GetProperty("ChildernTokens").GetValue(deserilizedObject);

                var title = childrenToken.GetType().GetProperty("title").GetValue(childrenToken);

                var author = childrenToken.GetType().GetProperty("author").GetValue(childrenToken);

                var VideoToBeAddedToTheList = new VideoItems()
                {
                    title = title.ToString(),
                    author = author.ToString()
                };
                listOfVideosItem.Add(VideoToBeAddedToTheList);
            }

            var resultOfGetTVideoItems = Get<VideoItems>(reqUrl).Result;
            var request = new HttpRequestMessage(HttpMethod.Get, reqUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.Send(request);

            response.EnsureSuccessStatusCode();

            var streamFromResponse = response.Content.ReadAsStream();
            var video = new YTVideo();

            var obj = streamFromResponse.WriteAndSerializeToJsonAsync<YTVideo>(video);

            var itemsOfObjectsStream = SerializeToStream(itemsObjects);

            var itemsOfObjectsDeserilized = itemsOfObjectsStream.ReadAndDeserializeFromJson<IEnumerable<VideoItems>>();

            var item = itemsObjects.FirstOrDefault().ToString();

            MemoryStream stream = SerializeToStream(item);

            var deserilizedObjectToVideo2 = stream.ReadAndDeserializeFromJson<VideoItems>();

            var deserilizedObjectToVideo = stream.ReadAndDeserializeFromJson<YTVideo>();

            var conv = JsonConvert.DeserializeObject(item);

            MemoryStream stream2 = SerializeToStream(conv);

            var itemee = itemsObjects.FirstOrDefault().CreateReader();

            //var content =  response.Content.ReadAsStringAsync().Result;

            //var mycontent = response.Content.ReadAsStream();

            //var model = mycontent.ReadAndDeserializeFromJson<YTVideo>();

            //content = content.TrimStart('\"');
            //content = content.TrimEnd('\"');
            //content = content.Replace("\\", "");

            //var video = new YTVideo(json);
        }

        public async Task<T> Get<T>(string route, string contentType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            //Do something
                        }
                    }

                    response.EnsureSuccessStatusCode();
                    return stream.ReadAndDeserializeFromJson<T>();
                }
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

        public static MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }
    }

    public class YTVideo
    {
        public VideoItems Items { get; set; }

        //public YTVideo(string json)
        //{
        //    JObject jObject = JObject.Parse(json);
        //    JToken jitems = jObject["items"];

        //}
    }

    public class VideoItems
    {
        public string author { get; set; }
        public string link { get; set; }
        public DateTime pubDate { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }

        //public VideoItems(string json)
        //{
        //    JObject jObject = JObject.Parse(json);
        //    JToken jvideo = jObject["items"];
        //    Author = (string)jvideo["author"];
        //    Link = (string)jvideo["link"];
        //    PubDate = (DateTime)jvideo["pubDate"];
        //    Title = (string)jvideo["title"];
        //    Thumbnail = (string)jvideo["thumbnail"];
        //}
    }
}

#region postmanstring

// var postmanString = "https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Fwww.youtube.com%2Ffeeds%2Fvideos.xml%3Fchannel_id%3DUCECu8pY7fhH3-orl-VIWyrA";

#endregion postmanstring

//var props = result.Properties();
//var proper = property;
//var name = property.Name;
//var value = property.Value;
//var path = property.Path;
//var arr = property.ToArray();
//var values = property.Values();
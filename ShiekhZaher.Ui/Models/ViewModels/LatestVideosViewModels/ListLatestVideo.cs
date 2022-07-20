using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Zaher.Ui.Models.ViewModels.LatestVideosViewModels
{
    public class ListLatestVideo

    {
        public string Author { get; set; }
        public string Link { get; set; }

        [Display(Name = "نشر في")]
        public DateTime PubDate { get; set; }

        public string DateInHijri => PubDate.Year < 1901 ? "0001-01-01 00:00 ص" : PubDate.ToString("HH:mm yyyy MMMM dd", new CultureInfo("ar-SA"));

        public string Title { get; set; }
        public string Thumbnail { get; set; }
    }
}

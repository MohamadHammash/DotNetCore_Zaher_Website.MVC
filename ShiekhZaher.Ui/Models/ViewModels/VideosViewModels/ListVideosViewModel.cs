using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Zaher.Core.Entities;

namespace Zaher.Ui.Models.ViewModels.VideosViewModels
{
    public class ListVideosViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان الفيديو")]
        public string Title { get; set; }

        [Display(Name = "رابط الفيديو")]
        public string URL { get; set; }

        [Display(Name = "نشر في")]
        public DateTime PublishingDate { get; set; }

        public string DateInHijri => PublishingDate.Year < 1901 ? "0001-01-01 00:00 ص" : PublishingDate.ToString("yyyy MMMM dd HH:mm tt", new CultureInfo("ar-SA"));

        [Display(Name = "قائمة التشغيل")]
        public Guid VideosListId { get; set; }

        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
        public VideosList VideosList { get; set; }
    }
}

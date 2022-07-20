using System;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;

namespace Zaher.Ui.Models.ViewModels.VideosViewModels
{
    public class EditVideoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "الرجاء كتابة عنوان الفيديو")]
        [Display(Name = "عنوان الفيديو")]
        public string Title { get; set; }

        [Required(ErrorMessage = "الرجاء اضافة رابط الفيديو")]
        [Display(Name = "رابط الفيديو")]
        public string URL { get; set; }

        [Display(Name = "نشر في")]
        public DateTime PublishingDate { get; set; }

        [Required]
        [Display(Name = "قائمة التشغيل")]
        public Guid? VideosListId { get; set; }

        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
        public VideosList VideosList { get; set; }
    }
}

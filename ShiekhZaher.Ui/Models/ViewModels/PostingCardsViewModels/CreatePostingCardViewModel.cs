using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;

namespace Zaher.Ui.Models.ViewModels.PostingCardsViewModels
{
    public class CreatePostingCardViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string Header { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [Display(Name = "المحتوى")]
        public string Content { get; set; }

        [Required(ErrorMessage = "الرجاء تحميل صورة")]
        [Display(Name = "الصورة")]
        public IFormFile Photo { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime PublishingDate { get; set; }
        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
    }
}

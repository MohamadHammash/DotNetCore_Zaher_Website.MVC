using System;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;

namespace Zaher.Ui.Models.ViewModels.PostingCardsViewModels
{
    public class EditPostingCardViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string Header { get; set; }

        [Display(Name = "المحتوى")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string Content { get; set; }

        [Display(Name = "الصورة")]
        public string ImagePath { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime PublishingDate { get; set; }
        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
    }
}

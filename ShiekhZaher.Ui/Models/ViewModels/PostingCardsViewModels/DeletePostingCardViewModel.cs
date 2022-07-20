using System;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;
using Zaher.Ui.Helpers;

namespace Zaher.Ui.Models.ViewModels.PostingCardsViewModels
{
    public class DeletePostingCardViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "العنوان")]
        public string Header { get; set; }

        [Display(Name = "المحتوى")]
        public string Content { get; set; }

        [Display(Name = "المحتوى")]
        public string StrippedContent => StripHtml.StripHTML(Content);

        [Display(Name = "الصورة")]
        public string ImagePath { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime PublishingDate { get; set; }
        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
    }
}

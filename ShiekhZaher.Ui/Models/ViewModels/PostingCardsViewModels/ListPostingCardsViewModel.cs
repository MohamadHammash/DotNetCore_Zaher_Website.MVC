using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Zaher.Core.Entities;
using Zaher.Ui.Helpers;

namespace Zaher.Ui.Models.ViewModels.PostingCardsViewModels
{
    public class ListPostingCardsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "العنوان")]
        public string Header { get; set; }

        [Display(Name = "المحتوى")]
        public string Content { get; set; }

        public string StrippedContent => !String.IsNullOrWhiteSpace(Content) ? StripHtml.StripHTML(Content) : " ";

        [Display(Name = "الصورة")]
        public string ImagePath { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime PublishingDate { get; set; }

        public string DateInHijri => PublishingDate.Year < 1901 ? "0001-01-01" : PublishingDate.ToString("yyyy MMMM dd", new CultureInfo("ar-SA"));

        public bool New => (DateTime.Now - PublishingDate).TotalDays <= 10 ? true : false;
        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }
    }
}

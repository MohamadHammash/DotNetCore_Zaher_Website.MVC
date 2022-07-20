using System;
using System.ComponentModel.DataAnnotations;
using Zaher.Ui.Helpers;

namespace Zaher.Ui.Models.ViewModels.QAsViewModels
{
    public class AnswerQAViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        [Display(Name = "الكنية")]
        public string LastName { get; set; }

        [Display(Name = "موضوع السؤال")]
        public string Subject { get; set; }

        [Display(Name = "السؤال")]
        public string Question { get; set; }
        [Display(Name = "الإجابة")]
        public string CaseNumber { get; set; }

        [EmailAddress]
        [Display(Name = "البريد الاكتروني ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [MinLength(4, ErrorMessage = "الرجاء أجب ب 4 أحرف كحد أدنى")]
        [Display(Name = "الإجابة")]
        public string Answer { get; set; }

        [Display(Name = "نشر في ")]
        public DateTime PublishingDate { get; set; }

        //public string Initials => $"{FirstName.FirstOrDefault()} {LastName.FirstOrDefault()}";
        public bool Answered => !String.IsNullOrWhiteSpace(Answer);

        public string StrippedAnswer => !String.IsNullOrWhiteSpace(Answer) ? StripHtml.StripHTML(Answer) : "";

        [Display(Name = "عنوان الباب")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public Guid SectionId { get; set; }
    }
}

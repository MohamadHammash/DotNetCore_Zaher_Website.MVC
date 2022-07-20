using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;

namespace Zaher.Ui.Models.ViewModels.QAsViewModels
{
    public class ListQAsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        [Display(Name = "الكنية")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "موضوع السؤال")]
        public string Subject { get; set; }

        [Display(Name = "السؤال")]
        public string Question { get; set; }

        [Display(Name = "رقم السؤال")]
        public string CaseNumber { get; set; }

        [Display(Name = "الإجابة")]
        public string Answer { get; set; }

        [EmailAddress]
        [Display(Name = "البريد الاكتروني ")]
        public string Email { get; set; }

        [Display(Name = "نشر في ")]
        public DateTime PublishingDate { get; set; }

        [Display(Name = "نشر في ")]
        public string DateInHijri => PublishingDate.Year < 1901 ? "0001-01-01 00:00 ص" : PublishingDate.ToString("yyyy MMMM dd HH:mm tt", new CultureInfo("ar-SA"));

        public string Initials => $"{FirstName.FirstOrDefault()} {LastName.FirstOrDefault()}";
        public bool Answered => !String.IsNullOrWhiteSpace(Answer);

        [Display(Name = "عنوان الباب")]
        public Guid SectionId { get; set; }

        public bool New => (DateTime.Now - PublishingDate).TotalDays <= 10 ? true : false;

        public ListSectionsViewModel Section { get; set; }
    }
}

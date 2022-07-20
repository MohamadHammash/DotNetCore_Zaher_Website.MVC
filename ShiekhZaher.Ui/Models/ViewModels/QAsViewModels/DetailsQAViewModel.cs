using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Zaher.Ui.Models.ViewModels.QAsViewModels
{
    public class DetailsQAViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        [EmailAddress]
        [Display(Name = "البريد الاكتروني ")]
        public string Email { get; set; }

        [Display(Name = "رقم السؤال")]
        public string CaseNumber { get; set; }

        [Display(Name = "نشر في ")]
        public DateTime PublishingDate { get; set; }

        public string Initials => $"{FirstName.FirstOrDefault()} {LastName.FirstOrDefault()}";
        public bool Answered => !String.IsNullOrWhiteSpace(Answer);

        [Display(Name = "عنوان الباب")]
        public Guid SectionId { get; set; }
    }
}

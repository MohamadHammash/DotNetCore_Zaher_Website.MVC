using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Zaher.Ui.Models.ViewModels.QAsViewModels
{
    public class EditQAViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [Display(Name = "الكنية")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [Display(Name = "موضوع السؤال")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        [Display(Name = "السؤال")]
        [StringLength(999, ErrorMessage = "حد الحروف المسموح هو 999 حرف فقط"), MinLength(10, ErrorMessage = "السؤال يجب أن يتكون من 10 حروف على الأقل")]
        public string Question { get; set; }

        [EmailAddress]
        [Display(Name = "البريد الاكتروني ")]
        public string Email { get; set; }

        [Display(Name = "رقم السؤال")]
        public string CaseNumber { get; set; }

        [Display(Name = "الإجابة")]
        public string Answer { get; set; }

        public string Initials => $"{FirstName.FirstOrDefault()} {LastName.FirstOrDefault()}";
        public bool Answered => !String.IsNullOrWhiteSpace(Answer);

        [Display(Name = "عنوان الباب")]
        [Required(ErrorMessage = "الرجاء اختيار القسم")]
        public Guid SectionId { get; set; }
    }
}

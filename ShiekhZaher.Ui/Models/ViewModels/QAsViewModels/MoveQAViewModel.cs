using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zaher.Ui.Models.ViewModels.QAsViewModels
{
    public class MoveQAViewModel
    {
        public Guid Id { get; set; }

        
        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        
        [Display(Name = "الكنية")]
        public string LastName { get; set; }

       
        [Display(Name = "موضوع السؤال")]
        public string Subject { get; set; }

       
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
        [Display(Name = "نشر في ")]
        public DateTime PublishingDate { get; set; }
        

        [Display(Name = "عنوان الباب")]
       
        public Guid SectionId { get; set; }
    }
}

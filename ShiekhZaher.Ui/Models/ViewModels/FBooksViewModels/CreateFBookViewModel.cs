using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;

namespace Zaher.Ui.Models.ViewModels.FBooksViewModels
{
    public class CreateFBookViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان القسم")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string Title { get; set; }

        [Display(Name = "الوصف")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string Description { get; set; }

        // nav prop
        //public ICollection<Section> Sections { get; set; }
        public ICollection<ListSectionsViewModel> Sections { get; set; }
    }
}

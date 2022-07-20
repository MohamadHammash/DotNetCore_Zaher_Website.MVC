using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;

namespace Zaher.Ui.Models.ViewModels.FBooksViewModels
{
    public class EditFBookViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان القسم")]
        public string Title { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        // nav prop
        //public ICollection<Section> Sections { get; set; }
        public ICollection<ListSectionsViewModel> Sections { get; set; }
    }
}

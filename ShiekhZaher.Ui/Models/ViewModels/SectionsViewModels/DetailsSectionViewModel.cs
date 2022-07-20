using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Models.ViewModels.SectionsViewModels
{
    public class DetailsSectionViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان الباب")]
        public string Title { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "عنوان القسم")]
        public Guid FBookId { get; set; }

        // nav props
        public FBook FBook { get; set; }

        //public ICollection<QA> QAs { get; set; }
        public ICollection<ListQAsViewModel> QAs { get; set; }

        public PaginationResult<ListQAsViewModel> QAsResult { get; set; }
    }
}

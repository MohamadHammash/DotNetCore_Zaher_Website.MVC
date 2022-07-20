using System;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Models.ViewModels.SectionsViewModels
{
    public class IndexSectionsViewModel
    {
        public Guid  FBookId { get; set; }
        public string FBookTitle { get; set; }
        public PaginationResult<ListQAsViewModel> QAs { get; set; }
        public PaginationResult<ListSectionsViewModel> Sections { get; set; }

        public string Type { get; set; }
        public string SearchBox { get; set; }
        public int Page { get; set; }

    }
}

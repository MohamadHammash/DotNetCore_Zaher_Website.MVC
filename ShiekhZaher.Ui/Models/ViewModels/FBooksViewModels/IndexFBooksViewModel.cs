using Zaher.Ui.Models.ViewModels.FBooksViewModels;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Models.ViewModels.FBooksViewModels
{
    public class IndexFBooksViewModel
    {
        public PaginationResult<ListQAsViewModel> QAs { get; set; }
        public PaginationResult<ListSectionsViewModel> Sections { get; set; }
        public PaginationResult<ListFBooksViewModel> FBooks { get; set; }
        public string Type { get; set; }
        public string SearchBox { get; set; }
        public int Page { get; set; }



    }
}

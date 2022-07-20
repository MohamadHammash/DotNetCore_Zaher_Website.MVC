using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Views.Shared.Components.Pagination
{
    public class PaginationViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PaginationResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}

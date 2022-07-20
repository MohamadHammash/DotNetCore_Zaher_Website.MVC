using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zaher.Ui.Services
{
    public interface ISelectServices
    {
        Task<IEnumerable<SelectListItem>> GetSectionsIdAsync();

        Task<IEnumerable<SelectListItem>> GetFBooksIdAsync();

        IEnumerable<SelectListItem> GetSearchType();

        IEnumerable<SelectListItem> GetSearchTypeSections();
        IEnumerable<SelectListItem> GetAvailableRoles();
    }
}

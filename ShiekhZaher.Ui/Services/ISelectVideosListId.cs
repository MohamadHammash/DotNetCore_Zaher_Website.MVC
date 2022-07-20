using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zaher.Ui.Services
{
    public interface ISelectVideosListId
    {
        Task<IEnumerable<SelectListItem>> GetVideosListIdAsync();
    }
}

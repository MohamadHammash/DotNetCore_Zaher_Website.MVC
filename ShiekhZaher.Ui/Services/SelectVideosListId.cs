using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Repositories;
using Zaher.Ui.Services;

namespace Zaher.Ui.Services
{
    public class SelectVideosListId : ISelectVideosListId
    {
        private readonly IUoW uoW;

        public SelectVideosListId(IUoW uoW)
        {
            this.uoW = uoW;
        }

        public async Task<IEnumerable<SelectListItem>> GetVideosListIdAsync()
        {
            var videosLists = await uoW.VideosListsRepository.GetAllVideosListsAsync();

            return videosLists.Select(vl => new SelectListItem
            {
                Text = vl.ListName.ToString(),
                Value = vl.Id.ToString()
            }).Distinct();
        }
    }
}

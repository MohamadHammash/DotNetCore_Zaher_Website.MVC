using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Repositories;
using Zaher.Ui.Helpers;
using Zaher.Ui.Services;

namespace Zaher.Ui.Services
{
    public class SelectServices : ISelectServices
    {
        private readonly IUoW uoW;

        public SelectServices(IUoW uoW)
        {
            this.uoW = uoW;
        }

        public async Task<IEnumerable<SelectListItem>> GetSectionsIdAsync()
        {
            var SectionsLists = await uoW.SectionsRepository.GetAllSectionsAsync();

            var retval = SectionsLists.Select(vl => new SelectListItem
            {
                Text = vl.Title.ToString(),
                Value = vl.Id.ToString(),
                Selected = false
            }).Distinct().ToList();

            retval.Insert(0, new SelectListItem
            {
                Text = "الرجاء اختيار الباب ليتم فرز السؤال",
                Value = string.Empty,
                Selected = true
            });

            return retval;
        }

        public async Task<IEnumerable<SelectListItem>> GetFBooksIdAsync()
        {
            var fBooksLists = await uoW.FBooksRepository.GetAllFBooksAsync();

            return fBooksLists.Select(vl => new SelectListItem
            {
                Text = vl.Title.ToString(),
                Value = vl.Id.ToString(),
                Selected = false
            }).Distinct();
        }

        public IEnumerable<SelectListItem> GetSearchType()
        {
            var list = new List<SelectListItem>
            {
               new SelectListItem {
                     Text = "ابحث في الأسئلة",
                Value = "QAs",
                Selected = true
                },
                 new SelectListItem {
                     Text = "ابحث في الأبواب",
                Value = "Sections",
                },
                new SelectListItem {
                     Text = "ابحث في الأقسام",
                Value = "FBooks",
                }
            };
            return list;
        }

        public IEnumerable<SelectListItem> GetSearchTypeSections()
        {
            return GetSearchType().Where(i => i.Value == "QAs" || i.Value == "Sections");
        }


        public IEnumerable<SelectListItem> GetAvailableRoles()
        {
            var list = new List<SelectListItem>
            {
                 new SelectListItem {
                     Text = "الرجاء اختيار مستوى الصلاحية",
                Value = "",
                Selected = true
                },
               new SelectListItem {
                     Text = RolesInArabic.Admin,
                Value =  "Admin",
                
                },
                 new SelectListItem {
                     Text =  RolesInArabic.Editor,
                Value ="Editor",
                },
                new SelectListItem {
                     Text = RolesInArabic.Moderator,
                Value ="Moderator",
                }
            };
            return list;
        }

    }
}

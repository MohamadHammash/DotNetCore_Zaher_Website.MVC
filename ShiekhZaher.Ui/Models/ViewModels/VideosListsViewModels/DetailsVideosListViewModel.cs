using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Models.ViewModels.VideosListsViewModels
{
    public class DetailsVideosListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "اسم القائمة")]
        public string ListName { get; set; }

        public string ImagePath { get; set; }

        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Video> Videos { get; set; }

        public PaginationResult<Video> VideosResult { get; set; }
    }
}

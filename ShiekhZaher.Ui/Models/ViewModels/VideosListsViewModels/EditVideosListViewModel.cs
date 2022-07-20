using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;
using Zaher.Ui.Models.ViewModels.VideosViewModels;

namespace Zaher.Ui.Models.ViewModels.VideosListsViewModels
{
    public class EditVideosListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "اسم القائمة")]
        [Required(ErrorMessage = "الرجاء تعبئة الحقل")]
        public string ListName { get; set; }

        public string ImagePath { get; set; }
        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }

        //public ICollection<Video> Videos { get; set; }
        public ICollection<ListVideosViewModel> Videos { get; set; }
    }
}

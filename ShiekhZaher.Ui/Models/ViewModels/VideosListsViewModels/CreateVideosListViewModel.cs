using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;
using Zaher.Ui.Models.ViewModels.VideosViewModels;

namespace Zaher.Ui.Models.ViewModels.VideosListsViewModels
{
    public class CreateVideosListViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "الرجاء كتابة اسم القائمة")]
        [Display(Name = "اسم القائمة")]
        public string ListName { get; set; }

        [Required(ErrorMessage = "الرجاء تحميل صورة")]
        [Display(Name = "الصورة")]
        public IFormFile Photo { get; set; }

        public string ApplicationUserId { get; set; }
        // Nav prop
        public ApplicationUser ApplicationUser { get; set; }

        //public ICollection<Video> Videos { get; set; }
        public ICollection<ListVideosViewModel> Videos { get; set; }
    }
}

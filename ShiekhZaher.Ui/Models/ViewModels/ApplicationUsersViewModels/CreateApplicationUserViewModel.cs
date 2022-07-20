using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Ui.Models.ViewModels.PostingCardsViewModels;
using Zaher.Ui.Models.ViewModels.VideosListsViewModels;
using Zaher.Ui.Models.ViewModels.VideosViewModels;

namespace Zaher.Ui.Models.ViewModels.ApplicationUsersViewModels
{
    public class CreateApplicationUserViewModel
    {
        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; }

        public string Role { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }
        public ICollection<ListVideosViewModel> Videos { get; set; }
        public ICollection<ListPostingCardsViewModel> PostingCards { get; set; }
        public ICollection<ListVideosListsViewModel> VideosLists { get; set; }



        //public ICollection<PostingCard> PostingCards { get; set; }
        //public ICollection<VideosList> VideosLists { get; set; }
    }
}

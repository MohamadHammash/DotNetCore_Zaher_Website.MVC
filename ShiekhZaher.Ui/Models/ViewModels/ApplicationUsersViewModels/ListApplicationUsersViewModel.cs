using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zaher.Core.Entities;

namespace Zaher.Ui.Models.ViewModels.ApplicationUsersViewModels
{
    public class ListApplicationUsersViewModel

    {
        public string Id { get; set; }
        [Display(Name = "الاسم")]
        public string FirstName { get; set; }

        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; }
        [Display(Name = "مستوى الصلاحية")]
        public string Role { get; set; }

        [Display(Name = "البريد الاكتروني")]
        public string Email { get; set; }
        [Phone]
        [Display(Name ="رقم الهاتف")]
        public string PhoneNumber { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Video> Videos { get; set; }
        public ICollection<PostingCard> PostingCards { get; set; }
        public ICollection<VideosList> VideosLists { get; set; }
    }
}

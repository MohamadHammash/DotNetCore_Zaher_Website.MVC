using System;
using System.Collections.Generic;
using Zaher.Core.Entities;

namespace Zaher.Core.Entities
{
    public class VideosList
    {
        public Guid Id { get; set; }
        public string ListName { get; set; }
        public string ImagePath { get; set; }
        public string ApplicationUserId { get; set; }

        // nav Prop
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}

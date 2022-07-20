using System;
using Zaher.Core.Entities;

namespace Zaher.Core.Entities
{
    public class PostingCard
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime PublishingDate { get; set; }

        //nav prop
        public ApplicationUser ApplicationUser { get; set; }
    }
}

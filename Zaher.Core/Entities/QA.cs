using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zaher.Core.Entities;

namespace Zaher.Core.Entities
{
    public class QA
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CaseNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime PublishingDate { get; set; }
        public string Initials => $"{FirstName.FirstOrDefault()} {LastName.FirstOrDefault()}";
        public bool Answered => !String.IsNullOrWhiteSpace(Answer);
        public Guid SectionId { get; set; }

        // nav prop
        public Section Section { get; set; }
    }
}

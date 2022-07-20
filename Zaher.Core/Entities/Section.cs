using System;
using System.Collections.Generic;
using Zaher.Core.Entities;

namespace Zaher.Core.Entities
{
    public class Section
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid FBookId { get; set; }

        // nav props
        public FBook FBook { get; set; }

        public ICollection<QA> QAs { get; set; }
    }
}

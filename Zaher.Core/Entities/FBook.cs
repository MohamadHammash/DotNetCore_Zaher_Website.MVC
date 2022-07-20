using System;
using System.Collections.Generic;
using Zaher.Core.Entities;

namespace Zaher.Core.Entities
{
    public class FBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // nav prop
        public ICollection<Section> Sections { get; set; }
    }
}

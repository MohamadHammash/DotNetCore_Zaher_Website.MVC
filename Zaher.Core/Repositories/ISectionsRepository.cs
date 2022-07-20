using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface ISectionsRepository
    {
        Task AddAsync(Section added);

        Task<IEnumerable<Section>> GetAllSectionsAsync(bool includeQAs = true);

        Task<Section> GetSectionByIdAsync(Guid id, bool includeQAs = true);

        void Update(Section fBook);

        void Remove(Section fBook);

        public bool Any(Guid id);
    }
}

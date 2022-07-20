using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface IFBooksRepository
    {
        Task AddAsync(FBook added);

        Task<IEnumerable<FBook>> GetAllFBooksAsync(bool includeSectionsAndQAs = true);

        Task<FBook> GetFBookByIdAsync(Guid id, bool includeSectionsAndQAs = true);

        void Update(FBook fBook);

        void Remove(FBook fBook);

        Task<IEnumerable<QA>> GetAllQAsInAnFBookAsync(Guid fBookId);

        Task<IEnumerable<Section>> GetAllSectionsInAnFBookAsync(Guid fBookId);

        public bool Any(Guid id);
    }
}

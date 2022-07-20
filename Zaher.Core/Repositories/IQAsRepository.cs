using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface IQAsRepository
    {
        Task AddAsync(QA added);

        Task<IEnumerable<QA>> GetAllQAsAsync(bool includeSections = false);

        Task<QA> GetQAByIdAsync(Guid id, bool includeSections = false);

        void Update(QA qA);

        void Remove(QA qA);

        public bool Any(Guid id);
    }
}

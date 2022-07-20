using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Data.Data;

namespace Zaher.Data.Repositories
{
    public class QAsRepository : IQAsRepository
    {
        private readonly ApplicationDbContext db;

        public QAsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(QA added)
        {
            await db.QAs.AddAsync(added);
        }

        public bool Any(Guid id)
        {
            return db.QAs.Any(q => q.Id == id);
        }

        public async Task<IEnumerable<QA>> GetAllQAsAsync(bool includeSections = false)
        {
            return
                includeSections ?
                await db.QAs
                .Include(q => q.Section)
                .OrderByDescending(q => q.PublishingDate)
                .ToListAsync()
                :
            await db.QAs
            .OrderByDescending(q => q.PublishingDate)
            .ToListAsync();
        }

        public async Task<QA> GetQAByIdAsync(Guid id, bool includeSections = false)
        {
            return
                includeSections ?
                await db.QAs
                .Include(q => q.Section)
                .FirstOrDefaultAsync(q => q.Id == id) :
                await db.QAs.FindAsync(id);
        }

        public void Remove(QA qA)
        {
            db.QAs.Remove(qA);
        }

        public void Update(QA qA)
        {
            db.QAs.Update(qA);
        }
    }
}

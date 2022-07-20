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
    public class FBooksRepository : IFBooksRepository
    {
        private readonly ApplicationDbContext db;

        public FBooksRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(FBook added)
        {
            await db.FBooks.AddAsync(added);
        }

        public bool Any(Guid id)
        {
            return db.FBooks.Any(f => f.Id == id);
        }

        public async Task<IEnumerable<FBook>> GetAllFBooksAsync(bool includeSectionsAndQAs = true)
        {
            return
                includeSectionsAndQAs ?
                await db.FBooks
                .Include(f => f.Sections)
                .ThenInclude(s => s.QAs)
                .ToListAsync()
                :
               await db.FBooks.ToListAsync();
        }

        public async Task<FBook> GetFBookByIdAsync(Guid id, bool includeSectionsAndQAs = true)
        {
            return
                 includeSectionsAndQAs ?

                await db.FBooks
                 .Include(f => f.Sections)
                .ThenInclude(s => s.QAs)
                .FirstOrDefaultAsync(f => f.Id == id)
                :
                  await db.FBooks.FindAsync(id);
        }

        public void Remove(FBook fBook)
        {
            db.FBooks.Remove(fBook);
        }

        public void Update(FBook fBook)
        {
            db.FBooks.Update(fBook);
        }

        public async Task<IEnumerable<Section>> GetAllSectionsInAnFBookAsync(Guid fBookId)
        {
            return await db.Sections
                .Include(s => s.QAs)
                .Where(s => s.FBook.Id == fBookId).ToListAsync();
        }

        public async Task<IEnumerable<QA>> GetAllQAsInAnFBookAsync(Guid fBookId)
        {
            return await db.QAs.Where(q => q.Section.FBook.Id == fBookId).ToListAsync();
        }
    }
}

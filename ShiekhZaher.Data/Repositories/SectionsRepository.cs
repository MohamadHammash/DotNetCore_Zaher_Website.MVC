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
    public class SectionsRepository : ISectionsRepository
    {
        private readonly ApplicationDbContext db;

        public SectionsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(Section added)
        {
            await db.Sections.AddAsync(added);
        }

        public bool Any(Guid id)
        {
            return db.Sections.Any(s => s.Id == id);
        }

        public async Task<IEnumerable<Section>> GetAllSectionsAsync(bool includeQAs = true)
        {
            return includeQAs ?
                    await db.Sections
                   .Include(s => s.QAs)
                   .ToListAsync()
                   :
                await db.Sections
                   .ToListAsync();
        }

        public async Task<Section> GetSectionByIdAsync(Guid id, bool includeQAs = true)
        {
            return includeQAs ?
                await db.Sections
              .Include(s => s.QAs)
                .FirstOrDefaultAsync(s => s.Id == id)
                :
                await db.Sections
                .FindAsync(id);
        }

        public void Remove(Section section)
        {
            db.Sections.Remove(section);
        }

        public void Update(Section section)
        {
            db.Sections.Update(section);
        }

        public async Task<IEnumerable<QA>> GetAllQAsInAnFBookAsync(Guid sectionId)
        {
            return await db.QAs.Where(q => q.Section.Id == sectionId).ToListAsync();
        }
    }
}

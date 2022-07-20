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
    public class VideosListsRepository : IVideosListsRepository
    {
        private readonly ApplicationDbContext db;

        public VideosListsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(VideosList added)
        {
            await db.VideosLists.AddAsync(added);
        }

        public bool Any(Guid id)
        {
            return db.VideosLists.Any(vl => vl.Id == id);
        }

        public async Task<IEnumerable<VideosList>> GetAllVideosListsAsync()
        {
            return await db.VideosLists.Include(vl => vl.Videos)

                 .ToListAsync();
        }

        public async Task<VideosList> GetVideosListByIdAsync(Guid id)
        {
            return await db.VideosLists.Include(vl => vl.Videos)

                .FirstOrDefaultAsync(vl => vl.Id == id);
        }

        public void Remove(VideosList videos)
        {
            if (videos.Videos is not null)
            {
                db.Videos.RemoveRange(videos.Videos);
            }
            db.VideosLists.Remove(videos);
        }

        public void Update(VideosList videos)
        {
            db.VideosLists.Update(videos);
        }
    }
}

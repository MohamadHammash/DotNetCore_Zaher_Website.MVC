using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface IVideosRepository
    {
        Task AddAsync(Video added);

        Task<IEnumerable<Video>> GetAllVideosAsync();

        Task<Video> GetVideoByIdAsync(Guid id);

        void Update(Video video);

        void Remove(Video video);

        public bool Any(Guid id);
    }
}

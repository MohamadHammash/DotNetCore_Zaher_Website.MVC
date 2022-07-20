using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface IPostingCardsRepository
    {
        Task AddAsync(PostingCard added);

        Task<IEnumerable<PostingCard>> GetAllPostingCardsAsync();

        Task<PostingCard> GetPostingCardByIdAsync(Guid id);

        void Update(PostingCard card);

        void Remove(PostingCard card);

        public bool Any(Guid id);
    }
}

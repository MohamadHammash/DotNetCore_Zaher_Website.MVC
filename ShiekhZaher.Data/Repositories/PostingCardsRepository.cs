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
    public class PostingCardsRepository : IPostingCardsRepository
    {
        private readonly ApplicationDbContext db;

        public PostingCardsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync(PostingCard added)
        {
            await db.PostingCards.AddAsync(added);
        }

        public bool Any(Guid id)
        {
            return db.PostingCards.Any(p => p.Id == id);
        }

        public async Task<IEnumerable<PostingCard>> GetAllPostingCardsAsync()
        {
            return await db.PostingCards
                .OrderByDescending(p => p.PublishingDate)
                 .ToListAsync();
        }

        public async Task<PostingCard> GetPostingCardByIdAsync(Guid id)
        {
            return await db.PostingCards

                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(PostingCard card)
        {
            db.PostingCards.Remove(card);
        }

        public void Update(PostingCard card)
        {
            db.PostingCards.Update(card);
        }
    }
}

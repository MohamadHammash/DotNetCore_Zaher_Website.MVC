using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Data.Data;

namespace Zaher.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                if (db.PostingCards.Any())
                {
                    return;
                }
                var postingCards = GetPostingCards();
                await db.AddRangeAsync(postingCards);
                await db.SaveChangesAsync();
            }
        }

        private static List<PostingCard> GetPostingCards()
        {
            var postingCards = new List<PostingCard>();

            for (int i = 0; i < 30; i++)
            {
                var fake = new Faker("ar");
                var postingCard = new PostingCard
                {
                    Header = fake.Company.CatchPhrase(),
                    Content = fake.Lorem.Text(),
                    PublishingDate = fake.Date.Recent()
                };
                postingCards.Add(postingCard);
            }
            return postingCards;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Data.Data;
using Zaher.Data.Repositories;

namespace Zaher.Data.Repositories
{
    public class UoW : IUoW
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> userManager;
        public IPostingCardsRepository PostingCardsRepository { get; set; }
        public IVideosListsRepository VideosListsRepository { get; set; }
        public IVideosRepository VideosRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IQAsRepository QAsRepository { get; set; }
        public ISectionsRepository SectionsRepository { get; set; }
        public IFBooksRepository FBooksRepository { get; set; }

        public UoW(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            PostingCardsRepository = new PostingCardsRepository(this.db);
            VideosListsRepository = new VideosListsRepository(this.db);
            VideosRepository = new VideosRepository(this.db);
            QAsRepository = new QAsRepository(this.db);
            SectionsRepository = new SectionsRepository(this.db);
            FBooksRepository = new FBooksRepository(this.db);
            UsersRepository = new UsersRepository(this.db, userManager);
        }

        public async Task CompleteAsync() => await db.SaveChangesAsync();

        public async Task<bool> SaveAsync() => (await db.SaveChangesAsync()) >= 0;
    }
}

using System.Threading.Tasks;
using Zaher.Core.Repositories;

namespace Zaher.Core.Repositories
{
    public interface IUoW
    {
        public IPostingCardsRepository PostingCardsRepository { get; set; }
        public IVideosListsRepository VideosListsRepository { get; set; }
        public IVideosRepository VideosRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        public IQAsRepository QAsRepository { get; set; }
        public ISectionsRepository SectionsRepository { get; set; }
        public IFBooksRepository FBooksRepository { get; set; }

        Task<bool> SaveAsync();

        Task CompleteAsync();
    }
}

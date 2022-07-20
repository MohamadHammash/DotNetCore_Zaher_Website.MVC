using System.Collections.Generic;
using System.Threading.Tasks;
using Zaher.Core.Entities;

namespace Zaher.Core.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(bool includeContent = false);

        string GetRole(ApplicationUser user);

        Task<ApplicationUser> GetUserByIdAsync(string id, bool includeContent = false);

        void Update(ApplicationUser user);

        void Remove(ApplicationUser user);

        public bool Any(string id);

        Task ChangeRoleAsync(ApplicationUser user);
    }
}

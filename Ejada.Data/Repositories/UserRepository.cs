using Ejada.Data.Repositories.GenericRepository;
using Ejada.Entities.IdentityModels;
using Ejada.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Ejada.Shared.Helpers;
using Ejada.Shared;
using X.PagedList;

namespace Ejada.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private EjadaContext _context;

        public UserRepository(EjadaContext context) : base(context)
        {
            _context = context;

        }


    }
}

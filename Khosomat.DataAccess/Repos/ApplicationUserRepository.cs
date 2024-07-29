using Khosomat.Entities.Interfaces;
using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.DataAccess.Repos
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
	{
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}

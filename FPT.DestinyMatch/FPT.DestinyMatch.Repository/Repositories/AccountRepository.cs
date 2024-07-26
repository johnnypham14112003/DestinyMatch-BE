using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public AccountRepository(DestinyMatchContext context) : base(context)
        {
        }

        //**************************[ METHODS ]**************************
        public async Task<Account?> GetValidAccountByEmail(string email)
        {
            var list = await GetAsync().Include(x => x.Member).FirstOrDefaultAsync(acc =>
            acc.Email.Equals(email) &&
            !acc.Status.ToLower().Equals("deleted"));
            return list;
        }
        public async Task<Account?> GetByIdIncludeMember(Guid accountId)
        {
            var acc = await DMDB.Accounts
                .Include(a => a.Member).ThenInclude(mem => mem.University)
                .Include(a => a.Member).ThenInclude(mem => mem.Major)
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.Id == accountId);
            return acc; 
        }

        public async Task<IEnumerable<Account>> GetListAsync(int amountItem, int pageIndex,
            string? keyword, bool sortByDate, string? statusSearch, string? roleSearch, bool sortDescending)
        {
            var query = DMDB.Accounts
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(acc => acc.Email.ToLower().Contains(keyword.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleSearch))
            {
                query = query.Where(acc => acc.Role.ToLower().Equals(roleSearch.ToLower()));
            }

            if (!string.IsNullOrEmpty(statusSearch))
            {
                query = query.Where(acc => acc.Status.ToLower().Equals(statusSearch.ToLower()));
            }

            // Sort by date if specified
            if (sortByDate==false)
            {
                query = sortDescending == true ?
                    query.OrderByDescending(acc => acc.Email) : query.OrderBy(acc => acc.Email);
            }
            else
            {
                query = sortDescending == true ?
                    query.OrderByDescending(acc => acc.CreateAt) : query.OrderBy(acc => acc.CreateAt);
            }

            // Apply paging
            var pagedAccounts = await query
                .Skip((pageIndex - 1) * amountItem)
                .Take(amountItem)
                .ToListAsync();

            return pagedAccounts;
        }
    }
}

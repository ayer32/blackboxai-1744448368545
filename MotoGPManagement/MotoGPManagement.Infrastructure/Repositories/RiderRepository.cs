using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoGPManagement.Core.Interfaces;
using MotoGPManagement.Core.Models;
using MotoGPManagement.Infrastructure.Data;

namespace MotoGPManagement.Infrastructure.Repositories
{
    public class RiderRepository : BaseRepository<Rider>, IRiderRepository
    {
        public RiderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Rider> GetRiderWithStatsAsync(Guid riderId)
        {
            return await _dbSet
                .Include(r => r.CurrentTeam)
                .Include(r => r.CareerHistory)
                    .ThenInclude(ch => ch.Team)
                .Include(r => r.PerformanceStats.Where(ps => ps.Season == DateTime.UtcNow.Year))
                .Include(r => r.RaceResults.Where(rr => rr.Race.StartTime.Year == DateTime.UtcNow.Year))
                    .ThenInclude(rr => rr.Race)
                .FirstOrDefaultAsync(r => r.Id == riderId);
        }

        public async Task<IEnumerable<RiderPerformanceStats>> GetRiderStatsBySeasonAsync(Guid riderId, int season)
        {
            return await _context.RiderPerformanceStats
                .Where(rps => rps.RiderId == riderId && rps.Season == season)
                .Include(rps => rps.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .ToListAsync();
        }

        public async Task<IEnumerable<RaceResult>> GetRiderResultsAsync(Guid riderId, int? season = null)
        {
            var query = _context.RaceResults
                .Where(rr => rr.RiderId == riderId);

            if (season.HasValue)
            {
                query = query.Where(rr => rr.Race.StartTime.Year == season.Value);
            }

            return await query
                .Include(rr => rr.Race)
                .Include(rr => rr.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .OrderByDescending(rr => rr.Race.StartTime)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Rider>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.CurrentTeam)
                .Include(r => r.PerformanceStats.Where(ps => ps.Season == DateTime.UtcNow.Year))
                .OrderBy(r => r.LastName)
                .ThenBy(r => r.FirstName)
                .ToListAsync();
        }

        public override async Task<Rider> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(r => r.CurrentTeam)
                .Include(r => r.PerformanceStats.Where(ps => ps.Season == DateTime.UtcNow.Year))
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}

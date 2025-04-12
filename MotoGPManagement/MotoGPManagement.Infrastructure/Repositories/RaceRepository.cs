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
    public class RaceRepository : BaseRepository<Race>, IRaceRepository
    {
        public RaceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Race>> GetUpcomingRacesAsync()
        {
            return await _dbSet
                .Where(r => r.StartTime > DateTime.UtcNow && r.Status == RaceStatus.Scheduled)
                .OrderBy(r => r.StartTime)
                .Include(r => r.TechnicalFlags)
                .ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRacesBySeasonAsync(int season)
        {
            return await _dbSet
                .Where(r => r.StartTime.Year == season)
                .Include(r => r.Results)
                    .ThenInclude(rr => rr.Rider)
                .Include(r => r.TechnicalFlags)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<Race> GetRaceWithDetailsAsync(Guid raceId)
        {
            return await _dbSet
                .Include(r => r.Results)
                    .ThenInclude(rr => rr.Rider)
                        .ThenInclude(rider => rider.CurrentTeam)
                .Include(r => r.TechnicalFlags)
                .FirstOrDefaultAsync(r => r.Id == raceId);
        }

        public async Task UpdateRaceStatusAsync(Guid raceId, RaceStatus status)
        {
            var race = await GetByIdAsync(raceId);
            if (race != null)
            {
                race.Status = status;
                await UpdateAsync(race);
            }
        }
    }
}

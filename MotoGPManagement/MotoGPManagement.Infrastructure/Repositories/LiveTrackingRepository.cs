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
    public class LiveTrackingRepository : BaseRepository<LiveRaceTracking>, ILiveTrackingRepository
    {
        public LiveTrackingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LiveRaceTracking>> GetCurrentRaceTrackingAsync(Guid raceId)
        {
            var latestTimestamp = await _dbSet
                .Where(lt => lt.RaceId == raceId)
                .MaxAsync(lt => lt.Timestamp);

            return await _dbSet
                .Where(lt => lt.RaceId == raceId && lt.Timestamp == latestTimestamp)
                .Include(lt => lt.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .OrderBy(lt => lt.CurrentPosition)
                .ToListAsync();
        }

        public async Task UpdateRiderPositionAsync(Guid raceId, Guid riderId, LiveRaceTracking tracking)
        {
            tracking.RaceId = raceId;
            tracking.RiderId = riderId;
            tracking.Timestamp = DateTime.UtcNow;

            await AddAsync(tracking);
        }

        public async Task<IEnumerable<LiveRaceTracking>> GetRiderTrackingHistoryAsync(Guid raceId, Guid riderId)
        {
            return await _dbSet
                .Where(lt => lt.RaceId == raceId && lt.RiderId == riderId)
                .Include(lt => lt.Rider)
                    .ThenInclude(r => r.CurrentTeam)
                .OrderByDescending(lt => lt.Timestamp)
                .Take(20) // Get last 20 tracking points
                .ToListAsync();
        }
    }
}

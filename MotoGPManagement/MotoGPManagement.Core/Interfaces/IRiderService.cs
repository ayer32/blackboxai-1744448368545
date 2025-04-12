using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoGPManagement.Application.DTOs;
using MotoGPManagement.Core.Models;

namespace MotoGPManagement.Core.Interfaces
{
    public interface IRiderService
    {
        // Rider Management
        Task<RiderDto> GetRiderByIdAsync(Guid riderId);
        Task<IEnumerable<RiderDto>> GetAllRidersAsync();
        Task<RiderDto> CreateRiderAsync(CreateRiderDto createRiderDto);
        Task<RiderDto> UpdateRiderAsync(UpdateRiderDto updateRiderDto);
        Task DeleteRiderAsync(Guid riderId);
        Task<RiderDto> GetRiderWithStatsAsync(Guid riderId);
        Task<RiderDto> UpdateRiderStatusAsync(Guid riderId, RiderStatus status);

        // Career History
        Task<RiderCareerHistoryDto> AddCareerHistoryAsync(AddRiderCareerHistoryDto historyDto);
        Task<IEnumerable<RiderCareerHistoryDto>> GetRiderCareerHistoryAsync(Guid riderId);
        Task UpdateCareerHistoryAsync(Guid historyId, AddRiderCareerHistoryDto historyDto);
        Task<IEnumerable<RiderCareerHistoryDto>> GetRiderTeamHistoryAsync(Guid riderId);

        // Performance Stats
        Task<RiderPerformanceStatsDto> GetRiderStatsAsync(Guid riderId, int season);
        Task<RiderPerformanceStatsDto> UpdateRiderStatsAsync(UpdateRiderStatsDto statsDto);
        Task<IEnumerable<RiderPerformanceStatsDto>> GetRiderHistoricalStatsAsync(Guid riderId);
        Task<Dictionary<string, int>> GetRiderChampionshipStandingsAsync(int season);

        // Race Results
        Task<IEnumerable<RaceResultDto>> GetRiderResultsAsync(Guid riderId, int? season = null);
        Task<RaceResultDto> GetRiderRaceResultAsync(Guid riderId, Guid raceId);
        Task<Dictionary<string, int>> GetRiderPointsProgressionAsync(Guid riderId, int season);
        Task<Dictionary<string, object>> GetRiderSeasonComparisonAsync(Guid riderId, int season1, int season2);

        // Performance Analysis
        Task<RiderSeasonStatsDto> GetRiderSeasonAnalysisAsync(Guid riderId, int season);
        Task<Dictionary<string, double>> GetRiderPerformanceMetricsAsync(Guid riderId, int season);
        Task<Dictionary<string, object>> GetRiderTeamComparisonAsync(Guid riderId, Guid teamId1, Guid teamId2);
        Task<IEnumerable<RaceResultDto>> GetRiderPodiumFinishesAsync(Guid riderId, int? season = null);

        // Team Related
        Task<TeamDto> GetCurrentTeamAsync(Guid riderId);
        Task<bool> AssignToTeamAsync(Guid riderId, Guid teamId, DateTime startDate);
        Task<bool> RemoveFromTeamAsync(Guid riderId, DateTime endDate);
        Task<IEnumerable<TeamDto>> GetEligibleTeamsForRiderAsync(Guid riderId);

        // Achievements and Records
        Task<Dictionary<string, int>> GetRiderAchievementsAsync(Guid riderId);
        Task<Dictionary<string, object>> GetRiderRecordsAsync(Guid riderId);
        Task<IEnumerable<RaceResultDto>> GetRiderBestResultsAsync(Guid riderId, int limit = 10);
        Task<Dictionary<string, double>> GetRiderTrackPerformanceAsync(Guid riderId, string trackName);
    }
}

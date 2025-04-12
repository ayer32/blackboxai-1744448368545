using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoGPManagement.Application.DTOs;
using MotoGPManagement.Core.Models;

namespace MotoGPManagement.Core.Interfaces
{
    public interface IFanEngagementService
    {
        // Fan Management
        Task<FanDto> GetFanByIdAsync(Guid fanId);
        Task<IEnumerable<FanDto>> GetAllFansAsync();
        Task<FanDto> GetFanWithPredictionsAsync(Guid fanId);
        Task<FanDto> UpdateFanProfileAsync(Guid fanId, FanDto fanDto);
        Task DeleteFanAsync(Guid fanId);

        // Race Predictions
        Task<RacePredictionDto> CreatePredictionAsync(CreateRacePredictionDto predictionDto);
        Task<IEnumerable<RacePredictionDto>> GetFanPredictionsAsync(Guid fanId);
        Task<RacePredictionDto> GetPredictionAsync(Guid predictionId);
        Task<IEnumerable<RacePredictionDto>> GetRacePredictionsAsync(Guid raceId);
        Task<Dictionary<string, int>> GetPredictionStatsAsync(Guid fanId);

        // Fantasy Teams
        Task<FantasyTeamDto> CreateFantasyTeamAsync(CreateFantasyTeamDto teamDto);
        Task<FantasyTeamDto> GetFantasyTeamAsync(Guid teamId);
        Task<FantasyTeamDto> GetFanFantasyTeamAsync(Guid fanId, int season);
        Task<bool> UpdateFantasyTeamRidersAsync(Guid teamId, List<Guid> riderIds);
        Task<Dictionary<string, int>> GetFantasyTeamStatsAsync(Guid teamId);
        Task<IEnumerable<FantasyTeamDto>> GetTopFantasyTeamsAsync(int season, int limit = 10);

        // Comments and Interaction
        Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto);
        Task<CommentDto> AddCommentReplyAsync(CreateCommentReplyDto replyDto);
        Task<IEnumerable<CommentDto>> GetRaceCommentsAsync(Guid raceId);
        Task<bool> UpdateCommentStatusAsync(Guid commentId, CommentStatus status);
        Task<bool> LikeCommentAsync(Guid commentId, Guid fanId);
        Task<bool> UnlikeCommentAsync(Guid commentId, Guid fanId);

        // Fan Engagement Analytics
        Task<Dictionary<string, int>> GetFanEngagementMetricsAsync(Guid fanId);
        Task<Dictionary<string, object>> GetFanActivitySummaryAsync(Guid fanId);
        Task<IEnumerable<RacePredictionDto>> GetTopPredictorsAsync(int season, int limit = 10);
        Task<Dictionary<string, int>> GetFanLeaderboardAsync(int limit = 100);

        // Notifications
        Task<bool> SubscribeToRaceUpdatesAsync(Guid fanId, Guid raceId);
        Task<bool> UnsubscribeFromRaceUpdatesAsync(Guid fanId, Guid raceId);
        Task<bool> SubscribeToRiderUpdatesAsync(Guid fanId, Guid riderId);
        Task<bool> UnsubscribeFromRiderUpdatesAsync(Guid fanId, Guid riderId);
        Task<IEnumerable<string>> GetFanSubscriptionsAsync(Guid fanId);
    }
}

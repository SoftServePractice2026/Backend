using  Application.DTOs;
using Shared;

namespace Application.Services.Session
{
        public interface ISessionService
        {
                Task<Result<Guid>> CreateSessionAsync(SessionCreateDto dto, CancellationToken cancellationToken);

                Task<Result<SessionListItemDto>> UpdateSessionAsync(
                        Guid targetId,
                        SessionUpdateDto dto,
                        CancellationToken cancellationToken);

                Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken);
                Task<Result<SessionListItemDto>> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
                Task<Result<List<SessionListItemDto>>> GetSessionAllAsync(CancellationToken cancellationToken);
                
                Task<Result<SessionFilterResultDto>> GetFilteredSessionsAsync(
                        SessionFilterDto filter, CancellationToken cancellationToken);
                
        }
}
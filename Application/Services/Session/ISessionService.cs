using  Application.DTOs;
using Shared;

namespace Application.Services.Session
{
        public interface ISessionService
        {
                Task<Result<Guid>> CreateSessionAsync(SessionCreateDto dto, CancellationToken cancellationToken);

                Task<Result<SessionCardDto>> UpdateSessionAsync(
                        Guid targetId,
                        SessionUpdateDto dto,
                        CancellationToken cancellationToken);

                Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken);
                Task<Result<SessionCardDto>> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
                Task<Result<List<SessionCardDto>>> GetSessionAllAsync(CancellationToken cancellationToken);
        }
}
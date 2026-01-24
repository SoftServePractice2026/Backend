using Application.DTOs;
namespace Application.Services.Session;

using Application.DTOs;
using Shared;

public interface ISessionService
{
        Task<Result<Guid>> CreateSessionAsync(SessionDtos.SessionCreateDto dto);
        Task<Result<SessionDtos.SessionCardDto>> UpdateSessionAllAsync(Guid targetId, SessionDtos.SessionUpdateAllDto dto);
        Task<Result<SessionDtos.SessionCardDto>> UpdateSessionTimeAsync(Guid targetId, SessionDtos.SessionUpdateTimeDto dto);
        Task<Result<SessionDtos.SessionCardDto>> UpdateSessionStatusAsync(Guid targetId, SessionDtos.SessionUpdateStatusDto dto);
        Task<Result<SessionDtos.SessionCardDto>> UpdateSessionPriceAsync(Guid targetId, SessionDtos.SessionUpdatePriceDto dto);

        Task<Result<bool>> DeleteSessionAsync(Guid id);

        Task<Result<SessionDtos.SessionCardDto>> GetSessionByIdAsync(Guid id);
        Task<Result<List<SessionDtos.SessionCardDto>>> GetSessionAllAsync();
}

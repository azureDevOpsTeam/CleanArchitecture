using ApplicationLayer.ViewModels;
using DomainLayer.Entities;

namespace ApplicationLayer.Extensions.Utilities
{
    public interface IRefreshTokenService
    {
        ServiceResult AddRefreshToken(RefreshToken refreshToken);

        ServiceResult UpdateRefreshToken(RefreshToken refreshToken);

        RefreshToken RefreshTokenGenerator(int userId, string tokenId);

        Task<ServiceResult> VerifyToken(TokenRequestViewModel tokenRequest);

        ServiceResult RemoveExpiredTokensFromDatabase();

        Task<ServiceResult> RevokeRefreshTokenByUserId(int userId);
    }
}
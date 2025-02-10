using ApplicationLayer.ViewModels;
using DomainLayer.Entities;

namespace ApplicationLayer.BusinessLogic.Interfaces
{
    public interface IRefreshTokenService
    {
        ServiceResult AddRefreshToken(RefreshToken refreshToken);

        RefreshToken RefreshTokenGenerator(int userId, string tokenId);

        ServiceResult RemoveExpiredTokensFromDatabase();

        Task<ServiceResult> RevokeRefreshTokenByUserId(int userId);

        ServiceResult UpdateRefreshToken(RefreshToken refreshToken);

        Task<ServiceResult> VerifyToken(TokenRequestViewModel tokenRequest);
    }
}
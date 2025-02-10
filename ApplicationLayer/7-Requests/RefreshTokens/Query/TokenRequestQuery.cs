using ApplicationLayer.ViewModels;
using MediatR;
using System.Text.Json.Serialization;

namespace ApplicationLayer.Requests.RefreshTokens.Query
{
    public class TokenRequestQuery : IRequest<HandlerResult>
    {
        public TokenRequestViewModel InputData { get; }

        [JsonConstructor]
        public TokenRequestQuery(TokenRequestViewModel inputData) => InputData = inputData;
    }
}
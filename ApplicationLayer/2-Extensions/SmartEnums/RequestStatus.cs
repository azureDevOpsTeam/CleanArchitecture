#region Usings

using Ardalis.SmartEnum;

#endregion end of Usings

namespace ApplicationLayer.Extensions.SmartEnums
{
    public sealed class RequestStatus : SmartEnum<RequestStatus>
    {
        #region Fields

        public static RequestStatus Successful = new(nameof(Successful), 0);

        public static RequestStatus Failed = new(nameof(Failed), 1);

        public static RequestStatus AuthenticationFailed = new(nameof(AuthenticationFailed), 2);

        public static RequestStatus Exists = new(nameof(Exists), 3);

        public static RequestStatus NotFound = new(nameof(NotFound), 4);

        public static RequestStatus RowUsed = new(nameof(RowUsed), 5);

        public static RequestStatus Duplicated = new(nameof(Duplicated), 6);

        public static RequestStatus ValidationFailed = new(nameof(ValidationFailed), 7);

        public static RequestStatus ConcurrencyIssue = new(nameof(ConcurrencyIssue), 8);

        public static RequestStatus ExpiredToken = new(nameof(ExpiredToken), 9);

        public static RequestStatus IncorrectUser = new(nameof(IncorrectUser), 10);

        public static RequestStatus InvalidToken = new(nameof(InvalidToken), 11);

        public static RequestStatus RefreshNotRequired = new(nameof(RefreshNotRequired), 12);

        public static RequestStatus ParcelRecivedFleet = new(nameof(ParcelRecivedFleet), 13);

        #endregion Fields

        #region Methods

        #region Constructors

        public RequestStatus(string name, int value) : base(name, value)
        {
        }

        #endregion Constructors

        #endregion Methods
    }
}
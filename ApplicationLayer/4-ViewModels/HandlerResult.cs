#region Usings

using ApplicationLayer.Common.Validations;
using ApplicationLayer.Extensions.SmartEnums;

#endregion

namespace ApplicationLayer
{
    public class HandlerResult
    {
        public HandlerResult(Type type)
        {
        }

        #region Fields

        public string FormUniqueId { get; }

        public ValidationResult ValidationResult { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        private string _message;
        private object _data;
        private RequestStatus _requestStatus;

        #endregion Fields

        #region Properties

        public NotificationType NotificationType => GetNotificationType(RequestStatus);

        #endregion Properties

        #region Methods

        private static NotificationType GetNotificationType(RequestStatus requestStatus)
        {
            return (int)requestStatus switch
            {
                var SuccessfulRow when SuccessfulRow.Equals(RequestStatus.Successful) => NotificationType.Success,
                var failedRow when failedRow.Equals(RequestStatus.Failed) => NotificationType.Error,
                _ => NotificationType.Warning,
            };
        }

        #endregion
    }

    public class HandlerResult<T> : HandlerResult
    {
        public HandlerResult() : base(typeof(T))
        {
        }
    }
}
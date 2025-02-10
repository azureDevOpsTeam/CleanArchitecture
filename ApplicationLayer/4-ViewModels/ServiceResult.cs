#region Usings

using ApplicationLayer.Extensions.ServiceMessages;
using ApplicationLayer.Extensions.SmartEnums;

#endregion

namespace ApplicationLayer
{
    public class ServiceResult
    {
        #region Fields

        private string _message;

        #endregion Fields

        #region Properties

        public string ErrorCode { get; set; }

        public string Message
        {
            get => string.IsNullOrEmpty(_message) ? string.Empty : _message;
            set => _message = value;
        }

        public object Data { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public NotificationType NotificationType => GetNotificationType(RequestStatus);

        #endregion Properties

        #region Methods

        private static NotificationType GetNotificationType(RequestStatus requestStatus)
        {
            switch (requestStatus)
            {
                case var SuccessfulRow when SuccessfulRow.Equals(RequestStatus.Successful):
                    return NotificationType.Success;

                case var failedRow when failedRow.Equals(RequestStatus.Failed):
                    return NotificationType.Error;

                default:
                    return NotificationType.Warning;
            }
        }

        public ServiceResult Failed(object data = null)
        {
            return new ServiceResult()
            {
                Data = data,
                RequestStatus = RequestStatus.Failed,
                Message = CommonMessages.Failed
            };
        }

        public ServiceResult Successful(object data = null)
        {
            return new ServiceResult()
            {
                Data = data,
                RequestStatus = RequestStatus.Successful,
                Message = CommonMessages.Successful
            };
        }

        #endregion Methods
    }

    public class ServiceResult<T>
    {
        #region Fields

        private string _message;

        #endregion Fields

        #region Properties

        public string ErrorCode { get; set; }

        public string Message
        {
            get => string.IsNullOrEmpty(_message) ? string.Empty : _message;
            set => _message = value;
        }

        public T Data { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public NotificationType NotificationType => GetNotificationType(RequestStatus);

        #endregion Properties

        #region Methods

        private static NotificationType GetNotificationType(RequestStatus requestStatus)
        {
            switch (requestStatus)
            {
                case var SuccessfulRow when SuccessfulRow.Equals(RequestStatus.Successful):
                    return NotificationType.Success;

                case var failedRow when failedRow.Equals(RequestStatus.Failed):
                    return NotificationType.Error;

                default:
                    return NotificationType.Warning;
            }
        }

        public ServiceResult<T> Failed(T data = default)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                RequestStatus = RequestStatus.Failed,
                Message = CommonMessages.Failed
            };
        }

        public ServiceResult<T> Successful(T data = default)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                RequestStatus = RequestStatus.Successful,
                Message = CommonMessages.Successful
            };
        }

        #endregion Methods
    }
}
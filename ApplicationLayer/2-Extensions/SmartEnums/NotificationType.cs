#region Usings

using Ardalis.SmartEnum;

#endregion end of Usings

namespace ApplicationLayer.Extensions.SmartEnums
{
    public class NotificationType : SmartEnum<NotificationType, byte>
    {
        #region Fields

        public static NotificationType Error = new(nameof(Error).ToLower(), 0);
        public static NotificationType Info = new(nameof(Info).ToLower(), 1);
        public static NotificationType Question = new(nameof(Question).ToLower(), 2);
        public static NotificationType Success = new(nameof(Success).ToLower(), 3);
        public static NotificationType Warning = new(nameof(Warning).ToLower(), 4);

        #endregion Fields

        #region Methods

        #region Constructors

        public NotificationType(string value, byte id) : base(value, id)
        {
        }

        #endregion Constructors

        #endregion Methods
    }
}
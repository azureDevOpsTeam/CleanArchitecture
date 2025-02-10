using Ardalis.SmartEnum;

namespace ApplicationLayer.Extensions.SmartEnums
{
    public class ApiDefinitions : SmartEnum<ApiDefinitions, byte>
    {
        #region Fields

        public static ApiDefinitions Mobile = new(nameof(Mobile), 0);
        public static ApiDefinitions ExternalService = new(nameof(ExternalService), 1);
        public static ApiDefinitions Users = new(nameof(Users), 2);
        public static ApiDefinitions Admin = new(nameof(Admin), 3);

        #endregion Fields

        #region Methods

        #region Constructors

        public ApiDefinitions(string value, byte id) : base(value, id)
        {
        }

        #endregion Constructors

        #endregion Methods
    }
}
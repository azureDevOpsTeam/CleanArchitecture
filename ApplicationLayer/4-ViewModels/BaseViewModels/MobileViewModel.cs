#region Usings

using System.Text.Json.Serialization;

#endregion

namespace ApplicationLayer.ViewModels.BaseViewModels
{
    public class MobileViewModel
    {
        #region Properties

        public string Mobile { get; set; }

        [JsonIgnore]
        public string Message { get; set; }

        [JsonIgnore]
        public string SecurityCode { get; set; }

        #endregion Properties
    }
}
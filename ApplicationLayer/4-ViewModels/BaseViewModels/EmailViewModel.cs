#region Usings

using System.Text.Json.Serialization;

#endregion

namespace ApplicationLayer.ViewModels.BaseViewModels
{
    public class EmailViewModel
    {
        #region Properties

        public string Email { get; set; }

        [JsonIgnore]
        public string SecurityCode { get; set; }

        #endregion
    }
}
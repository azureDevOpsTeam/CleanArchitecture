#region Usings

using System.Text.Json.Serialization;

#endregion

namespace ApplicationLayer.ViewModels.BaseViewModels
{
    public class CaptchaValueViewModel
    {
        public string CaptchaValue { get; set; }

        [JsonIgnore]
        public string DefaultCaptcha { get; set; }
    }
}
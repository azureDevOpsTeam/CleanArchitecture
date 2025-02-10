using System.ComponentModel;

namespace ApplicationLayer.ViewModels.BaseViewModels
{
    public class UserAccountViewModel
    {
        [DefaultValue(null)]
        public List<int> UserAccountIds { get; set; }
    }
}
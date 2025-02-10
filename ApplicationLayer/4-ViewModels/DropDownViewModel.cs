using System.Text.Json.Serialization;

namespace ApplicationLayer.ViewModels
{
    public class DropDownViewModel
    {
        #region Properties

        public List<DropDownItemViewModel> ListItems { get; set; }

        #endregion Properties
    }

    public class DropDownItemViewModel
    {
        #region Properties

        [JsonIgnore]
        public string Text { get; set; }

        public string Label { get => Text; }

        public string Value { get; set; }

        #endregion Properties
    }
}
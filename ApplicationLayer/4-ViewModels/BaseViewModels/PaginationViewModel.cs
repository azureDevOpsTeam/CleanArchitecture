namespace ApplicationLayer.ViewModels.BaseViewModels
{
    public class PaginationViewModel
    {
        private int _Page;

        public int Page
        {
            get => _Page;
            set => _Page = value == 0 ? 1 : value;
        }

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value <= 0 ? 10 : (value > 100 ? 100 : value);
            }
        }
    }
}
namespace InfrastructureLayer.Helpers
{
    public class DisposeAction : IDisposable
    {
        #region Fields

        public static readonly DisposeAction Empty = new DisposeAction(null);
        private Action _action;

        #endregion Fields

        #region Methods

        #region Constructors

        public DisposeAction(Action action)
        {
            _action = action;
        }

        #endregion Constructors

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }

        #endregion Methods
    }
}
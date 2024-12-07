namespace AoC2024.Visualization;

public abstract class ViewModelBase : NotifyPropertyChanged
{
    private bool _disposed;

    private readonly SynchronizationContext _synchronizationContext;
    private readonly List<ViewModelBase> _children;

    public ViewModelBase()
    {
        _synchronizationContext = SynchronizationContext.Current!;
        _children = new List<ViewModelBase>();
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                foreach (var child in _children)
                {
                    child.Dispose();
                }
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    protected void Synchronize(Action action)
    {
        _synchronizationContext.Post(new SendOrPostCallback((_) => action()), null);
    }

    protected TViewModel RegisterChild<TViewModel>(Func<TViewModel> viewModelFactory)
        where TViewModel : ViewModelBase
    {
        var result = viewModelFactory();
        _children.Add(result);
        return result;
    }
}

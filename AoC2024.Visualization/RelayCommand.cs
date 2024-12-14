using System.Globalization;
using System.Windows.Input;

namespace AoC2024.Visualization;

/// <summary>
/// Generic implementation of <see cref="ICommand"/>.
/// </summary>
/// <typeparam name="T">The type argument which this command will be based on.</typeparam>
[Serializable]
public class RelayCommand<T> : IRelayCommand<T>
{
    #region Fields

    private Action<T?> _execute;
    private Predicate<T?> _canExecute;

    private event EventHandler? _canExecuteChangedInternal;

    private readonly SynchronizationContext _synchronizationContext;

    #endregion

    #region Events

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            _canExecuteChangedInternal += value;
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
            _canExecuteChangedInternal -= value;
        }
    }

    #endregion

    #region Constructor(s)/destructor

    public RelayCommand(Action<T?> execute)
        : this(execute, x => true) // default CanExecute returns true
    { }

    public RelayCommand(Action<T?> execute, Predicate<T?> canExecute)
    {
        _synchronizationContext = SynchronizationContext.Current!;

        _execute = execute ?? throw new ArgumentNullException("execute");
        _canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Returns if the validation for executing the command has succeeded.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the execution of the command.</param>
    /// <returns>A boolean value that indicates whether validation has succeeded.</returns>
    public bool CanExecute(T? parameter)
    {
        return _canExecute != null && _canExecute(parameter);
    }

    /// <summary>
    /// Executes the action that is associated with this command.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the execution of the command.</param>
    public void Execute(T? parameter)
    {
        Synchronize(() => _execute(parameter));
    }

    /// <summary>
    /// Resets this command instance.
    /// </summary>
    public void Destroy()
    {
        _canExecute = c => false;
        _execute = e => { return; };
    }

    #endregion

    #region Protected methods

    protected void OnCanExecuteChanged()
    {
        var handler = _canExecuteChangedInternal;
        if (handler != null)
        {
            Synchronize(() => handler(this, EventArgs.Empty));
        }
    }

    protected void Synchronize(Action action)
    {
        _synchronizationContext.Post(new SendOrPostCallback((_) => action()), null);
    }

    #endregion

    #region Private methods

    bool ICommand.CanExecute(object? parameter)
    {
        return parameter is T ?
            CanExecute((T)parameter) :
            (parameter is IConvertible ?
                CanExecute((T)Convert.ChangeType(parameter, typeof(T), CultureInfo.InvariantCulture)) :
                CanExecute(default)
                )
            ;
    }

    void ICommand.Execute(object? parameter)
    {
        if (parameter is T)
        {
            Execute((T)parameter);
        }
        else if (parameter is IConvertible)
        {
            Execute((T)Convert.ChangeType(parameter, typeof(T)));
        }
        else
        {
            Execute(default);
        }
    }

    #endregion
}

/// <summary>
/// Non-generic <see cref="RelayCommand"/> uses <see cref="RelayCommand{T}"/> where T is <see cref="object"/>.
/// </summary>
[Serializable]
public class RelayCommand : RelayCommand<object>
{
    #region Constructor(s)/destructor

    public RelayCommand(Action<object?> execute)
        : base(execute, x => true) // default CanExecute returns true
    { }

    public RelayCommand(Action execute)
        : this(new Action<object?>(x => execute()))
    { }

    public RelayCommand(Action<object?> execute, Predicate<object?> canExecute)
        : base(execute, canExecute)
    { }

    public RelayCommand(Action execute, Func<bool> canExecute)
        : this(new Action<object?>(x => execute()), new Predicate<object?>(x => canExecute()))
    { }

    #endregion
}

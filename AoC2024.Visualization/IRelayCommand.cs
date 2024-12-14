using System.Windows.Input;

namespace AoC2024.Visualization;

/// <summary>
/// Interface for the generic implementation of <see cref="ICommand"/>.
/// </summary>
/// <typeparam name="T">The type argument which this command will be based on.</typeparam>
public interface IRelayCommand<T> : ICommand
{
    #region Methods

    /// <summary>
    /// Returns if the validation for executing the command has succeeded.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the execution of the command.</param>
    /// <returns>A boolean value that indicates whether validation has succeeded.</returns>
    bool CanExecute(T parameter);

    /// <summary>
    /// Executes the action that is associated with this command.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the execution of the command.</param>
    void Execute(T parameter);

    /// <summary>
    /// Resets this command instance.
    /// </summary>
    void Destroy();

    #endregion
}

public interface IRelayCommand : IRelayCommand<object>
{ }

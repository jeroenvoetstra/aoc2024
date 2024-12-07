using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AoC2024.Visualization;

public abstract class NotifyPropertyChanged : INotifyPropertyChanged, IDisposable
{
    private bool _disposed;

    public event PropertyChangedEventHandler? PropertyChanged;

    ~NotifyPropertyChanged()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // DISPOSE
            }

            _disposed = true;
        }
    }

    protected virtual bool SetValue<T>(ref T target, T value, [CallerMemberName] string? propertyName = null)
    {
        var result = false;

        if ((target == null && value != null)
            || (target != null && value == null)
            || (target != null && value != null && !target.Equals(value))
            )
        {
            result = true;
            target = value;
            RaisePropertyChanged(propertyName);
        }

        return result;
    }

    protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
        RaisePropertyChanged(GetMemberName(propertyExpression));
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(propertyName);
    }

    protected static string GetMemberName<T>(Expression<Func<T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"{nameof(propertyExpression)} should represent access to a member");
        }

        return memberExpression.Member.Name;
    }

    private void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

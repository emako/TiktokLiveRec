using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentAvaloniaUI.Violeta.Mvvm;

public class ReactiveCollection<T> : ObservableCollection<T>
{
    public ReactiveCollection()
    {
    }

    public ReactiveCollection(IEnumerable<T> collection) : base(collection)
    {
    }

    public ReactiveCollection(IList<T> list) : base(list)
    {
    }

    public ReactiveCollection(ICollection<T> collection) : base(collection)
    {
    }

    public ReactiveCollection(IQueryable<T> queryable) : base(queryable)
    {
    }

    protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
        RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    public virtual void AddRange(IEnumerable<T> range)
    {
        foreach (T item in range)
        {
            Items.Add(item);
        }

        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public virtual void Reset(IEnumerable<T> range)
    {
        Items.Clear();
        AddRange(range);
    }

    public virtual void Remove(Func<T, bool> predicate)
    {
        T[] array = [.. this.Where(predicate)];
        foreach (T item in array)
        {
            Remove(item);
        }
    }

    public void MoveUp(T item)
    {
        int num = IndexOf(item);
        if (num > 0)
        {
            Move(num, num - 1);
        }
    }

    public void MoveDown(T item)
    {
        int num = IndexOf(item);
        if (num >= 0 && num < Count - 1)
        {
            Move(num, num + 1);
        }
    }
}

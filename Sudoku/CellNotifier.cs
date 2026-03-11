using System;
using System.Collections.Generic;

namespace Sudoku;

public class CellNotifier : IObservable<Cell>
{
    private readonly List<IObserver<Cell>> _observers = new();

    public IDisposable Subscribe(IObserver<Cell> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubscriber(_observers, observer);
    }

    public void Notify(Cell cell)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(cell);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<Cell>> _observers;
        private readonly IObserver<Cell> _observer;

        public Unsubscriber(List<IObserver<Cell>> observers, IObserver<Cell> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
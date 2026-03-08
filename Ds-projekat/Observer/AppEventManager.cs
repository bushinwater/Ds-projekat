using System;
using System.Collections.Generic;

namespace Ds_projekat.Observer
{
    internal sealed class AppEventManager : IAppSubject
    {
        private static readonly Lazy<AppEventManager> _instance =
            new Lazy<AppEventManager>(() => new AppEventManager());

        public static AppEventManager Instance => _instance.Value;

        private readonly List<IAppObserver> _observers = new List<IAppObserver>();

        private AppEventManager() { }

        public void Attach(IAppObserver observer)
        {
            if (observer == null || _observers.Contains(observer))
                return;

            _observers.Add(observer);
        }

        public void Detach(IAppObserver observer)
        {
            if (observer == null)
                return;

            _observers.Remove(observer);
        }

        public void Notify(AppNotification notification)
        {
            foreach (IAppObserver observer in _observers.ToArray())
            {
                observer.Update(notification);
            }
        }
    }
}

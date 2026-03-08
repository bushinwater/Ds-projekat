namespace Ds_projekat.Observer
{
    internal interface IAppSubject
    {
        void Attach(IAppObserver observer);
        void Detach(IAppObserver observer);
        void Notify(AppNotification notification);
    }
}

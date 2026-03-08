namespace Ds_projekat.Observer
{
    internal class ReportLogObserver : IAppObserver
    {
        private readonly Form1 _form;

        public ReportLogObserver(Form1 form)
        {
            _form = form;
        }

        public void Update(AppNotification notification)
        {
            _form?.AppendObserverMessage(notification);
        }
    }
}

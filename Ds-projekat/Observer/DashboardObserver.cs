namespace Ds_projekat.Observer
{
    internal class DashboardObserver : IAppObserver
    {
        private readonly Form1 _form;

        public DashboardObserver(Form1 form)
        {
            _form = form;
        }

        public void Update(AppNotification notification)
        {
            _form?.RefreshDashboardFromObserver(notification);
        }
    }
}

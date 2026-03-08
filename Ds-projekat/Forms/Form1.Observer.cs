using Ds_projekat.Observer;
using System;
using System.Windows.Forms;

namespace Ds_projekat
{
    public partial class Form1
    {
        private DashboardObserver _dashboardObserver;
        private ReportLogObserver _reportLogObserver;

        private void InitializeObserverPattern()
        {
            _dashboardObserver = new DashboardObserver(this);
            _reportLogObserver = new ReportLogObserver(this);

            AppEventManager.Instance.Attach(_dashboardObserver);
            AppEventManager.Instance.Attach(_reportLogObserver);

            AppEventManager.Instance.Notify(
                new AppNotification(
                    "System",
                    "Init",
                    "Observer pattern je aktiviran. Dashboard i Reports sada automatski reaguju na promene u GUI-u."
                )
            );
        }


        private void NotifyObservers(string module, string action, string message)
        {
            AppEventManager.Instance.Notify(new AppNotification(module, action, message));
        }

        internal void RefreshDashboardFromObserver(AppNotification notification)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => RefreshDashboardFromObserver(notification)));
                return;
            }

            LoadDashboardData();
        }

        internal void AppendObserverMessage(AppNotification notification)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AppendObserverMessage(notification)));
                return;
            }

            if (txtReportStatus == null)
                return;

            txtReportStatus.AppendText(notification + Environment.NewLine);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            AppEventManager.Instance.Detach(_dashboardObserver);
            AppEventManager.Instance.Detach(_reportLogObserver);
            base.OnFormClosed(e);
        }
    }
}

using System;

namespace Ds_projekat.Observer
{
    internal class AppNotification
    {
        public string Module { get; }
        public string Action { get; }
        public string Message { get; }
        public DateTime Timestamp { get; }

        public AppNotification(string module, string action, string message)
        {
            Module = module ?? "System";
            Action = action ?? "Update";
            Message = message ?? string.Empty;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Timestamp:dd.MM.yyyy HH:mm:ss}] [{Module}/{Action}] {Message}";
        }
    }
}

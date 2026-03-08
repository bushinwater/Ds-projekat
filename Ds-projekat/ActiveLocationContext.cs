using System;

namespace Ds_projekat
{
    // Jednostavan shared state za trenutno aktivnu lokaciju u admin panelu.
    internal sealed class ActiveLocationContext
    {
        private static readonly Lazy<ActiveLocationContext> _instance =
            new Lazy<ActiveLocationContext>(() => new ActiveLocationContext());

        public static ActiveLocationContext Instance => _instance.Value;

        public int ActiveLocationId { get; private set; }
        public string ActiveLocationName { get; private set; } = "";

        public event EventHandler ActiveLocationChanged;

        private ActiveLocationContext()
        {
        }

        public void SetActiveLocation(int locationId, string locationName)
        {
            ActiveLocationId = locationId;
            ActiveLocationName = locationName ?? "";
            ActiveLocationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

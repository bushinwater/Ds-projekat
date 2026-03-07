using System.Collections.Generic;

namespace Ds_projekat.Export
{
    internal interface IReportExporter
    {
        void ExportUsers(string filePath, List<User> users);
        void ExportReservations(string filePath, List<Reservation> reservations);
        void ExportResources(string filePath, List<Resource> resources);
        void ExportLocations(string filePath, List<Location> locations);
        void ExportMembershipTypes(string filePath, List<MembershipType> membershipTypes);
    }
}
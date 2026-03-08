using Ds_projekat.Services;
using System;

namespace Ds_projekat.Commands
{
    // Command pattern: svaka export akcija ima svoj Execute.
    internal interface IReportCommand
    {
        ServiceResult Execute(string filePath);
    }

    internal class ReportCommandFactory
    {
        private readonly ReportFacade _reportFacade;

        public ReportCommandFactory(ReportFacade reportFacade)
        {
            _reportFacade = reportFacade;
        }

        public IReportCommand Create(string reportType, DateTime selectedMonth)
        {
            switch (reportType)
            {
                case "users":
                    return new ExportUsersCommand(_reportFacade);
                case "resources":
                    return new ExportResourcesCommand(_reportFacade);
                case "locations":
                    return new ExportLocationsCommand(_reportFacade);
                case "memberships":
                    return new ExportMembershipsCommand(_reportFacade);
                case "monthly":
                    return new ExportMonthlyUsageCommand(_reportFacade, selectedMonth.Year, selectedMonth.Month);
                default:
                    return new ExportReservationsCommand(_reportFacade);
            }
        }
    }

    internal sealed class ExportUsersCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        public ExportUsersCommand(ReportFacade reportFacade) => _reportFacade = reportFacade;
        public ServiceResult Execute(string filePath) => _reportFacade.ExportUsersToCsv(filePath);
    }

    internal sealed class ExportResourcesCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        public ExportResourcesCommand(ReportFacade reportFacade) => _reportFacade = reportFacade;
        public ServiceResult Execute(string filePath) => _reportFacade.ExportResourcesToCsv(filePath);
    }

    internal sealed class ExportLocationsCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        public ExportLocationsCommand(ReportFacade reportFacade) => _reportFacade = reportFacade;
        public ServiceResult Execute(string filePath) => _reportFacade.ExportLocationsToCsv(filePath);
    }

    internal sealed class ExportMembershipsCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        public ExportMembershipsCommand(ReportFacade reportFacade) => _reportFacade = reportFacade;
        public ServiceResult Execute(string filePath) => _reportFacade.ExportMembershipTypesToCsv(filePath);
    }

    internal sealed class ExportReservationsCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        public ExportReservationsCommand(ReportFacade reportFacade) => _reportFacade = reportFacade;
        public ServiceResult Execute(string filePath) => _reportFacade.ExportAllReservationsToCsv(filePath);
    }

    internal sealed class ExportMonthlyUsageCommand : IReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly int _year;
        private readonly int _month;

        public ExportMonthlyUsageCommand(ReportFacade reportFacade, int year, int month)
        {
            _reportFacade = reportFacade;
            _year = year;
            _month = month;
        }

        public ServiceResult Execute(string filePath)
        {
            return _reportFacade.ExportMonthlyUsageReportToCsv(filePath, _year, _month);
        }
    }
}

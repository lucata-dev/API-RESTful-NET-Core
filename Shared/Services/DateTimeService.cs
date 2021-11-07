using Application.Interfaces;
using System;

namespace Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUTC => DateTime.UtcNow;
    }
}

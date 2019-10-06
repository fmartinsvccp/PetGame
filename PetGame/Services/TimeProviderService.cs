using System;
using PetGame.Services.Interface;

namespace PetGame.Services
{
    public class TimeProviderService : ITimeProviderService
    {
        public int MinutesSinceLastUpdate(DateTimeOffset lastUpdate)
        {
            return (int)DateTimeOffset.UtcNow.Subtract(lastUpdate).TotalMinutes;
        }
    }
}

using System;

namespace PetGame.Services.Interface
{
    public interface ITimeProviderService
    {
        int MinutesSinceLastUpdate(DateTimeOffset lastUpdate);
    }
}

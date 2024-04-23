namespace Auth.Core.Interfaces.Integration;

public interface ISyncProducer : IDisposable
{
    Task ProduceAsync<T>(string topic, T value);
}
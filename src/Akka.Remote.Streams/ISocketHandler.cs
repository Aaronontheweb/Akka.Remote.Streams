using Akka.Actor;

namespace Akka.Remote.Streams;

/// <summary>
/// Why was the connection terminated?
/// </summary>
public enum TerminationReason
{
    WeInitiatedShutdown,
    TheyInitiatedShutdown,
    Aborted,
    Error,
    Unknown
}

public interface ISocketHandler : IEquatable<ISocketHandler>
{
    /// <summary>
    /// Completes when the connection is terminated
    /// </summary>
    public Task<TerminationReason> WhenTerminated { get; }

    /// <summary>
    /// Indicates that the read handler is set
    /// </summary>
    public Task<Done> WhenReadOpen { get; }
        
    public Address RemoteAddress { get; }
        
    public Address LocalAddress { get; }
}
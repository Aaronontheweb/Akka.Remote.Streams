using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
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

public abstract class BaseConnectionContext : IAsyncDisposable
{
    /// <summary>
    /// Completes when the connection is terminated
    /// </summary>
    public virtual Task<TerminationReason> WhenTerminated { get; set; }
    
    /// <summary>
    /// Aborts the underlying connection.
    /// </summary>
    public abstract void Abort();

    /// <summary>
    /// Indicates that the read handler is set
    /// </summary>
    public virtual Task<Done> WhenReadOpen { get; set; }
        
    public virtual EndPoint? RemoteAddress { get; set; }
        
    public virtual EndPoint? LocalAddress { get; set; }

    public virtual ValueTask DisposeAsync()
    {
        return default;
    }
}

internal abstract class TransportConnection : BaseConnectionContext
{
    public IDuplexPipe Application { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the <see cref="IDuplexPipe"/> that can be used to read or write data on this connection.
    /// </summary>
    public abstract IDuplexPipe Transport { get; set; }

    public override void Abort()
    {
        Debug.Assert(Application != null);
        Application.Input.CancelPendingRead();
    }
}
namespace NemtPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore.Storage;
using NemtPlatform.Application.Contracts.Persistence;

/// <summary>
/// Implementation of the Unit of Work pattern using Entity Framework Core.
/// Coordinates database operations and manages transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly NemtPlatformDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(NemtPlatformDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of state entries written to the database.
    /// </returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins a new database transaction.
    /// All subsequent operations will be part of this transaction until committed or rolled back.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a transaction is already in progress.</exception>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction, persisting all changes made during the transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is in progress.</exception>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction, discarding all changes made during the transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is in progress.</exception>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Disposes the Unit of Work and releases all resources.
    /// Rolls back any active transaction before disposal.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected implementation of Dispose pattern.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                _context.Dispose();
            }

            _disposed = true;
        }
    }
}

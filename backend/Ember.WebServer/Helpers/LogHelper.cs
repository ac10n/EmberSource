using System.Diagnostics;
using Ember.Domain.Helpers;
using Ember.Domain.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Helpers;

public class LogHelper(IServiceProvider serviceProvider) : ILogHelper
{
    public IScopedLog<TRecord> BeginLogScope<TRecord, TArgs>(TArgs args)
        where TRecord : IScopedLogRecord
        where TArgs : ILogRecordCreator<TRecord>
    {
        return LogImpl<TRecord, TArgs>.Create(args, serviceProvider);
    }
}

public class LogImpl<TRecord, TArgs>(TArgs args, IServiceProvider serviceProvider) : IScopedLog<TRecord>, ICreate<TRecord, TArgs>
    where TArgs : ILogRecordCreator<TRecord>
    where TRecord : IScopedLogRecord
{
    private readonly Stopwatch stopwatch = Stopwatch.StartNew();
    public TRecord Record { get; private set; } = args.CreateLogRecord(serviceProvider.GetService<IRequestLogContext>()?.RequestLogId);

    public static LogImpl<TRecord, TArgs> Create(TArgs args, IServiceProvider serviceProvider)
    {
        var logImpl = new LogImpl<TRecord, TArgs>(args, serviceProvider);
        logImpl.Record.StartTime = DateTime.UtcNow;
        return logImpl;
    }

    public async ValueTask DisposeAsync()
    {
        await Persist();
    }

    public async Task Persist()
    {
        Record.Duration = stopwatch.Elapsed;
        stopwatch.Stop();

        var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<EmberDbContext>>();
        using var dbContext = new EmberDbContext(dbContextOptions);
        dbContext.Add(Record);
        await dbContext.SaveChangesAsync();
    }
}

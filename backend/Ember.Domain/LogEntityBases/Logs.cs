using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.Domain.LogEntityBases;

public interface IScopedLogRecord
{
    Guid? RequestId { get; set; }
    DateTimeOffset StartTime { get; set; }
    TimeSpan Duration { get; set; }
}

public interface ILogRecordCreator<TRecord>
{
    TRecord CreateLogRecord(Guid? requestId);
}

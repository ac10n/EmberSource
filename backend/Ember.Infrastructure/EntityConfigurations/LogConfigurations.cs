using Ember.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.EntityConfigurations;

public sealed class ActionLogConfiguration : IEntityTypeConfiguration<ActionLog>
{
    public void Configure(EntityTypeBuilder<ActionLog> builder)
    {
        builder.ToTable("ActionLogs", "LOG");

        builder.HasIndex(x => x.RequestId);
        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Request)
            .WithMany()
            .HasForeignKey(x => x.RequestId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}

public sealed class InteractionLogConfiguration : IEntityTypeConfiguration<InteractionLog>
{
    public void Configure(EntityTypeBuilder<InteractionLog> builder)
    {
        builder.ToTable("InteractionLogs", "LOG");

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}

public sealed class ProcessLogConfiguration : IEntityTypeConfiguration<ProcessLog>
{
    public void Configure(EntityTypeBuilder<ProcessLog> builder)
    {
        builder.ToTable("ProcessLogs", "LOG");

        builder.HasIndex(x => x.RequestId);

        builder.HasOne(x => x.Request)
            .WithMany()
            .HasForeignKey(x => x.RequestId);
    }
}

public sealed class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.ToTable("RequestLogs", "LOG");

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}

public sealed class ResponseLogConfiguration : IEntityTypeConfiguration<ResponseLog>
{
    public void Configure(EntityTypeBuilder<ResponseLog> builder)
    {
        builder.ToTable("ResponseLogs", "LOG");

        builder.HasIndex(x => x.RequestLogId);

        builder.HasOne(x => x.RequestLog)
            .WithMany()
            .HasForeignKey(x => x.RequestLogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using AiKnowledgeAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Persistence.Configurations
{
    public sealed class JobExecutionEntityConfiguration: IEntityTypeConfiguration<JobExecutionEntity>
    {
        public void Configure(EntityTypeBuilder<JobExecutionEntity> builder)
        {
            builder.ToTable("job_executions");

            builder.HasKey(x => x.JobId);

            builder.Property(x => x.JobId)
                .HasColumnName("job_id")
                .IsRequired();

            builder.Property(x => x.WorkflowId)
                .HasColumnName("workflow_id")
                .IsRequired();

            builder.Property(x => x.Environment)
                .HasColumnName("environment")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(x => x.ExecutedAt)
                .HasColumnName("executed_at")
                .IsRequired();

            builder.HasIndex(x =>
                new { x.WorkflowId, x.Environment, x.ExecutedAt });

            builder.HasIndex(x =>
                new { x.WorkflowId, x.Environment, x.Status, x.ExecutedAt });
        }
    }

}

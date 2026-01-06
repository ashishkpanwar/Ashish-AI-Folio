using AiKnowledgeAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Persistence
{
    public sealed class JobExecutionDbContext : DbContext
    {
        public DbSet<JobExecutionEntity> JobExecutions => Set<JobExecutionEntity>();

        public JobExecutionDbContext(DbContextOptions<JobExecutionDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(JobExecutionDbContext).Assembly);
        }
    }

}

using AiKnowledgeAssistant.Domain.Enums;
using AiKnowledgeAssistant.Infrastructure.Persistence;
using AiKnowledgeAssistant.Infrastructure.Persistence.Entities;
using AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces;
using AiKnowledgeAssistant.Infrastructure.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Repositories
{
    public sealed class JobExecutionReader : IJobExecutionReader
    {
        private readonly JobExecutionDbContext _dbContext;

        public JobExecutionReader(JobExecutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GetByIdAsync + GetFailureWindowAsync
        public async Task<JobExecution?> GetByIdAsync(string jobId)
        {
            JobExecutionEntity? entity =
                await _dbContext.JobExecutions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.JobId == jobId);

            return entity == null ? null : Map(entity);
        }


        public async Task<IReadOnlyList<JobExecution>> GetFailureWindowAsync(
    string workflowId,
    string environment,
    DateTimeOffset fromTime,
    TimeSpan maxLookback,
    int maxJobs)
        {
            DateTimeOffset lookbackStart = fromTime.Subtract(maxLookback);

            // 1️⃣ Find the most recent SUCCESS before fromTime
            DateTimeOffset? lastSuccessTime =
                await _dbContext.JobExecutions
                    .AsNoTracking()
                    .Where(x =>
                        x.WorkflowId == workflowId &&
                        x.Environment == environment &&
                        x.Status == JobStatus.Success &&
                        x.ExecutedAt <= fromTime &&
                        x.ExecutedAt >= lookbackStart)
                    .OrderByDescending(x => x.ExecutedAt)
                    .Select(x => (DateTimeOffset?)x.ExecutedAt)
                    .FirstOrDefaultAsync();

            DateTimeOffset failureWindowStart =
                lastSuccessTime ?? lookbackStart;

            // 2️⃣ Fetch failures AFTER last success
            List<JobExecutionEntity> failures =
                await _dbContext.JobExecutions
                    .AsNoTracking()
                    .Where(x =>
                        x.WorkflowId == workflowId &&
                        x.Environment == environment &&
                        x.Status == JobStatus.Failed &&
                        x.ExecutedAt > failureWindowStart &&
                        x.ExecutedAt <= fromTime)
                    .OrderBy(x => x.ExecutedAt)
                    .Take(maxJobs)
                    .ToListAsync();

            return failures.Select(Map).ToList();
        }

        private static JobExecution Map(JobExecutionEntity entity)
        {
            return new JobExecution
            {
                JobId = entity.JobId,
                WorkflowId = entity.WorkflowId,
                Environment = entity.Environment,
                Status = entity.Status,
                ExecutedAt = entity.ExecutedAt
            };
        }


    }

}

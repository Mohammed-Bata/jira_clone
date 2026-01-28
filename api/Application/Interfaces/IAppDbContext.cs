using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Project> Projects { get; }
        public DbSet<ProjectColumn> ProjectColumns { get; }
        public DbSet<WorkItem> WorkItems { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HumanResources.Models;

namespace HumanResources.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HumanResources.Models.DepartmentViewModel>? DepartmentViewModel { get; set; }
        public DbSet<HumanResources.Models.ProjectViewModel>? ProjectViewModel { get; set; }
        public DbSet<HumanResources.Models.AllEmployeeViewModel>? AllEmployeeViewModel { get; set; }
        public DbSet<HumanResources.Models.AbsenceViewModel>? AbsenceViewModel { get; set; }
        public DbSet<HumanResources.Models.AbsenceAllView>? AbsenceAllView { get; set; }
    }
}
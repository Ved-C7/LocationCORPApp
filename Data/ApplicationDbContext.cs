using LocationCORPApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationCORPApp.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<HoltecLocations> Locations { get; set; }
    public DbSet<LocationBuildings> Buildings { get; set; }
    public DbSet<HoltecBuildingFloors> Floors { get; set; }
    public DbSet<HoltecOfficeSpaces> Offices { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<EmployeeLocations> EmployeeLocation { get; set; }


}




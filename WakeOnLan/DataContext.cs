using System.Net;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace WakeOnLan;

public class DataContext : DbContext
{
    public DbSet<Target> Targets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=Targets.db",
            x => { x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); });
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Target>().ToTable("Targets");

        //builder.Entity<Target>().Property(x => x.HostEntry).HasConversion(
        //    from => from.HostName,
        //    to => Dns.GetHostEntry(to));

        builder.Entity<Target>().HasKey(x => x.Name);

        base.OnModelCreating(builder);
    }
}
using Itventory.web.Entidades.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Entidades
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }

        public DbSet<WorkArea> WorkAreas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Peripheral> Peripherals { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OfficeSuite> OfficeSuites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OtherPeripheral> OtherPeripherals { get; set; }
        public DbSet<WorkStation> WorkStations { get; set; }
        public DbSet<Act> Acts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Actas
            //modelBuilder.Entity<Act>().ToTable("");
            modelBuilder.Entity<Act>().Property(prop => prop.Status)
                .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Act>().HasQueryFilter(a => !a.IsDeleted);

            //Categorias

            //Devices
            modelBuilder.Entity<Device>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Device>().HasQueryFilter(d => !d.IsDeleted);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.DeviceType)
            .WithMany(s => s.DeviceTypes)
            .HasForeignKey(p => p.DeviceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.DeviceBrand)
            .WithMany(s => s.DeviceBrands)
            .HasForeignKey(p => p.DeviceBrandId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.Processor)
            .WithMany(s => s.Processors)
            .HasForeignKey(p => p.ProcessorId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.WindowsLicense)
            .WithMany(s => s.WindowsLicenses)
            .HasForeignKey(p => p.WindowsLicenseId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>()
            .HasOne(d => d.AntivirusLicense)
            .WithMany(s => s.AntivirusLicenses)
            .HasForeignKey(p => p.AntivirusLicenseId)
            .OnDelete(DeleteBehavior.Restrict);

            //Empleados
            modelBuilder.Entity<Employee>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Employee>()
            .HasOne(e => e.WorkArea)
            .WithMany(wa => wa.Employees)
            .HasForeignKey(e => e.WorkAreaId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
            .HasMany(e => e.WorkStations)
            .WithOne(ws => ws.Employee)
            .HasForeignKey(ws => ws.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

            //OfficeSuites
            modelBuilder.Entity<OfficeSuite>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<OfficeSuite>().HasQueryFilter(os => !os.IsDeleted);

            //Otros perifericos
            modelBuilder.Entity<OtherPeripheral>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<OtherPeripheral>().HasQueryFilter(op => !op.IsDeleted);

            //Perifericos 
            modelBuilder.Entity<Peripheral>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Peripheral>().HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Peripheral>()
            .HasOne(p => p.PeripheralType)
            .WithMany(s => s.PeripheralTypes)
            .HasForeignKey(p => p.PeripheralTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Peripheral>()
            .HasOne(p => p.PeripheralBrand)
            .WithMany(s => s.PeripheralBrands)
            .HasForeignKey(p => p.PeripheralBrandId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Peripheral>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
            });

            //Productos
            modelBuilder.Entity<Product>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Employee)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

            //Licencias de software
            modelBuilder.Entity<SoftwareLicense>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<SoftwareLicense>().HasQueryFilter(sl => !sl.IsDeleted);

            modelBuilder.Entity<SoftwareLicense>()
            .HasOne(sl => sl.Subcategory)
            .WithMany(sc => sc.SoftwareLicenses)
            .HasForeignKey(sl => sl.SubcategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            //Subcategorias
            modelBuilder.Entity<Subcategory>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<Subcategory>().HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<Subcategory>()
            .HasOne(sc => sc.Category)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            //Areas de trabajo
            modelBuilder.Entity<WorkArea>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Disponible).HasConversion<string>();

            modelBuilder.Entity<WorkArea>().HasQueryFilter(wa => !wa.IsDeleted);

            //Estaciones de trabajo

            modelBuilder.Entity<WorkStation>().Property(prop => prop.Status)
            .HasDefaultValue(Status.Pendiente).HasConversion<string>();

            modelBuilder.Entity<WorkStation>().HasQueryFilter(ws => !ws.IsDeleted);

            modelBuilder.Entity<WorkStation>()
            .HasMany(ws => ws.Acts)
            .WithOne(a => a.WorkStation)
            .HasForeignKey(a => a.WorkStationId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkStation>()
            .HasMany(ws => ws.OtherPeripherals)
            .WithMany(op => op.WorkStations);

            modelBuilder.Entity<WorkStation>()
            .HasMany(ws => ws.Peripherals)
            .WithMany(p => p.WorkStations);

            modelBuilder.Entity<WorkStation>()
            .HasOne(ws => ws.ComputerDevice)
            .WithMany(s => s.ComputerDevices)
            .HasForeignKey(p => p.ComputerDeviceId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkStation>()
            .HasOne(ws => ws.SmartPhoneDevice)
            .WithMany(s => s.SmartPhoneDevices)
            .HasForeignKey(p => p.SmartPhoneDeviceId)
            .OnDelete(DeleteBehavior.Restrict);

            SeedingConstants.Seed(modelBuilder);
        }
        
    }
}

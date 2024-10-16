using Microsoft.EntityFrameworkCore;

namespace Itventory.web.Entidades.Seeding
{
    public static class SeedingConstants
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var Perifericos = new Category { Id = 1, Name = "Periféricos" };
            var Licencias = new Category { Id = 2, Name = "Licencias" };
            var Dispositivos = new Category { Id = 3, Name = "Dispositivos" };
            var Marcas = new Category { Id = 4, Name = "Marcas" };
            var ProcesadoresComputadores = new Category { Id = 5, Name = "Procesadores de PC" };
            var ProcesadoresTelefonos = new Category { Id = 6, Name = "Procesadores de Telefonos" };

            modelBuilder.Entity<Category>().HasData(Perifericos,Licencias,Dispositivos,Marcas,ProcesadoresComputadores,ProcesadoresTelefonos);

            var Mouses = new Subcategory { Id = 1, CategoryId = 1, Name = "Mouses" };
            var Teclados = new Subcategory { Id = 2, CategoryId = 1, Name = "Teclados" };
            var Monitores = new Subcategory { Id = 3, CategoryId = 1, Name = "Monitores" };

            var LicWindows = new Subcategory { Id = 4, CategoryId = 2, Name = "Licencias de Windows" };
            var LicAntivirus = new Subcategory { Id = 5, CategoryId = 2, Name = "Licencias de Antivirus" };

            var Computador = new Subcategory { Id = 6, CategoryId = 3, Name = "Computadores" };
            var SmartPhone = new Subcategory { Id = 7, CategoryId = 3, Name = "SmartPhones" };

            var HP = new Subcategory { Id = 8, CategoryId = 4, Name = "HP" };
            var Apple = new Subcategory { Id = 9, CategoryId = 4, Name = "Apple" };
            var Lenovo = new Subcategory { Id = 10, CategoryId = 4, Name = "Lenovo" };

            var Intel = new Subcategory { Id = 11, CategoryId = 5, Name = "Intel Core i7-11700K (8 Núcleos, 3.6 GHz Frecuencia Base)"};
            var Ryzen = new Subcategory { Id = 12, CategoryId = 5, Name = "AMD Ryzen 7 5800X (8 núcleos, 3.8 GHz Frecuencia Base)"};
            var SnapDragon = new Subcategory { Id = 13, CategoryId = 6, Name = "Qualcomm Snapdragon"};

            modelBuilder.Entity<Subcategory>().HasData(Mouses, Teclados, Monitores, LicWindows, LicAntivirus, Computador, SmartPhone, HP, Apple, Lenovo, Intel, Ryzen, SnapDragon);

            var Administracion = new WorkArea { Id = 1, Name = "Administración" };
            var Gerencia = new WorkArea { Id = 2, Name = "Gerencia" };
            var It = new WorkArea { Id = 3, Name = "It" };
            var Proyectos = new WorkArea { Id = 4, Name = "Proyectos Desarrollo Corporativo" };
            var Servicios = new WorkArea { Id = 5, Name = "Servicios Especiales" };
            var Fleet = new WorkArea { Id = 6, Name = "Fleet Managers" };
            var Instalacion = new WorkArea { Id = 7, Name = "Instalacion y Mantenimiento" };
            var Centro = new WorkArea { Id = 8, Name = "Centro de Control" };
            var Ventas = new WorkArea { Id = 9, Name = "Ventas" };

            modelBuilder.Entity<WorkArea>().HasData(Administracion, Gerencia, It, Proyectos, Servicios, Fleet, Instalacion, Centro, Ventas);


        }
    }
}

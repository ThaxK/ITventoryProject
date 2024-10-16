using Itventory.web.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Itventory.web.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {

        public IActionResult Index()
        {
            //Consulta de la grafica de barras de employees
            List<Employee> employeesList = new List<Employee>();

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }

            }

            var chartEmployeeData = new List<object>
            {
                new { Category = "Disponibles", Value = employeesList.Count(e => e.Status == Status.Disponible)},
                new { Category = "Asignados", Value = employeesList.Count(e => e.Status == Status.Asignado)}
            };


            
            ViewBag.chartEmployeeData = chartEmployeeData;

            //Consulta de la grafico de workstations
            var chartWorkStationData = GraficoWorkStations();
            ViewBag.chartWorkStationData = chartWorkStationData;

            //Consulta de la grafica de barras de devices
            var chartDeviceData = GraficoDispositivos();
            ViewBag.chartDeviceData = chartDeviceData;

            //Consulta de la grafica de barras de perifericos
            var chartPeripheralData = GraficoPerifericos();
            ViewBag.chartPeripheralData = chartPeripheralData;

            //Consulta de la grafica de barras de perifericos
            var chartLicenseData = GraficoLicencias();
            ViewBag.chartLicenseData = chartLicenseData;

            //Consulta de la grafica de barras de perifericos
            var chartEmployeeBrData = GraficoEmpleadosPorArea();
            ViewBag.chartEmployeeBrData = chartEmployeeBrData;

            //Consulta de la grafica de barras de suites de office
            var chartSuiteData = GraficoSuites();
            ViewBag.chartSuiteData = chartSuiteData;

            return View();
        }

        private List<object> GraficoWorkStations()
        {
            List<WorkStation> workStationsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkStations/GetWorkStations";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                workStationsList = JsonConvert.DeserializeObject<List<WorkStation>>(content);
            }

            var chartWorkStationData = new List<object>
            {
                new { Category = "Asignadas", Value = workStationsList.Count(ws => ws.Status == Status.Asignado) },
                new { Category = "Pendientes", Value = workStationsList.Count(ws => ws.Status == Status.Pendiente) }
            };

            return chartWorkStationData;
        }

        private List<object> GraficoEmpleados()
        {
            List<Employee> employeesList= new List<Employee>();

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                }
                
            }

            var chartEmployeeData = new List<object>
            {
                new { Category = "Disponibles", Value = employeesList.Count(e => e.Status == Status.Disponible)},
                new { Category = "Asignados", Value = employeesList.Count(e => e.Status == Status.Asignado)}
            };

            return chartEmployeeData;
        }

        private List<object> GraficoDispositivos()
        {
            List<Device> devicesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Devices/GetDevices";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                devicesList = JsonConvert.DeserializeObject<List<Device>>(content);
            }

            List<Subcategory> subcategoriesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetSubcategories";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                subcategoriesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);

            }

            for (int i = 0; i < devicesList.Count; i++)
            {
                foreach (var subcategory in subcategoriesList)
                {
                    if (devicesList[i].DeviceBrandId == subcategory.Id)
                    {
                        devicesList[i].DeviceBrand = subcategory;
                    }
                    if (devicesList[i].DeviceTypeId == subcategory.Id)
                    {
                        devicesList[i].DeviceType = subcategory;
                    }
                    if (devicesList[i].ProcessorId == subcategory.Id)
                    {
                        devicesList[i].Processor = subcategory;
                    }
                }
            }

            var chartDeviceData = devicesList
                .Where(d => !d.IsDeleted)
                .GroupBy(d => new { d.DeviceTypeId, d.DeviceType.Name })
                .Select(g => new
                {
                    DeviceTypeId = g.Key.DeviceTypeId,
                    DeviceTypeName = g.Key.Name,
                    Disponibles = g.Count(d => d.Status == Status.Disponible),
                    Asignados = g.Count(d => d.Status == Status.Asignado)
                })
                .ToList<object>();

            return chartDeviceData;
        }

        private List<object> GraficoLicencias()
        {
            List<SoftwareLicense> softwareLicensesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/GetSoftwareLicenses";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                softwareLicensesList = JsonConvert.DeserializeObject<List<SoftwareLicense>>(content);
            }

            List<Subcategory> subcategoriesList;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/GetSubcategories";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                subcategoriesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }

            for (int i = 0; i < softwareLicensesList.Count; i++)
            {
                foreach (var subcategory in subcategoriesList)
                {
                    if (subcategory.Id == softwareLicensesList[i].SubcategoryId)
                    {
                        softwareLicensesList[i].Subcategory = subcategory;
                    }
                }
            }

            var chartLicenseData = softwareLicensesList
                .Where(s => !s.IsDeleted)
                .GroupBy(s => new { s.SubcategoryId, s.Subcategory.Name })
                .Select(g => new
                {
                    SubcategoryId = g.Key.SubcategoryId,
                    SubcategoryName = g.Key.Name,
                    Disponibles = g.Count(s => s.Status == Status.Disponible),
                    NoDisponibles = g.Count(s => s.Status != Status.Disponible)
                })
                .ToList<object>();

            return chartLicenseData;
        }

        private List<object> GraficoPerifericos()
        {
            List<Peripheral> peripheralsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetPeripherals";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                peripheralsList = JsonConvert.DeserializeObject<List<Peripheral>>(content);
            }

            List<Subcategory> typesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetTypes";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                typesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }
            List<Subcategory> brandsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetBrands";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                brandsList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }

            for (int i = 0; i < peripheralsList.Count; i++)
            {
                foreach (var brand in brandsList)
                {
                    if (brand.Id == peripheralsList[i].PeripheralBrandId)
                    {
                        peripheralsList[i].PeripheralBrand = brand;
                    }
                }
                foreach (var type in typesList)
                {
                    if (type.Id == peripheralsList[i].PeripheralTypeId)
                    {
                        peripheralsList[i].PeripheralType = type;
                    }
                }
            }

            var chartPeripheralData = peripheralsList
                .Where(p => !p.IsDeleted)
                .GroupBy(p => new { p.PeripheralTypeId, p.PeripheralType.Name })
                .Select(g => new
                {
                    PeripheralTypeId = g.Key.PeripheralTypeId,
                    PeripheralTypeName = g.Key.Name,
                    Disponibles = g.Count(p => p.Status == Status.Disponible),
                    Asignados = g.Count(p => p.Status == Status.Asignado)
                })
                .ToList<object>();

            return chartPeripheralData;
        }

        private List<object> GraficoEmpleadosPorArea()
        {
            List<WorkArea> workAreasList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkArea/GetWorkAreas";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                workAreasList = JsonConvert.DeserializeObject<List<WorkArea>>(content);
            }

            List<Employee> employeesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            

            for (int i = 0; i < workAreasList.Count; i++)
            {
                workAreasList[i].Employees = new List<Employee>();
                foreach (var employee in employeesList)
                {
                    if (workAreasList[i].Id == employee.WorkAreaId)
                    {
                        workAreasList[i].Employees.Add(employee);
                    }
                }
            }

            var chartEmployeeBrData = workAreasList
                .Where(w => !w.IsDeleted)
                .Select(w => new
                {
                    WorkAreaId = w.Id,
                    WorkAreaName = w.Name,
                    Count = w.Employees.Count(e => !e.IsDeleted)
                })
                .ToList<object>();

            return chartEmployeeBrData;
        }

        private List<object> GraficoSuites()
        {
            List<OfficeSuite> officeSuitesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetOficceSuites";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                officeSuitesList = JsonConvert.DeserializeObject<List<OfficeSuite>>(content);
            }

            var chartSuiteData = new List<object>
            {
                new { Category = "Disponibles", Value = officeSuitesList.Count(s => s.Status == Status.Disponible)},
                new { Category = "Asignados", Value = officeSuitesList.Count(s => s.Status == Status.Asignado)}
            };

            return chartSuiteData;
        }
    }
}

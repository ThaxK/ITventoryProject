using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using Itventory.web.Models;
using NuGet.Packaging;
using System.Drawing;
using Syncfusion.EJ2.Linq;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Framework;
using Syncfusion.Pdf;
using Syncfusion.EJ2.Base;
using System.IO.Compression;
using Syncfusion.Pdf.Graphics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Itventory.web.Controllers
{
    public class WorkStationsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WorkStationsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: WorkStations
        public IActionResult Index(Status? status)
        {
            List<WorkStation> workStationsList= new List<WorkStation>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkStations/GetWorkStations";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    workStationsList = JsonConvert.DeserializeObject<List<WorkStation>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<Device> devicesList = GetDevices();
            List<Employee> employeeList = GetEmployees();

            for (int i = 0; i < workStationsList.Count; i++)
            {
                foreach (var device in devicesList)
                {
                    if (workStationsList[i].ComputerDeviceId==device.Id)
                    {
                        workStationsList[i].ComputerDevice = device;
                    }
                }
                foreach (var employee in employeeList)
                {
                    if (workStationsList[i].EmployeeId==employee.Id)
                    {
                        workStationsList[i].Employee = employee;
                    }
                }
            }

            ViewBag.datasource = workStationsList
              .Select(y => new
              {
                  Id = y.Id,
                  Employee = y.Employee,
                  Devices = y.ComputerDevice.DeviceBrand.Name + " " + y.ComputerDevice.Model,
                  Status = y.Status.ToString()
              })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(int Id, int EmployeeId)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/IsNameAvailable?Id=" + Id + "&EmployeeId=" + EmployeeId;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        // GET: WorkStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            WorkStationModel workStationModel= new WorkStationModel();

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/GetWorkStationDetails?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                workStationModel = JsonConvert.DeserializeObject<WorkStationModel>(content);
            }

            WorkStation workStation = new WorkStation();
            workStation.Id = workStationModel.Id;
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;

            if (workStation == null)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            List<Employee> employeesList = GetEmployees();
            foreach (var employee in employeesList)
            {
                if (workStation.EmployeeId==employee.Id)
                {
                    workStation.Employee = employee;
                }
            }

            List<Device> devicesList = GetDevices();
            foreach (var device in devicesList)
            {
                if (workStation.ComputerDeviceId==device.Id)
                {
                    workStation.ComputerDevice = device;
                }
                if (workStation.SmartPhoneDeviceId==device.Id)
                {
                    workStation.SmartPhoneDevice = device;
                }
            }
            if (workStation.PeripheralsIds!=null)
            {
                List<Peripheral> peripheralsList = GetPeripherals();
                foreach (var peripheral in peripheralsList)
                {
                    for (int i = 0; i < workStation.PeripheralsIds.Count; i++)
                    {
                        if (peripheral.Id == workStation.PeripheralsIds[i])
                        {
                            workStation.Peripherals.Add(peripheral);
                        }
                    }
                }
            }

            if (workStation.OtherPeripheralsIds!=null)
            {
                List<OtherPeripheral> otherPeripheralsList = GetOtherPeripherals();
                foreach (var otherPeripheral in otherPeripheralsList)
                {
                    for (int i = 0; i < workStation.OtherPeripheralsIds.Count; i++)
                    {
                        if (otherPeripheral.Id == workStation.OtherPeripheralsIds[i])
                        {
                            workStation.OtherPeripherals.Add(otherPeripheral);
                        }
                    }
                }
            }
            

            return View(workStation);
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employeesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "WorkStations/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);

            }
            return employeesList;
        }
        
        private List<Device> GetDevices()
        {
            List<Device> devicesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "WorkStations/GetDevices";

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

            List<SoftwareLicense> softwareLicenseList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetSoftwareLicenses";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                softwareLicenseList = JsonConvert.DeserializeObject<List<SoftwareLicense>>(content);

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
            for (int i = 0; i < devicesList.Count; i++)
            {
                foreach (var softwareLicense in softwareLicenseList)
                {
                    if (devicesList[i].WindowsLicenseId == softwareLicense.Id)
                    {
                        devicesList[i].WindowsLicense = softwareLicense;
                    }
                    if (devicesList[i].AntivirusLicenseId == softwareLicense.Id)
                    {
                        devicesList[i].AntivirusLicense = softwareLicense;
                    }
                }
            }

            return devicesList;
        }
        
        private List<OtherPeripheral> GetOtherPeripherals()
        {
            List<OtherPeripheral> otherPeripheralsList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "WorkStations/GetOtherPeripherals";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                otherPeripheralsList = JsonConvert.DeserializeObject<List<OtherPeripheral>>(content);

            }
            return otherPeripheralsList;
        }
        
        private List<Peripheral> GetPeripherals()
        {
            List<Peripheral> peripheralsList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "WorkStations/GetPeripherals";

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

            return peripheralsList;
        }

        // GET: WorkStations/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["EmployeeId"] = new SelectList(GetEmployees().Where(e => e.Status == (Status)1).OrderBy(e => e.Name).Select(y => new { y.Id, Name = y.Name + " " + y.LastName }), "Id", "Name");
                ViewData["PcId"] = new SelectList(GetDevices().Where(d => d.Status == (Status)1 && d.DeviceTypeId == 6).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand");
                ViewData["SmarthphonesId"] = new SelectList(GetDevices().Where(d => d.Status == (Status)1 && d.DeviceTypeId == 7).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand");
                ViewData["OtherPeripheralsId"] = new SelectList(GetOtherPeripherals().Where(o => o.Status == (Status)1).OrderBy(o => o.Name).Select(o => new { o.Id, Brand = o.Name + " : " + o.Status.ToString() }), "Id", "Brand");

                var peripheralCategories = new Dictionary<string, SelectList>();

                var categories = GetPeripherals()
                    .Select(p => p.PeripheralType.Name)
                    .Distinct()
                    .ToList();

                foreach (var category in categories)
                {
                    var peripherals = GetPeripherals()
                        .Where(p => p.PeripheralType.Name == category && p.Status == (Status)1)
                        .OrderBy(p => p.PeripheralBrandId)
                        .Select(y => new { y.Id, Brand = y.PeripheralType.Name + " " + y.PeripheralBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) + " : " + y.Status.ToString() });

                    if (peripherals == null || !peripherals.Any())
                    {
                        continue;
                    }

                    var key = category.ToString();

                    while (peripheralCategories.ContainsKey(key))
                    {
                        key += "_Duplicate";
                    }

                    peripheralCategories.Add(key, new SelectList(peripherals, "Id", "Brand"));
                }


                ViewBag.PeripheralCategories = peripheralCategories;

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, EmployeeId, ComputerDeviceId,SmartPhoneDeviceId ,PeripheralsIds, OtherPeripheralsIds")] WorkStation workStation)
        {
            
            if (ModelState.IsValid)
            {
                WorkStationModel workStationModel = new WorkStationModel();
                workStationModel.Id = workStation.Id;
                workStationModel.EmployeeId = workStation.EmployeeId;
                workStationModel.ComputerDeviceId = workStation.ComputerDeviceId;
                workStationModel.SmartPhoneDeviceId = workStation.SmartPhoneDeviceId;
                workStationModel.PeripheralsIds = workStation.PeripheralsIds;
                workStationModel.OtherPeripheralsIds = workStation.OtherPeripheralsIds;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "WorkStations/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(workStationModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Estacion de Trabajo Creada Correctamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Seleccione un Empleado y un Computador!";
                        TempData["SweetAlertType"] = "warning";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            var peripheralIds = workStation.Peripherals.Select(p => p.Id).ToList();
            var idsPerifericosAsignados = workStation.OtherPeripherals.Select(op => op.Id).ToList();

            ViewData["EmployeeId"] = new SelectList(GetEmployees().Where(e => e.Status == (Status)1 || e.Id == workStation.EmployeeId).OrderBy(e => e.Name).Select(y => new { y.Id, Name = y.Name + " " + y.LastName }), "Id", "Name", workStation.EmployeeId);
            ViewData["PcId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.ComputerDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 6))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.ComputerDeviceId);
            ViewData["SmarthphonesId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.SmartPhoneDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 7))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.SmartPhoneDeviceId);

            var perifericosDisponibles = GetOtherPeripherals()
                .Where(o => o.Status == (Status)1)
                .OrderBy(o => o.Name)
                .Select(y => new
                {
                    Id = y.Id,
                    Nombre = y.Name + " : " + (idsPerifericosAsignados.Contains(y.Id) ? "Asignado" : "Disponible")
                })
                .ToList();

            foreach (var periferico in perifericosDisponibles.Where(p => p.Nombre.Contains("Asignado")))
            {
                var perifericoAsignado = workStation.OtherPeripherals.FirstOrDefault(op => op.Id == periferico.Id);
                if (perifericoAsignado != null)
                {
                    perifericoAsignado.Status = (Status)1;
                }
            }
            ViewData["OtherPeripheralsId"] = new SelectList(perifericosDisponibles, "Id", "Nombre", workStation.OtherPeripherals);

            var peripheralCategories = new Dictionary<string, SelectList>();
            var categories = GetPeripherals()
                .Select(p => p.PeripheralType.Name)
                .Distinct()
                .ToList();

            foreach (var category in categories)
            {
                var peripherals = GetPeripherals()
                    .Where(p => p.PeripheralType.Name == category && (p.Status == (Status)1 || peripheralIds.Contains(p.Id)))
                    .OrderBy(p => p.PeripheralBrandId)
                    .Select(y => new { y.Id, Brand = y.PeripheralType.Name + " " + y.PeripheralBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) + " : " + y.Status.ToString() });

                var key = category.ToString();

                while (peripheralCategories.ContainsKey(key))
                {
                    key += "_Duplicate";
                }

                peripheralCategories.Add(key, new SelectList(peripherals, "Id", "Brand"));
            }

            ViewBag.PeripheralCategories = peripheralCategories;
            return View(workStation);
        }


        

        // GET: WorkStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            WorkStationModel workStationModel = new WorkStationModel();

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/GetWorkStationDetails?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                workStationModel = JsonConvert.DeserializeObject<WorkStationModel>(content);
            }

            WorkStation workStation = new WorkStation();
            workStation.Id = workStationModel.Id;
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;

            if (workStation == null)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            List<Employee> employeesList = GetEmployees();
            foreach (var employee in employeesList)
            {
                if (workStation.EmployeeId == employee.Id)
                {
                    workStation.Employee = employee;
                }
            }

            List<Device> devicesList = GetDevices();
            foreach (var device in devicesList)
            {
                if (workStation.ComputerDeviceId == device.Id)
                {
                    workStation.ComputerDevice = device;
                }
                if (workStation.SmartPhoneDeviceId == device.Id)
                {
                    workStation.SmartPhoneDevice = device;
                }
            }
            if (workStation.PeripheralsIds != null)
            {
                List<Peripheral> peripheralsList = GetPeripherals();
                foreach (var peripheral in peripheralsList)
                {
                    for (int i = 0; i < workStation.PeripheralsIds.Count; i++)
                    {
                        if (peripheral.Id == workStation.PeripheralsIds[i])
                        {
                            workStation.Peripherals.Add(peripheral);
                        }
                    }
                }
            }

            if (workStation.OtherPeripheralsIds != null)
            {
                List<OtherPeripheral> otherPeripheralsList = GetOtherPeripherals();
                foreach (var otherPeripheral in otherPeripheralsList)
                {
                    for (int i = 0; i < workStation.OtherPeripheralsIds.Count; i++)
                    {
                        if (otherPeripheral.Id == workStation.OtherPeripheralsIds[i])
                        {
                            workStation.OtherPeripherals.Add(otherPeripheral);
                        }
                    }
                }
            }

            if (workStation == null || workStation.IsDeleted)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var peripheralIds = workStation.Peripherals.Select(p => p.Id).ToList();
            var idsPerifericosAsignados = workStation.OtherPeripherals.Select(op => op.Id).ToList();

            ViewData["EmployeeId"] = new SelectList(GetEmployees().Where(e => e.Status == (Status)1 || e.Id == workStation.EmployeeId).OrderBy(e => e.Name).Select(y => new { y.Id, Name = y.Name + " " + y.LastName }), "Id", "Name", workStation.EmployeeId);
            ViewData["PcId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.ComputerDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 6))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.ComputerDeviceId);
            ViewData["SmarthphonesId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.SmartPhoneDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 7))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.SmartPhoneDeviceId);

            var perifericosDisponibles = GetOtherPeripherals()
                .Where(o => o.Status == (Status)1)
                .OrderBy(o => o.Name)
                .Select(y => new
                {
                    Id = y.Id,
                    Nombre = y.Name + " : " + (idsPerifericosAsignados.Contains(y.Id) ? "Asignado" : "Disponible")
                })
                .ToList();

            foreach (var periferico in perifericosDisponibles.Where(p => p.Nombre.Contains("Asignado")))
            {
                var perifericoAsignado = workStation.OtherPeripherals.FirstOrDefault(op => op.Id == periferico.Id);
                if (perifericoAsignado != null)
                {
                    perifericoAsignado.Status = (Status)1;
                }
            }
            ViewData["OtherPeripheralsId"] = new SelectList(perifericosDisponibles, "Id", "Nombre", workStation.OtherPeripherals);

            var peripheralCategories = new Dictionary<string, SelectList>();
            var categories = GetPeripherals()
                .Select(p => p.PeripheralType.Name)
                .Distinct()
                .ToList();

            foreach (var category in categories)
            {
                var peripherals = GetPeripherals()
                    .Where(p => p.PeripheralType.Name == category && (p.Status == (Status)1 || peripheralIds.Contains(p.Id)))
                    .OrderBy(p => p.PeripheralBrandId)
                    .Select(y => new { y.Id, Brand = y.PeripheralType.Name + " " + y.PeripheralBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) + " : " + y.Status.ToString() })
                    .ToList(); 

                if (peripherals.Any())
                {
                    var key = category.ToString();

                    while (peripheralCategories.ContainsKey(key))
                    {
                        key += "_Duplicate";
                    }

                    peripheralCategories.Add(key, new SelectList(peripherals, "Id", "Brand"));
                }
            }

            ViewBag.PeripheralCategories = peripheralCategories;


            return View(workStation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,ComputerDeviceId,SmartPhoneDeviceId,PeripheralsIds,OtherPeripheralsIds")] WorkStation workStation)
        {
            if (id != workStation.Id)
            {
                TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                WorkStationModel workStationModel = new WorkStationModel();
                workStationModel.Id = workStation.Id;
                workStationModel.EmployeeId = workStation.EmployeeId;
                workStationModel.ComputerDeviceId = workStation.ComputerDeviceId;
                workStationModel.SmartPhoneDeviceId = workStation.SmartPhoneDeviceId;
                workStationModel.PeripheralsIds = workStation.PeripheralsIds;
                workStationModel.OtherPeripheralsIds = workStation.OtherPeripheralsIds;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "WorkStations/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(workStationModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Estacion de Trabajo Editada Correctamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                        TempData["SweetAlertType"] = "error";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }


            var peripheralIds = workStation.Peripherals.Select(p => p.Id).ToList();
            var idsPerifericosAsignados = workStation.OtherPeripherals.Select(op => op.Id).ToList();

            ViewData["EmployeeId"] = new SelectList(GetEmployees().Where(e => e.Status == (Status)1 || e.Id == workStation.EmployeeId).OrderBy(e => e.Name).Select(y => new { y.Id, Name = y.Name + " " + y.LastName }), "Id", "Name", workStation.EmployeeId);
            ViewData["PcId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.ComputerDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 6))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.ComputerDeviceId);
            ViewData["SmarthphonesId"] = new SelectList(GetDevices().Where(d => (d.Id == workStation.SmartPhoneDeviceId || (d.Status == (Status)1 && d.DeviceTypeId == 7))).OrderBy(d => d.DeviceBrandId).Select(y => new { y.Id, Brand = y.DeviceType.Name + " " + y.DeviceBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Brand", workStation.SmartPhoneDeviceId);

            var perifericosDisponibles = GetOtherPeripherals()
                .Where(o => o.Status == (Status)1)
                .OrderBy(o => o.Name)
                .Select(y => new
                {
                    Id = y.Id,
                    Nombre = y.Name + " : " + (idsPerifericosAsignados.Contains(y.Id) ? "Asignado" : "Disponible")
                })
                .ToList();

            foreach (var periferico in perifericosDisponibles.Where(p => p.Nombre.Contains("Asignado")))
            {
                var perifericoAsignado = workStation.OtherPeripherals.FirstOrDefault(op => op.Id == periferico.Id);
                if (perifericoAsignado != null)
                {
                    perifericoAsignado.Status = (Status)1;
                }
            }
            ViewData["OtherPeripheralsId"] = new SelectList(perifericosDisponibles, "Id", "Nombre", workStation.OtherPeripherals);

            var peripheralCategories = new Dictionary<string, SelectList>();
            var categories = GetPeripherals()
                .Select(p => p.PeripheralType.Name)
                .Distinct()
                .ToList();

            foreach (var category in categories)
            {
                var peripherals = GetPeripherals()
                    .Where(p => p.PeripheralType.Name == category && (p.Status == (Status)1 || peripheralIds.Contains(p.Id)))
                    .OrderBy(p => p.PeripheralBrandId)
                    .Select(y => new { y.Id, Brand = y.PeripheralType.Name + " " + y.PeripheralBrand.Name + " " + y.Series.Substring(y.Series.Length - 4) + " : " + y.Status.ToString() });

                var key = category.ToString();

                while (peripheralCategories.ContainsKey(key))
                {
                    key += "_Duplicate";
                }

                peripheralCategories.Add(key, new SelectList(peripherals, "Id", "Brand"));
            }

            ViewBag.PeripheralCategories = peripheralCategories; 
            return View(workStation);
        }


        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                int result = JsonConvert.DeserializeObject<int>(content);

                switch (result)
                {
                    case 3:
                        TempData["AlertMessage"] = "Estacion de Trabajo Eliminada Correctamente!";
                        TempData["SweetAlertType"] = "succes";
                        return RedirectToAction(nameof(Index));
                    case 2:
                        TempData["AlertMessage"] = "Estacion de Trabajo Asignada!";
                        TempData["SweetAlertType"] = "error";
                        return RedirectToAction(nameof(Index));
                    default:
                        TempData["AlertMessage"] = "Estacion de Trabajo No Encontrada!";
                        TempData["SweetAlertType"] = "error";
                        return RedirectToAction(nameof(Index));

                }
            }
            
        }


        [HttpPost]
        public byte[] GenerarPDF(string base64Image, int Id)
        {
            WorkStationModel workStationModel = new WorkStationModel();

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/GetWorkStationDetails?id=" + Id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                workStationModel = JsonConvert.DeserializeObject<WorkStationModel>(content);
            }

            WorkStation workStation = new WorkStation();
            workStation.Id = workStationModel.Id;
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;

            List<Employee> employeesList = GetEmployees();
            foreach (var employee in employeesList)
            {
                if (workStation.EmployeeId == employee.Id)
                {
                    workStation.Employee = employee;
                }
            }

            var cd = System.AppContext.BaseDirectory;
            string rutaArchivoWord = _hostingEnvironment.WebRootPath + @"/pdfVM/ACTA_DE_ENTREGA_ELEMENTOS.docx";

            WordDocument originalDocument = new WordDocument();

            using (FileStream fileStream = new FileStream(rutaArchivoWord, FileMode.Open, FileAccess.Read))
            {
                originalDocument.Open(fileStream, FormatType.Docx);
            }

            WordDocument modifiedDocument = originalDocument.Clone();

            byte[] imageBytes = System.Convert.FromBase64String(base64Image);

            string idEstacion = workStation.Id.ToString();

            modifiedDocument.Replace("{IdEstacion}", idEstacion, false, true);

            WParagraph paragraph = modifiedDocument.LastParagraph;
            WSection section = modifiedDocument.AddSection() as WSection;

            WPicture picture = (WPicture)paragraph.AppendPicture(imageBytes);

            float pageWidth = modifiedDocument.LastSection.PageSetup.PageSize.Width - modifiedDocument.LastSection.PageSetup.Margins.Left - modifiedDocument.LastSection.PageSetup.Margins.Right;
            float imageWidth = pageWidth;

            if (picture.Width > imageWidth)
            {
                float scaleFactor = imageWidth / picture.Width;
                picture.Width = imageWidth;
                picture.Height *= scaleFactor;
            }

            WParagraph additionalText = (WParagraph)modifiedDocument.LastSection.AddParagraph();
            additionalText.AppendText("El trabajador se compromete a hacer un adecuado y correcto uso de éstos, haciéndose responsable por su buen manejo y utilización, en el entendido que los costos de toda reparación que se generen por maltrato o daño y que estén por fuera de su garantía, incluso por su pérdida o necesidad de reemplazo, serán asumidos por éste autorizando a la empresa mediante la firma de la presente acta para realizar por nómina los descuentos a que haya lugar. En caso de terminación de la vinculación laboral o reversión de la modalidad de teletrabajo a presencial, el colaborador deberá retornar a la empresa los anteriores elementos y/o equipos.");

            WParagraph signatureLine = (WParagraph)modifiedDocument.LastSection.AddParagraph();
            signatureLine.AppendText("\n\n\n\nResponsable, \n\n______________________________\n C.C No. \n\n\n\n\n");


            WParagraph companyInfoRight = (WParagraph)section.HeadersFooters.Footer.AddParagraph();
            companyInfoRight.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
            companyInfoRight.AppendText("Principal: Av. 7N No. 26N - 35 Cali Colombia,\n");
            companyInfoRight.AppendText("Sedes: Bogotá - Medellín - Barranquilla\n");
            companyInfoRight.AppendText("Tel: (57)(2)641 0747\n");
            companyInfoRight.AppendText("Móvil: (57) 310 464 7027\n");
            companyInfoRight.AppendText("Email: contactenos@navisaf.com\n");
            companyInfoRight.AppendText("www.navisaf.co");


            DocIORenderer converter = new DocIORenderer();
            PdfDocument document = converter.ConvertToPDF(modifiedDocument);

            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            document.Close(true);

            byte[] pdfBytes = stream.ToArray();

            return pdfBytes;
        }

        [AllowAnonymous]
        public ActionResult DescargarPDF(string base64Image, int Id)
        {
            byte[] pdfBytes = GenerarPDF(base64Image, Id);

            return File(pdfBytes, "application/pdf", "ImageInsertion.pdf");
        }



        [HttpPost]
        public ActionResult GenerarPDFFinalizacion(string base64Image, int Id)
        {
            WorkStationModel workStationModel = new WorkStationModel();

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkStations/GetWorkStationDetails?id=" + Id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                workStationModel = JsonConvert.DeserializeObject<WorkStationModel>(content);
            }

            WorkStation workStation = new WorkStation();
            workStation.Id = workStationModel.Id;
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;

            List<Employee> employeesList = GetEmployees();
            foreach (var employee in employeesList)
            {
                if (workStation.EmployeeId == employee.Id)
                {
                    workStation.Employee = employee;
                }
            }

            var cd = System.AppContext.BaseDirectory;
            string rutaArchivoWord = _hostingEnvironment.WebRootPath + @"/pdfVM/ACTA_DE_FINALIZACION_DE_ELEMENTOS.docx";

            WordDocument originalDocument = new WordDocument();

            using (FileStream fileStream = new FileStream(rutaArchivoWord, FileMode.Open, FileAccess.Read))
            {
                originalDocument.Open(fileStream, FormatType.Docx);
            }

            WordDocument modifiedDocument = originalDocument.Clone();

            byte[] imageBytes = System.Convert.FromBase64String(base64Image);

            string idEstacion = workStation.Id.ToString();

            modifiedDocument.Replace("{IdEstacion}", idEstacion, false, true);

            WParagraph paragraph = modifiedDocument.LastParagraph;
            WPicture picture = (WPicture)paragraph.AppendPicture(imageBytes);

            float pageWidth = modifiedDocument.LastSection.PageSetup.PageSize.Width - modifiedDocument.LastSection.PageSetup.Margins.Left - modifiedDocument.LastSection.PageSetup.Margins.Right;
            float imageWidth = pageWidth;

            if (picture.Width > imageWidth)
            {
                float scaleFactor = imageWidth / picture.Width;
                picture.Width = imageWidth;
                picture.Height *= scaleFactor;
            }

            WParagraph additionalText = (WParagraph)modifiedDocument.LastSection.AddParagraph();
            additionalText.AppendText("");

            WParagraph signatureLine = (WParagraph)modifiedDocument.LastSection.AddParagraph();
            signatureLine.AppendText("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");

            WParagraph companyInfoRight = (WParagraph)modifiedDocument.LastSection.HeadersFooters.Footer.AddParagraph();
            companyInfoRight.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
            companyInfoRight.AppendText("Principal: Av. 7N No. 26N - 35 Cali Colombia,\n");
            companyInfoRight.AppendText("Sedes: Bogotá - Medellín - Barranquilla\n");
            companyInfoRight.AppendText("Tel: (57)(2)641 0747\n");
            companyInfoRight.AppendText("Móvil: (57) 310 464 7027\n");
            companyInfoRight.AppendText("Email: contactenos@navisaf.com\n");
            companyInfoRight.AppendText("www.navisaf.co");

            DocIORenderer converter = new DocIORenderer();
            PdfDocument document = converter.ConvertToPDF(modifiedDocument);

            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            document.Close(true);

            byte[] pdfBytes = stream.ToArray();
            return File(pdfBytes, "application/pdf", "ImageInsertion.pdf");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;
using Newtonsoft.Json;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen")]
    public class DevicesController : Controller
    {

        // GET: Devices
        public IActionResult Index(Status? status)
        {
            List<Device> devicesList= new List<Device>();

            using(var client= new HttpClient())
            {
                string url = APIConnection.Url + "Devices/GetDevices";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    devicesList = JsonConvert.DeserializeObject<List<Device>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<Subcategory> subcategoriesList= GetSubcategories();
            for (int i = 0; i < devicesList.Count; i++)
            {
                foreach (var subcategory in subcategoriesList)
                {
                    if (devicesList[i].DeviceBrandId==subcategory.Id)
                    {
                        devicesList[i].DeviceBrand = subcategory;
                    }
                    if (devicesList[i].DeviceTypeId == subcategory.Id)
                    {
                        devicesList[i].DeviceType = subcategory;
                    }
                    if(devicesList[i].ProcessorId == subcategory.Id)
                    {
                        devicesList[i].Processor = subcategory;
                    }
                }
            }

            List<SoftwareLicense> softwareLicenses = GetSoftwareLicenses();

            for (int i = 0; i < devicesList.Count; i++)
            {
                foreach (var softwareLicense in softwareLicenses)
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

            ViewBag.datasource = devicesList
              .Select(y => new
              {
                  Id = y.Id,
                  DeviceType = y.DeviceType,
                  DeviceBrand = y.DeviceBrand,
                  Model = y.Model,
                  Series = y.Series,
                  Ram = y.Ram,
                  ProcessorId = y.Processor,
                  SolidStateDrive = y.SolidStateDrive,
                  HardDiskDrive = y.HardDiskDrive,
                  WindowsId = y.WindowsLicense != null ? y.WindowsLicense.Name : "No Disponible",
                  AntivirusId = y.AntivirusLicense != null ? y.AntivirusLicense.Name : "No Disponible"
              })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(string Series, int Id, int DeviceTypeId)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Devices/IsNameAvailable?Series="+Series+"&Id="+Id+"&DeviceTypeId="+DeviceTypeId;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Dispositivo!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            DeviceModel deviceModel;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetDevice?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                deviceModel = JsonConvert.DeserializeObject<DeviceModel>(content);

            }

            Device device = new Device();
            device.Id = deviceModel.Id;
            device.DeviceTypeId = deviceModel.DeviceTypeId;
            device.DeviceBrandId = deviceModel.DeviceBrandId;
            device.Model = deviceModel.Model;
            device.Series = deviceModel.Series;
            device.ProcessorId = deviceModel.ProcessorId;
            device.Ram = deviceModel.Ram;
            device.SolidStateDrive = deviceModel.SolidStateDrive;
            device.HardDiskDrive = deviceModel.HardDiskDrive;
            device.WindowsLicenseId = deviceModel.WindowsLicenseId;
            device.AntivirusLicenseId = deviceModel.AntivirusLicenseId;

            if (device == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Dispositivo!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }


            List<Subcategory> subcategoriesList = GetSubcategories();
            
            foreach (var subcategory in subcategoriesList)
            {
                if (device.DeviceBrandId == subcategory.Id)
                {
                    device.DeviceBrand = subcategory;
                }
                if (device.DeviceTypeId == subcategory.Id)
                {
                    device.DeviceType = subcategory;
                }
                if (device.ProcessorId == subcategory.Id)
                {
                    device.Processor = subcategory;
                }
            }
            

            List<SoftwareLicense> softwareLicenses = GetSoftwareLicenses();

            foreach (var softwareLicense in softwareLicenses)
            {
                if (device.WindowsLicenseId == softwareLicense.Id)
                {
                    device.WindowsLicense = softwareLicense;
                }
                if (device.AntivirusLicenseId == softwareLicense.Id)
                {
                    device.AntivirusLicense = softwareLicense;
                }
            }

            return View(device);
        }

        private List<Subcategory> GetSubcategories()
        {
            List<Subcategory> subcategoriesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetSubcategories";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                subcategoriesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);

            }
            return subcategoriesList;
        }
        private List<SoftwareLicense> GetSoftwareLicenses()
        {
            List<SoftwareLicense> softwareLicenseList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetSoftwareLicenses";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                softwareLicenseList = JsonConvert.DeserializeObject<List<SoftwareLicense>>(content);

            }
            return softwareLicenseList;
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            List<Subcategory> subcategoriesList = GetSubcategories();
            List<SoftwareLicense> softwareLicenseList= GetSoftwareLicenses();

            var listaProcesadoresPc = subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 5).Select(y => new { value = y.Id, text = y.Name });
            var listaProcesadoresTel = subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 6).Select(y => new { value = y.Id, text = y.Name });

            ViewData["ListaProcesadoresIdPc"] = listaProcesadoresPc;
            ViewData["ListaProcesadoresIdTel"] = listaProcesadoresTel;
            ViewData["AntivirusLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 5).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name");
            ViewData["DeviceBrandId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 4), "Id", "Name");
            ViewData["DeviceTypeId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 3), "Id", "Name");
            ViewData["WindowsLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 4).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeviceTypeId,DeviceBrandId,Model,Series,ProcessorId,Ram,SolidStateDrive,HardDiskDrive,WindowsLicenseId,AntivirusLicenseId")] Device device)
        {
            if (ModelState.IsValid)
            {
                DeviceModel deviceModel = new DeviceModel();
                deviceModel.Id = device.Id;
                deviceModel.DeviceTypeId = device.DeviceTypeId;
                deviceModel.DeviceBrandId = device.DeviceBrandId;
                deviceModel.Model = device.Model;
                deviceModel.Series = device.Series;
                deviceModel.ProcessorId = device.ProcessorId;
                deviceModel.Ram = device.Ram;
                deviceModel.SolidStateDrive = device.SolidStateDrive;
                deviceModel.HardDiskDrive = device.HardDiskDrive;
                deviceModel.WindowsLicenseId = device.WindowsLicenseId;
                deviceModel.AntivirusLicenseId = device.AntivirusLicenseId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Devices/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(deviceModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Dispositivo Creado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            List<Subcategory> subcategoriesList = GetSubcategories();
            List<SoftwareLicense> softwareLicenseList = GetSoftwareLicenses();

            var listaProcesadoresPc = subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 5).Select(y => new { value = y.Id, text = y.Name });
            var listaProcesadoresTel = subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 6).Select(y => new { value = y.Id, text = y.Name });

            ViewData["ListaProcesadoresIdPc"] = listaProcesadoresPc;
            ViewData["ListaProcesadoresIdTel"] = listaProcesadoresTel;
            ViewData["AntivirusLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 5).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name");
            ViewData["DeviceBrandId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 4), "Id", "Name");
            ViewData["DeviceTypeId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 3), "Id", "Name");
            ViewData["WindowsLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 4).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name");
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Dispositivo!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            DeviceModel deviceModel;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/GetDevice?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                deviceModel = JsonConvert.DeserializeObject<DeviceModel>(content);

            }

            if (deviceModel == null || ((int)deviceModel.Status) == 2)
            {

                TempData["AlertMessage"] = "No se puede acceder a este Dispositivo!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            List<Subcategory> subcategoriesList = GetSubcategories();
            List<SoftwareLicense> softwareLicenseList = GetSoftwareLicenses();

            ViewData["DeviceTypeId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 3), "Id", "Name", deviceModel.DeviceTypeId);
            ViewData["AntivirusLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 5 || s.Id == deviceModel.AntivirusLicenseId).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name", deviceModel.AntivirusLicenseId);
            ViewData["DeviceBrandId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 4 || s.Id == deviceModel.DeviceBrandId), "Id", "Name", deviceModel.DeviceBrandId);
            ViewData["WindowsLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 4 || s.Id == deviceModel.WindowsLicenseId).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name", deviceModel.WindowsLicenseId);
            ViewData["ListaProcesadoresIdPc"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 5 || s.Id == deviceModel.ProcessorId), "Id", "Name", deviceModel.ProcessorId);
            ViewData["ListaProcesadoresIdTel"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 6 || s.Id == deviceModel.ProcessorId), "Id", "Name", deviceModel.ProcessorId);
            return View(deviceModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceTypeId,DeviceBrandId,Model,Series,ProcessorId,Ram,SolidStateDrive,HardDiskDrive,WindowsLicenseId,AntivirusLicenseId")] Device device)
        {
            if (id != device.Id)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Dispositivo!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                DeviceModel deviceModel = new DeviceModel();
                deviceModel.Id = device.Id;
                deviceModel.DeviceTypeId = device.DeviceTypeId;
                deviceModel.DeviceBrandId = device.DeviceBrandId;
                deviceModel.Model = device.Model;
                deviceModel.Series = device.Series;
                deviceModel.ProcessorId = device.ProcessorId;
                deviceModel.Ram = device.Ram;
                deviceModel.SolidStateDrive = device.SolidStateDrive;
                deviceModel.HardDiskDrive = device.HardDiskDrive;
                deviceModel.WindowsLicenseId = device.WindowsLicenseId;
                deviceModel.AntivirusLicenseId = device.AntivirusLicenseId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Devices/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(deviceModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Dispositivo Editado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["AlertMessage"] = "No se puede editar este Dispositivo!";
                        TempData["SweetAlertType"] = "error";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            List<Subcategory> subcategoriesList = GetSubcategories();
            List<SoftwareLicense> softwareLicenseList = GetSoftwareLicenses();

            ViewData["AntivirusLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 5 || s.Id == device.AntivirusLicenseId).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name", device.AntivirusLicenseId);
            ViewData["DeviceBrandId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 4 || s.Id == device.DeviceBrandId), "Id", "Name", device.DeviceBrandId);
            ViewData["DeviceTypeId"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 3), "Id", "Name", device.DeviceTypeId);
            ViewData["WindowsLicenseId"] = new SelectList(softwareLicenseList.OrderBy(c => c.Name).Where(s => ((int)s.Status) == 1 && s.SubcategoryId == 4 || s.Id == device.WindowsLicenseId).Select(y => new { Id = y.Id, Name = y.Name + " " + y.Series.Substring(y.Series.Length - 4) }), "Id", "Name", device.WindowsLicenseId);
            ViewData["ListaProcesadoresIdPc"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 5 || s.Id == device.ProcessorId), "Id", "Name", device.ProcessorId);
            ViewData["ListaProcesadoresIdTel"] = new SelectList(subcategoriesList.OrderBy(c => c.Name).Where(s => s.CategoryId == 6 || s.Id == device.ProcessorId), "Id", "Name", device.ProcessorId);
            return View(device);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Devices/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);
                if (result==true)
                {
                    TempData["AlertMessage"] = "Dispositivo Eliminado Exitosamente!";
                    TempData["SweetAlertType"] = "success";
                }
                else
                {
                    TempData["AlertMessage"] = "No se puede Eliminar este Dispositivo!";
                    TempData["SweetAlertType"] = "error";
                }
            }

            return RedirectToAction("Index");

        }

    }
}
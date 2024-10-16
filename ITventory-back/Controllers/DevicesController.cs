using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WebApplication1.Models;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    //[Authorize(Roles = "Administrador, Almacen")]
    [ApiController]
    [Route("Devices")]
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devices
        /// <summary>
        /// Obtiene la lista de dispositivos 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los dispositivos de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de dispositivos
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de dispositivos</response>
        /// <response code="500">Si ocurre un error al obtener los dispositivos</response>
        [HttpGet]
        [Route("GetDevices")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Indica que devuelve un 200 en caso de éxito
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que devuelve un 500 en caso de error
        public string Index()
        {
            IQueryable<Device> devicesQuery = _context.Devices.Where(w => !w.IsDeleted);

            List<Device> devicesList = devicesQuery.ToList();

            string json= JsonConvert.SerializeObject(devicesList);

            return json;
        }

        /// <summary>
        /// Permite ver si el nombre esta disponible
        /// </summary>
        /// <remarks>
        /// Este endpoint permite saber si un nombre ya esta registrado
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON que contiene un booleano</response>
        /// <response code="500">Si ocurre un error al ver los nombres registrados</response>
        [HttpGet("IsNameAvailable")]
        public IActionResult IsNameAvailable(string Series, int Id, int DeviceTypeId)
        {
            bool isAvailable = !_context.Devices.Any(w => w.Series == Series && w.Id != Id && w.DeviceTypeId == DeviceTypeId && !w.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene una lista de subcategorias
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista subcategorias
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene una lista de subcategorias
        /// </returns>
        /// <response code="200">Devuelve un JSON que contiene una lista de subcategorias</response>
        /// <response code="500">Si ocurre un error al obtener las subcategorias</response>
        [HttpGet("GetSubcategories")]
        public IActionResult GetSubcategories()
        {
            List<Subcategory> subcategoriesList=_context.Subcategories.ToList();
            return Json(subcategoriesList);
        }

        /// <summary>
        /// Obtiene una lista de licencias de software
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener una lista de licencias de software
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene una lista de licencias de software
        /// </returns>
        /// <response code="200">Devuelve un JSON que contiene una lista de licencias de software</response>
        /// <response code="500">Si ocurre un error al obtener las licencias de software</response>
        [HttpGet("GetSoftwareLicenses")]
        public IActionResult GetSoftwareLicenses()
        {
            List<SoftwareLicense> softwareLicensesList=_context.SoftwareLicenses.ToList();
            return Json(softwareLicensesList);
        }


        /// <summary>
        /// Permite crear un nuevo dispositivo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un dispositivo
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el dispositivo</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DeviceModel deviceModel)
        {
            Device device = new Device();
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

            if (device.DeviceTypeId == 7)
            {
                device.WindowsLicenseId = null;
                device.AntivirusLicenseId = null;
            }

            var licenseWindows = await _context.SoftwareLicenses.FindAsync(device.WindowsLicenseId);
            var licenseAntivirus = await _context.SoftwareLicenses.FindAsync(device.AntivirusLicenseId);

            if (licenseWindows != null && ((int)licenseWindows.Status) == 1)
            {
                licenseWindows.Status = (Status)2;
                _context.Update(licenseWindows);
            }

            if (licenseAntivirus != null && ((int)licenseAntivirus.Status) == 1)
            {
                licenseAntivirus.Status = (Status)2;
                _context.Update(licenseAntivirus);
            }

            _context.Add(device);
            await _context.SaveChangesAsync();
            return Json(true);
        }


        /// <summary>
        /// Permite obtener un dispositivo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener un dispositivo de la base de datos mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un dispositivo
        /// </returns>
        /// <response code="200">Devuelve un JSON con un dispositivo</response>
        /// <response code="500">Si ocurre un error al obtener el dispositivo</response>
        [HttpGet("GetDevice")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return StatusCode(500,"Id nulo o error en la conexión a la base de datos");
            }

            var device = await _context.Devices.Include(x => x.DeviceType).SingleOrDefaultAsync(d => d.Id == id);

            if (device == null || device.IsDeleted || ((int)device.Status) == 2)
            {
                return StatusCode(500,"Error al obtener el dispositivo");
            }

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

            return Json(deviceModel);

        }

        /// <summary>
        /// Permite editar un dispositivo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un dispositivo de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al obtener el dispositivo</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(DeviceModel deviceModel)
        {
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

            var existingComputer = await _context.Devices.AsNoTracking().FirstOrDefaultAsync(c => c.Id == device.Id);
            if (existingComputer == null)
            {
                return StatusCode(500,"No se encontró un dispositivo con ese Id");
            }

            try
            {
                if (device.DeviceTypeId == 7)
                {
                    if (device.WindowsLicense == null)
                    {
                        _context.Update(device);
                    }
                    if (device.AntivirusLicense == null)
                    {
                        _context.Update(device);
                    }
                }
                else
                {
                    UpdateWindowsLicense(existingComputer, device);
                    UpdateAntivirusLicense(existingComputer, device);
                }

                _context.Update(device);

                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(device.Id))
                {
                    return StatusCode(500,"Error en la conexión con la base de datos");
                }
                else
                {
                    return StatusCode(500, "Error en la conexión con la base de datos");
                }
            }
        }

        private void UpdateWindowsLicense(Device existingComputer, Device updatedComputer)
        {
            if (existingComputer.WindowsLicenseId != updatedComputer.WindowsLicenseId)
            {
                var previousWindowsLicense = _context.SoftwareLicenses.Find(existingComputer.WindowsLicenseId);
                if (previousWindowsLicense != null)
                {
                    previousWindowsLicense.Status = (Status)1;
                    _context.Update(previousWindowsLicense);
                }

                var newWindowsLicense = _context.SoftwareLicenses.Find(updatedComputer.WindowsLicenseId);
                if (newWindowsLicense != null)
                {
                    if (((int)existingComputer.Status) == 1 && !newWindowsLicense.IsDeleted)
                    {
                        newWindowsLicense.Status = (Status)2;
                    }
                    else if (existingComputer.IsDeleted)
                    {
                        newWindowsLicense.Status = (Status)1;
                    }
                    _context.Update(newWindowsLicense);
                }
            }
        }

        private void UpdateAntivirusLicense(Device existingComputer, Device updatedComputer)
        {
            if (existingComputer.AntivirusLicenseId != updatedComputer.AntivirusLicenseId)
            {
                var previousAntivirusLicense = _context.SoftwareLicenses.Find(existingComputer.AntivirusLicenseId);
                if (previousAntivirusLicense != null)
                {
                    previousAntivirusLicense.Status = (Status)1;
                    _context.Update(previousAntivirusLicense);
                }

                var newAntivirusLicense = _context.SoftwareLicenses.Find(updatedComputer.AntivirusLicenseId);
                if (newAntivirusLicense != null)
                {
                    if (((int)existingComputer.Status) == 1 && !newAntivirusLicense.IsDeleted)
                    {
                        newAntivirusLicense.Status = (Status)2;
                    }
                    else if (existingComputer.IsDeleted)
                    {
                        newAntivirusLicense.Status = (Status)1;
                    }
                    _context.Update(newAntivirusLicense);
                }
            }
        }

        /// <summary>
        /// Permite eliminar un dispositivo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un dispositivo de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON que contiene un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar el dispositivo</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            var devices = _context.Devices.Find(id);

            if (((int)devices.Status) == 2)
            {
                return StatusCode(500, "Error, el dispositivo se encuentra asignado");
            }

            if (devices != null)
            {
                if (devices.AntivirusLicenseId.HasValue)
                {
                    var antivirusLicense = await _context.SoftwareLicenses.FindAsync(devices.AntivirusLicenseId.Value);
                    if (antivirusLicense != null)
                    {
                        antivirusLicense.Status = (Status)1;
                    }
                }

                if (devices.WindowsLicenseId.HasValue)
                {
                    var windowsLicense = await _context.SoftwareLicenses.FindAsync(devices.WindowsLicenseId.Value);
                    if (windowsLicense != null)
                    {
                        windowsLicense.Status = (Status)1;
                    }
                }


                devices.Status = (Status)1;
                devices.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500, "Error, no se encontró un dispositivo con ese Id");
            }
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
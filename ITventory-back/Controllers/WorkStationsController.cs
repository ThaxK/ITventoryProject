using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using NuGet.Packaging;
using System.Drawing;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Framework;
using System.IO.Compression;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("WorkStations")]
    public class WorkStationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public WorkStationsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: WorkStations
        /// <summary>
        /// Obtiene la lista de estaciones de trabajo 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las estaciones de trabajo de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de estaciones de trabajo
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de estaciones de trabajo</response>
        /// <response code="500">Si ocurre un error al obtener las estaciones de trabajo</response>
        [HttpGet("GetWorkStations")]
        public string Index()
        {
            IQueryable<WorkStation> workstationQuery = _context.WorkStations;

            List<WorkStation> workStationsList= workstationQuery.ToList();

            string json= JsonConvert.SerializeObject(workStationsList);

            return json;
        }

        /// <summary>
        /// Permite saber si una estación de trabajo ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si una estación de trabajo ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar la estación de trabajo en la base de datos</response>
        [HttpGet("IsNameAvailable")]
        public IActionResult IsNameAvailable(int Id, int EmployeeId)
        {
            bool isAvailable = !_context.WorkStations.Any(w => w.Id != Id && w.EmployeeId == EmployeeId);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene la lista de empleados 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los empleados de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de empleados
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de empleados</response>
        /// <response code="500">Si ocurre un error al obtener los empleados</response>
        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            List<Employee> employeesList= _context.Employees.ToList();
            return Json(employeesList);
        }

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
        [HttpGet("GetDevices")]
        public IActionResult GetDevices()
        {
            List<Device> devicesList= _context.Devices.ToList();
            return Json(devicesList);
        }

        /// <summary>
        /// Obtiene la lista de perifericos mecanicos
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los perifericos mecanicos de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de perifericos mecanicos
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de perifericos mecanicos</response>
        /// <response code="500">Si ocurre un error al obtener los perifericos mecanicos</response>
        [HttpGet("GetOtherPeripherals")]
        public IActionResult GetOtherPeripherals()
        {
            List<OtherPeripheral> otherPeripheralsList= _context.OtherPeripherals.ToList();
            return Json(otherPeripheralsList);
        }

        /// <summary>
        /// Obtiene la lista de perifericos 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los perifericos de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de perifericos
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de perifericos</response>
        /// <response code="500">Si ocurre un error al obtener los perifericos</response>
        [HttpGet("GetPeripherals")]
        public IActionResult GetPeripherals()
        {
            List<Peripheral> peripheralsList= _context.Peripherals.ToList();
            return Json(peripheralsList);
        }

        // GET: WorkStations/Details/5
        /// <summary>
        /// Obtiene una estación de trabajo con todos sus detalles
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los detalles de un estación de trabajo mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la estación de trabajo
        /// </returns>
        /// <response code="200">Devuelve un JSON con la estación de trabajo</response>
        /// <response code="500">Si ocurre un error al obtener la estación de trabajo</response>
        [HttpGet("GetWorkStationDetails")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkStations == null)
            {
                return StatusCode(500,"Id nulo o error en la conexión con la base de datos");
            }

            WorkStation workStation = await _context.WorkStations
                .Include(w => w.Employee)
                .Include(w => w.ComputerDevice).ThenInclude(w => w.AntivirusLicense)
                .Include(w => w.ComputerDevice).ThenInclude(w => w.WindowsLicense)
                .Include(w => w.ComputerDevice).ThenInclude(w => w.DeviceBrand)
                .Include(w => w.ComputerDevice).ThenInclude(w => w.Processor)
                .Include(w => w.ComputerDevice).ThenInclude(w => w.DeviceType)
                .Include(w => w.SmartPhoneDevice).ThenInclude(w => w.DeviceBrand)
                .Include(w => w.SmartPhoneDevice).ThenInclude(w => w.Processor)
                .Include(w => w.SmartPhoneDevice).ThenInclude(w => w.DeviceType)
                .Include(w => w.Peripherals).ThenInclude(w => w.PeripheralType)
                .Include(w => w.Peripherals).ThenInclude(w => w.PeripheralBrand)
                .Include(w => w.OtherPeripherals)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (workStation == null)
            {
                return StatusCode(500,"No existe una estación de trabajo con ese Id");
            }

            workStation.OtherPeripheralsIds = new List<int>();
            workStation.OtherPeripheralsIds = workStation.OtherPeripherals.Select(op => op.Id).ToList();
            workStation.PeripheralsIds = new List<int>();
            workStation.PeripheralsIds = workStation.Peripherals.Select(p => p.Id).ToList();


            WorkStationModel workStationModel = new WorkStationModel();
            workStationModel.Id = workStation.Id;
            workStationModel.EmployeeId = workStation.EmployeeId;
            workStationModel.ComputerDeviceId = workStation.ComputerDeviceId;
            workStationModel.SmartPhoneDeviceId = workStation.SmartPhoneDeviceId;
            workStationModel.PeripheralsIds = workStation.PeripheralsIds;
            workStationModel.OtherPeripheralsIds = workStation.OtherPeripheralsIds;

            return Json(workStationModel);
        }

        /// <summary>
        /// Permite crear una estación de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una estación de trabajo nueva en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear la estación de trabajo</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(WorkStationModel workStationModel)
        {
            WorkStation workStation = new WorkStation();
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;


            workStation.Peripherals = new List<Peripheral>();
            await AddPeripheralsToWorkStation(workStation);

            workStation.OtherPeripherals = new List<OtherPeripheral>();
            await AddOtherPeripheralsToWorkStation(workStation);

            var employee = await _context.Employees.FindAsync(workStation.EmployeeId);
            var devices = await _context.Devices.Where(d => d.Id == workStation.ComputerDeviceId || d.Id == workStation.SmartPhoneDeviceId).ToListAsync();

            if (employee == null || devices.Count < 1)
            {
                return StatusCode(500,"Empleado inexistente o computador no asignado");
            }

            employee.Status = (Status)2;
            devices.ForEach(d => d.Status = (Status)2);

            _context.Add(workStation);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        private async Task AddPeripheralsToWorkStation(WorkStation workStation)
        {
            var peripheralsIds = new List<int>();

            if (workStation.PeripheralsIds != null)
            {
                peripheralsIds.AddRange(workStation.PeripheralsIds);
            }

            foreach (var peripheralId in peripheralsIds)
            {
                var peripheral = await _context.Peripherals.FindAsync(peripheralId);
                if (peripheral != null)
                {
                    workStation.Peripherals.Add(peripheral);
                    peripheral.Status = (Status)2;
                }
            }
        }

        private async Task AddOtherPeripheralsToWorkStation(WorkStation workStation)
        {
            var otherPeripheralsIds = new List<int>();

            if (workStation.OtherPeripheralsIds != null)
            {
                otherPeripheralsIds.AddRange(workStation.OtherPeripheralsIds);
            }

            foreach (var otherPeripheralId in otherPeripheralsIds)
            {
                var otherPeripheral = await _context.OtherPeripherals.FindAsync(otherPeripheralId);
                if (otherPeripheral != null)
                {
                    workStation.OtherPeripherals.Add(otherPeripheral);
                }
            }
        }

        /// <summary>
        /// Permite editar una estación de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar una estación de trabajo en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar la estación de trabajo</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(WorkStationModel workStationModel)
        {
            WorkStation workStation = new WorkStation();
            workStation.Id = workStationModel.Id;
            workStation.EmployeeId = workStationModel.EmployeeId;
            workStation.ComputerDeviceId = workStationModel.ComputerDeviceId;
            workStation.SmartPhoneDeviceId = workStationModel.SmartPhoneDeviceId;
            workStation.PeripheralsIds = workStationModel.PeripheralsIds;
            workStation.OtherPeripheralsIds = workStationModel.OtherPeripheralsIds;

            try
            {
                var existingWorkStation = await _context.WorkStations
                 .Include(x => x.Employee)
                 .Include(x => x.ComputerDevice)
                 .Include(x => x.SmartPhoneDevice)
                 .Include(x => x.Peripherals)
                 .Include(x => x.OtherPeripherals)
                 .FirstOrDefaultAsync(w => w.Id == workStation.Id);


                await UpdateEmployeeStatus(existingWorkStation, workStation);
                await UpdateDeviceStatus(existingWorkStation, workStation);
                await UpdateSmartPhoneStatus(existingWorkStation, workStation);
                await UpdatePeripheralsInWorkStation(existingWorkStation, workStation);
                await UpdateOthersPeripheralsInWorkStation(existingWorkStation, workStation);
                _context.Update(existingWorkStation);
                await _context.SaveChangesAsync();
                return Json(true);

            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500,"Error en la conexion con la base de datos al editar");
            }


        }

        private async Task UpdateEmployeeStatus(WorkStation existingWorkStation, WorkStation workStation)
        {
            if (existingWorkStation.EmployeeId != workStation.EmployeeId)
            {
                var existingEmployee = await _context.Employees.FindAsync(existingWorkStation.EmployeeId);
                if (existingEmployee != null)
                {
                    existingEmployee.Status = (Status)1;
                    _context.Update(existingEmployee);
                }

                var newEmployee = await _context.Employees.FindAsync(workStation.EmployeeId);
                if (newEmployee != null)
                {
                    existingWorkStation.EmployeeId = newEmployee.Id;
                    newEmployee.Status = (Status)2;
                    _context.Update(newEmployee);
                }
            }
        }

        private async Task UpdateDeviceStatus(WorkStation existingWorkStation, WorkStation workStation)
        {
            if (existingWorkStation.ComputerDeviceId != workStation.ComputerDeviceId)
            {
                var existingDevice = await _context.Devices.FindAsync(existingWorkStation.ComputerDeviceId);
                if (existingDevice != null)
                {
                    existingDevice.Status = (Status)1;
                    _context.Update(existingDevice);
                }

                var newDevice = await _context.Devices.FindAsync(workStation.ComputerDeviceId);
                if (newDevice != null)
                {
                    existingWorkStation.ComputerDeviceId = newDevice.Id;
                    newDevice.Status = (Status)2;
                    _context.Update(newDevice);
                }
            }
        }

        private async Task UpdateSmartPhoneStatus(WorkStation existingWorkStation, WorkStation workStation)
        {
            if (existingWorkStation.SmartPhoneDeviceId != workStation.SmartPhoneDeviceId)
            {
                var existingSmartPhone = await _context.Devices.FindAsync(existingWorkStation.SmartPhoneDeviceId);
                if (existingSmartPhone != null)
                {
                    existingSmartPhone.Status = (Status)1;
                    _context.Update(existingSmartPhone);
                }

                var newSmartPhone = await _context.Devices.FindAsync(workStation.SmartPhoneDeviceId);
                if (newSmartPhone != null)
                {
                    existingWorkStation.SmartPhoneDeviceId = newSmartPhone.Id;
                    newSmartPhone.Status = (Status)2;
                    _context.Update(newSmartPhone);
                }
                else
                {
                    existingWorkStation.SmartPhoneDeviceId = null;
                }
            }
        }

        private async Task UpdatePeripheralsInWorkStation(WorkStation existingWorkStation, WorkStation workStation)
        {
            if (existingWorkStation != null && workStation != null)
            {
                var updatedPeripheralIds = workStation.PeripheralsIds ?? new List<int>();

                var peripheralsToRemove = existingWorkStation.Peripherals.Where(p => !updatedPeripheralIds.Contains(p.Id)).ToList();
                var peripheralsToAddIds = updatedPeripheralIds.Except(existingWorkStation.Peripherals.Select(p => p.Id)).ToList();

                // Elimina los periféricos
                existingWorkStation.Peripherals = existingWorkStation.Peripherals.Except(peripheralsToRemove).ToList();
                foreach (var peripheral in peripheralsToRemove)
                {
                    peripheral.Status = (Status)1;
                    _context.Update(peripheral);
                }

                // Agrega los nuevos periféricos
                foreach (var id in peripheralsToAddIds)
                {
                    var peripheral = await _context.Peripherals.FindAsync(id);
                    if (peripheral != null)
                    {
                        existingWorkStation.Peripherals.Add(peripheral);
                        peripheral.Status = (Status)2;
                        _context.Update(peripheral);
                    }
                }
            }
        }

        private async Task UpdateOthersPeripheralsInWorkStation(WorkStation existingWorkStation, WorkStation workStation)
        {
            if (existingWorkStation != null)
            {
                var existingOtherPeripheralIds = existingWorkStation.OtherPeripherals.Select(o => o.Id).ToList();
                var updatedOtherPeripheralIds = workStation.OtherPeripheralsIds ?? new List<int>();

                var otherPeripheralsToRemove = existingWorkStation.OtherPeripherals
                    .Where(o => !updatedOtherPeripheralIds.Contains(o.Id))
                    .ToList();
                var otherPeripheralsToAddIds = updatedOtherPeripheralIds.Except(existingOtherPeripheralIds).ToList();

                foreach (var otherPeripheralToRemove in otherPeripheralsToRemove)
                {
                    if (otherPeripheralToRemove != null)
                    {
                        existingWorkStation.OtherPeripherals.Remove(otherPeripheralToRemove);

                        var otherPeripheral = await _context.OtherPeripherals.FindAsync(otherPeripheralToRemove.Id);
                        _context.Update(otherPeripheral);
                    }
                }

                foreach (var otherPeripheralId in otherPeripheralsToAddIds)
                {
                    var otherPeripheral = await _context.OtherPeripherals.FindAsync(otherPeripheralId);
                    if (otherPeripheral != null)
                    {
                        existingWorkStation.OtherPeripherals.Add(otherPeripheral);
                        _context.Update(otherPeripheral);
                    }
                }
            }
            _context.Update(existingWorkStation);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Permite eliminar una estación de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una estación de trabajo de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero 1= Estación de trabajo inexistente, 2= Estación de trabajo asignada, 3= Estación de trabajo eliminada exitosamente</response>
        /// <response code="500">Si ocurre un error al eliminar la estación de trabajo</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            var workstation = await _context.WorkStations
                        .Include(w => w.ComputerDevice)
                        .Include(w => w.SmartPhoneDevice)
                        .Include(w => w.Peripherals)
                        .Include(w => w.OtherPeripherals)
                        .Include(w => w.Employee)
                        .FirstOrDefaultAsync(w => w.Id == id);

            if (workstation == null)
            {
                return Json(1);
            }

            if (workstation.Status == (Status)2)
            {
                return Json(2);
            }

            AssignStatusOfWorkstation(workstation);
            UpdatePeripheralStatus(workstation, true);
            UpdateOtherPeripheralStatus(workstation, true);

            workstation.Peripherals = null;
            workstation.OtherPeripherals = null;

            _context.Update(workstation);

            await _context.SaveChangesAsync();
            return Json(3);
        }

        private void UpdatePeripheralStatus(WorkStation workStation, bool removeItems)
        {
            if (workStation != null)
            {
                foreach (var peripheral in workStation.Peripherals.ToList())
                {
                    peripheral.Status = (Status)1;
                    _context.Update(peripheral);

                    if (removeItems)
                    {
                        workStation.Peripherals.Remove(peripheral);
                    }
                }
            }
        }

        private void UpdateOtherPeripheralStatus(WorkStation workStation, bool removeItems)
        {
            if (workStation.OtherPeripherals != null)
            {
                foreach (var otherPeripheral in workStation.OtherPeripherals.ToList())
                {
                    if (removeItems)
                    {
                        workStation.OtherPeripherals.Remove(otherPeripheral);
                    }
                }
            }
        }

        private void AssignStatusOfWorkstation(WorkStation workstation)
        {

            if (workstation.SmartPhoneDeviceId != null)
            {
                workstation.SmartPhoneDevice.Status = (Status)1;
            }

            workstation.ComputerDevice.Status = (Status)1;
            workstation.Employee.Status = (Status)1;
            workstation.Status = (Status)1;
            workstation.IsDeleted = true;
        }

        private bool WorkStationExists(int id)
        {
            return _context.WorkStations.Any(e => e.Id == id);
        }
    }
}

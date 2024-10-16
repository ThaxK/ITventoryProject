using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("OfficeSuites")]
    public class OfficeSuitesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfficeSuitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de suites de office 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las suites de office que estan en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de suites de office
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de suites de office</response>
        /// <response code="500">Si ocurre un error al obtener las suites de office</response>
        [HttpGet("GetOficceSuites")]
        public string Index()
        {
            IQueryable<OfficeSuite> officeSuitesQuery = _context.OfficeSuites;

            List<OfficeSuite> officeSuitesList = officeSuitesQuery.ToList<OfficeSuite>();

            foreach (var officeSuite in officeSuitesList)
            {
                officeSuite.SelectedEmployeeIds = officeSuite.Employees.Select(e => e.Id).ToList();
            }

            string json= JsonConvert.SerializeObject(officeSuitesList); 

            return json;
        }

        /// <summary>
        /// Permite saber si un usuario ya tiene una suite de office registrada 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un usuario ya tiene una suite de office registrada
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el usuario en la base de datos</response>
        [HttpGet("IsUserNameAvailable")]
        public IActionResult IsUserNameAvailable(string UserName, int id)
        {
            bool isAvailable = !_context.OfficeSuites.Any(e => e.UserName == UserName && e.Id != id && !e.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene una lista de suites de office con todos sus detalles y empleados asignados
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene una lista de suites de office con todos sus detalles y empleados asignados
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene una lista con las suites de office
        /// </returns>
        /// <response code="200">Devuelve un JSON con una lista de suites de office</response>
        /// <response code="500">Si ocurre un error al consultar las suites de office</response>
        [HttpGet("DetailsList")]
        public string DetailsList()
        {
            IQueryable<OfficeSuite> officeSuitesQuery = _context.OfficeSuites;

            List<OfficeSuite> officeSuitesList = officeSuitesQuery.ToList<OfficeSuite>();
            List<OfficeSuiteModelDetails> officeSuiteModelDetailsList = new List<OfficeSuiteModelDetails>();

            foreach (var officeSuite in officeSuitesList)
            {
                officeSuite.SelectedEmployeeIds = officeSuite.Employees.Select(e => e.Id).ToList();
                OfficeSuiteModelDetails officeSuiteModelDetail = new OfficeSuiteModelDetails
                {
                    Id = officeSuite.Id,
                    UserName = officeSuite.UserName,
                    Series = officeSuite.Series,
                    StartDate = officeSuite.StartDate,
                    FinishDate = officeSuite.FinishDate,
                    Status = officeSuite.Status,
                    Stock = officeSuite.Stock,
                    SelectedEmployeeIds = new List<int>(officeSuite.SelectedEmployeeIds)
                };
                officeSuiteModelDetailsList.Add(officeSuiteModelDetail);
            }

            string json = JsonConvert.SerializeObject(officeSuiteModelDetailsList);

            return json;
        }

        /// <summary>
        /// Obtiene una suite de office con todos sus detalles y empleados asignados
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene una suite de office con todos sus detalles y empleados asignados
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la suite de office
        /// </returns>
        /// <response code="200">Devuelve un JSON con una suite de office</response>
        /// <response code="500">Si ocurre un error al consultar la suite de office</response>
        [HttpGet("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OfficeSuites == null)
            {
                return Json(false);
            }

            var officeSuite = await _context.OfficeSuites
                .Include(os => os.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (officeSuite == null)
            {
                return Json(false);
            }

            officeSuite.SelectedEmployeeIds = officeSuite.Employees.Select(e => e.Id).ToList();

            OfficeSuiteModelDetails officeSuiteModelDetails = new OfficeSuiteModelDetails();
            officeSuiteModelDetails.Id = officeSuite.Id;
            officeSuiteModelDetails.UserName = officeSuite.UserName;
            officeSuiteModelDetails.Series = officeSuite.Series;
            officeSuiteModelDetails.StartDate = officeSuite.StartDate;
            officeSuiteModelDetails.FinishDate = officeSuite.FinishDate;
            officeSuiteModelDetails.SelectedEmployeeIds = officeSuite.SelectedEmployeeIds;
            officeSuiteModelDetails.Stock = officeSuite.Stock;
            officeSuiteModelDetails.Status = officeSuite.Status;

            return Json(officeSuiteModelDetails);
        }

        /// <summary>
        /// Obtiene una lista de empleados que NO estan asignados a una suite de office
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene una lista de empleados que NO estan asignados a una suite de office
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene una lista con los empleados
        /// </returns>
        /// <response code="200">Devuelve un JSON con una lista de empleados</response>
        /// <response code="500">Si ocurre un error al consultar los empleados</response>
        [HttpGet("GetEmployeesWithoutOfficeSuite")]
        public IActionResult EmployeesWithoutOfficeSuite(int id)
        {

            List<Employee> employeesWithoutOfficeSuite = _context.Employees
                .Where(e => e.OfficeSuites.Count == 0 || e.OfficeSuites.Any(os => os.Id == id))
                .ToList();
            

            return Json(employeesWithoutOfficeSuite);
        }

        /// <summary>
        /// Obtiene una lista de empleados que SI estan asignados a una suite de office
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene una lista de empleados que SI estan asignados a una suite de office
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene una lista con los empleados
        /// </returns>
        /// <response code="200">Devuelve un JSON con una lista de empleados</response>
        /// <response code="500">Si ocurre un error al consultar los empleados</response>
        [HttpGet("GetEmployeesWithOfficeSuite")]
        public IActionResult EmployeesWithOfficeSuite(int id)
        {

            List<Employee> employeesWithoutOfficeSuite = _context.Employees
                .Where(e => e.OfficeSuites.Count == 0 || e.OfficeSuites.Any(os => os.Id == id))
                .OrderBy(e => e.Name)
                .ToList();

            return Json(employeesWithoutOfficeSuite);
        }

        /// <summary>
        /// Permite crear una suite de office
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una suite de office nueva en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear la suite de office</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OfficeSuiteModel officeSuiteModel)
        {
            try
            {
                OfficeSuite officeSuite = new OfficeSuite();
                officeSuite.UserName = officeSuiteModel.UserName;
                officeSuite.Series = officeSuiteModel.Series;
                officeSuite.StartDate = officeSuiteModel.StartDate;
                officeSuite.FinishDate = officeSuiteModel.FinishDate;
                officeSuite.SelectedEmployeeIds = officeSuiteModel.SelectedEmployeeIds;
                if (officeSuite.SelectedEmployeeIds != null && officeSuite.SelectedEmployeeIds.Any())
                {
                    // Calcular el nuevo valor de Stock
                    int selectedEmployeeCount = officeSuite.SelectedEmployeeIds.Count;
                    officeSuite.Stock = selectedEmployeeCount;
                    officeSuite.Status = Status.Asignado;

                    // Cargar empleados desde la base de datos y agregarlos a OfficeSuite
                    var selectedEmployees = await _context.Employees
                        .Where(e => officeSuite.SelectedEmployeeIds.Contains(e.Id))
                        .ToListAsync();

                    officeSuite.Employees.AddRange(selectedEmployees);
                }
                else
                {
                    officeSuite.Stock = 0;
                }

                _context.Add(officeSuite);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500,"Error en la conexión con la base de datos");
            }
            
        }

        /// <summary>
        /// Obtiene una suite de office para editar
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una suite de office mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la suite de office
        /// </returns>
        /// <response code="200">Devuelve un JSON con la suite de office</response>
        /// <response code="500">Si ocurre un error al obtener la suite de office</response>
        [HttpGet("GetOfficeSuite")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OfficeSuites == null)
            {
                return Json(false);
            }

            OfficeSuite officeSuite = await _context.OfficeSuites
                .Include(os => os.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (officeSuite == null || officeSuite.IsDeleted)
            {
                return Json(false);
            }

            officeSuite.SelectedEmployeeIds = officeSuite.Employees.Select(e => e.Id).ToList();

            OfficeSuiteModel officeSuiteModel = new OfficeSuiteModel();
            officeSuiteModel.Id = officeSuite.Id;
            officeSuiteModel.UserName = officeSuite.UserName;
            officeSuiteModel.Series = officeSuite.Series;
            officeSuiteModel.StartDate = officeSuite.StartDate;
            officeSuiteModel.FinishDate = officeSuite.FinishDate;
            officeSuiteModel.SelectedEmployeeIds = officeSuite.SelectedEmployeeIds;

            return Json(officeSuiteModel);
        }

        /// <summary>
        /// Permite editar una suite de office
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar una suite de office en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar la suite de office</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(OfficeSuiteModel officeSuiteModel)
        {
            OfficeSuite officeSuite = new OfficeSuite();
            officeSuite.Id = officeSuiteModel.Id;
            officeSuite.UserName = officeSuiteModel.UserName;
            officeSuite.Series = officeSuiteModel.Series;
            officeSuite.StartDate = officeSuiteModel.StartDate;
            officeSuite.FinishDate = officeSuiteModel.FinishDate;
            officeSuite.SelectedEmployeeIds = officeSuiteModel.SelectedEmployeeIds;
            try
            {
                var existingOfficeSuite = await _context.OfficeSuites
                    .Include(os => os.Employees)
                    .FirstOrDefaultAsync(os => os.Id == officeSuite.Id);

                // Actualizar empleados y campo Stock
                await UpdateEmployeesAndStock(existingOfficeSuite, officeSuite);

                _context.Update(existingOfficeSuite);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                if (!OfficeSuiteExists(officeSuite.Id))
                {
                    return StatusCode(500,"Error en la conexión con la base de datos, Error: "+exception);
                }
                else
                {
                    throw;
                }
            }
            return Json(true);
        }

        private async Task UpdateEmployeesAndStock(OfficeSuite existingOfficeSuite, OfficeSuite updatedOfficeSuite)
        {
            // Actualizar fechas
            existingOfficeSuite.StartDate = updatedOfficeSuite.StartDate;
            existingOfficeSuite.FinishDate = updatedOfficeSuite.FinishDate;

            // Actualizar empleados
            await UpdateEmployeesInOfficeSuite(existingOfficeSuite, updatedOfficeSuite);

            // Actualizar campo Stock basado en la cantidad de usuarios seleccionados
            existingOfficeSuite.Stock = CountSelectedUsers(updatedOfficeSuite);

            // Actualizar el estado de la suite de office
            existingOfficeSuite.Status = DetermineOfficeSuiteStatus((int)existingOfficeSuite.Stock);
        }

        private Status DetermineOfficeSuiteStatus(int stock)
        {
            // Determinar el estado basado en el campo Stock
            if (stock == 0)
            {
                return Status.Disponible;
            }
            else
            {
                return Status.Asignado;
            }
        }

        private async Task UpdateEmployeesInOfficeSuite(OfficeSuite existingOfficeSuite, OfficeSuite updatedOfficeSuite)
        {
            var selectedEmployeeIds = updatedOfficeSuite.SelectedEmployeeIds ?? new List<int>();

            // Obtener empleados seleccionados desde la base de datos
            var selectedEmployees = await _context.Employees
                .Where(e => selectedEmployeeIds.Contains(e.Id))
                .ToListAsync();

            // Limpiar empleados existentes y agregar los seleccionados
            existingOfficeSuite.Employees.Clear();
            existingOfficeSuite.Employees.AddRange(selectedEmployees);
        }

        private int CountSelectedUsers(OfficeSuite officeSuite)
        {
            return officeSuite.SelectedEmployeeIds?.Count ?? 0;
        }
        private bool OfficeSuiteExists(int id)
        {
            return _context.OfficeSuites.Any(e => e.Id == id);
        }

        /// <summary>
        /// Permite eliminar una suite de office
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una suite de office de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar la suite de office</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var officeSuite = await _context.OfficeSuites
                .Include(os => os.Employees)
                .FirstOrDefaultAsync(os => os.Id == id);

            if (officeSuite == null)
            {
                return StatusCode(500,"La suite de office no existe");
            }

            // Liberar a los empleados de la Suite de Office
            ClearEmployeesInDeletedOfficeSuite(officeSuite);

            // Realizar la eliminación lógica
            officeSuite.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Json(true);
        }

        private void ClearEmployeesInDeletedOfficeSuite(OfficeSuite officeSuite)
        {
            // Liberar a los empleados de la Suite de Office que se va a eliminar
            if (officeSuite.Employees != null)
            {
                foreach (var employee in officeSuite.Employees)
                {
                    employee.OfficeSuites.Remove(officeSuite);
                }
            }
        }
    }
}

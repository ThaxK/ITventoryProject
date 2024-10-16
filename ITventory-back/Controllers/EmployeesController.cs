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

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("Employees")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
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
        public string Index()
        {
            IQueryable<Employee> employeesQuery = _context.Employees.Where(w => !w.IsDeleted);

            List<Employee> employees = employeesQuery.ToList();

            string json = JsonConvert.SerializeObject(employees);

            return json;
        }

        /// <summary>
        /// Permite saber si un documento ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un documento ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el documento en la base de datos</response>
        [HttpGet("isDocumentAvailable")]
        public IActionResult IsDocumentAvailable(string DocumentNumber, int id)
        {
            bool isAvailable = !_context.Employees.Any(e => e.DocumentNumber == DocumentNumber && e.Id != id && !e.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene todos los detalles de un empleado 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los detalles de un empleado mediante el id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el empleado con todos sus detalles
        /// </returns>
        /// <response code="200">Devuelve un JSON con el empleado y todos sus detalles</response>
        /// <response code="500">Si ocurre un error al obtener al empleado</response>
        [HttpGet("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return Json(false);
            }

            Employee employee = await _context.Employees
                .Include(e => e.OfficeSuites)
                .Include(e => e.WorkStations)
                .Include(e => e.Products)
                .FirstOrDefaultAsync(e => e.Id == id);

            
            if (employee == null)
            {
                return StatusCode(500, "Error, no se encontro un empleado con ese Id");
            }

            EmployeeModelDetails employeeModelDetails = new EmployeeModelDetails
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                DocumentNumber = employee.DocumentNumber,
                Phone = employee.Phone,
                Email = employee.Email,
                Address = employee.Address,
                Status = employee.Status,
                IsDeleted = employee.IsDeleted,
                CreateAt = employee.CreateAt,
                UpdateAt = employee.UpdateAt,
                WorkAreaId = employee.WorkAreaId
            };

            return Json(employeeModelDetails);
        }

        /// <summary>
        /// Permite crear un empleado
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo empleado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el empleado</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeModel employeeModel)
        {
            Employee employee= new Employee();
            employee.Name = employeeModel.Name;
            employee.LastName=employeeModel.LastName;
            employee.DocumentNumber = employeeModel.DocumentNumber;
            employee.Phone = employeeModel.Phone;
            employee.Email = employeeModel.Email;
            employee.Address = employeeModel.Address;
            employee.WorkAreaId = employeeModel.WorkAreaId;

            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500,"Error en la conexión con la base de datos");
            }
        }

        /// <summary>
        /// Obtiene un empleado 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un empleado mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el empleado 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el empleado</response>
        /// <response code="500">Si ocurre un error al obtener al empleado</response>
        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return StatusCode(500,"Id nulo o error en la conexión con la base de datos");
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return StatusCode(500,"No se encontro un empleado por ese Id");
            }

            return Json(employee);
        }

        /// <summary>
        /// Permite editar un empleado
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un empleado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el empleado</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EmployeeModel updatedEmployee)
        {
            Employee employee = new Employee();
            employee.Id = updatedEmployee.Id;
            employee.Name = updatedEmployee.Name;
            employee.LastName = updatedEmployee.LastName;
            employee.DocumentNumber = updatedEmployee.DocumentNumber;
            employee.Phone = updatedEmployee.Phone;
            employee.Email = updatedEmployee.Email;
            employee.Address = updatedEmployee.Address;
            employee.WorkAreaId = updatedEmployee.WorkAreaId;

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(employee.Id);

                if (existingEmployee == null)
                {
                    return StatusCode(500,"El empleado no existe");
                }

                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

                existingEmployee.UpdateTimestamp();

                await _context.SaveChangesAsync();

                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(updatedEmployee.Id))
                {
                    return StatusCode(500,"Error en la conexión con la base de datos");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        /// <summary>
        /// Permite eliminar un empleado
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un empleado de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero 1= Empleado no existente, 2= Empleado con cosas asignadas, 3= Eliminado correctamente</response>
        /// <response code="500">Si ocurre un error al eliminar el empleado</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Products)
                .Include(e => e.WorkStations)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return Json(1);
            }

            if ((employee.Products != null && employee.Products.Any()) || (employee.WorkStations != null && employee.WorkStations.Any()))
            {
                return Json(2);
            }
            else
            {
                employee.Status = Status.Disponible;
                employee.IsDeleted = true;

                await _context.SaveChangesAsync();
                return Json(3);
            }
        }
    }
}
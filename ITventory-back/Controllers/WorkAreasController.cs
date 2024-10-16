using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using Newtonsoft.Json;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("WorkArea")]
    public class WorkAreasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkAreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de areas de trabajo 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las areas de trabajo de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de areas de trabajo
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de areas de trabajo</response>
        /// <response code="500">Si ocurre un error al obtener las areas de trabajo</response>
        [HttpGet("GetWorkAreas")]
        public dynamic Index()
        {
            IQueryable<WorkArea> workAreasQuery = _context.WorkAreas.Where(w => !w.IsDeleted);


            List<WorkArea> workAreasData = workAreasQuery.ToList();

            //ViewBag.datasource = workAreasData;
            //ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            string workAreas= JsonConvert.SerializeObject(workAreasData);

            return workAreas;
        }

        /// <summary>
        /// Permite saber si un area de trabajo ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un area de trabajo ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el area de trabajo en la base de datos</response>
        [HttpPost("IsNameAvailable")]
        public dynamic IsNameAvailable(string name, int id)
        {
            bool isAvailable = !_context.WorkAreas.Any(w => w.Name == name && w.Id != id && !w.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Permite crear un area de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una area de trabajo nueva en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el area de trabajo</response>
        [HttpPost("Create")]
        public dynamic Create(WorkAreaModel workAreaModel)
        {
            WorkArea workArea = new WorkArea();
            workArea.Name = workAreaModel.Name;
            _context.Add(workArea);
            _context.SaveChangesAsync();
            return Json(true);
        }

        /// <summary>
        /// Obtiene un area de trabajo 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un area de trabajo mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el area de trabajo 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el area de trabajo</response>
        /// <response code="500">Si ocurre un error al obtener el area de trabajo</response>
        [HttpGet("GetWorkArea")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return StatusCode(500,"Id nulo");
            }

            WorkArea workArea = await _context.WorkAreas.FindAsync(id);

            if (workArea == null)
            {
                return StatusCode(500,"No existe un area de trabajo con ese Id");
            }

            return Json(workArea);
        }

        /// <summary>
        /// Permite editar un area de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un area de trabajo en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el area de trabajo</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(WorkAreaModel updatedWorkArea)
        {
            try
            {
                var existingWorkArea = await _context.WorkAreas.FindAsync(updatedWorkArea.Id);

                if (existingWorkArea == null)
                {
                    return StatusCode(500,"No existe un area de trabajo con ese Id");
                }

                _context.Entry(existingWorkArea).CurrentValues.SetValues(updatedWorkArea);

                existingWorkArea.UpdateTimestamp();

                await _context.SaveChangesAsync();

                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkAreaExists(updatedWorkArea.Id))
                {
                    return StatusCode(500,"No existe un area de trabajo con ese Id");
                }
                else
                {
                    return StatusCode(500, "Error en la conexión con la base de datos");
                }
            }
        }

        private bool WorkAreaExists(int id)
        {
            return _context.WorkAreas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Permite eliminar un area de trabajo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un area de trabajo de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero 1= Area de trabajo eliminada correctamente, 2= Area de trabajo con empleados asignados, 3= No existe un area de trabajo con ese Id</response>
        /// <response code="500">Si ocurre un error al eliminar el area de trabajo</response>
        [HttpPost("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            var workArea = _context.WorkAreas
                .Include(w => w.Employees)
                .FirstOrDefault(w => w.Id == id);

            if (workArea == null)
            {
                return Json(3);
            }

            if (workArea.Employees != null && workArea.Employees.Any())
            {
                return Json(2);
            }
            else
            {
                workArea.Status = Status.Disponible;
                workArea.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(1);
            }
        }
    }
}
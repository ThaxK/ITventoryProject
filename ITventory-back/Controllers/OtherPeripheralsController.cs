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
    [ApiController]
    [Route("OtherPeripherals")]
    public class OtherPeripheralsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtherPeripheralsController(ApplicationDbContext context)
        {
            _context = context;
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
        public string Index()
        {
            IQueryable<OtherPeripheral> otherperipheralsQuery = _context.OtherPeripherals.Where(w => !w.IsDeleted);

            List<OtherPeripheral> otherPeripheralsList = otherperipheralsQuery.ToList();

            string json= JsonConvert.SerializeObject(otherPeripheralsList);

            return json;
        }

        /// <summary>
        /// Permite saber si un nombre de periferico mecanico ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un nombre de periferico mecanico ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el periferico mecanico en la base de datos</response>
        [HttpGet("IsNameAvailable")]
        public IActionResult IsNameAvailable(string Name, int id)
        {
            bool isAvailable = !_context.OtherPeripherals.Any(w => w.Name == Name && w.Id != id && !w.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Permite crear un periferico mecanico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo periferico mecanico en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el periferico mecanico</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OtherPeripheralModel otherPeripheralModel)
        {
            OtherPeripheral otherPeripheral = new OtherPeripheral();
            otherPeripheral.Name = otherPeripheralModel.Name;
            _context.Add(otherPeripheral);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        /// <summary>
        /// Obtiene un periferico mecanico 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un periferico mecanico mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el periferico mecanico 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el periferico mecanico</response>
        /// <response code="500">Si ocurre un error al obtener al periferico mecanico</response>
        [HttpGet("GetOtherPeripheral")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OtherPeripherals == null)
            {
                return StatusCode(500,"Id nulo o error en la conexión con la base de datos");
            }

            OtherPeripheral otherPeripheral = await _context.OtherPeripherals.FindAsync(id);
            if (otherPeripheral == null || otherPeripheral.IsDeleted || ((int)otherPeripheral.Status) == 2)
            {
                return StatusCode(500,"No existe un periferico con ese Id o fue eliminado");
            }

            OtherPeripheralModel otherPeripheralModel = new OtherPeripheralModel();
            otherPeripheralModel.Id= otherPeripheral.Id;
            otherPeripheralModel.Name= otherPeripheral.Name;
            otherPeripheralModel.Status= otherPeripheral.Status;

            return Json(otherPeripheralModel);
        }

        /// <summary>
        /// Permite editar un periferico mecanico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un periferico mecanico en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el periferico mecanico</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(OtherPeripheralModel otherPeripheralModel)
        {
            OtherPeripheral otherPeripheral = new OtherPeripheral();
            otherPeripheral.Name = otherPeripheralModel.Name;
            otherPeripheral.Id = otherPeripheralModel.Id;
            otherPeripheral.Status = otherPeripheralModel.Status;

            try
            {
                _context.Update(otherPeripheral);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OtherPeripheralExists(otherPeripheral.Id))
                {
                    return StatusCode(500,"No existe un periferico mecanico por ese Id");
                }
                else
                {
                    return StatusCode(500,"Error en la conexión con la base de datos");
                }
            }
        }

        /// <summary>
        /// Permite eliminar un periferico mecanico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un periferico mecanico de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar el periferico mecanico</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            var otherperipherals = _context.OtherPeripherals.Find(id);

            if (((int)otherperipherals.Status) == 2)
            {
                return StatusCode(500,"El periferico mecanico se encuentra asignado");
            }

            if (otherperipherals != null || ((int)otherperipherals.Status) != 2)
            {
                otherperipherals.Status = (Status)1;
                otherperipherals.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500,"No se encontro un periferico con ese Id");
            }
        }

        private bool OtherPeripheralExists(int id)
        {
            return _context.OtherPeripherals.Any(e => e.Id == id);
        }
    }
}

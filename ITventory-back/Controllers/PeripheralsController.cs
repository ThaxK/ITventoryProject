using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WebApplication1.Models;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("Peripherals")]
    public class PeripheralsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeripheralsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Peripherals
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
        public string Index()
        {
            IQueryable<Peripheral> peripheralsQuery = _context.Peripherals.Where(w => !w.IsDeleted);

            List<Peripheral> peripheralsList = peripheralsQuery.ToList();


            string json = JsonConvert.SerializeObject(peripheralsList);

            return json;
        }

        /// <summary>
        /// Permite saber si un nombre de periferico ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un nombre de periferico ya esta registrado en la base de datos. Es necesario enviar el nombre y el Id del tipo de periferico
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el periferico en la base de datos</response>
        [HttpGet("IsNameAvailable")]
        public IActionResult IsNameAvailable(string series, int id, int PeripheralTypeId)
        {
            bool isAvailable = !_context.Peripherals.Any(w => w.Series == series && w.Id != id && w.PeripheralTypeId == PeripheralTypeId && !w.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene la lista de tipos de perifericos
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los tipos de perifericos de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de tipos
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de tipos de perifericos</response>
        /// <response code="500">Si ocurre un error al obtener los tipos de perifericos</response>
        [HttpGet("GetTypes")]
        public IActionResult GetTypes()
        {
            List<Subcategory> typesList = _context.Subcategories.Where(x => x.CategoryId == 1).ToList();

            return Json(typesList);
        }

        /// <summary>
        /// Obtiene la lista de marcas
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todas las marcas de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de marcas
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de marcas</response>
        /// <response code="500">Si ocurre un error al obtener las marcas</response>
        [HttpGet("GetBrands")]
        public IActionResult GetBrands()
        {

            List<Subcategory> brandsList = _context.Subcategories.Where(x => x.CategoryId == 4).ToList();

            return Json(brandsList);
        }

        /// <summary>
        /// Permite crear un periferico 
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo periferico en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el periferico</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PeripheralModel peripheralModel)
        {
            Peripheral peripheral = new Peripheral();
            peripheral.PeripheralTypeId = peripheralModel.PeripheralTypeId;
            peripheral.PeripheralBrandId = peripheralModel.PeripheralBrandId;
            peripheral.Model = peripheralModel.Model;
            peripheral.Series = peripheralModel.Series;
            peripheral.Price = peripheralModel.Price;
            peripheral.PurchaseDate = peripheralModel.PurchaseDate;

            _context.Add(peripheral);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        /// <summary>
        /// Obtiene un periferico 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un periferico mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el periferico 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el periferico</response>
        /// <response code="500">Si ocurre un error al obtener al periferico</response>
        [HttpGet("GetPeripheral")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode(500,"Id nulo");
            }

            var peripheral = await _context.Peripherals.FindAsync(id);

            if (peripheral == null || peripheral.IsDeleted || ((int)peripheral.Status) == 2)
            {
                return StatusCode(500,"No existe un periferico con ese Id");
            }
            
            return Json(peripheral);
        }

        /// <summary>
        /// Permite editar un periferico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un periferico en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el periferico</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(PeripheralModel peripheralModel)
        {
            Peripheral peripheral = new Peripheral();
            peripheral.Id = peripheralModel.Id;
            peripheral.PeripheralTypeId = peripheralModel.PeripheralTypeId;
            peripheral.PeripheralBrandId = peripheralModel.PeripheralBrandId;
            peripheral.Model = peripheralModel.Model;
            peripheral.Series = peripheralModel.Series;
            peripheral.Price = peripheralModel.Price;
            peripheral.PurchaseDate = peripheralModel.PurchaseDate;
            peripheral.Status = peripheralModel.Status;

            if (peripheral.IsDeleted)
            {
                return StatusCode(500,"El periferico fue eliminado anteriormente");
            }

            try
            {
                _context.Update(peripheral);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeripheralExists(peripheral.Id))
                {
                    return StatusCode(500,"No existe un periferico por ese Id");
                }
                else
                {
                    return StatusCode(500,"Error en la conexión con la base de datos");
                    throw;
                }
            }
        }

        /// <summary>
        /// Permite eliminar un periferico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un periferico de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar el periferico</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            var peripherals = _context.Peripherals.Find(id);

            if (((int)peripherals.Status) == 2)
            {
                return StatusCode(500,"El periferico se encuentra asignado");
            }

            if (peripherals != null)
            {
                peripherals.Status = (Status)1;
                peripherals.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500,"No existe un periferico con ese Id");
            }
        }

        private bool PeripheralExists(int id)
        {
            return _context.Peripherals.Any(e => e.Id == id);
        }
    }
}

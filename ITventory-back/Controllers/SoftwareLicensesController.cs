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
    [Route("SoftwareLicenses")]
    public class SoftwareLicensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoftwareLicensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de licencias de software 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las licencias de software de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de licencias de software
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de licencias de software</response>
        /// <response code="500">Si ocurre un error al obtener las licencias de software</response>
        [HttpGet("GetSoftwareLicenses")]
        public string Index()
        {
            IQueryable<SoftwareLicense> licensesQuery = _context.SoftwareLicenses.Where(w => !w.IsDeleted);

            List<SoftwareLicense> softwareLicensesList= licensesQuery.ToList();

            string json= JsonConvert.SerializeObject(softwareLicensesList);

            return json;
        }

        /// <summary>
        /// Permite saber si un serial ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un serial ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el serial en la base de datos</response>
        [HttpGet("IsSerialAvailable")]
        public IActionResult IsSerialAvailable(string Series, int id, int SubcategoryId)
        {
            bool isAvailable = !_context.SoftwareLicenses.Any(w => w.Series == Series && w.Id != id && w.SubcategoryId == SubcategoryId && !w.IsDeleted);
            return Json(isAvailable);
        }

        // GET: SoftwareLicenses/GetSubcatecories
        /// <summary>
        /// Obtiene una lista de subcategorias
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene una lista de subcategorias
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista con las subcategorias
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de subcategorias</response>
        /// <response code="500">Si ocurre un error al consultar las subcategorias en la base de datos</response>
        [HttpGet("GetSubcategories")]
        public IActionResult GetSubcategories()
        {
            List<Subcategory> subcategoriesList = _context.Subcategories.Where(x => x.CategoryId == 2 && x.IsDeleted == false).ToList();
            
            return Json(subcategoriesList);
        }

        // POST: SoftwareLicenses/Create
        /// <summary>
        /// Permite crear una licencia de software
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una nueva licencia de software en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear la licencia</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SoftwareLicenseModel softwareLicenseModel)
        {
            SoftwareLicense softwareLicense = new SoftwareLicense();
            softwareLicense.Name = softwareLicenseModel.Name;
            softwareLicense.Series = softwareLicenseModel.Series;
            softwareLicense.ProductKey = softwareLicenseModel.ProductKey;
            softwareLicense.StartDate = softwareLicenseModel.StartDate;
            softwareLicense.FinishDate = softwareLicenseModel.FinishDate;
            softwareLicense.SubcategoryId = softwareLicenseModel.SubcategoryId;

            _context.Add(softwareLicense);
            await _context.SaveChangesAsync();

            return Json(true);
        }

        // GET: SoftwareLicenses/Edit/5
        /// <summary>
        /// Obtiene una licencia de software 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una licencia de software mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la licencia de software 
        /// </returns>
        /// <response code="200">Devuelve un JSON con la licencia de software</response>
        /// <response code="500">Si ocurre un error al obtener la licencia</response>
        [HttpGet("GetSoftwareLicense")]
        public async Task<IActionResult> GetSoftwareLicense(int? id)
        {
            if (id == null || _context.SoftwareLicenses == null)
            {
                return Json(false);
            }

            var softwareLicense = await _context.SoftwareLicenses.FindAsync(id);
            if (softwareLicense == null || softwareLicense.IsDeleted || ((int)softwareLicense.Status) == 2)
            {
                return Json(false);
            }
            
            return Json(softwareLicense);
        }

        // POST: SoftwareLicenses/Edit/5
        /// <summary>
        /// Permite editar una licencia de software
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar una licencia de software en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar la licencia</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(SoftwareLicenseModel softwareLicenseModel)
        {
            SoftwareLicense softwareLicense = new SoftwareLicense();
            softwareLicense.Id = softwareLicenseModel.Id;
            softwareLicense.Name = softwareLicenseModel.Name;
            softwareLicense.Series = softwareLicenseModel.Series;
            softwareLicense.ProductKey = softwareLicenseModel.ProductKey;
            softwareLicense.StartDate = softwareLicenseModel.StartDate;
            softwareLicense.FinishDate = softwareLicenseModel.FinishDate;
            softwareLicense.SubcategoryId = softwareLicenseModel.SubcategoryId;

            try
            {
                _context.Update(softwareLicense);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SoftwareLicenseExists(softwareLicense.Id))
                {
                    return StatusCode(500,"No existe una licencia con ese Id");
                }
                else
                {
                    return StatusCode(500,"Error en la conexión con la base de datos");
                }
            }
        }

        /// <summary>
        /// Permite eliminar una licencia de software
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una licencia de software de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar la licencia</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            var licenses = _context.SoftwareLicenses.Find(id);

            if (((int)licenses.Status) == 2)
            {
                return StatusCode(500,"La licencia se encuentra asignada");
            }

            if (licenses != null)
            {
                licenses.Status = (Status)1;
                licenses.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500,"No existe una licencia con ese Id");
            }
        }

        private bool SoftwareLicenseExists(int id)
        {
            return _context.SoftwareLicenses.Any(e => e.Id == id);
        }
    }
}
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApplication1.Model;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("Subcategories")]
    public class SubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubcategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de subcategorias  
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las subcategorias de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de subcategorias
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de subcategorias</response>
        /// <response code="500">Si ocurre un error al obtener las subcategorias</response>
        [HttpGet("GetSubcategories")]
        public string Index()
        {
            IQueryable<Subcategory> subcategoriesQuery = _context.Subcategories.Where(w => !w.IsDeleted);

            List<Subcategory> datasource = subcategoriesQuery.ToList();

            string json = JsonConvert.SerializeObject(datasource);

            return json;
        }

        /// <summary>
        /// Permite saber si una subcategoria ya esta registrada
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si una subcategoria ya esta registrada en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar la subcategoria en la base de datos</response>
        [HttpGet("IsNameAvailable")]
        public IActionResult IsNameAvailable(string name, int id, int CategoryId)
        {
            bool isAvailable = !_context.Subcategories.Any(w => w.Name == name && w.Id != id && w.CategoryId == CategoryId && !w.IsDeleted);
            return Json(isAvailable);
        }

        /// <summary>
        /// Obtiene la lista de categorias  
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve las categorias de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de categorias
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de categorias</response>
        /// <response code="500">Si ocurre un error al obtener las categorias</response>
        [HttpGet("GetCategories")]
        public string GetCategories()
        {
            IQueryable<Category> categories = _context.Categories.Where(x => x.Id != 3 && x.Id != 2);

            List<Category> categoriesList = categories.ToList();

            string json = JsonConvert.SerializeObject(categoriesList);

            return json;
        }

        /// <summary>
        /// Obtiene la lista completa de categorias 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos las categorias de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista completa de categorias
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de categorias</response>
        /// <response code="500">Si ocurre un error al obtener las categorias</response>
        [HttpGet("GetCategoriesVisualizer")]
        public string GetCategoriesVisualizer()
        {
            IQueryable<Category> categories = _context.Categories;

            List<Category> categoriesList = categories.ToList();

            string json = JsonConvert.SerializeObject(categoriesList);

            return json;
        }

        /// <summary>
        /// Permite crear una subcategoria
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear una nueva subcategoria en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear la subcategoria</response>
        [HttpPost("Create")]
        public IActionResult Create(SubcategoryModel subcategoryModel)
        {
            Subcategory subcategory = new Subcategory();
            subcategory.Name = subcategoryModel.Name;
            subcategory.Status = subcategoryModel.Status;
            subcategory.CategoryId = subcategoryModel.CategoryId;
            _context.Add(subcategory);
            _context.SaveChangesAsync();
            return Json(true);
        }

        /// <summary>
        /// Obtiene una subcategoria
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una subcategoria mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la subcategoria 
        /// </returns>
        /// <response code="200">Devuelve un JSON con la subcategoria</response>
        /// <response code="500">Si ocurre un error al obtener la subcategoria</response>
        [HttpGet("GetSubcategory")]
        public async Task<IActionResult> Edit(int? id)
        {

            Subcategory subcategory =await _context.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return StatusCode(500,"No existe una subcategoria con ese Id");
            }
            if (subcategory.IsDeleted)
            {
                return StatusCode(500, "La subcategoria fue eliminada");
            }
            if (subcategory.Id <= 8)
            {
                return StatusCode(500, "La subcategoria no es accesible, esta protegida para cuidar el buen funcionamiento del sistema");
            }

            return Json(subcategory);
        }

        /// <summary>
        /// Permite editar una subcategoria
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar una subcategoria en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar la subcategoria</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(SubcategoryModel subcategoryModel)
        {
            Subcategory subcategory = new Subcategory();
            try
            {
                subcategory.Id = subcategoryModel.Id;
                subcategory.Name = subcategoryModel.Name;
                subcategory.CategoryId = subcategoryModel.CategoryId;
                _context.Update(subcategory);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubcategoryExists(subcategory.Id))
                {
                    return StatusCode(500,"No existe una subcategoria con ese Id");
                }
                else
                {
                    return StatusCode(500, "Error en la conexión con la base de datos");
                }
            }
        }

        /// <summary>
        /// Permite eliminar una subcategoria
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar una subcategoria de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar el empleado</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {

            var subcategory = _context.Subcategories.Find(id);

            if (((int)subcategory.Status) == 2)
            {
                return StatusCode(500,"La subcategoria se encuentra asignada");
            }

            if (subcategory != null && id > 8)
            {
                subcategory.Status = (Status)1;
                subcategory.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500,"La subcategoria no existe o esta protegida");
            }

        }

        [HttpGet("SubcategoryExists")]
        private bool SubcategoryExists(int id)
        {
            return _context.Subcategories.Any(e => e.Id == id);
        }
    }
}

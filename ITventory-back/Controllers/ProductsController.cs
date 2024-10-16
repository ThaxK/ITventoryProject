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
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        /// <summary>
        /// Obtiene la lista de productos 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los productos de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de productos
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de productos</response>
        /// <response code="500">Si ocurre un error al obtener los productos</response>
        [HttpGet("GetProducts")]
        public string Index()
        {
            IQueryable<Product> productQuery = _context.Products;

            List<Product> productsList = productQuery.ToList();

            string json= JsonConvert.SerializeObject(productsList);

            return json;
        }

        /// <summary>
        /// Permite crear un producto
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo producto en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al crear el producto</response>
        [HttpPost("Create")]
        public bool Create(ProductModel productModel)
        {
            Product product = new Product();
            product.ProductName = productModel.ProductName;
            product.UserName = productModel.UserName;
            product.StartDate = productModel.StartDate;
            product.FinishDate = productModel.FinishDate;
            product.EmployeeId = productModel.EmployeeId;

            _context.Add(product);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Obtiene un producto 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un producto mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el producto 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el producto</response>
        /// <response code="500">Si ocurre un error al obtener al producto</response>
        [HttpGet("GetProduct")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode(500,"Id nulo");
            }

            var product = _context.Products.Find(id);

            if (product == null)
            {
                return StatusCode(500,"No existe un producto por ese Id");
            }

            return Json(product);
        }

        /// <summary>
        /// Permite editar un producto
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un producto en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el producto</response>
        [HttpPost("Edit")]
        public IActionResult Edit(ProductModel productModel)
        {
            Product product = new Product();
            product.Id = productModel.Id;
            product.ProductName = productModel.ProductName;
            product.UserName = productModel.UserName;
            product.StartDate = productModel.StartDate;
            product.FinishDate = productModel.FinishDate;
            product.EmployeeId = productModel.EmployeeId;
            try
            {
                _context.Update(product);
                _context.SaveChanges();
                return Json(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return StatusCode(500,"No existe un producto por ese Id");
                }
                else
                {
                    return StatusCode(500, "Error en la conexión con la base de datos");
                    throw;
                }
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        /// <summary>
        /// Permite eliminar un producto
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un producto de la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al eliminar el producto</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                product.Status = (Status)1;
                product.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return Json(true);
        }
    }
}

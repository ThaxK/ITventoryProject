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
using Newtonsoft.Json.Linq;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, It")]
    public class ProductsController : Controller
    {

        // GET: Products
        public IActionResult Index(Status? status)
        {
            List<Product> productsList= new List<Product>();

            using(var client = new HttpClient())
            {
                string url = APIConnection.Url + "Products/GetProducts";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    productsList = JsonConvert.DeserializeObject<List<Product>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<Employee> employeesList=GetEmployees();

            for (int i = 0; i < productsList.Count; i++)
            {
                foreach (var employee in employeesList)
                {
                    if (productsList[i].EmployeeId==employee.Id)
                    {
                        productsList[i].Employee = employee;
                    }
                }
            }

            var productsData = productsList
                .Select(y => new
                {
                    Id = y.Id,
                    ProductName = y.ProductName,
                    UserName = y.UserName,
                    EmployeeId = y.Employee.FullName,
                    StartDate = y.StartDate,
                    FinishDate = y.FinishDate,
                    CreateAt = y.CreateAt,
                    UpdateAt = y.UpdateAt
                })
                .ToList();

            ViewBag.datasource = productsData;
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employeesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            return employeesList;
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            // Cargar la lista de empleados para mostrar en la vista
            List<Employee> employeesList = GetEmployees();
            ViewData["EmployeeId"] = new SelectList(employeesList, "Id", "FullName");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductName,UserName,StartDate,FinishDate,EmployeeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                ProductModel productModel = new ProductModel();
                productModel.Id = product.Id;
                productModel.ProductName=product.ProductName;
                productModel.UserName=product.UserName;
                productModel.StartDate = product.StartDate;
                productModel.FinishDate= product.FinishDate;
                productModel.EmployeeId= product.EmployeeId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url+"Products/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(productModel);

                    var httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = bool.Parse(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Producto Adicional Creado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            // Si hay un error, recargar la lista de empleados
            List<Employee> employeesList = GetEmployees();
            ViewData["EmployeeId"] = new SelectList(employeesList, "Id", "FullName", product.EmployeeId);

            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product;

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Products/GetProduct?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(content);
            }

            if (product == null)
            {
                return NotFound();
            }

            // Cargar la lista de empleados para mostrar en la vista
            List<Employee> employeesList = GetEmployees();
            ViewData["EmployeeId"] = new SelectList(employeesList, "Id", "FullName", product.EmployeeId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ProductName,UserName,StartDate,FinishDate,EmployeeId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ProductModel productModel = new ProductModel();
                productModel.Id = product.Id;
                productModel.ProductName = product.ProductName;
                productModel.UserName = product.UserName;
                productModel.StartDate = product.StartDate;
                productModel.FinishDate = product.FinishDate;
                productModel.EmployeeId = product.EmployeeId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Products/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(productModel);

                    var httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = bool.Parse(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Registro Modificado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["EmployeeId"] = new SelectList(GetEmployees(), "Id", "FullName", product.EmployeeId);

            return View(product);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            bool result;
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Products/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<bool>(content);
            }

            TempData["AlertMessage"] = "Registro Eliminado Exitosamente!";
            TempData["SweetAlertType"] = "success";

            return RedirectToAction("Index");
        }
    }
}

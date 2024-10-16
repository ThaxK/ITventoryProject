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
using Itventory.web.Models;
using WebApplication1.Models;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, It")]
    public class OfficeSuitesController : Controller
    {

        // GET: OfficeSuites
        public IActionResult Index(Status? status)
        {
            List<OfficeSuite> officeSuitesList= new List<OfficeSuite>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url+ "OfficeSuites/GetOficceSuites";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    officeSuitesList = JsonConvert.DeserializeObject<List<OfficeSuite>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            var employeesData = officeSuitesList
                .Select(y => new
                {
                    Id = y.Id,
                    UserName = y.UserName,
                    Stock = y.Stock,
                    StartDate = y.StartDate,
                    FinishDate = y.FinishDate,
                    CreateAt = y.CreateAt,
                    UpdateAt = y.UpdateAt
                })
                .ToList();

            ViewBag.datasource = employeesData;
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsUserNameAvailable(string UserName, int id)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/IsUserNameAvailable?UserName="+UserName+"&id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            
            return Json(isAvailable);
        }

        // GET: OfficeSuites/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OfficeSuiteModelDetails officeSuiteModelDetails;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/Details?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                officeSuiteModelDetails = JsonConvert.DeserializeObject<OfficeSuiteModelDetails>(content);
            }

            OfficeSuite officeSuite = new OfficeSuite();
            officeSuite.Id = officeSuiteModelDetails.Id;
            officeSuite.UserName = officeSuiteModelDetails.UserName;
            officeSuite.Series = officeSuiteModelDetails.Series;
            officeSuite.StartDate = officeSuiteModelDetails.StartDate;
            officeSuite.FinishDate = officeSuiteModelDetails.FinishDate;
            officeSuite.SelectedEmployeeIds = officeSuiteModelDetails.SelectedEmployeeIds;
            officeSuite.Status = officeSuiteModelDetails.Status;
            officeSuite.Stock = officeSuiteModelDetails.Stock;

            if (officeSuite == null)
            {
                return NotFound();
            }

            List<Employee> EmployeesList;
            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                EmployeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            for (int i = 0; i < officeSuite.SelectedEmployeeIds.Count; i++)
            {
                foreach (var employee in EmployeesList)
                {
                    if (officeSuite.SelectedEmployeeIds[i]==employee.Id)
                    {
                        officeSuite.Employees.Add(employee);
                    }
                }
            }

            return View(officeSuite);
        }

        private List<Product> GetProducts()
        {
            List<Product> productsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Products/GetProducts";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                productsList = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            
            return productsList;
        }

        private List<Employee> GetEmployeesWithoutOfficeSuite()
        {
            List<Employee> employeesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetEmployeesWithoutOfficeSuite";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            return employeesList;
        }

        // GET: OfficeSuites/Create
        public IActionResult Create()
        {
            ViewBag.Products = GetProducts();

            List<Employee> employeesWithoutOfficeSuite = GetEmployeesWithoutOfficeSuite();

            ViewBag.Employees = employeesWithoutOfficeSuite;

            var officeSuite = new OfficeSuite();
            return View(officeSuite);
        }

        // POST: OfficeSuites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Series,StartDate,FinishDate,SelectedProductIds,SelectedEmployeeIds")] OfficeSuite officeSuite)
        {
            if (ModelState.IsValid)
            {
                OfficeSuiteModel officeSuiteModel = new OfficeSuiteModel();
                officeSuiteModel.Id=officeSuite.Id;
                officeSuiteModel.UserName=officeSuite.UserName;
                officeSuiteModel.Series=officeSuite.Series;
                officeSuiteModel.StartDate=officeSuite.StartDate;
                officeSuiteModel.FinishDate=officeSuite.FinishDate;
                officeSuiteModel.SelectedEmployeeIds= new List<int>();
                if (officeSuite.SelectedEmployeeIds != null)
                {
                    officeSuiteModel.SelectedEmployeeIds = officeSuite.SelectedEmployeeIds;
                }

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "OfficeSuites/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(officeSuiteModel);

                    var httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = bool.Parse(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Suite de Office Creada Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            List<Employee> EmployeesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                EmployeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            ViewData["Employees"] = new SelectList(EmployeesList, "Id", "Name", officeSuite.SelectedEmployeeIds);
            return View(officeSuite);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OfficeSuiteModel officeSuiteModel;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetOfficeSuite?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                officeSuiteModel = JsonConvert.DeserializeObject<OfficeSuiteModel>(content);
            }

            OfficeSuite officeSuite = new OfficeSuite();
            officeSuite.Id = officeSuiteModel.Id;
            officeSuite.UserName = officeSuiteModel.UserName;
            officeSuite.Series = officeSuiteModel.Series;
            officeSuite.StartDate = officeSuiteModel.StartDate;
            officeSuite.FinishDate = officeSuiteModel.FinishDate;
            officeSuite.SelectedEmployeeIds = officeSuiteModel.SelectedEmployeeIds;

            if (officeSuite == null || officeSuite.IsDeleted)
            {
                return RedirectToAction("Index");
            }

            // Obtener la lista de empleados sin asignar y aquellos asignados a la suite de office

            List<Employee> employeesWithoutOfficeSuite;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetEmployeesWithOfficeSuite?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesWithoutOfficeSuite = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            for (int i = 0; i < officeSuite.SelectedEmployeeIds.Count; i++)
            {
                foreach (var employee in employeesWithoutOfficeSuite)
                {
                    if (officeSuite.SelectedEmployeeIds[i] == employee.Id)
                    {
                        employee.OfficeSuites = new List<OfficeSuite>();
                        employee.OfficeSuites.Add(officeSuite);
                    }
                }
            }

            var employeesList = employeesWithoutOfficeSuite
                .OrderBy(e => e.Name)
                .Select(y => new
                {
                    y.Id,
                    Name = $"{y.Name} {y.LastName} {y.DocumentNumber} {(y.OfficeSuites.Count > 0 ? ": Asignado" : ": Disponible")}"
                });



            ViewData["AssignedEmployeeIds"] = officeSuite.SelectedEmployeeIds;

            ViewData["Employees"] = new SelectList(employeesList, "Id", "Name");

            return View(officeSuite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Series,StartDate,FinishDate,SelectedProductIds,SelectedEmployeeIds")] OfficeSuite officeSuite)
        {
            if (id != officeSuite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                OfficeSuiteModel officeSuiteModel = new OfficeSuiteModel();
                officeSuiteModel.Id = officeSuite.Id;
                officeSuiteModel.UserName = officeSuite.UserName;
                officeSuiteModel.Series = officeSuite.Series;
                officeSuiteModel.StartDate = officeSuite.StartDate;
                officeSuiteModel.FinishDate = officeSuite.FinishDate;
                officeSuiteModel.SelectedEmployeeIds = new List<int>();
                if (officeSuite.SelectedEmployeeIds!=null)
                {
                    officeSuiteModel.SelectedEmployeeIds = officeSuite.SelectedEmployeeIds;
                }
                

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "OfficeSuites/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(officeSuiteModel);

                    var httpConten = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = bool.Parse(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Suite de Office Actualizada Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            List<Employee> employeesWithoutOfficeSuite;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetEmployeesWithOfficeSuite?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesWithoutOfficeSuite = JsonConvert.DeserializeObject<List<Employee>>(content);
            }

            ViewData["Employees"] = new SelectList(employeesWithoutOfficeSuite.Where(e => e.OfficeSuites.Count == 0 || e.OfficeSuites.Any(os => os.Id == id)).OrderBy(e => e.Name).Select(y => new { y.Id, Name = y.Name + " " + y.LastName }), "Id", "Name", officeSuite.SelectedEmployeeIds);

            return View(officeSuite);
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);

                if (result==true)
                {
                    TempData["AlertMessage"] = "Suite de Office Eliminada Exitosamente, Todos los empleados asociados ahora estan disponibles!";
                    TempData["SweetAlertType"] = "success";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

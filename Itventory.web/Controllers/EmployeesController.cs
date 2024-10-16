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
using Itventory.web.Models;
using WebApplication1.Models;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, Recursos Humanos")]
    public class EmployeesController : Controller
    {

        // GET: Employees
        public IActionResult Index(Status? status)
        {
            List<Employee> EmployeesList;

            using (var client = new HttpClient()) {

                string url= APIConnection.Url+ "Employees/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    EmployeesList = JsonConvert.DeserializeObject<List<Employee>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }


            List<WorkArea> WorkAreaList= GetWorkAreas();

            for (int i = 0; i < EmployeesList.Count; i++)
            {
                foreach (var workArea in WorkAreaList)
                {
                    if (EmployeesList[i].WorkAreaId==workArea.Id)
                    {
                        EmployeesList[i].WorkArea= workArea;
                    }
                }
            }

            var employeesData = EmployeesList
                .Select(y => new
                {
                    Id = y.Id,
                    Name = y.Name,
                    LastName = y.LastName,
                    WorkAreaId = y.WorkArea.Name,
                    CreateAt = y.CreateAt,
                    UpdateAt = y.UpdateAt
                })
                .ToList();

            ViewBag.datasource = employeesData;
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsDocumentAvailable(string DocumentNumber, int id)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Employees/isDocumentAvailable?DocumentNumber="+DocumentNumber+"&id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        private List<WorkArea> GetWorkAreas()
        {
            List<WorkArea> workAreasList;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkArea/GetWorkAreas";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                workAreasList = JsonConvert.DeserializeObject<List<WorkArea>>(content);
            }
            return workAreasList;
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EmployeeModelDetails employeeModelDetails;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Employees/Details?id="+ id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {
                    employeeModelDetails = JsonConvert.DeserializeObject<EmployeeModelDetails>(content);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }

            if (employeeModelDetails == null)
            {
                return NotFound();
            }

            Employee employee = new Employee
            {
                Id = employeeModelDetails.Id,
                Name = employeeModelDetails.Name,
                LastName = employeeModelDetails.LastName,
                DocumentNumber = employeeModelDetails.DocumentNumber,
                Phone = employeeModelDetails.Phone,
                Email = employeeModelDetails.Email,
                Address = employeeModelDetails.Address,
                Status = employeeModelDetails.Status,
                IsDeleted = employeeModelDetails.IsDeleted,
                CreateAt = employeeModelDetails.CreateAt,
                UpdateAt = employeeModelDetails.UpdateAt,
                WorkAreaId = employeeModelDetails.WorkAreaId
            };

            List<OfficeSuite> officeSuitesList = new List<OfficeSuite>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OfficeSuites/GetOficceSuites";

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

            employee.OfficeSuites = new List<OfficeSuite>();
            foreach (var officeSuite in officeSuitesList)
            {
                OfficeSuiteModelDetails officeSuiteModelDetails= new OfficeSuiteModelDetails();

                using (var client = new HttpClient())
                {
                    string url = APIConnection.Url + "OfficeSuites/Details?id=" + officeSuite.Id;

                    client.DefaultRequestHeaders.Clear();

                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        officeSuiteModelDetails = JsonConvert.DeserializeObject<OfficeSuiteModelDetails>(content);
                    }
                    catch (Exception exception)
                    {

                    }
                    
                }

                officeSuite.SelectedEmployeeIds = officeSuiteModelDetails.SelectedEmployeeIds;

                for (int i = 0; i < officeSuite.SelectedEmployeeIds.Count; i++)
                {
                    
                    if (officeSuite.SelectedEmployeeIds[i] == employee.Id)
                    {
                        employee.OfficeSuites.Add(officeSuite);
                    }
                }
            }

            List<WorkStation> workStationsList = new List<WorkStation>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkStations/GetWorkStations";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    workStationsList = JsonConvert.DeserializeObject<List<WorkStation>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }

            }

            employee.WorkStations = new List<WorkStation>();
            foreach (var workStation in workStationsList)
            {
                if (employee.Id==workStation.EmployeeId)
                {
                    employee.WorkStations.Add(workStation);
                }
            }

            List<Product> productsList = new List<Product>();

            using (var client = new HttpClient())
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
            employee.Products = new List<Product>();
            foreach (var product in productsList)
            {
                if (employee.Id==product.EmployeeId)
                {
                    employee.Products.Add(product);
                }
            }

            List<WorkArea> workAreas = GetWorkAreas();
            ViewData["WorkAreaId"] = new SelectList(workAreas, "Id", "Name");

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            List<WorkArea> workAreas = GetWorkAreas();
            ViewData["WorkAreaId"] = new SelectList(workAreas, "Id", "Name");

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,DocumentNumber,Phone,Email,Address,WorkAreaId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                EmployeeModel employeeModel = new EmployeeModel();
                employeeModel.Id = employee.Id;
                employeeModel.Name = employee.Name;
                employeeModel.LastName = employee.LastName;
                employeeModel.DocumentNumber = employee.DocumentNumber;
                employeeModel.Phone = employee.Phone;
                employeeModel.Email = employee.Email;
                employeeModel.Address = employee.Address;
                employeeModel.WorkAreaId = employee.WorkAreaId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Employees/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(employeeModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Empleado creado exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Error al crear el empleado";
                        TempData["SweetAlertType"] = "error";
                    }
                }

            }

            List<WorkArea> workAreas = GetWorkAreas();
            ViewData["WorkAreaId"] = new SelectList(workAreas, "Id", "Name");

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Employees/GetEmployee?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {
                    employee = JsonConvert.DeserializeObject<Employee>(content);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }

            if (employee == null)
            {
                return NotFound();
            }

            List<WorkArea> workAreas = GetWorkAreas();
            ViewData["WorkAreaId"] = new SelectList(workAreas, "Id", "Name");

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,DocumentNumber,Phone,Email,Address,Status,WorkAreaId")] Employee updatedEmployee)
        {
            if (id != updatedEmployee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                EmployeeModel employeeModel = new EmployeeModel();
                employeeModel.Id = updatedEmployee.Id;
                employeeModel.Name = updatedEmployee.Name;
                employeeModel.LastName = updatedEmployee.LastName;
                employeeModel.DocumentNumber = updatedEmployee.DocumentNumber;
                employeeModel.Phone = updatedEmployee.Phone;
                employeeModel.Email = updatedEmployee.Email;
                employeeModel.Address = updatedEmployee.Address;
                employeeModel.WorkAreaId = updatedEmployee.WorkAreaId;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Employees/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(employeeModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result==true)
                    {
                        TempData["AlertMessage"] = "Empleado actualizado exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Error al actualizar el empleado";
                        TempData["SweetAlertType"] = "error";
                    }
                }
            }
            return View(updatedEmployee);
        } 

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Employees/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {
                    int result = JsonConvert.DeserializeObject<int>(content);
                    switch (result)
                    {
                        case 2:
                            TempData["AlertMessage"] = "No se puede eliminar el Empleado porque tiene productos de office o estaciones de trabajo asociadas.";
                            TempData["SweetAlertType"] = "error";
                            break;
                        case 3:
                            TempData["AlertMessage"] = "Empleado Eliminado Exitosamente!";
                            TempData["SweetAlertType"] = "success";
                            break;
                        default: return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
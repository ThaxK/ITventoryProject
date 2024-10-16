using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    public class ActsController : Controller
    {


        // GET: Acts
        public async Task<IActionResult> Index()
        {
            List<Act> actsList= new List<Act>();

            using(var client = new HttpClient())
            {
                string url = APIConnection.Url + "Acts/GetActs";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    actsList = JsonConvert.DeserializeObject<List<Act>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<WorkStation> workStationsList = GetWorkStations();

            
            for (int i = 0; i < actsList.Count; i++)
            {
                foreach (var workStation in workStationsList)
                {
                    if (actsList[i].WorkStationId==workStation.Id)
                    {
                        actsList[i].WorkStation = workStation;
                    }
                }
            }

            ViewBag.datasource = actsList
              .Select(y => new
              {
                  Id = y.Id,
                  Name = y.Name,
                  Url = y.URL,
                  Employee = y.WorkStation.Employee.Name + " " + y.WorkStation.Employee.LastName + " : " + y.WorkStation.Employee.DocumentNumber,
                  WorkStation = y.WorkStationId,

                  Status = y.Status.ToString()

              })
                .ToList();

            return View();
        }

        private List<WorkStation> GetWorkStations()
        {
            List<WorkStation> workStationsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkStations/GetWorkStations";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                workStationsList = JsonConvert.DeserializeObject<List<WorkStation>>(content);
            }

            List<Employee> employeesList;

            using (var client = new HttpClient())
            {

                string url = APIConnection.Url + "WorkStations/GetEmployees";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                employeesList = JsonConvert.DeserializeObject<List<Employee>>(content);

            }

            for (int i = 0; i < workStationsList.Count; i++)
            {
                foreach (var employee in employeesList)
                {
                    if (workStationsList[i].EmployeeId==employee.Id)
                    {
                        workStationsList[i].Employee = employee;
                    }
                }
            }

            return workStationsList;
        }

        // GET: Acts/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewData["WorkStationId"] = new SelectList(GetWorkStations().Where(e => e.Status != (Status)1).OrderBy(e => e.Employee.Name).Select(y => new { y.Id, EmployeeId = "No. " + y.Id + " : " + y.Employee.Name + " " + y.Employee.LastName }), "Id", "EmployeeId");
            return View();
        }

        // POST: Acts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,URL,WorkStationId,File")] Act act)
        {
            if (ModelState.IsValid)
            {
                ActModel actModel = new ActModel();
                actModel.Id = act.Id;
                actModel.Name = act.Name;
                actModel.WorkStationId = act.WorkStationId;
                actModel.File = act.File;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Acts/Create";

                    using (var form = new MultipartFormDataContent())
                    {
                        form.Add(new StringContent(actModel.Id.ToString()), "Id");
                        form.Add(new StringContent(actModel.Name), "Name");
                        form.Add(new StringContent(actModel.WorkStationId.ToString()), "WorkStationId");

                        if (actModel.File != null)
                        {
                            var stream = actModel.File.OpenReadStream();
                            var streamContent = new StreamContent(stream);
                            streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "File",
                                FileName = actModel.File.FileName
                            };
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(actModel.File.ContentType);
                            form.Add(streamContent);
                        }

                        var response = await client.PostAsync(uri, form);
                        var content = await response.Content.ReadAsStringAsync();
                        bool result = JsonConvert.DeserializeObject<bool>(content);
                        if (result)
                        {
                            TempData["AlertMessage"] = "Acta Creada Exitosamente!";
                            TempData["SweetAlertType"] = "success";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Error al subir el archivo PDF.";
                        }
                    }
                }
            }
            ViewData["WorkStationId"] = new SelectList(GetWorkStations().Where(e => e.Status == (Status)3).OrderBy(e => e.Employee.Name).Select(y => new { y.Id, EmployeeId = y.Id + ": " + y.Employee.Name + " " + y.Employee.LastName }), "Id", "EmployeeId", act.WorkStationId);
            return View(act);
        }

        // GET: Acts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "Acta No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            ActModel actModel = new ActModel();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Acts/GetAct?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                actModel = JsonConvert.DeserializeObject<ActModel>(content);
            }

            Act act = new Act();
            act.Id = actModel.Id;
            act.Name = actModel.Name;
            act.WorkStationId = actModel.WorkStationId;
            act.File = actModel.File;

            if (act == null)
            {
                TempData["AlertMessage"] = "Acta No Disponible!";
                TempData["SweetAlertType"] = "warning";
                return RedirectToAction(nameof(Index));
            }

            List<WorkStation> workStationsList = GetWorkStations();

            foreach (var workStation in workStationsList)
            {
                if (act.WorkStationId == workStation.Id)
                {
                    act.WorkStation = workStation;
                }
            }

            ViewData["WorkStationId"] = new SelectList(GetWorkStations().Where(e => e.EmployeeId == act.WorkStation.EmployeeId).OrderBy(e => e.Employee.Name).Select(y => new { y.Id, EmployeeId = y.Id + ": " + y.Employee.Name + " " + y.Employee.LastName }), "Id", "EmployeeId", act.WorkStationId);
            return View(act);
        }

        // POST: Acts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,URL,WorkStationId,File")] Act act)
        {
            if (Id != act.Id)
            {
                TempData["AlertMessage"] = "Acta No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {

                ActModel actModel = new ActModel();
                actModel.Id = act.Id;
                actModel.Name = act.Name;
                actModel.WorkStationId = act.WorkStationId;
                actModel.File = act.File;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Acts/Edit";

                    client.DefaultRequestHeaders.Clear();

                    using (var form = new MultipartFormDataContent())
                    {
                        form.Add(new StringContent(actModel.Id.ToString()), "Id");
                        form.Add(new StringContent(actModel.Name), "Name");
                        form.Add(new StringContent(actModel.WorkStationId.ToString()), "WorkStationId");

                        if (actModel.File != null)
                        {
                            var stream = actModel.File.OpenReadStream();
                            var streamContent = new StreamContent(stream);
                            streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "File",
                                FileName = actModel.File.FileName
                            };
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(actModel.File.ContentType);
                            form.Add(streamContent);
                        }

                        var response = await client.PostAsync(uri, form);
                        var content = await response.Content.ReadAsStringAsync();
                        int result = JsonConvert.DeserializeObject<int>(content);
                        switch (result)
                        {
                            case 1:
                                TempData["AlertMessage"] = "Acta Editada Correctamente!";
                                TempData["SweetAlertType"] = "success";
                                return RedirectToAction(nameof(Index));
                            case 2:
                                TempData["AlertMessage"] = "Error al subir el archivo PDF.";
                                break;
                            case 3:
                                TempData["AlertMessage"] = "Acta No Encontrada!";
                                TempData["SweetAlertType"] = "error";
                                break;
                        }
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkStationId"] = new SelectList(GetWorkStations().Where(e => e.Status == (Status)3).OrderBy(e => e.Employee.Name).Select(y => new { y.Id, EmployeeId = y.Id + ": " + y.Employee.Name + " " + y.Employee.LastName }), "Id", "EmployeeId", act.WorkStationId);
            return View(act);
        }

        public IActionResult Download(int id)
        {
            ActModelDownload actModelDownload = new ActModelDownload();

            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Acts/GetActDownload?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                actModelDownload = JsonConvert.DeserializeObject<ActModelDownload>(content);
            }

            Act act = new Act();
            act.Id = actModelDownload.Id;
            act.Name = actModelDownload.Name;
            act.URL = actModelDownload.URL;
            act.WorkStationId = actModelDownload.WorkStationId;
            act.File = actModelDownload.File;


            if (act == null)
            {
                TempData["AlertMessage"] = "Acta No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var url = act.URL;
            string pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs", url);

            if (!System.IO.File.Exists(pdfFilePath))
            {
                TempData["AlertMessage"] = "Acta No Encontrada!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            string pdfFileName = Path.GetFileName(pdfFilePath);
            var Ruta = System.IO.File.OpenRead(pdfFilePath);


            using (MemoryStream stream = new MemoryStream())
            {
                using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(stream);
                }

                byte[] pdfBytes = stream.ToArray();

                return File(pdfBytes, "application/pdf", pdfFileName);

            }
        }

        public async Task<IActionResult> DeleteAct(int id)
        {


            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Acts/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);
                TempData["AlertMessage"] = "Acta Eliminada Correctamente!";
                TempData["SweetAlertType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}

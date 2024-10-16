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
using WebApplication1.Models;
using System.Security.Policy;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Recursos Humanos")]
    public class WorkAreasController : Controller
    {

        [Authorize(Roles = "Administrador, Talento Humano")]
        public IActionResult Index(Status? status)
        {
            List<WorkArea> workAreasList= new List<WorkArea>();

            var content = APIConnection.GetRequest("WorkArea/GetWorkAreas");

            try
            {
                workAreasList = JsonConvert.DeserializeObject<List<WorkArea>>(content);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "Error En La Conexión";
                TempData["SweetAlertType"] = "error";
                return View();
            }

            var workAreasData = workAreasList
                .Select(y => new
                {
                    Id = y.Id,
                    Name = y.Name,
                    CreateAt = y.CreateAt,
                    UpdateAt = y.UpdateAt
                })
                .ToList();

            ViewBag.datasource = workAreasData;
            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(string name, int id)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkArea/IsNameAvailable?name=" + name + "&id=" + id;

                client.DefaultRequestHeaders.Clear();

                var httpConten = new StringContent("", System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, httpConten).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        [Authorize(Roles = "Administrador, Talento Humano")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] WorkArea workArea)
        {
            if (ModelState.IsValid)
            {
                APIConnection.GetRequest("WorkArea/Create");

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "WorkArea/Create";

                    client.DefaultRequestHeaders.Clear();

                    WorkAreaModel workAreaModel= new WorkAreaModel();
                    workAreaModel.Name = workArea.Name;
                    workAreaModel.Id=workArea.Id;

                    string json = JsonConvert.SerializeObject(workAreaModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Area de Trabajo Creada Exitosamente, Ve a la parte detalles para descargar el Acta Inicial.";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
                
            }
            return View(workArea);
        }

        [Authorize(Roles = "Administrador, Talento Humano")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkArea workArea;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "WorkArea/GetWorkArea?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {
                    workArea = JsonConvert.DeserializeObject<WorkArea>(content);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }

            }
            if (workArea == null)
            {
                return NotFound();
            }
            return View(workArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] WorkArea updatedWorkArea)
        {
            if (id != updatedWorkArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        string uri = APIConnection.Url + "WorkArea/Edit";

                        client.DefaultRequestHeaders.Clear();


                        WorkAreaModel workAreaModel = new WorkAreaModel();
                        workAreaModel.Id = updatedWorkArea.Id;
                        workAreaModel.Name = updatedWorkArea.Name;
                        workAreaModel.Status = updatedWorkArea.Status;

                        string json = JsonConvert.SerializeObject(workAreaModel);

                        var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                        var response = client.PostAsync(uri, httpConten).Result;
                        var content = response.Content.ReadAsStringAsync().Result;
                        bool result = JsonConvert.DeserializeObject<bool>(content);
                        if (result == true)
                        {
                            TempData["AlertMessage"] = "Area de Trabajo Actualizada Exitosamente, Ve a la parte detalles para descargar el Acta de Actualizacion.!";
                            TempData["SweetAlertType"] = "success";
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedWorkArea);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "WorkArea/Delete?id="+id;

                client.DefaultRequestHeaders.Clear();

                var httpConten = new StringContent("", System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, httpConten).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                int result = JsonConvert.DeserializeObject<int>(content);
                switch (result)
                {
                    case 1:
                        TempData["AlertMessage"] = "Area de Trabajo Eliminada Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        break;
                    case 2:
                        TempData["AlertMessage"] = "No se puede eliminar el area de trabajo porque tiene empleados asignados.";
                        TempData["SweetAlertType"] = "error";
                        break;
                    default: return NotFound();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
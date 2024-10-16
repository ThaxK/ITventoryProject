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

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, Recursos Humanos")]
    public class OtherPeripheralsController : Controller
    {

        public IActionResult Index(Status? status)
        {
            List<OtherPeripheral> otherPeripheralsList= new List<OtherPeripheral>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OtherPeripherals/GetOtherPeripherals";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    otherPeripheralsList = JsonConvert.DeserializeObject<List<OtherPeripheral>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
            }

            ViewBag.datasource = otherPeripheralsList
              .Select(y => new
              {
                  Id = y.Id,
                  Name = y.Name,
                  CreateAt = y.CreateAt,
                  UpdateAt = y.UpdateAt,
              })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(string Name, int id)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OtherPeripherals/IsNameAvailable?Name="+Name+"&id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] OtherPeripheral otherPeripheral)
        {
            if (ModelState.IsValid)
            {
                OtherPeripheralModel otherPeripheralModel= new OtherPeripheralModel();
                otherPeripheralModel.Id = otherPeripheral.Id;
                otherPeripheralModel.Name = otherPeripheral.Name;
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "OtherPeripherals/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(otherPeripheralModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Periferico Creado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(otherPeripheral);
            
        }


        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Periferico!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            OtherPeripheralModel otherPeripheralModel;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OtherPeripherals/GetOtherPeripheral?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                otherPeripheralModel = JsonConvert.DeserializeObject<OtherPeripheralModel>(content);
            }

            OtherPeripheral otherPeripheral = new OtherPeripheral();
            otherPeripheral.Id = otherPeripheralModel.Id;
            otherPeripheral.Name = otherPeripheralModel.Name;
            otherPeripheral.Status = otherPeripheralModel.Status;

            if (otherPeripheral == null || otherPeripheral.IsDeleted || ((int)otherPeripheral.Status) == 2)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Periferico!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            return View(otherPeripheral);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] OtherPeripheral otherPeripheral)
        {
            if (id != otherPeripheral.Id || otherPeripheral.IsDeleted)
            {
                TempData["AlertMessage"] = "No se Encontro este Periferico!";
                TempData["SweetAlertType"] = "error";
            }

            if (ModelState.IsValid)
            {
                
                    OtherPeripheralModel otherPeripheralModel = new OtherPeripheralModel();
                    otherPeripheralModel.Id = otherPeripheral.Id;
                    otherPeripheralModel.Name = otherPeripheral.Name;
                    otherPeripheralModel.Status = otherPeripheral.Status;
                    using (var client = new HttpClient())
                    {
                        string uri = APIConnection.Url + "OtherPeripherals/Edit";

                        client.DefaultRequestHeaders.Clear();

                        string json = JsonConvert.SerializeObject(otherPeripheralModel);

                        var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                        var response = client.PostAsync(uri, httpConten).Result;
                        var content = response.Content.ReadAsStringAsync().Result;
                        bool result = JsonConvert.DeserializeObject<bool>(content);
                        if (result == true)
                        {
                            TempData["AlertMessage"] = "Periferico Editado Exitosamente!";
                            TempData["SweetAlertType"] = "success";
                        }
                        else
                        {
                            TempData["AlertMessage"] = "No se logro editar este Periferico!";
                            TempData["SweetAlertType"] = "error";
                        }
                        return RedirectToAction(nameof(Index));
                    }
            }
            return View(otherPeripheral);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "OtherPeripherals/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);

                if (result == true)
                {
                    TempData["AlertMessage"] = "Periferico Eliminado Exitosamente!";
                    TempData["SweetAlertType"] = "success";
                }
                else
                {
                    TempData["AlertMessage"] = "No se puede Eliminar este Periferico!";
                    TempData["SweetAlertType"] = "error";
                    
                }
                return RedirectToAction("Index");
            }

        }
    }
}

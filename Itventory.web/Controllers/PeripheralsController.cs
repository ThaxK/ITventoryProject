using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using Syncfusion.EJ2.Linq;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Itventory.web.Models;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, It")]
    public class PeripheralsController : Controller
    {

        // GET: Peripherals
        public IActionResult Index(Status? status)
        {
            List<Peripheral> peripheralsList= new List<Peripheral>();

            using(var client = new HttpClient())
            {
                string url = APIConnection.Url+ "Peripherals/GetPeripherals";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    peripheralsList = JsonConvert.DeserializeObject<List<Peripheral>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<Subcategory> typesList = GetTypes();
            List<Subcategory> brandsList = GetBrands();

            for (int i = 0; i < peripheralsList.Count; i++)
            {
                foreach (var brand in brandsList)
                {
                    if (brand.Id == peripheralsList[i].PeripheralBrandId)
                    {
                        peripheralsList[i].PeripheralBrand = brand;
                    }
                }
                foreach (var type in typesList)
                {
                    if (type.Id == peripheralsList[i].PeripheralTypeId)
                    {
                        peripheralsList[i].PeripheralType = type;
                    }
                }
            }

            ViewBag.datasource = peripheralsList
              .Select(y => new
              {
                  Id = y.Id,
                  Type = y.PeripheralType,
                  Brand = y.PeripheralBrand,
                  Model = y.Model,
                  Series = y.Series,
                  Price = y.Price,
                  PurchaseDate = y.PurchaseDate
              })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        private List<Subcategory> GetTypes()
        {
            List<Subcategory> typesList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetTypes";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                typesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }

            return typesList;
        }

        private List<Subcategory> GetBrands()
        {
            List<Subcategory> brandsList;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetBrands";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                brandsList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }

            return brandsList;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(string series, int id, int PeripheralTypeId)
        {
            bool isAvailable ;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/IsNameAvailable?series="+series+"&id="+id+ "&PeripheralTypeId="+PeripheralTypeId;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        // GET: Peripherals/Create
        public IActionResult Create()
        {
            // Obtener datos necesarios para la vista, por ejemplo, las categorías y marcas
            ViewData["PeripheralTypeId"] = new SelectList(GetTypes(), "Id", "Name");
            ViewData["PeripheralBrandId"] = new SelectList(GetBrands(), "Id", "Name");

            return View();
        }

        // POST: Peripherals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PeripheralTypeId,PeripheralBrandId,Model,Series,Price,PurchaseDate")] Peripheral peripheral)
        {
            if (ModelState.IsValid)
            {
                PeripheralModel peripheralModel = new PeripheralModel();
                peripheralModel.Id = peripheral.Id;
                peripheralModel.PeripheralTypeId = peripheral.PeripheralTypeId;
                peripheralModel.PeripheralBrandId = peripheral.PeripheralBrandId;
                peripheralModel.Model = peripheral.Model;
                peripheralModel.Series = peripheral.Series;
                peripheralModel.Price = peripheral.Price;
                peripheralModel.PurchaseDate = peripheral.PurchaseDate;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Peripherals/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(peripheralModel);

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
            ViewData["PeripheralBrandId"] = new SelectList(GetBrands(), "Id", "Name", peripheral.PeripheralBrandId);
            ViewData["PeripheralTypeId"] = new SelectList(GetTypes(), "Id", "Name", peripheral.PeripheralTypeId);
            return View(peripheral);
        }

        // GET: Peripherals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Periferico!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            Peripheral peripheral;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/GetPeripheral?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                peripheral = JsonConvert.DeserializeObject<Peripheral>(content);
            }

            if (peripheral == null || peripheral.IsDeleted || ((int)peripheral.Status) == 2)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Periferico!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            ViewData["PeripheralBrandId"] = new SelectList(GetBrands(), "Id", "Name", peripheral.PeripheralBrandId);
            ViewData["PeripheralTypeId"] = new SelectList(GetTypes(), "Id", "Name", peripheral.PeripheralTypeId);
            return View(peripheral);
        }

        // POST: Peripherals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Series,Status,PeripheralTypeId,PeripheralBrandId,Price,PurchaseDate")] Peripheral peripheral)
        {
            if (id != peripheral.Id || peripheral.IsDeleted)
            {
                TempData["AlertMessage"] = "No se Encontro este Periferico!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PeripheralModel peripheralModel = new PeripheralModel();
                peripheralModel.Id = peripheral.Id;
                peripheralModel.PeripheralTypeId = peripheral.PeripheralTypeId;
                peripheralModel.PeripheralBrandId = peripheral.PeripheralBrandId;
                peripheralModel.Model = peripheral.Model;
                peripheralModel.Series = peripheral.Series;
                peripheralModel.Price = peripheral.Price;
                peripheralModel.PurchaseDate = peripheral.PurchaseDate;
                peripheralModel.Status = peripheral.Status;

                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Peripherals/Edit";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(peripheralModel);

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
                        TempData["AlertMessage"] = "No se Logro Editar  este Periferico!";
                        TempData["SweetAlertType"] = "error";
                    }
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["PeripheralBrandId"] = new SelectList(GetBrands(), "Id", "Name", peripheral.PeripheralBrandId);
            ViewData["PeripheralTypeId"] = new SelectList(GetTypes(), "Id", "Name", peripheral.PeripheralTypeId);
            return View(peripheral);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Peripherals/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);
                if (result==true)
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

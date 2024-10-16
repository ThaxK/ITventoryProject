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
    [Authorize(Roles = "Administrador, Almacen, It")]
    public class SoftwareLicensesController : Controller
    {

        public IActionResult Index(Status? status)
        {
            List<SoftwareLicense> softwareLicensesList= new List<SoftwareLicense>();

            using(var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/GetSoftwareLicenses";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    softwareLicensesList = JsonConvert.DeserializeObject<List<SoftwareLicense>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            List<Subcategory> subcategoriesList = GetSubcategories();
            for (int i = 0; i < softwareLicensesList.Count; i++)
            {
                foreach (var subcategory in subcategoriesList)
                {
                    if (subcategory.Id == softwareLicensesList[i].SubcategoryId)
                    {
                        softwareLicensesList[i].Subcategory = subcategory;
                    }
                }
            }

            ViewBag.datasource = softwareLicensesList
             .Select(y => new
             {
                 Id = y.Id,
                 Subcategory = y.Subcategory,
                 Name = y.Name,
                 Series = y.Series,
                 ProductKey = y.ProductKey,
                 StartDate = y.StartDate,
                 FinishDate = y.FinishDate
             }).ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsSerialAvailable(string Series, int id, int SubcategoryId)
        {
            bool isAvailable ;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/IsSerialAvailable?Series="+Series+"&id="+id+"&SubcategoryId="+SubcategoryId;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }
            return Json(isAvailable);
        }

        // GET: SoftwareLicenses/Create
        public IActionResult Create()
        {
            List<Subcategory> subcategoriesList=GetSubcategories();
            ViewData["SubcategoryId"] = new SelectList(subcategoriesList, "Id", "Name");
            return View();
        }

        private List<Subcategory> GetSubcategories()
        {
            List<Subcategory> subcategoriesList;
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/GetSubcategories";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                subcategoriesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
            }
            return subcategoriesList;
        }

        // POST: SoftwareLicenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Series,ProductKey,Stock,StartDate,FinishDate,SubcategoryId")] SoftwareLicense softwareLicense)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "SoftwareLicenses/Create";

                    client.DefaultRequestHeaders.Clear();

                    SoftwareLicenseModel softwareLicenseModel = new SoftwareLicenseModel();
                    softwareLicenseModel.Id= softwareLicense.Id;
                    softwareLicenseModel.Name= softwareLicense.Name;
                    softwareLicenseModel.Series= softwareLicense.Series;
                    softwareLicenseModel.ProductKey= softwareLicense.ProductKey;
                    softwareLicenseModel.StartDate= softwareLicense.StartDate;
                    softwareLicenseModel.FinishDate= softwareLicense.FinishDate;
                    softwareLicenseModel.SubcategoryId= softwareLicense.SubcategoryId;

                    string json = JsonConvert.SerializeObject(softwareLicenseModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Licencia Creada Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            List<Subcategory> subcategoriesList = GetSubcategories();
            ViewData["SubcategoryId"] = new SelectList(subcategoriesList, "Id", "Name", softwareLicense.SubcategoryId);
            return View(softwareLicense);
        }

        // GET: SoftwareLicenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "No se puede acceder a esta Licencia!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            SoftwareLicense softwareLicense;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/GetSoftwareLicense?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                
                try
                {
                    softwareLicense = JsonConvert.DeserializeObject<SoftwareLicense>(content);

                    if (softwareLicense == null || softwareLicense.IsDeleted || ((int)softwareLicense.Status) == 2)
                    {
                        TempData["AlertMessage"] = "No se puede acceder a esta Licencia!";
                        TempData["SweetAlertType"] = "error";
                        return RedirectToAction("Index");
                    }

                    List<Subcategory> subcategoriesList = GetSubcategories();

                    ViewData["SubcategoryId"] = new SelectList(subcategoriesList, "Id", "Name", softwareLicense.SubcategoryId);
                    return View(softwareLicense);
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "No se puede acceder a esta Licencia!";
                    TempData["SweetAlertType"] = "error";
                    return RedirectToAction("Index");
                }
            }
            
        }

        // POST: SoftwareLicenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Series,ProductKey,Stock,StartDate,FinishDate,Status,SubcategoryId")] SoftwareLicense softwareLicense)
        {
            if (id != softwareLicense.Id)
            {
                TempData["AlertMessage"] = "No se Encontro Esta Licencia!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "SoftwareLicenses/Edit";

                    client.DefaultRequestHeaders.Clear();

                    SoftwareLicenseModel softwareLicenseModel = new SoftwareLicenseModel();
                    softwareLicenseModel.Id = softwareLicense.Id;
                    softwareLicenseModel.Name = softwareLicense.Name;
                    softwareLicenseModel.Series = softwareLicense.Series;
                    softwareLicenseModel.ProductKey = softwareLicense.ProductKey;
                    softwareLicenseModel.StartDate = softwareLicense.StartDate;
                    softwareLicenseModel.FinishDate = softwareLicense.FinishDate;
                    softwareLicenseModel.SubcategoryId = softwareLicense.SubcategoryId;

                    string json = JsonConvert.SerializeObject(softwareLicenseModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Licencia Editada Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                    }
                    else
                    {
                        TempData["AlertMessage"] = "No se Logro Editar a esta Licencia!";
                        TempData["SweetAlertType"] = "error";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<Subcategory> subcategoriesList = GetSubcategories();
            ViewData["SubcategoryId"] = new SelectList(subcategoriesList, "Id", "Name", softwareLicense.SubcategoryId);
            return View(softwareLicense);
        }

        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "SoftwareLicenses/Delete?id=" + id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool result = JsonConvert.DeserializeObject<bool>(content);
                if (result == true)
                {
                    TempData["AlertMessage"] = "Licencia Eliminada Exitosamente!";
                    TempData["SweetAlertType"] = "success";
                }
                else
                {
                    TempData["AlertMessage"] = "No se Puede Eliminar Esta a este Licencia!";
                    TempData["SweetAlertType"] = "error";
                }

                return RedirectToAction("Index");
            }

            
        }
    }
}
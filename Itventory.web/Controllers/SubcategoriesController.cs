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
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using WebApplication1.Model;
using System.Security.Policy;

namespace Itventory.web.Controllers
{
    [Authorize(Roles = "Administrador, Almacen, It")]
    public class SubcategoriesController : Controller
    {
        // GET: Subcategories
        public IActionResult Index(Status? status)
        {
            List<Subcategory> SubcategoriesList= new List<Subcategory>();
            List<Category> categoriesList;

            using (var client = new HttpClient()) {

                string url = APIConnection.Url+"Subcategories/GetSubcategories";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    SubcategoriesList = JsonConvert.DeserializeObject<List<Subcategory>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }

            }
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Subcategories/GetCategoriesVisualizer";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                categoriesList = JsonConvert.DeserializeObject<List<Category>>(content);

            }

            for(int i=0;i<SubcategoriesList.Count ; i++)
            {
                foreach (var category in categoriesList)
                {
                    if (SubcategoriesList[i].CategoryId == category.Id)
                    {
                        SubcategoriesList[i].Category = category;
                    }
                }
            }

            ViewBag.datasource = SubcategoriesList
               .Select(y => new
               {
                   Id = y.Id,
                   Category = y.Category,
                   Name = y.Name
               })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameAvailable(string name, int id,int CategoryId)
        {
            bool isAvailable;
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Subcategories/IsNameAvailable?name="+name+"&id="+id+"&CategoryId="+CategoryId;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                isAvailable = JsonConvert.DeserializeObject<bool>(content);
            }

            return Json(isAvailable);
        }

        // GET: Subcategories/Create
        public IActionResult Create()
        {
            List<Category> categoriesList= GetCategories();

            ViewData["CategoryId"] = new SelectList(categoriesList.Where(x=>x.Id!=3 && x.Id!=2), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,CategoryId")] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                SubcategoryModel subcategoryModel = new SubcategoryModel();
                subcategoryModel.Id = subcategory.Id;
                subcategoryModel.Name = subcategory.Name;
                subcategoryModel.Status = subcategory.Status;
                subcategoryModel.CategoryId = subcategory.CategoryId;

                
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Subcategories/Create";

                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(subcategoryModel);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result == true)
                    {
                        TempData["AlertMessage"] = "Recurso Creado Exitosamente!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            List<Category> categoriesList=GetCategories();

            ViewData["CategoryId"] = new SelectList(categoriesList, "Id", "Name", subcategory.CategoryId);
            return View(subcategory);
        }

        private List<Category> GetCategories()
        {
            List<Category> categoriesList = new List<Category>();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Subcategories/GetCategories";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                categoriesList = JsonConvert.DeserializeObject<List<Category>>(content);
            }

            return categoriesList;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["AlertMessage"] = "Recurso No Encontrado!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            Subcategory subcategory;

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Subcategories/GetSubcategory?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {
                    subcategory = JsonConvert.DeserializeObject<Subcategory>(content);
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "No se puede acceder a este Recurso!";
                    TempData["SweetAlertType"] = "error";
                    return RedirectToAction("Index");
                }
                
            }

            if (subcategory == null || subcategory.IsDeleted ||subcategory.Id <= 8)
            {
                TempData["AlertMessage"] = "No se puede acceder a este Recurso!";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index");
            }
            List<Category> categoriesList = GetCategories();
            ViewData["CategoryId"] = new SelectList(categoriesList, "Id", "Name", subcategory.CategoryId);
            return View(subcategory);
        }

        // POST: Subcategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId")] Subcategory subcategory)
        {
            if (id != subcategory.Id || subcategory.IsDeleted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                    using (var client = new HttpClient())
                    {
                        string uri = APIConnection.Url + "Subcategories/Edit";

                        client.DefaultRequestHeaders.Clear();


                        SubcategoryModel subcategoryModel = new SubcategoryModel();
                        subcategoryModel.Id = subcategory.Id;
                        subcategoryModel.Name = subcategory.Name;
                        subcategoryModel.CategoryId = subcategory.CategoryId;

                        string json = JsonConvert.SerializeObject(subcategoryModel);

                        var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                        var response = client.PostAsync(uri, httpConten).Result;
                        var content = response.Content.ReadAsStringAsync().Result;
                        bool result = JsonConvert.DeserializeObject<bool>(content);
                        if (result == true)
                        {
                            TempData["AlertMessage"] = "Recurso Editado Exitosamente!";
                            TempData["SweetAlertType"] = "success";
                        }
                        else
                        {
                            TempData["AlertMessage"] = "No se logro editar a este Recurso!";
                            TempData["SweetAlertType"] = "error";
                        }
                    }
                    return RedirectToAction(nameof(Index));
            }
            List<Category> categoriesList = GetCategories();
            ViewData["CategoryId"] = new SelectList(categoriesList, "Id", "Name", subcategory.CategoryId);
            return View(subcategory);
        }


        public async Task<IActionResult> IsDeleteTrueAsync(int id)
        {
            using (var client = new HttpClient())
            {
                string uri = APIConnection.Url + "Subcategories/Delete?id="+id;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                bool result = JsonConvert.DeserializeObject<bool>(content);
                if (result == true)
                {
                    TempData["AlertMessage"] = "Recurso Eliminado Exitosamente!";
                    TempData["SweetAlertType"] = "success";
                    
                }
                else
                {
                    TempData["AlertMessage"] = "Recurso ocupado, no se puede eliminar!";
                    TempData["SweetAlertType"] = "warning";
                    
                }
            }
            return RedirectToAction("Index");
        }

        
    }
}

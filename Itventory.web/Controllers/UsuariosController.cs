using Itventory.web.Entidades;
using Itventory.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System;

namespace Itventory.web.Controllers
{
    public class UsuariosController : Controller
    {
        
        private static string UserId;

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            List<UsuarioVM> usersList= new List<UsuarioVM>();

            using (var client = new HttpClient())
            {
                
                string url = APIConnection.Url+ "Usuarios/GetUsuarios";

                client.DefaultRequestHeaders.Clear();

                try
                {
                    var response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    usersList = JsonConvert.DeserializeObject<List<UsuarioVM>>(content);
                }
                catch (Exception exception)
                {
                    TempData["AlertMessage"] = "Error En La Conexión";
                    TempData["SweetAlertType"] = "error";
                    return View();
                }
                
            }

            ViewBag.datasource = usersList;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = userManager.GetUserId(User); // Obtener el ID del usuario actual
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(user);

            var model = new EditVM
            {
                Id = user.Id,
                Email = user.Email,
                Rol = roles.FirstOrDefault(), // Obtener el primer rol del usuario (puedes ajustar esto según tus necesidades)
                RolesDisponibles = roleManager.Roles.Select(r => r.Name).ToList()
            };

            return View("Profile", model); // Utiliza la misma vista que para el cambio de contraseña

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(EditVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.Password);

                    if (!result.Succeeded)
                    {
                        // Manejar errores al cambiar la contraseña
                        AddErrors(result);
                        return View(model);
                    }
                }

                var resultUpdate = await userManager.UpdateAsync(user);

                if (resultUpdate.Succeeded)
                {
                    TempData["AlertMessage"] = "Contraseña cambiada exitosamente.";
                    TempData["SweetAlertType"] = "success";
                    return RedirectToAction("Profile", "Usuarios");
                }
                else
                {
                    AddErrors(resultUpdate);
                }
            }

            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailAvailable(string email, string id)
        {
            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Usuarios/IsEmailAvailable?email="+email;

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                bool isEmailAvailable = JsonConvert.DeserializeObject<bool>(content);
                if (isEmailAvailable!=true)
                {
                    return Json($"El correo electrónico {email} ya está en uso.");
                }
            }
            
            return Json(true);
        }

        private RegisterVM GetRoles()
        {
            RegisterVM modelo = new RegisterVM();

            using (var client = new HttpClient())
            {
                string url = APIConnection.Url + "Usuarios/GetRoles";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                modelo = JsonConvert.DeserializeObject<RegisterVM>(content);
            }

            return modelo;
        }

        public IActionResult Create()
        {
            // Obtiene la lista de roles disponibles
            var rolesDisponibles = roleManager.Roles.Select(r => r.Name).ToList();

            var modelo = new RegisterVM
            {
                // Asigna la lista de roles disponibles al modelo
                RolesDisponibles = rolesDisponibles
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM model)
        {
            RegisterVM registerVM = GetRoles();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Usuarios/Create";

                    client.DefaultRequestHeaders.Clear();

                    model.RolesDisponibles = registerVM.RolesDisponibles;

                    string json = JsonConvert.SerializeObject(model);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    int result = JsonConvert.DeserializeObject<int>(content);
                    switch(result)
                    {
                        case 1:
                            TempData["AlertMessage"] = "Usuario creado exitosamente.";
                            TempData["SweetAlertType"] = "success";
                            return RedirectToAction(nameof(Index));
                            break;
                        case 2:
                            ModelState.AddModelError(string.Empty, "Error al asignar el rol al usuario.");
                            break;
                        case 3:
                            ModelState.AddModelError(string.Empty, "Error al crear el usuario");
                            break;
                        default:
                            break;
                    }
                }
            }
            
            model.RolesDisponibles = registerVM.RolesDisponibles;
            return View(model);
        }
        
        public IActionResult Edit(string id)
        {
            EditVM model;
            try
            {
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Usuarios/GetUser?id=" + id;

                    client.DefaultRequestHeaders.Clear();

                    var response = client.GetAsync(uri).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<EditVM>(content);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditVM model)
        {
            RegisterVM registerVM = GetRoles();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string uri = APIConnection.Url + "Usuarios/Edit";

                    client.DefaultRequestHeaders.Clear();

                    model.RolesDisponibles=registerVM.RolesDisponibles;

                    string json= JsonConvert.SerializeObject(model);

                    var httpConten = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, httpConten).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    bool result = JsonConvert.DeserializeObject<bool>(content);
                    if (result==true)
                    {
                        TempData["AlertMessage"] = "Usuario actualizado exitosamente.";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al editar el usuario");
                    }
                }

            }
            model.RolesDisponibles = registerVM.RolesDisponibles;
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Profile", "Usuarios");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Datos de inicio de sesion incorrectos");
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> SoftDelete(string Id)
        {
            var mensaje = "";
            using (var client = new HttpClient())
            {

                string uri = APIConnection.Url+"Usuarios/Delete?id="+Id;

                client.DefaultRequestHeaders.Clear();

                
                var response = client.GetAsync(uri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                int resultado = JsonConvert.DeserializeObject<int>(content);
                switch (resultado)
                {
                    case 1:
                        mensaje = "Usuario activado exitosamente.";
                        break;
                    case 2:
                        mensaje = "Usuario desactivado exitosamente.";
                        break;
                    default:
                        return NotFound();
                        break;
                }
            }
            TempData["AlertMessage"] = mensaje;
            TempData["SweetAlertType"] = "success";
            return RedirectToAction("Index", new { mensaje });
        }
    }
}
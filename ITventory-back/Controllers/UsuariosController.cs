using Itventory.web.Entidades;
using Itventory.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("Usuarios")]
    public class UsuariosController : Controller
    {
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

        /// <summary>
        /// Obtiene la lista de usuarios 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los usuarios de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de usuarios
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de usuarios</response>
        /// <response code="500">Si ocurre un error al obtener los usuarios</response>
        [HttpGet("GetUsuarios")]
        public async Task<string> Index()
        {
            var usuariosNoBloqueados = await context.Users
                .Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTime.UtcNow)
                .ToListAsync();

            // Mapear la lista de usuarios a UsuarioVM y agregar información de roles
            List<UsuarioVM> usuariosVM = new List<UsuarioVM>();
            foreach (var user in usuariosNoBloqueados)
            {
                var usuarioVM = new UsuarioVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    Rol = (await userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                usuariosVM.Add(usuarioVM);
            }

            //ViewBag.datasource = usuariosVM;
            string json = JsonConvert.SerializeObject(usuariosVM);

            return json;
        }

        /// <summary>
        /// Obtiene el perfil de un usuario 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los detalles de un usuario mediante el id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el usuario con todos sus detalles
        /// </returns>
        /// <response code="200">Devuelve un JSON con el usuario</response>
        /// <response code="500">Si ocurre un error al obtener al usuario</response>
        [HttpPost]
        [Route("Profile")]
        public async Task<IActionResult> Profile(string userId)
        {
            //var userId = userManager.GetUserId(User); // Obtener el ID del usuario actual
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return StatusCode(500,"No existe un usuario con ese Id");
            }

            var roles = await userManager.GetRolesAsync(user);

            var model = new EditVM
            {
                Id = user.Id,
                Email = user.Email,
                Rol = roles.FirstOrDefault(), // Obtener el primer rol del usuario (puedes ajustar esto según tus necesidades)
                RolesDisponibles = roleManager.Roles.Select(r => r.Name).ToList()
            };


            return Json(model); // Utiliza la misma vista que para el cambio de contraseña
        }

        /// <summary>
        /// Permite saber si un correo ya esta registrado 
        /// </summary>
        /// <remarks>
        /// Este endpoint alerta si un correo ya esta registrado en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al consultar el correo en la base de datos</response>
        [HttpGet("IsEmailAvailable")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return Json(false);
            }

            return Json(true);
        }

        /// <summary>
        /// Obtiene la lista de roles 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los roles de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de roles
        /// </returns>
        /// <response code="200">Devuelve un JSON con la lista de roles</response>
        /// <response code="500">Si ocurre un error al obtener los roles</response>
        [HttpGet("GetRoles")]
        public string GetRoles()
        {
            // Obtiene la lista de roles disponibles
            var rolesDisponibles = roleManager.Roles.Select(r => r.Name).ToList();

            RegisterVM modelo = new RegisterVM
            {
                // Asigna la lista de roles disponibles al modelo
                RolesDisponibles = rolesDisponibles
            };

            string json = JsonConvert.SerializeObject(modelo);

            return json;
        }

        /// <summary>
        /// Permite crear un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo usuario en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero, 1= Creado exitosamente, 2= Error en la asignación del rol, 3=Error en la creacion del usuario, 4=Los datos enviados estan erroneos</response>
        /// <response code="500">Si ocurre un error al crear el usuario</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEnd = null;
                    await userManager.UpdateAsync(user);

                    var roleResult = await userManager.AddToRoleAsync(user, model.Rol);
                    if (roleResult.Succeeded)
                    {
                        //Funciona
                        return Json(1);
                    }
                    else
                    {
                        //Error al asignar el rol
                        await userManager.DeleteAsync(user);
                        return Json(2);
                    }
                }
                else
                {
                    //error al crearlo
                    return Json(3);
                }
            }
            //error en el modelo
            return Json(4);
        }

        /// <summary>
        /// Obtiene un usuario 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un usuario mediante su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene el usuario 
        /// </returns>
        /// <response code="200">Devuelve un JSON con el usuario</response>
        /// <response code="500">Si ocurre un error al obtener el usuario</response>
        [HttpGet("GetUser")]
        public IActionResult Edit(string id)
        {
            // Obtener el usuario por el Id
            var user = userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return StatusCode(500,"No existe un usuario con ese Id");
            }

            // Obtener los roles disponibles
            var rolesDisponibles = roleManager.Roles.Select(r => r.Name).ToList();

            // Obtener los roles del usuario
            var userRoles = userManager.GetRolesAsync(user).Result;

            // Crear el modelo EditVM
            EditVM model = new EditVM
            {
                Id = user.Id,
                Email = user.Email,
                RolesDisponibles = rolesDisponibles,
                Rol = userRoles.FirstOrDefault() // Puedes ajustar esto según tu lógica
            };

            return Json(model);
        }

        /// <summary>
        /// Permite editar un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un usuario en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al editar el usuario</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditVM model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return StatusCode(500,"No existe un usuario con ese Id");
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, token, model.Password);
                }

                if (!string.IsNullOrEmpty(model.Rol))
                {
                    var roles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, roles);
                    await userManager.AddToRoleAsync(user, model.Rol);
                }

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    StatusCode(500,"Error al editar el usuario");
                }
            }
            return StatusCode(500, "Los datos enviados son erroneos");
        }

        /// <summary>
        /// Permite loguear un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite loguear un usuario
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al loguear el usuario</response>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(500,"Error en los datos enviados");
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);
            string userId= userManager.GetUserId(User);
            if (resultado.Succeeded)
            {
                return Json(userId);
            }
            else
            {
                return StatusCode(500,"Error al loguear, contraseña o correo invalidos");
            }
        }

        /// <summary>
        /// Permite desloguear un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite desloguear un usuario
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano</response>
        /// <response code="500">Si ocurre un error al desloguear el usuario</response>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Json(true);
        }

        /// <summary>
        /// Permite activar/desactivar un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite activar o desactivar un usuario en la base de datos por su Id
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un entero
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero 1= Usuario activado exitosamente, 2= Usuario desactivado exitosamente, 3= Usuario inexistente con ese Id</response>
        /// <response code="500">Si ocurre un error al eliminar el usuario</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> SoftDelete(string Id)
        {
            var usuario = await userManager.FindByIdAsync(Id);

            if (usuario == null)
            {
                return Json(3);
            }

            if (usuario.LockoutEnabled)
            {
                // Si está bloqueado, desbloquear (reactivar)
                usuario.LockoutEnabled = false;
                usuario.LockoutEnd = null; // Limpiar LockoutEnd para desbloquear de inmediato
            }
            else
            {
                // Si no está bloqueado, bloquear (desactivar)
                usuario.LockoutEnabled = true;
                usuario.LockoutEnd = DateTimeOffset.MaxValue; // Bloquear indefinidamente
            }

            await userManager.UpdateAsync(usuario);

            // Otros pasos que puedas necesitar

            if (usuario.LockoutEnabled)
            {
                return Json(2);
            }
            else
            {
                return Json(1);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Itventory.web.Entidades;
using System.Security.Policy;
using Newtonsoft.Json;
using WebApplication1.Models;
using Microsoft.Build.Framework;

namespace Itventory.web.Controllers
{
    [ApiController]
    [Route("Acts")]
    public class ActsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Acts
        /// <summary>
        /// Obtiene una lista de las actas
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todas las actas almacenadas en la base de datos.
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene la lista de actas.
        /// </returns>
        /// <response code="200">Devuelve un JSON que contiene la lista de actas</response>
        /// <response code="500">Si ocurre un error al obtener las actas</response>
        [HttpGet("GetActs")]
        public async Task<IActionResult> Index()
        {
            List<Act> actsList = _context.Acts.ToList();

            return Json(actsList);
        }

        // POST: Acts/Create
        /// <summary>
        /// Crea una acta
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un acta en la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un booleano en caso de crear el acta</response>
        /// <response code="500">Si ocurre un error al crear el acta</response>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm]ActModel actModel)
        {
            Act act = new Act();
            act.Name = actModel.Name;
            act.WorkStationId = actModel.WorkStationId;
            act.File = actModel.File;

            var workStation = await _context.WorkStations.Include(ws => ws.Employee).FirstOrDefaultAsync(ws => ws.Id == act.WorkStationId);

            if (workStation==null)
            {
                return StatusCode(500,"No se encontró la WorkStation");
            }

            if (act.Name == "Acta de Finalizacion")
            {
                workStation.Status = (Status)1;
            }
            else
            {

                workStation.Status = (Status)2;
            }

            if (act.File != null && Path.GetExtension(act.File.FileName).ToLower() == ".pdf")
            {
                string pdfFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");

                string pdfFileName = act.Name+"_"+workStation.Employee.Name+"_"+workStation.Employee.LastName+".pdf";

                string pdfFilePath = Path.Combine(pdfFolderPath, pdfFileName);

                using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                {
                    await act.File.CopyToAsync(stream);
                }

                act.URL = pdfFilePath;

                _context.Update(workStation);
                _context.Add(act);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                return StatusCode(500,"Error al crear el acta");
            }
        }

        // GET: Acts/Edit/5
        /// <summary>
        /// Obtiene un acta de la base de datos mediante su Id
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener un acta de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un acta
        /// </returns>
        /// <response code="200">Devuelve un JSON con un acta</response>
        /// <response code="500">Si ocurre un error al obtener el acta</response>
        [HttpGet("GetAct")]
        public async Task<IActionResult> GetAct(int? id)
        {
            if (id == null || _context.Acts == null)
            {
                return StatusCode(500,"Id nulo o error en la conexion a la base de datos");
            }

            var act = await _context.Acts.Include(x => x.WorkStation).ThenInclude(x => x.Employee).FirstOrDefaultAsync(ws => ws.Id == id);
            if (act == null)
            {
                return StatusCode(500, "Error al obtener el acta, no existe un acta con ese Id");
            }

            ActModel actModel = new ActModel();
            actModel.Id = act.Id;
            actModel.Name = act.Name;
            actModel.WorkStationId = act.WorkStationId;
            actModel.File = act.File;

            return Json(actModel);
        }

        /// <summary>
        /// Obtiene un acta de la base de datos mediante su Id para descargarla
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener un acta de la base de datos para descargarla
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un acta para descargar
        /// </returns>
        /// <response code="200">Devuelve un JSON con un acta para descargar</response>
        /// <response code="500">Si ocurre un error al obtener el acta</response>
        [HttpGet("GetActDownload")]
        public async Task<IActionResult> GetActDownload(int? id)
        {
            if (id == null || _context.Acts == null)
            {
                return StatusCode(500,"Id nulo o error en la conexion con la base de datos");
            }

            var act = await _context.Acts.Include(x => x.WorkStation).ThenInclude(x => x.Employee).FirstOrDefaultAsync(ws => ws.Id == id); ;
            if (act == null)
            {
                return StatusCode(500, "Error al obtener el acta, no existe un acta con ese Id");
            }

            ActModelDownload actModel = new ActModelDownload();
            actModel.Id = act.Id;
            actModel.Name = act.Name;
            actModel.URL = act.URL;
            actModel.WorkStationId = act.WorkStationId;
            actModel.File = act.File;

            return Json(actModel);
        }

        // POST: Acts/Edit/5
        /// <summary>
        /// Edita un acta de la base de datos
        /// </summary>
        /// <remarks>
        /// Este endpoint permite editar un acta de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un entero que indica lo siguiente: 1. Se edito correctamente, 2. No se pudo editar, 3. Error en la conexion a la base de datos</response>
        /// <response code="500">Si ocurre un error al editar el acta</response>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(ActModel actModel)
        {
            Act act = new Act();
            act.Id = (int) actModel.Id;
            act.Name = actModel.Name;
            act.WorkStationId = actModel.WorkStationId;
            act.File = actModel.File;

            try
            {
                var workStation = await _context.WorkStations.Include(ws => ws.Employee).FirstOrDefaultAsync(ws => ws.Id == act.WorkStationId);

                if (act.File != null && Path.GetExtension(act.File.FileName).ToLower() == ".pdf")
                {
                    string pdfFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");

                    string pdfFileName = $"{act.Name}_{workStation.Employee.Name}_{workStation.Employee.LastName}.pdf";

                    string pdfFilePath = Path.Combine(pdfFolderPath, pdfFileName);

                    using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await act.File.CopyToAsync(stream);
                    }

                    _context.Update(workStation);
                    await _context.SaveChangesAsync();
                    return Json(1);
                }
                else
                {
                    return Json(2);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(3);
            }
        }

        /// <summary>
        /// Elimina un acta de la base de datos
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un acta de la base de datos
        /// </remarks>
        /// <returns>
        /// Un JSON que contiene un booleano
        /// </returns>
        /// <response code="200">Devuelve un JSON con un booleano </response>
        /// <response code="500">Si ocurre un error al eliminar el acta</response>
        [HttpGet("Delete")]
        public async Task<IActionResult> DeleteAct(int id)
        {
            try
            {
                var Acta = await _context.Acts.FindAsync(id);


                Acta.IsDeleted = true;

                _context.Update(Acta);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch(Exception exception)
            {
                return StatusCode(500, "Error, no se encontró un acta con ese Id");
            }
        }
    }
}

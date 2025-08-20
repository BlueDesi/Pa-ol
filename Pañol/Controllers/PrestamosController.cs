using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pañol.Data;
using Pañol.Models;

namespace Pañol.Controllers
{
    public class PrestamosController : Controller
    {
        private PañolContext db = new PañolContext();


        public ActionResult DevolucionPropios()
        {
            int usuarioId = (int)(Session["UsuarioId"] ?? 0);

            var prestamosPendientes = db.Prestamos
                .Include(p => p.Profesor)
                .Include(p => p.Usuario)
                .Include(p => p.PrestamoItems.Select(pi => pi.Item))
                .Where(p => !p.Cancela && p.UsuarioId == usuarioId) 
                .ToList();

            return View(prestamosPendientes);
        }
        public ActionResult Buscar(string criterio)
        {
            var prestamos = db.Prestamos
         .Include(p => p.Profesor)
         .Include(p => p.Usuario)
         .Include(p => p.PrestamoItems.Select(pi => pi.Item))
         .Where(p => string.IsNullOrEmpty(criterio)
                     || p.Profesor.Apellido.ToLower().Contains(criterio.ToLower()))
         .ToList();

            ViewBag.Criterio = criterio;
            return View(prestamos);
        }
        //buscar
        public ActionResult Pendientes()
        {
            var prestamosPendientes = db.Prestamos
        .Include(p => p.Profesor)
        .Include(p => p.Usuario)
        .Include(p => p.PrestamoItems.Select(pi => pi.Item))
        .Where(p => !p.Cancela)
        .ToList();

            return View(prestamosPendientes);
        }
        public ActionResult Finalizar(int id)
        {
            var prestamo = db.Prestamos
        .Include(p => p.Profesor)
        .Include(p => p.Usuario)
        .Include(p => p.PrestamoItems.Select(pi => pi.Item)) 
        .FirstOrDefault(p => p.Id == id);

            if (prestamo == null)
            {
                return HttpNotFound();
            }

            return View(prestamo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finalizar(int id, string nota)
        {
            var prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }

            prestamo.Cancela = true;
            prestamo.Observacion = nota;

            var itemsPrestados = db.PrestamosItem.Where(pi => pi.PrestamoId == id).Select(pi => pi.Item).ToList();
            foreach (var item in itemsPrestados)
            {
                item.Disponible = true;
            }
            prestamo.FechaHora_D = DateTime.Now; // 

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET: Prestamos
        public ActionResult Index()
        {
            var prestamos = db.Prestamos
                .Include(p => p.Profesor)
                .Include(p => p.Usuario)
                .Include(p => p.PrestamoItems.Select(pi => pi.Item))
                .OrderByDescending(p => p.FechaHora_E) //
                .ToList();

            return View(prestamos);
        }

        // GET: Prestamos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // GET: Prestamos/Create
        public ActionResult Create()
        {
            int usuarioId = (int)(Session["UsuarioId"] ?? 0);
            var usuario = db.Usuarios.Find(usuarioId);

            var profesor = db.Profesores.FirstOrDefault(p => p.Dni == usuario.Dni);

            ViewBag.UsuarioNombreApellido = profesor != null
                ? profesor.Apellido + ", " + profesor.Nombre
                : usuario.Username;

            ViewBag.UsuarioRol = usuario.Rol == 1 ? "Administrador" : "Empleado";
            ViewBag.UsuarioEmail = usuario.Username;

            ViewBag.UsuarioId = usuario.Id;

            ViewBag.ProfesorId = new SelectList(
                db.Profesores.Select(p => new { p.Id, NombreCompleto = p.Apellido + ", " + p.Nombre }),
                "Id", "NombreCompleto", profesor?.Id // ← Si coincide por DNI, lo deja seleccionado
            );

            ViewBag.ItemsDisponibles = db.Items
                .Where(i => i.Disponible)
                .OrderBy(i => i.Detalle)
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    [Bind(Include = "Id,ProfesorId,UsuarioId,Curso,FechaHora_E,FechaHora_D,Retira")] Prestamo prestamo,
    int[] selectedItems,int? UsuarioId)

           
        {
            if (ModelState.IsValid)
            {
                int usuarioId = (int)(Session["UsuarioId"] ?? 0);
                var usuario = db.Usuarios.Find(usuarioId);
                prestamo.FechaHora_E = DateTime.Now;
                db.Prestamos.Add(prestamo);
                db.SaveChanges();

                if (selectedItems != null && selectedItems.Any())
                {
                    foreach (var itemId in selectedItems)
                    {
                        var prestamoItem = new PrestamoItem
                        {
                            PrestamoId = prestamo.Id,
                            ItemId = itemId
                        };
                        db.PrestamosItem.Add(prestamoItem);

                        var item = db.Items.Find(itemId);
                        if (item != null)
                        {
                            item.Disponible = false;
                        }
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // 🔁 Si algo falla, volvemos a cargar TODO
            ViewBag.ProfesorId = new SelectList(db.Profesores, "Id", "Nombre", prestamo.ProfesorId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Username", prestamo.UsuarioId);
            ViewBag.ItemsDisponibles = db.Items.Where(i => i.Disponible).OrderBy(p => p.Detalle).ToList();

            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfesorId = new SelectList(db.Profesores, "Id", "Nombre", prestamo.ProfesorId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Username", prestamo.UsuarioId);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProfesorId,UsuarioId,Curso,FechaHora_E,FechaHora_D,Retira")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prestamo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfesorId = new SelectList(db.Profesores, "Id", "Nombre", prestamo.ProfesorId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Username", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo prestamo = db.Prestamos.Find(id);
            db.Prestamos.Remove(prestamo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

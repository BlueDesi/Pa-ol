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

        // GET: Prestamos
        public ActionResult Index()
        {
            var prestamos = db.Prestamos
                     .Include(p => p.Profesor)
                     .Include(p => p.Usuario)
                     .Include(p => p.PrestamoItems.Select(pi => pi.Item)); // ← importante

            return View(prestamos.ToList());
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
            ViewBag.ProfesorId = new SelectList(db.Profesores, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Username");
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
    int[] selectedItems)
        {
            if (ModelState.IsValid)
            {
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

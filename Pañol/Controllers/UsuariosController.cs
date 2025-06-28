using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Pañol.Data;
using Pañol.Models;
using Pañol.Filtros;
using Pañol.Helpers;
namespace Pañol.Controllers
{
    public class UsuariosController : Controller
    {
        private PañolContext db = new PañolContext();

        // GET: Usuarios
       // [Admin]
        public ActionResult Index()
        { 
            return View(db.Usuarios.ToList());
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Usuario user1)
        { string valor = "nulo";
            string Phas = Hasheador.Hashear(user1.Password);
            user1.Password = Phas;
            var q = await db.Usuarios.FirstOrDefaultAsync(m => m.Username == user1.Username && m.Password == user1.Password);
            if (q!=null)

                if (q != null)
                {
                    valor = "Bienvenido";
                    Session["Username"] = q.Username;
                    Session["IdRol"] = q.Rol;
                }
                else
                {
                    valor = "Error";
                    Session["Username"] = null;
                    Session["IdRol"] = null;

                }
            if (Session["IdRol"]?.ToString() == "1")
            {
                return RedirectToAction("PanelAdmin", "Panel");
            }
            else
            {
                ViewBag.Mensaje = valor;
                return RedirectToAction("PanelUser", "Panel");

            }


        }

        public ActionResult Resultao()
        {
            return View();
        }

        // GET: Usuarios/Details/5
      // [Admin]
        public ActionResult Details(int? id)
        { //rechaza
            if (Session["Rol"]?.ToString() != "Admin")
                return RedirectToAction("Login");
            //rechaza
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
       // [Admin]
        // GET: Usuarios/Create
        public ActionResult Create()
        { //rechaza
            ViewBag.Dnis = db.Profesores.ToList();
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
       // [Admin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Rol,Dni")] Usuario usuario)
        { //rechaza

            if (string.IsNullOrWhiteSpace(usuario.Dni))
            {
                ModelState.AddModelError("Dni", "Debe ingresar un DNI.");
            }
            else if (!db.Profesores.Any(p => p.Dni == usuario.Dni))
            {
                ModelState.AddModelError("Dni", "El DNI ingresado no está registrado. Por favor, registre primero ese profesor en la opción 'Agregar Profesor'.");
            }

            //rechaza
            if (ModelState.IsValid)
            {
                usuario.Password = Hasheador.Hashear(usuario.Password);
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
       // [Admin]
        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Dnis = db.Profesores.ToList();

            //rechaza
            // if (Session["Rol"]?.ToString() != "Admin")
            //   return RedirectToAction("Login");
            //rechaza
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
       // [Admin]
        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Rol,Dni")] Usuario usuario)
        { //rechaza

            // if (Session["Rol"]?.ToString() != "Admin")
            //   return RedirectToAction("Login");
            //rechaza


            if (string.IsNullOrWhiteSpace(usuario.Dni))
            {
                ModelState.AddModelError("Dni", "Debe ingresar un DNI.");
            }
            else if (!db.Profesores.Any(p => p.Dni == usuario.Dni))
            {
                ModelState.AddModelError("Dni", "El DNI ingresado no está registrado. Por favor, registre primero ese profesor en la opción 'Agregar Profesor'.");
            }
            if (ModelState.IsValid)
            {
                usuario.Password = Hasheador.Hashear(usuario.Password);

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
      //  [Admin]
        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

     //   [Admin]
        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {  
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Models.TipoParametro;
using Web.Utilitario;

namespace Web.Controllers
{
    public class TipoParametroController : Controller
    {
        private TipoParametroContext db = new TipoParametroContext();

        // GET: TipoParametro
        [Authorize(Roles = "ADMINISTRADOR, PARAMETRIZADOR")]
        public ActionResult Index()
        {
            return View(db.TipoParametroes.ToList());
        }

        public JsonResult listarTipoParametro()
        {
            try
            {
                List<TipoParametro> lista = db.TipoParametroes.ToList();
                return Json(lista.OrderBy(x => x.CodTipoParametro));
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        // GET: TipoParametro/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoParametro tipoParametro = db.TipoParametroes.Find(id);
            if (tipoParametro == null)
            {
                return HttpNotFound();
            }
            return View(tipoParametro);
        }

        // GET: TipoParametro/Create
        [Authorize(Roles = "ADMINISTRADOR, PARAMETRIZADOR")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoParametro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CodTipoParametro,TipoParametroDescripcion,Estado,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] TipoParametro tipoParametro)
        {
            if (ModelState.IsValid)
            {
                db.TipoParametroes.Add(tipoParametro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoParametro);
        }

        // GET: TipoParametro/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoParametro tipoParametro = db.TipoParametroes.Find(id);
            if (tipoParametro == null)
            {
                return HttpNotFound();
            }
            return View(tipoParametro);
        }

        // POST: TipoParametro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CodTipoParametro,TipoParametroDescripcion,Estado,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] TipoParametro tipoParametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoParametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoParametro);
        }

        // GET: TipoParametro/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoParametro tipoParametro = db.TipoParametroes.Find(id);
            if (tipoParametro == null)
            {
                return HttpNotFound();
            }
            return View(tipoParametro);
        }

        // POST: TipoParametro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TipoParametro tipoParametro = db.TipoParametroes.Find(id);
            db.TipoParametroes.Remove(tipoParametro);
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

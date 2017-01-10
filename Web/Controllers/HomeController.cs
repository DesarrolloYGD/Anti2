using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private Alumno alumno = new Alumno();
        private Curso curso = new Curso();
        public ActionResult Index()
        {
            return View(alumno.Listar());
        }

        public ActionResult Crud2(int id)
        {
            return View(alumno.Obtener(id));
            
        }

        public ActionResult Ver(int id)
        {
            return View(alumno.Obtener(id));
        }


        public ActionResult Crud(int id = 0)
        {
            ViewBag.Cursos = curso.Todo();
            return View(id > 0 ? alumno.Obtener(id):alumno);

        }

        public ActionResult Eliminar(int id)
        {
            alumno.Eliminar(id);
            return Redirect("~/home");

        }

        public ActionResult Guardar( Alumno model, int[] cursos = null)
        {
            if(cursos != null)
            {
                foreach (var c in cursos)
                    model.Curso.Add(new Curso { id = c });
            }
            else
            {
                ModelState.AddModelError("curso", "Debe seleccionar al menos un examen");
            }

            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Home/Crud/" + model.id);
            }
            else
            {
                ViewBag.Cursos = curso.Todo();
                return View("~/Views/Home/Crud.cshtml",model);
            }
           
            
        }

    }
}
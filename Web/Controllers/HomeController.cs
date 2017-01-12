using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Crud2(int id)
        {
            return View(alumno.Obtener(id));
            
        }

        public ActionResult Ver(int id)
        {
            return View(alumno.Obtener(id));
        }

        public ActionResult Exportar(int id)
        {
            var alum = alumno.Obtener(id);
            

            //ITextSharp
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            MemoryStream stream = new MemoryStream();

            

            try
            {
                doc.AddTitle("Hoja-clinica" + alum.Rut + "_" + DateTime.Now);
                doc.AddCreator("Medico");

                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
                pdfWriter.CloseStream = false;

                doc.Open();
                doc.Add(new Paragraph("Ficha Clinica Paciente N° " + alum.id));
                doc.Add(Chunk.NEWLINE);
                doc.Close();
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }

            doc.Close();

            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required

            return File(stream, "application/pdf", "Hoja-clinica" + alum.Rut + "_" + DateTime.Now+".pdf");
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
                return Redirect("~/Home/Ver/" + model.id);
            }
            else
            {
                ViewBag.Cursos = curso.Todo();
                return View("~/Views/Home/Crud.cshtml",model);
            }
           
            
        }

    }
}
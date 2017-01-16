using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntiAging2._0.Controllers
{
    public class HomeController : Controller
    {
        private Paciente paciente = new Paciente();
        private Examen examen = new Examen();
        public ActionResult Index()
        {
            return View(paciente.Listar());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Crud2(int id)
        {
            return View(paciente.Obtener(id));

        }

        public ActionResult Ver(int id)
        {
            return View(paciente.Obtener(id));
        }

        public ActionResult Exportar(int id)
        {
            var alum = paciente.Obtener(id);



            //ITextSharp
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            MemoryStream stream = new MemoryStream();



            try
            {
                DateTime fecha = DateTime.Now;
                string date = fecha.ToString("dd.MM:yyyy");

                doc.AddTitle("Hoja-clinica" + alum.Rut + "_" + DateTime.Now);
                doc.AddCreator("Medico");

                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
                pdfWriter.CloseStream = false;

                //creacion de tipos de fuentes
                //direccion de la fuente
                var arial_bold_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

                doc.Open();
                Font _standardFont = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, BaseColor.BLACK);
                Font _boldFont = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK);
                Font _subtitulo = new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, BaseColor.BLACK);

                Paragraph titulo = new Paragraph("Ficha Clinica Paciente N° " + alum.id);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(Chunk.NEWLINE);
                Paragraph fech = new Paragraph();
                fech.Add(new Chunk("Documento emitido con fecha: " + date, _standardFont));
                doc.Add(fech);
                doc.Add(Chunk.NEWLINE);
                Paragraph datos = new Paragraph();
                datos.Add(new Chunk("DATOS PACIENTE", _subtitulo));
                doc.Add(datos);
                doc.Add(Chunk.NEWLINE);
                Paragraph nombre = new Paragraph();
                Paragraph rut = new Paragraph();
                Paragraph peso = new Paragraph();
                Paragraph edad = new Paragraph();

                nombre.Add(new Chunk("Nombre: ", _boldFont));
                nombre.Add(new Chunk(alum.Nombre + " " + alum.Apellido, _standardFont));

                rut.Add(new Chunk("Rol Único Nacional: ", _boldFont));
                rut.Add(new Chunk(alum.Rut, _standardFont));

                edad.Add(new Chunk("Edad: ", _boldFont));
                edad.Add(new Chunk(alum.Edad.ToString() + " años", _standardFont));

                peso.Add(new Chunk("Peso: ", _boldFont));
                peso.Add(new Chunk(alum.Peso.ToString() + " Kg", _standardFont));

                doc.Add(nombre); doc.Add(rut); doc.Add(edad); doc.Add(peso); doc.Add(Chunk.NEWLINE); doc.Add(Chunk.NEWLINE);

                Paragraph examenes = new Paragraph();
                examenes.Add(new Chunk("EXAMENES SELECCIONADOS", _subtitulo));
                doc.Add(examenes);
                doc.Add(Chunk.NEWLINE);

                int count = 0;
                foreach (var c in alum.Examen)
                {
                    count = count + 1;
                    Paragraph exam = new Paragraph();
                    exam.Add(new Chunk(count.ToString() + " ", _boldFont));
                    exam.Add(new Chunk(c.Nombre, _standardFont));
                    doc.Add(exam);

                }



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

            return File(stream, "application/pdf", "Hoja-clinica" + alum.Rut + "_" + DateTime.Now + ".pdf");
        }


        public ActionResult Crud(int id = 0)
        {
            ViewBag.Examenes = examen.Todo();
            return View(id > 0 ? paciente.Obtener(id) : paciente);

        }

        public ActionResult Eliminar(int id)
        {
            paciente.Eliminar(id);
            return Redirect("~/home");

        }

        public ActionResult Guardar(Paciente model, int[] examenes = null)
        {
            if (examenes != null)
            {
                foreach (var c in examenes)
                    model.Examen.Add(new Examen { id = c });
            }
            else
            {
                ModelState.AddModelError("examen", "Debe seleccionar al menos un examen");
            }

            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Home/Ver/" + model.id);
            }
            else
            {
                ViewBag.Examenes = examen.Todo();
                return View("~/Views/Home/Crud.cshtml", model);
            }


        }
    }
}
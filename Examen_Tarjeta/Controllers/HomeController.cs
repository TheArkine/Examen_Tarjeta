using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Examen_Tarjeta.Models;
using System.IO;

namespace Examen_Tarjeta.Controllers
{
    public class HomeController : Controller
    {
        Examen_TarjetaEntities BD = new Examen_TarjetaEntities();
        public ActionResult Tarjetas(string BancoEmisor)
        {

           List<Get_Tarjetas_Result> lista = new List<Get_Tarjetas_Result>();

            lista = BD.Get_Tarjetas(BancoEmisor).ToList();
            return View(lista);

        }

        public ActionResult Agregar()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Agregar(Get_Tarjetas_Result modeloVista)
        {
            int registros = 0;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase carga = Request.Files[i];
                string ImageName = Path.GetFileName(carga.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);
                carga.SaveAs(physicalPath);

                try
                {
                    registros = BD.Insert_Tarjetas(
                        modeloVista.Numero_Tarjeta.ToString(),
                        Convert.ToInt32(modeloVista.CVV),
                        modeloVista.Fecha_Exp,
                        modeloVista.Banco,
                        carga.FileName,
                        modeloVista.Emisor,
                        modeloVista.Dueno
                        );

                }
                catch (Exception e)
                {
                    Response.Write("<script laguage=JavaScript>alert('Error al agregar..');</script>");

                }
                finally
                {
                    if (registros > 0)
                    {
                        Response.Write("<script laguage=JavaScript>alert('Tarjeta agregada');</script>");

                    }
                    else
                    {
                        Response.Write("<script laguage=JavaScript>alert('Error al agregar..');</script>");

                    }
                }

            }

           
            return Redirect("~/Home/Tarjetas");

        }

        public ActionResult Editar(int id)
        {
            Get_Tarjeta_Result modeloVista = new Get_Tarjeta_Result();
            modeloVista = this.BD.Get_Tarjeta(id).FirstOrDefault();
            Session["Id_Tarjeta"] = id;
            return View(modeloVista);
        }

        //Método que modifica la cuenta en la BD
        [HttpPost]
        public ActionResult Editar(Get_Tarjetas_Result modeloVista)
        {
            int registros = 0;

            try
            {
                registros = this.BD.Edit_Tarjetas(
                        Convert.ToInt16(Session["Id_Tarjeta"]),
                       modeloVista.Numero_Tarjeta,
                        Convert.ToInt32(modeloVista.CVV)
                    );
            }
            catch (Exception e)
            {
                Response.Write("<script laguage=JavaScript>alert('Error al editar..');</script>");
            }
            finally
            {
                if (registros > 0)
                {
                    Response.Write("<script laguage=JavaScript>alert('Editada..');</script>");
                }
                else
                {
                    Response.Write("<script laguage=JavaScript>alert('Error al editar..');</script>");
                }
            }

            return RedirectToAction("Tarjetas", "Home");
        }


        [HttpPost]
        public ActionResult Eliminar(int Id_Tarjeta)
        {
            int Registros = 0;

            try
            {
                Registros = this.BD.Delete_Tarjeta(Id_Tarjeta);
            }
            catch (Exception)
            {
                Response.Write("<script laguage=JavaScript>alert('Error al eliminar..');</script>");
            }
            finally
            {
                if (Registros > 0)
                {
                    Response.Write("<script laguage=JavaScript>alert('Tarjeta eliminada..');</script>");
                }
                else
                {
                    Response.Write("<script laguage=JavaScript>alert('Error al eliminar..');</script>");
                }
            }
            return RedirectToAction("Tarjetas", "Home");
        }

        [HttpPost]
        public ActionResult CambiarEstado(int Id_Tarjeta, bool Estado)
        {
            int Registros = 0;

            try
            {
                Registros = this.BD.Edit_Estado(Id_Tarjeta, Estado);
            }
            catch (Exception)
            {
                Response.Write("<script laguage=JavaScript>alert('Error..');</script>");
            }
            finally
            {
                if (Registros > 0)
                {
                    Response.Write("<script laguage=JavaScript>alert('Editada..');</script>");
                }
                else
                {
                    Response.Write("<script laguage=JavaScript>alert('Error..');</script>");
                }
            }
            return RedirectToAction("Tarjetas", "Home");
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Utilitario;
using Datos;
using Negocio;
using Web.Models.Producto;
using Web.Models;
using Web.Models.Cotizacion;
using Rotativa;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Web.Service.RC.FactElect;
using Web.Models.Clientes;
using System.Data.Entity.Migrations;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data.SqlClient;

namespace Web.Controllers
{
    public class CotizacionController : BaseController
    {
        private CotizacionContext db = new CotizacionContext();
        private ParametroContext dbParam = new ParametroContext();
        private TipoParametroContext dbTipo = new TipoParametroContext();
        WH_ProductoServicioNEG productoServicioNEG = new WH_ProductoServicioNEG();

        // GET: Cotizacion
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Cotizacions.OrderByDescending(x => (x.FechaModificacion == null) ? x.FechaRegistro : x.FechaModificacion)); //.ThenBy(x => x.NumeroCotizacion).ThenBy(x => x.Correlativo).ThenByDescending(x => x.CorrelativoInicial));
        }

        // GET: Cotizacion/Details/5
        [Authorize]
        public ActionResult Details(long id)
        {
            Cotizacion cotizacion = db.Cotizacions.Find(id);
            if (cotizacion == null)
                return HttpNotFound();

            ViewBag.ID = id;
            var modelo = ObtenerCotizacionViewModel(cotizacion, id);
            return View(modelo);
        }

        public ActionResult Print(long id)
        {
            Cotizacion cotizacion = db.Cotizacions.Find(id);
            if (cotizacion == null)
                return HttpNotFound();

            var modelo = ObtenerCotizacionViewModel(cotizacion, id);
            return View(modelo);
        }

        private CotizacionViewModel ObtenerCotizacionViewModel(Cotizacion cotizacion, long id)
        {
            ClienteNEG clienteNEG = new ClienteNEG();
            List<Cliente> clientes = clienteNEG.listarCliente();
            List<MostrarClienteViewModel> listaClientes = MostrarClienteViewModel.convert(clientes);
            List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();
            List<MostrarProductoServicioViewModels> productos = MostrarProductoServicioViewModels.convert(resultado);
            CotizacionViewModel modelo = new CotizacionViewModel();

            modelo.Cliente = (from c in listaClientes
                              where c.empresaNumeroDocumento == cotizacion.RUC
                              select c).FirstOrDefault();
            modelo.Cotizacion = cotizacion;
            modelo.Certificados = (from c in db.CotizacionCertificado.ToList()
                                   where c.IdCotizacion == id
                                   select c).ToList();
            modelo.Detalles = (from cp in db.CotizacionProducto.ToList()
                               join prd in productos on cp.IdProducto equals prd.idProducto
                               join p in dbParam.Parametroes on cp.IdParametro equals p.ID
                               join t in dbTipo.TipoParametroes on cp.IdTipoParametro equals t.ID
                               where cp.IdCotizacion == id
                               select new DetalleCotizacionViewModel { producto = prd, parametro = p, tipoParametro = t, productoCotizacion = cp, Precio = cp.Precio }).ToList();
            modelo.Inspeccion = (from i in db.CotizacionInspeccion.ToList()
                                 where i.IdCotizacion == id
                                 select i).ToList();
            modelo.Resumen = (from i in db.CotizacionResumen.ToList()
                              where i.IdCotizacion == id
                              select i).ToList();
            modelo.NombreUsuario = NombreUsuario(cotizacion.IdUsuarioRegistro);
            return modelo;
        }

        [HttpGet]
        [ActionDownload]
        public FileResult PrintCotizacion(long id)
        {
            var cotizacion = db.Cotizacions.Find(id);
            Session["NombreArchivo"] = cotizacion.NumeroCotizacion + "-" + "Cotización-" + cotizacion.Solicitante.Trim().ToUpper();
            string RptPath = "";
            LocalReport lr = null;
            string filePath = System.IO.Path.GetTempFileName();

            try
            {
                if (cotizacion.TipoCotizacion == "AMB")
                {
                    RptPath = Server.MapPath("~/Content/Reportes/CotizacionAmbiental.rdlc");
                }
                else
                {
                    RptPath = Server.MapPath("~/Content/Reportes/CotizacionAlimentos.rdlc");
                }
                
                lr = new LocalReport
                {
                    ReportPath = RptPath,
                    EnableExternalImages = true,
                    EnableHyperlinks = true
                };
                               
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                DataSet ds = comprobanteNEG.ObtenerCotizacionImpresion(cotizacion.ID);
                ClienteNEG clienteNEG = new ClienteNEG();
                List<Cliente> clientes = clienteNEG.listarCliente();
                List<MostrarClienteViewModel> listaClientes = MostrarClienteViewModel.convert(clientes);

                var cliente = (from c in listaClientes
                               where c.empresaNumeroDocumento == cotizacion.RUC
                               select c).FirstOrDefault();

                lr.SetParameters(new ReportParameter("RUC", ds.Tables[0].Rows[0]["RUC"].ToString()));
                lr.SetParameters(new ReportParameter("RazonSocial", ds.Tables[0].Rows[0]["Solicitante"].ToString()));
                lr.SetParameters(new ReportParameter("Direccion", cliente.empresaDomicilio));
                lr.SetParameters(new ReportParameter("Responsable", ds.Tables[0].Rows[0]["Contacto"].ToString()));
                lr.SetParameters(new ReportParameter("Telefono", ds.Tables[0].Rows[0]["Telefono"].ToString()));
                lr.SetParameters(new ReportParameter("Email", ds.Tables[0].Rows[0]["Email"].ToString()));
                lr.SetParameters(new ReportParameter("TipoDocumentoSolicitado", ds.Tables[0].Rows[0]["TipoDocumentoSolicitado"].ToString()));
                lr.SetParameters(new ReportParameter("DescripcionDelProducto", ds.Tables[0].Rows[0]["DescripcionProducto"].ToString()));
                lr.SetParameters(new ReportParameter("CantidadDeMuestra", ds.Tables[0].Rows[0]["CantidadMuestra"].ToString()));
                lr.SetParameters(new ReportParameter("NumeroCotizacion", ds.Tables[0].Rows[0]["NumeroCotizacion"].ToString()));
                lr.SetParameters(new ReportParameter("Fecha", ds.Tables[0].Rows[0]["Fecha"].ToString()));
                lr.SetParameters(new ReportParameter("Subtotal", ds.Tables[0].Rows[0]["SubTotal"].ToString()));
                lr.SetParameters(new ReportParameter("DescuentoValor", ds.Tables[0].Rows[0]["MontoDescuento"].ToString()));
                lr.SetParameters(new ReportParameter("DescuentoPorcentaje", ds.Tables[0].Rows[0]["PorcentajeDescuento"].ToString()));
                lr.SetParameters(new ReportParameter("SubtotalFinal", ds.Tables[0].Rows[0]["SubTotalFinal"].ToString()));
                lr.SetParameters(new ReportParameter("IGV", ds.Tables[0].Rows[0]["IGV"].ToString()));
                lr.SetParameters(new ReportParameter("Total", ds.Tables[0].Rows[0]["Total"].ToString()));
                lr.SetParameters(new ReportParameter("Proyecto", ds.Tables[0].Rows[0]["Proyecto"].ToString()));
                lr.SetParameters(new ReportParameter("DiasEntrega", ds.Tables[0].Rows[0]["DiasEntrega"].ToString()));
                lr.SetParameters(new ReportParameter("CorreoConfirmacion", ds.Tables[0].Rows[0]["CorreoConfirmacion"].ToString()));
                lr.SetParameters(new ReportParameter("CondicionPago1", ds.Tables[0].Rows[0]["CondicionPago_1"].ToString()));
                lr.SetParameters(new ReportParameter("CondicionPago2", ds.Tables[0].Rows[0]["CondicionPago_2"].ToString()));
                lr.SetParameters(new ReportParameter("Banco", ds.Tables[0].Rows[0]["Banco"].ToString()));
                lr.SetParameters(new ReportParameter("Moneda", ds.Tables[0].Rows[0]["Moneda"].ToString()));
                lr.SetParameters(new ReportParameter("CuentaCorriente", ds.Tables[0].Rows[0]["CuentaCorriente"].ToString()));
                lr.SetParameters(new ReportParameter("CuentaAhorro", ds.Tables[0].Rows[0]["CuentaAhorro"].ToString()));
                lr.SetParameters(new ReportParameter("CCI", ds.Tables[0].Rows[0]["CCI"].ToString()));
                lr.SetParameters(new ReportParameter("Detracciones", ds.Tables[0].Rows[0]["Detracciones"].ToString()));
                lr.SetParameters(new ReportParameter("Usuario", NombreUsuario(cotizacion.IdUsuarioRegistro)));
                lr.SetParameters(new ReportParameter("Observaciones", cotizacion.Observaciones));

                lr.DataSources.Add(new ReportDataSource("CotizacionCabecera", ds.Tables[0]));
                lr.DataSources.Add(new ReportDataSource("Productos", ds.Tables[1]));
                lr.DataSources.Add(new ReportDataSource("Certificados", ds.Tables[2]));
                lr.DataSources.Add(new ReportDataSource("PlanInspeccion", ds.Tables[3]));
                lr.DataSources.Add(new ReportDataSource("Resumen", ds.Tables[4]));                
            }
            catch (Exception e)
            {
                RptPath = Server.MapPath("~/Content/Reportes/Vacio.rdlc");
                lr = new LocalReport
                {
                    ReportPath = RptPath,
                    EnableExternalImages = true,
                    EnableHyperlinks = true
                };
            }

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = lr.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            using (FileStream stream = System.IO.File.OpenWrite(filePath))
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            //CLOSE REPORT OBJECT           
            lr.Dispose();
            return File(filePath, "application/pdf");
        }

        [Authorize]
        // GET: Cotizacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cotizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdCotizacion,TipoCotizacion,NumeroCotizacion,Solicitante,RUC,Contacto,Email,Fecha,TipoDocumentoSolicitado,DescripcionProducto,CantidadMuestra,Telefono,SubTotal,IGV,Total,DiasEntrega,CorreoConfirmacion,CondicionPago_1,CondicionPago_2,Banco,Moneda,CuentaCorriente,CuentaAhorro,CCI,Observaciones,CondicionPago_De,Detracciones")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                db.Cotizacions.Add(cotizacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cotizacion);
        }

        // GET: Cotizacion/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ID = id;
            return View();
        }

        // POST: Cotizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdCotizacion,TipoCotizacion,NumeroCotizacion,Solicitante,RUC,Contacto,Email,Fecha,TipoDocumentoSolicitado,DescripcionProducto,CantidadMuestra,Telefono,SubTotal,IGV,Total,DiasEntrega,CorreoConfirmacion,CondicionPago_1,CondicionPago_2,Banco,Moneda,CuentaCorriente,CuentaAhorro,CCI,Observaciones,CondicionPago_De,Detracciones")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cotizacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cotizacion);
        }

        // GET: Cotizacion/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cotizacion cotizacion = db.Cotizacions.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            return View(cotizacion);
        }

        // POST: Cotizacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Cotizacion cotizacion = db.Cotizacions.Find(id);

            List<CotizacionCertificado> certificados = (from c in db.CotizacionCertificado.ToList()
                                                        where c.IdCotizacion == id
                                                        select c).ToList();
            foreach (CotizacionCertificado certificado in certificados)
            {
                db.CotizacionCertificado.Remove(certificado);
            }

            List<CotizacionProducto> productos = (from p in db.CotizacionProducto.ToList()
                                                  where p.IdCotizacion == id
                                                  select p).ToList();

            foreach (CotizacionProducto producto in productos)
            {
                db.CotizacionProducto.Remove(producto);
            }

            db.Cotizacions.Remove(cotizacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "ADMINISTRADOR")]
        #region Listar Producto
        public JsonResult listarProducto()
        {
            try
            {
                List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();
                List<MostrarProductoServicioViewModels> lista = MostrarProductoServicioViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        public JsonResult ObtenerCotizacion(int id)
        {
            try
            {
                Cotizacion cotizacion = db.Cotizacions.Find(id);
                if (cotizacion == null)
                    throw new Exception("No se encontró la cotización.");

                var modelo = ObtenerCotizacionViewModel(cotizacion, id);
                return Json(modelo);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        public JsonResult listarTipoCotizacion()
        {
            try
            {
                return Json(db.TipoCotizacion.OrderBy(x => x.ID));
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        public JsonResult RegistrarCotizacion(CotizacionViewModel cotizacionViewModel)
        {
            try
            {
                string idUsuario = (string)Session["IdUser"];
                Cotizacion cotizacionMod = db.Cotizacions.Find(cotizacionViewModel.Cotizacion.ID);
                Cotizacion cotizacion = cotizacionViewModel.Cotizacion;
                DateTime fechaRegistro = DateTime.Now;

                if (cotizacionMod != null)
                    fechaRegistro = cotizacionMod.FechaRegistro;

                string numeroCotizacion = "", numeroCotizacionBD = cotizacion.NumeroCotizacion, letra = "";

                if (string.IsNullOrEmpty(numeroCotizacionBD))
                {
                    List<Cotizacion> cotizaciones = (from c in db.Cotizacions.ToList()
                                                     where c.FechaRegistro.Date == fechaRegistro.Date && c.TipoCotizacion == cotizacion.TipoCotizacion && c.CorrelativoInicial == true
                                                     select c).ToList();

                    string formato = (cotizacion.TipoCotizacion == "AMB") ? "{0:yyyyMMdd}.{1}.DA" : "{0:yyyyMMdd}.{1}";
                    numeroCotizacion = string.Format(formato, fechaRegistro, (cotizaciones.Count + 1).ToString("00"));
                    cotizacionViewModel.Cotizacion.CorrelativoInicial = true;
                    cotizacionViewModel.Cotizacion.Correlativo = (cotizaciones.Count + 1).ToString("00");
                    cotizacionViewModel.Cotizacion.IdUsuarioRegistro = idUsuario;
                    cotizacionViewModel.Cotizacion.IdUsuarioModificacion = null;
                    cotizacionViewModel.Cotizacion.FechaModificacion = null;
                }
                else
                {
                    List<Cotizacion> cotizaciones = (from c in db.Cotizacions.ToList()
                                                     where c.FechaRegistro.Date == fechaRegistro.Date && c.TipoCotizacion == cotizacion.TipoCotizacion && c.CorrelativoInicial == false
                                                     && c.Correlativo == cotizacion.Correlativo
                                                     select c).ToList();

                    Util.ObtenerCorrelativoAlfabetico(cotizaciones.Count + 1, ref letra);

                    string formato = (cotizacion.TipoCotizacion == "AMB") ? "{0}{1}.DA" : "{0}{1}";

                    if (cotizacion.TipoCotizacion == "AMB")
                        numeroCotizacionBD = numeroCotizacionBD.Replace(".DA", "");

                    string ultimo = numeroCotizacionBD.Substring(numeroCotizacionBD.Length - 1);
                    numeroCotizacion = string.Format(formato, (Util.IsNumeric(ultimo)) ? numeroCotizacionBD : numeroCotizacionBD.Substring(0, numeroCotizacionBD.Length - letra.Length), letra);
                    cotizacionViewModel.Cotizacion.CorrelativoInicial = false;
                    cotizacionViewModel.Cotizacion.Correlativo = cotizacion.Correlativo;
                    cotizacionViewModel.Cotizacion.IdUsuarioRegistro = cotizacionMod.IdUsuarioRegistro;
                    cotizacionViewModel.Cotizacion.IdUsuarioModificacion = idUsuario;
                    cotizacionViewModel.Cotizacion.FechaModificacion = DateTime.Now;
                    cotizacionViewModel.Cotizacion.SerieNumero = "";
                }

                cotizacionViewModel.Cotizacion.NumeroCotizacion = numeroCotizacion;
                cotizacionViewModel.Cotizacion.FechaRegistro = fechaRegistro;

                db.Cotizacions.Add(cotizacionViewModel.Cotizacion);
                db.SaveChanges();

                long idCotizacion = cotizacionViewModel.Cotizacion.ID;

                if (cotizacionViewModel.Certificados != null)
                {
                    foreach (CotizacionCertificado certificado in cotizacionViewModel.Certificados)
                    {
                        certificado.IdCotizacion = idCotizacion;
                        db.CotizacionCertificado.Add(certificado);
                        db.SaveChanges();
                    }
                }

                foreach (CotizacionProducto producto in cotizacionViewModel.Productos)
                {
                    producto.IdCotizacion = idCotizacion;
                    db.CotizacionProducto.Add(producto);
                    db.SaveChanges();
                }

                if (cotizacionViewModel.Inspeccion != null)
                {
                    foreach (CotizacionInspeccion inspeccion in cotizacionViewModel.Inspeccion)
                    {
                        inspeccion.IdCotizacion = idCotizacion;
                        db.CotizacionInspeccion.Add(inspeccion);
                        db.SaveChanges();
                    }
                }

                if (cotizacionViewModel.Resumen != null)
                {
                    foreach (CotizacionResumen resumen in cotizacionViewModel.Resumen)
                    {
                        resumen.IdCotizacion = idCotizacion;
                        db.CotizacionResumen.Add(resumen);
                        db.SaveChanges();
                    }
                }

                var mensaje = "Se grabó correctamente la cotización.";
                var respuesta = new
                {
                    Mensaje = mensaje,
                    NumeroCotizacion = numeroCotizacion
                };
                return Json(respuesta);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GenerarComprobante(long id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                Cotizacion cotizacion = db.Cotizacions.Find(id);
                if (cotizacion == null)
                    throw new Exception("No se encontró la cotización.");

                List<Cliente> resultado = new ClienteNEG().listarCliente();
                List<MostrarClienteViewModel> lista = MostrarClienteViewModel.convert(resultado);
                MostrarClienteViewModel cliente = (from c in lista
                                                   where c.numeroDocumento == cotizacion.RUC
                                                   select c).FirstOrDefault();

                Service1Client client = new Service1Client();
                string serie = "";
                long numero = 0;
                comprobanteNEG.ListarSerieCorrelativo(ref serie, ref numero);

                En_Emisor emisor = new En_Emisor
                {
                    NumeroDocumentoIdentidad = "20602034675"
                };

                En_Receptor receptor = new En_Receptor
                {
                    CodigoDomicilioFiscal = "0002",
                    CodigoPais = "PE",
                    CodigoUbigeo = "150101",
                    Contacto = new En_Contacto
                    {
                        Correo = cliente.contactoCorreo,
                        Nombre = string.Format("{0} {1}", cliente.contactoNombre, cliente.contactoApellidos),
                        Telefono = cliente.contactoTelefono
                    },
                    Departamento = "LIMA",
                    Direccion = cliente.empresaDomicilio,
                    Distrito = "SAN JUAN DE LURIGANCHO",
                    NombreComercial = cliente.empresaNombre,
                    NumeroDocumentoIdentidad = cliente.empresaNumeroDocumento,
                    PaginaWeb = "",
                    Provincia = "LIMA",
                    RazonSocial = cliente.empresaNombre,
                    TipoDocumentoIdentidad = "6",
                    Urbanizacion = ""
                };

                En_ComprobanteDetalleImpuestos comprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos
                {
                    AfectacionIGV = "10",
                    CodigoInternacionalTributo = "1000",
                    CodigoTributo = "VAT",
                    MontoBase = cotizacion.SubTotal,
                    MontoTotalImpuesto = cotizacion.IGV,
                    NombreTributo = "IGV",
                    Porcentaje = 18.00M
                };

                En_ComprobanteDetalle detalle = new En_ComprobanteDetalle
                {
                    Cantidad = 1,
                    Codigo = "0000",
                    CodigoSunat = "",
                    CodigoTipoPrecio = "01",
                    Descripcion = cotizacion.DescripcionProducto,
                    ImpuestoTotal = cotizacion.IGV,
                    Item = 1,
                    Total = cotizacion.SubTotal,
                    UnidadMedida = "NIU",
                    ValorVentaUnitario = cotizacion.SubTotal,
                    ValorVentaUnitarioIncIgv = cotizacion.Total
                };

                detalle.MultiDescripcion = new string[1];
                detalle.MultiDescripcion.SetValue("COT." + cotizacion.NumeroCotizacion, 0);

                detalle.ComprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos[1];
                detalle.ComprobanteDetalleImpuestos.SetValue(comprobanteDetalleImpuestos, 0);

                En_MontosTotales montos = new En_MontosTotales
                {
                    Gravado = new En_Gravado
                    {
                        Total = cotizacion.IGV,
                        GravadoIGV = new En_GrabadoIGV
                        {
                            MontoBase = cotizacion.SubTotal,
                            MontoTotalImpuesto = cotizacion.IGV,
                            Porcentaje = 18
                        }
                    }
                };

                En_Leyenda leyenda = new En_Leyenda
                {
                    Codigo = "1000",
                    Valor = Util.Enletras(cotizacion.Total)
                };

                En_ComprobanteElectronico comprobante = new En_ComprobanteElectronico
                {
                    FechaEmision = DateTime.Now.ToString("yyyy-MM-dd"),
                    HoraEmision = DateTime.Now.ToString("HH:mm:ss"),
                    Moneda = "PEN",
                    ImporteTotal = cotizacion.Total,
                    Emisor = emisor,
                    Receptor = receptor,
                    MontoTotales = montos,
                    SerieNumero = string.Format("{0}-{1}", serie, numero),
                    TipoComprobante = "01",
                    TipoOperacion = "0101",
                    TotalImpuesto = cotizacion.IGV,
                    TotalPrecioVenta = cotizacion.Total,
                    TotalValorVenta = cotizacion.SubTotal
                };

                comprobante.Leyenda = new En_Leyenda[1];
                comprobante.Leyenda.SetValue(leyenda, 0);
                comprobante.ComprobanteDetalle = new En_ComprobanteDetalle[1];
                comprobante.ComprobanteDetalle.SetValue(detalle, 0);
                comprobante.TextoDetraccion = cotizacion.Detracciones;

                En_Respuesta respuesta = client.RegistroComprobante(comprobante);
                cotizacion.SerieNumero = comprobante.SerieNumero;

                if (respuesta.Codigo == "0")
                {
                    respuesta.Descripcion += " Comprobante: " + cotizacion.SerieNumero;
                    comprobanteNEG.ActualizarSerieCorrelativo(serie);
                    db.Set<Cotizacion>().AddOrUpdate(cotizacion);
                    db.SaveChanges();
                }

                return Json(respuesta);
                //return RedirectToAction("Details", new { id = id });
            }
            catch (Exception e)
            {
                En_Respuesta respuesta = new En_Respuesta
                {
                    Codigo = "99",
                    Descripcion = e.Message.ToString()
                };
                return Json(respuesta);
            }
        }
    }

    public class ActionDownloadAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string nombreArchivo = (string)filterContext.HttpContext.Session["NombreArchivo"];
            nombreArchivo = string.Format("{0}.pdf", nombreArchivo.Replace(' ', '_'));
            filterContext.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);

            base.OnResultExecuted(filterContext);
        }
    }
}

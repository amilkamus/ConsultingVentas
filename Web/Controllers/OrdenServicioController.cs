using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Web.Utilitario;
using Datos;
using Negocio;
using Web.Models.Producto;
using Web.Models;
using Web.Models.Cotizacion;
using Rotativa;
using Web.Models.OrdenServicio;
using System.Net;
using System.Data.Entity.Migrations;
using Microsoft.Reporting.WebForms;
using Web.Models.Clientes;
using System.IO;
using Entidad;
using NPOI.SS.UserModel;

namespace Web.Controllers
{
    public class OrdenServicioController : BaseController
    {
        private CotizacionContext db = new CotizacionContext();
        // GET: OrdenServicio
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public JsonResult ListarOrdenesServicio(EnOrdenServicioIn ordenServicioIn)
        {
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnOrdenServicioOut> lista = comprobanteNEG.ListarOrdenServicio(ordenServicioIn);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Exportar(EnOrdenServicioIn ordenServicioIn)
        {
            int filaError = 0;
            int columnaError = 0;
            string nombreColumnaError = "";
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnOrdenServicioOut> lista = comprobanteNEG.ListarOrdenServicio(ordenServicioIn);

                IWorkbook wb = new NPOI.XSSF.UserModel.XSSFWorkbook();

                ISheet sheet = wb.CreateSheet("OrdenServicio");
                IRow row;
                ICell cell;
                IFont font = wb.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;

                ICellStyle styleNegrita = wb.CreateCellStyle();
                styleNegrita.SetFont(font);

                ICellStyle styleNegritaBorde = wb.CreateCellStyle();
                styleNegritaBorde.SetFont(font);
                styleNegritaBorde.BorderBottom = BorderStyle.Thin;
                styleNegritaBorde.BorderTop = BorderStyle.Thin;
                styleNegritaBorde.BorderLeft = BorderStyle.Thin;
                styleNegritaBorde.BorderRight = BorderStyle.Thin;

                ICellStyle styleBorde = wb.CreateCellStyle();
                styleBorde.BorderBottom = BorderStyle.Thin;
                styleBorde.BorderTop = BorderStyle.Thin;
                styleBorde.BorderLeft = BorderStyle.Thin;
                styleBorde.BorderRight = BorderStyle.Thin;

                ICellStyle styleEntero = wb.CreateCellStyle();
                styleEntero.DataFormat = wb.CreateDataFormat().GetFormat("0");

                ICellStyle styleDecimal = wb.CreateCellStyle();
                styleDecimal.DataFormat = wb.CreateDataFormat().GetFormat("0.00");

                ICellStyle styleFecha = wb.CreateCellStyle();
                styleFecha.DataFormat = wb.CreateDataFormat().GetFormat("dd/mm/yyyy");

                ICellStyle styleFechaHora = wb.CreateCellStyle();
                styleFechaHora.DataFormat = wb.CreateDataFormat().GetFormat("dd/mm/yyyy HH:mm:ss");

                int rowIndex = 0;

                // cabcera del reporte
                int columna = 1;
                row = sheet.CreateRow(rowIndex);

                Dictionary<string, string> columnasCabecera = new Dictionary<string, string>();
                columnasCabecera.Add("NumeroOrdenServicio", "Número Orden Servicio");
                columnasCabecera.Add("NumeroCotizacion", "Número Cotización");
                columnasCabecera.Add("RUC", "RUC Solicitante");
                columnasCabecera.Add("Solicitante", "Razón Social Solicitante");
                columnasCabecera.Add("Fecha", "Fecha");
                columnasCabecera.Add("DescripcionProducto", "Descripción Producto");
                columnasCabecera.Add("Observaciones", "Observaciones Producto");
                columnasCabecera.Add("ObservacionesInforme", "Observaciones Informe");
                columnasCabecera.Add("UsuarioRegistro", "Usuario Registro");

                foreach (KeyValuePair<string, string> cabecera in columnasCabecera)
                {
                    cell = row.CreateCell(columna);
                    cell.SetCellValue(cabecera.Value);
                    cell.CellStyle = styleNegritaBorde;
                    columna += 1;
                }

                rowIndex += 1;
                string[] columnasNumericas = new[] { "" };
                string[] columnasFecha = new[] { "Fecha" };
                string[] columnasFechaHora = new[] { "" };
                filaError = rowIndex;
                foreach (var item in lista)
                {
                    columna = 1;
                    row = sheet.CreateRow(rowIndex);
                    foreach (KeyValuePair<string, string> cabecera in columnasCabecera)
                    {
                        string nombreColumna = cabecera.Key;
                        columnaError = columna;
                        nombreColumnaError = nombreColumna;
                        cell = row.CreateCell(columna);

                        if (columnasNumericas.Contains(nombreColumna))
                        {
                            cell.SetCellValue(Convert.ToDouble(item.GetType().GetProperty(cabecera.Key).GetValue(item, null)));
                            cell.CellStyle = styleDecimal;
                        }
                        else if (columnasFecha.Contains(nombreColumna))
                        {
                            cell.SetCellValue(item.GetType().GetProperty(cabecera.Key).GetValue(item, null).ToString());
                            cell.CellStyle = styleFecha;
                        }
                        else
                            cell.SetCellValue(item.GetType().GetProperty(cabecera.Key).GetValue(item, null).ToString());

                        cell.CellStyle = styleBorde;
                        columna += 1;
                    }
                    rowIndex += 1;
                }

                rowIndex += 1;

                byte[] xlsInBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);
                    xlsInBytes = ms.ToArray();
                }
                return File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteOrdenServicio.xlsx");
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Mensaje = "Ocurrió un error al procesar la información, Columna: " + nombreColumnaError.ToString() + ", Fila: " + filaError.ToString() + " error: " + e.Message.ToString()
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Edit(long id)
        {
            OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
            long idCotizacion = (from c in db.Cotizacions.ToList()
                                 where c.NumeroCotizacion == ordenServicio.NumeroCotizacion
                                 select c.ID).FirstOrDefault();

            ViewBag.IdNumeroOrdenServicio = id;
            ViewBag.IdCotizacion = idCotizacion;

            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Delete(long id)
        {
            OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
            if (ordenServicio == null)
            {
                return HttpNotFound();
            }
            return View(ordenServicio);
        }

        [Authorize]
        public ActionResult Details(long id)
        {
            OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
            if (ordenServicio == null)
            {
                return HttpNotFound();
            }

            Cotizacion cotizacion = (from c in db.Cotizacions
                                     where c.NumeroCotizacion == ordenServicio.NumeroCotizacion
                                     select c).FirstOrDefault();

            OrdenServicioViewModel modelo = ObtenerOrdenServicioViewModel(cotizacion, ordenServicio);

            ViewBag.IdNumeroOrdenServicio = id;
            ViewBag.IdCotizacion = cotizacion.ID;

            return View(modelo);
        }

        public ActionResult Print(long id)
        {
            OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
            if (ordenServicio == null)
            {
                return HttpNotFound();
            }

            Cotizacion cotizacion = (from c in db.Cotizacions
                                     where c.NumeroCotizacion == ordenServicio.NumeroCotizacion
                                     select c).FirstOrDefault();

            OrdenServicioViewModel modelo = ObtenerOrdenServicioViewModel(cotizacion, ordenServicio);

            return View(modelo);
        }

        [HttpGet]
        [ActionDownload]
        public FileResult PrintOrdenServicio(long id)
        {
            var ordenServicio = db.OrdenServicio.Find(id);
            var cotizacion = (from c in db.Cotizacions.ToList()
                              where c.NumeroCotizacion == ordenServicio.NumeroCotizacion
                              select c).FirstOrDefault();
            Session["NombreArchivo"] = ordenServicio.NumeroOrdenServicio + "-" + "OS-" + cotizacion.Solicitante.Trim().ToUpper();

            string RptPath = "";
            LocalReport lr = null;
            string filePath = System.IO.Path.GetTempFileName();

            try
            {
                if (cotizacion.TipoCotizacion == "AMB")
                {
                    RptPath = Server.MapPath("~/Content/Reportes/OrdenServicioAmbiental.rdlc");
                }
                else
                {
                    RptPath = Server.MapPath("~/Content/Reportes/OrdenServicioAlimentos.rdlc");
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

                lr.SetParameters(new ReportParameter("OrdenServicio", ordenServicio.NumeroOrdenServicio));
                lr.SetParameters(new ReportParameter("NumeroCotizacion", ordenServicio.NumeroCotizacion));

                lr.SetParameters(new ReportParameter("RazonSocial", cotizacion.Solicitante));
                lr.SetParameters(new ReportParameter("RUC", cotizacion.RUC));
                lr.SetParameters(new ReportParameter("Direccion", cliente.empresaDomicilio));
                lr.SetParameters(new ReportParameter("Contacto", cotizacion.Contacto));
                lr.SetParameters(new ReportParameter("Email", cotizacion.Email));
                lr.SetParameters(new ReportParameter("Telefono", cotizacion.Telefono));
                lr.SetParameters(new ReportParameter("NombreProducto", cotizacion.DescripcionProducto));

                lr.SetParameters(new ReportParameter("NumeroCopias", ordenServicio.NumeroCopiasInforme.ToString()));
                lr.SetParameters(new ReportParameter("OtrosInformes", ordenServicio.ObservacionesInforme));

                lr.SetParameters(new ReportParameter("DireccionEnvio", ordenServicio.DireccionEnvioMateriales));
                lr.SetParameters(new ReportParameter("FechaEnvioMateriales", ordenServicio.FechaEnvioMateriales));
                lr.SetParameters(new ReportParameter("ContactoMateriales", ordenServicio.ContactoMateriales));

                lr.SetParameters(new ReportParameter("ContactoInspeccion", ordenServicio.ContactoInspeccion));
                lr.SetParameters(new ReportParameter("EmailInspeccion", ordenServicio.EmailInspeccion));
                lr.SetParameters(new ReportParameter("TelefonoInspeccion", ordenServicio.TelefonoInspeccion));
                lr.SetParameters(new ReportParameter("CoordinadorInspeccion", ordenServicio.CoordinadorInspeccion));
                lr.SetParameters(new ReportParameter("LugarInspeccion", ordenServicio.LugarInspeccion));
                lr.SetParameters(new ReportParameter("FechaInspeccion", ordenServicio.FechaInspeccion));
                lr.SetParameters(new ReportParameter("HoraInspeccion", ordenServicio.HoraInspeccion));
                lr.SetParameters(new ReportParameter("TipoServicioInspeccion", ordenServicio.TipoServicioInspeccion));
                lr.SetParameters(new ReportParameter("NombreProductoInspeccion", ordenServicio.NombreProductoInspeccion));
                lr.SetParameters(new ReportParameter("PresentacionInspeccion", ordenServicio.PresentacionInspeccion));
                lr.SetParameters(new ReportParameter("CantidadLoteInspeccion", ordenServicio.CantidadLoteInspeccion));
                lr.SetParameters(new ReportParameter("CodigosLoteInspeccion", ordenServicio.CodigosLoteInspeccion));
                //lr.SetParameters(new ReportParameter("OtrosInspeccion", ordenServicio.ObservacionesInspeccion));
                lr.SetParameters(new ReportParameter("OtrosInspeccion", cotizacion.Observaciones));

                lr.SetParameters(new ReportParameter("Usuario", NombreUsuario(ordenServicio.IdUsuarioRegistro)));
                lr.SetParameters(new ReportParameter("FechaRegistro", ordenServicio.FechaRegistro.ToString("dd/MM/yyyy")));
                lr.SetParameters(new ReportParameter("EmisionDigital", (cotizacion.EmisionDigital) ? "Si" : "No"));

                lr.SetParameters(new ReportParameter("TipoDocumento", cotizacion.TipoDocumentoSolicitado));
                lr.SetParameters(new ReportParameter("Observaciones", cotizacion.Observaciones));
                
                lr.DataSources.Add(new ReportDataSource("CotizacionCabecera", ds.Tables[0]));
                lr.DataSources.Add(new ReportDataSource("Productos", ds.Tables[1]));
                lr.DataSources.Add(new ReportDataSource("Certificados", ds.Tables[2]));
                lr.DataSources.Add(new ReportDataSource("PlanInspeccion", ds.Tables[3]));
                lr.DataSources.Add(new ReportDataSource("Resumen", ds.Tables[4]));
            }
            catch (Exception)
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

        private OrdenServicioViewModel ObtenerOrdenServicioViewModel(Cotizacion cotizacion, OrdenServicio ordenServicio)
        {
            ClienteNEG clienteNEG = new ClienteNEG();
            WH_ProductoServicioNEG productoServicioNEG = new WH_ProductoServicioNEG();
            ParametroContext dbParam = new ParametroContext();
            TipoParametroContext dbTipo = new TipoParametroContext();
            List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();
            List<MostrarProductoServicioViewModels> productos = MostrarProductoServicioViewModels.convert(resultado);
            OrdenServicioViewModel modelo = new OrdenServicioViewModel();
            List<Cliente> clientes = clienteNEG.listarCliente();
            List<MostrarClienteViewModel> listaClientes = MostrarClienteViewModel.convert(clientes);

            modelo.Cliente = (from c in listaClientes
                              where c.empresaNumeroDocumento == cotizacion.RUC
                              select c).FirstOrDefault();
            modelo.Cotizacion = cotizacion;
            modelo.OrdenServicio = ordenServicio;
            modelo.Certificados = (from c in db.CotizacionCertificado.ToList()
                                   where c.IdCotizacion == cotizacion.ID
                                   select c).ToList();
            modelo.Detalles = (from cp in db.CotizacionProducto.ToList()
                               join prd in productos on cp.IdProducto equals prd.idProducto
                               join p in dbParam.Parametroes on cp.IdParametro equals p.ID
                               join t in dbTipo.TipoParametroes on cp.IdTipoParametro equals t.ID
                               where cp.IdCotizacion == cotizacion.ID
                               select new DetalleCotizacionViewModel { producto = prd, parametro = p, tipoParametro = t, productoCotizacion = cp }).ToList();
            modelo.Inspeccion = (from i in db.CotizacionInspeccion.ToList()
                                 where i.IdCotizacion == cotizacion.ID
                                 select i).ToList();
            modelo.Resumen = (from i in db.CotizacionResumen.ToList()
                              where i.IdCotizacion == cotizacion.ID
                              select i).ToList();
            modelo.NombreUsuario = NombreUsuario(ordenServicio.IdUsuarioRegistro);
            return modelo;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
            db.OrdenServicio.Remove(ordenServicio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public JsonResult ObtenerOrdenServicio(int id)
        {
            try
            {
                OrdenServicio ordenServicio = db.OrdenServicio.Find(id);
                if (ordenServicio == null)
                    throw new Exception("No se encontró la orden de servicio.");

                return Json(ordenServicio);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public JsonResult listarCotizaciones()
        {
            try
            {
                //original
                //IOrderedQueryable<Cotizacion> cotizaciones = db.Cotizacions.OrderBy(x => x.TipoCotizacion).ThenBy(x => x.NumeroCotizacion).ThenBy(x => x.Correlativo).ThenByDescending(x => x.CorrelativoInicial);
                //return Json(cotizaciones);

                //2da opcion seteando el MaxValue
                //IOrderedQueryable<Cotizacion> cotizaciones = db.Cotizacions.OrderBy(x => x.TipoCotizacion).ThenBy(x => x.NumeroCotizacion).ThenBy(x => x.Correlativo).ThenByDescending(x => x.CorrelativoInicial);
                //var jsonResult = Json(cotizaciones, JsonRequestBehavior.AllowGet);
                //jsonResult.MaxJsonLength = int.MaxValue;
                //return jsonResult;

                var cotizaciones = db.Cotizacions.Select(p => new
                {
                    p.ID,
                    p.TipoCotizacion,
                    p.NumeroCotizacion,
                    p.SubTotal,
                    p.IGV,
                    p.Total
                }).ToList();

                return Json(cotizaciones);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public JsonResult RegistrarOrdenServicio(OrdenServicio ordenServicio)
        {
            try
            {
                string numeroOrdenServicio = "";
                string idUsuario = (string)Session["IdUser"];

                if (string.IsNullOrEmpty(ordenServicio.NumeroOrdenServicio) || string.IsNullOrWhiteSpace(ordenServicio.NumeroOrdenServicio))
                {
                    DateTime fechaRegistro = DateTime.Now;

                    DateTime dt;
                    try//15.07.2020 - 13L
                    {
                        DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(fechaRegistro.ToString()), DateTimeKind.Utc);

                        var kind = convertedDate.Kind; // will equal DateTimeKind.Utc
                        dt = convertedDate.ToLocalTime().AddHours(-9);
                        fechaRegistro = dt;
                    }
                    catch
                    {
                        string a = "";
                        fechaRegistro = DateTime.Now;
                    }


                    int correlativo = (from o in db.OrdenServicio.ToList()
                                       where o.FechaRegistro.Date == fechaRegistro.Date
                                       select o.Correlativo).LastOrDefault();
                    correlativo++;

                    if (ordenServicio.NumeroCotizacion.EndsWith(".DA"))
                        numeroOrdenServicio = String.Format("OS {0:yyMMdd}.{1}.DA", fechaRegistro, correlativo.ToString("00"));
                    else
                        numeroOrdenServicio = String.Format("OS {0:yyMMdd}.{1}", fechaRegistro, correlativo.ToString("00"));

                    ordenServicio.IdUsuarioRegistro = idUsuario;
                    ordenServicio.FechaRegistro = fechaRegistro;
                    ordenServicio.Correlativo = correlativo;
                    ordenServicio.NumeroOrdenServicio = numeroOrdenServicio;
                    db.OrdenServicio.Add(ordenServicio);
                    db.SaveChanges();
                }
                else
                {
                    OrdenServicio ordenServicioBD = db.OrdenServicio.Find(ordenServicio.ID);
                    numeroOrdenServicio = ordenServicio.NumeroOrdenServicio;

                    ordenServicio.IdUsuarioRegistro = ordenServicioBD.IdUsuarioRegistro;
                    ordenServicio.FechaRegistro = ordenServicioBD.FechaRegistro;
                    ordenServicio.Correlativo = ordenServicioBD.Correlativo;

                    ordenServicio.IdUsuarioModificacion = idUsuario;
                    ordenServicio.FechaModificacion = DateTime.Now;

                    //db.Entry<OrdenServicio>(ordenServicio).State = EntityState.Modified;
                    db.Set<OrdenServicio>().AddOrUpdate(ordenServicio);
                    db.SaveChanges();
                }

                var mensaje = "Se grabó correctamente la orden de servicio.";
                var respuesta = new
                {
                    Mensaje = mensaje,
                    NumeroOrdenServicio = numeroOrdenServicio
                };
                return Json(respuesta);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
    }
}
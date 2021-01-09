using System;
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
using Web.Service.RC.FactElect;
using Web.Models.Clientes;
using System.Data.Entity.Migrations;
using Microsoft.Reporting.WebForms;
using System.IO;
using Web.Models.OrdenServicio;
using Web.Models.Parametro;
using Entidad;
using NPOI.SS.UserModel;

namespace Web.Controllers
{
    public class CotizacionController : BaseController
    {
        private CotizacionContext db = new CotizacionContext();
        private ParametroContext dbParam = new ParametroContext();
        private TipoParametroContext dbTipo = new TipoParametroContext();
        WH_ProductoServicioNEG productoServicioNEG = new WH_ProductoServicioNEG();

        // GET: Cotizacion
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public JsonResult ListarCotizaciones(EnContizacionIn cotizacionIn)
        {
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnCotizacionOut> lista = comprobanteNEG.ListarCotizaciones(cotizacionIn);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public JsonResult ListarUsuarios()
        {
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnUsuario> lista = comprobanteNEG.ListarUsuarios();
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Exportar(EnContizacionIn cotizacionIn)
        {
            int filaError = 0;
            int columnaError = 0;
            string nombreColumnaError = "";
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnCotizacionOut> lista = comprobanteNEG.ListarCotizaciones(cotizacionIn);

                IWorkbook wb = new NPOI.XSSF.UserModel.XSSFWorkbook();

                ISheet sheet = wb.CreateSheet("Cotizacion");
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
                columnasCabecera.Add("TipoCotizacion", "Tipo Cotización");
                columnasCabecera.Add("NumeroCotizacion", "Número Cotización");
                columnasCabecera.Add("SerieNumero", "Serie y Número");
                columnasCabecera.Add("RUC", "RUC Solicitante");
                columnasCabecera.Add("Solicitante", "Razón Social Solicitante");
                columnasCabecera.Add("Fecha", "Fecha");
                columnasCabecera.Add("DescripcionProducto", "Descripción Producto");
                columnasCabecera.Add("Observaciones", "Observaciones Producto");
                columnasCabecera.Add("Contacto", "Contacto");
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
                return File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCotizacion.xlsx");
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

        [Authorize]
        public ActionResult Payment(long id)
        {
            /*
            Cotizacion cotizacion = db.Cotizacions.Find(id);
            if (cotizacion == null)
                return HttpNotFound();
            */
            ViewBag.ID = id;
            //var modelo = ObtenerCotizacionViewModel(cotizacion, id);
            return View(); // modelo);
        }

        public ActionResult Print(long id)
        {
            Cotizacion cotizacion = db.Cotizacions.Find(id);
            if (cotizacion == null)
                return HttpNotFound();

            var modelo = ObtenerCotizacionViewModel(cotizacion, id);
            return View(modelo);
        }

        private CotizacionViewModelOut ObtenerCotizacionViewModel(Cotizacion cotizacion, long id)
        {
            CotizacionViewModelOut modelo = new CotizacionViewModelOut();

            ClienteNEG clienteNEG = new ClienteNEG();
            List<Cliente> clientes = clienteNEG.listarCliente();
            List<MostrarClienteViewModel> listaClientes = MostrarClienteViewModel.convert(clientes);
            List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();
            List<MostrarProductoServicioViewModels> productos = MostrarProductoServicioViewModels.convert(resultado);


            modelo.Cliente = (from c in listaClientes
                              where c.empresaNumeroDocumento == cotizacion.RUC
                              select c).FirstOrDefault();
            modelo.Cotizacion = cotizacion;

            CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();

            modelo.Certificados = comprobanteNEG.ObtenerCertificadosCotizacion(id);

            modelo.Detalles = (from cp in db.CotizacionProducto.ToList()
                               join prd in productos on cp.IdProducto equals prd.idProducto
                               join p in dbParam.Parametroes on cp.IdParametro equals p.ID
                               join t in dbTipo.TipoParametroes on cp.IdTipoParametro equals t.ID
                               where cp.IdCotizacion == id
                               select new DetalleCotizacionViewModel { producto = prd, parametro = p, tipoParametro = t, productoCotizacion = cp, Precio = cp.Precio }).ToList();
            modelo.Inspeccion = comprobanteNEG.ObtenerInspeccionesCotizacion(id);
            modelo.Resumen = comprobanteNEG.ObtenerResumenesCotizacion(id);
            modelo.NombreUsuario = NombreUsuario(cotizacion.IdUsuarioRegistro);
            modelo.Cobranza = comprobanteNEG.ListarCobranzasPorCotizacion(id);
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

                if (cliente.empresaDomicilio == null)//16.07.2020
                {
                    lr.SetParameters(new ReportParameter("Direccion", cliente.titularDomicilio));
                }
                else
                {
                    lr.SetParameters(new ReportParameter("Direccion", cliente.empresaDomicilio));
                }

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
                lr.SetParameters(new ReportParameter("EmisionDigital", (cotizacion.EmisionDigital) ? "Si" : "No"));

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

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
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
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
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
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
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

                CotizacionViewModelOut modelo = ObtenerCotizacionViewModel(cotizacion, id);
                return Json(modelo);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        public JsonResult ObtenerParametrosProducto(long id)
        {
            try
            {
                CotizacionViewModel modelo = new CotizacionViewModel();
                List<WH_ProductoServicio> resultado = productoServicioNEG.listarProducto();
                List<MostrarProductoServicioViewModels> productos = MostrarProductoServicioViewModels.convert(resultado);

                DataTable tblParametros = productoServicioNEG.ListarParametrosProducto(id);
                List<ParametroViewModel> parametros = new List<ParametroViewModel>();
                if (tblParametros != null)
                {
                    foreach (DataRow fila in tblParametros.Rows)
                    {
                        ParametroViewModel parametro = new ParametroViewModel();

                        parametro.ID = long.Parse(fila["idParametro"].ToString());
                        parametro.CodParametro = long.Parse(fila["CodParametro"].ToString());
                        parametro.ParametroDescripcion = fila["ParametroDescripcion"].ToString();
                        parametro.Metodologia = fila["Metodologia"].ToString();
                        parametro.Precio = decimal.Parse(fila["Precio"].ToString());
                        string cadena = fila["Activo"].ToString();
                        parametro.Activo = (cadena == "0") ? false : true;
                        parametro.IdTipoParametro = long.Parse(fila["idTipoParametro"].ToString());
                        parametro.IdProducto = long.Parse(fila["idProducto"].ToString());

                        parametros.Add(parametro);
                    }
                }

                modelo.Detalles = (from cp in parametros
                                   join prd in productos on cp.IdProducto equals prd.idProducto
                                   join p in dbParam.Parametroes on cp.ID equals p.ID
                                   join t in dbTipo.TipoParametroes on cp.IdTipoParametro equals t.ID
                                   where cp.Activo == true
                                   select new DetalleCotizacionViewModel { producto = prd, parametro = p, tipoParametro = t }).ToList();

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

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
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

                DateTime dt;
                if (cotizacionMod == null)
                {
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
                }


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

                if (!string.IsNullOrEmpty(numeroCotizacionBD))
                {
                    OrdenServicio ordenServicio = (from o in db.OrdenServicio.ToList()
                                                   where o.NumeroCotizacion == cotizacionMod.NumeroCotizacion
                                                   select o).FirstOrDefault();

                    if (ordenServicio != null)
                    {
                        ordenServicio.IdUsuarioModificacion = idUsuario;
                        ordenServicio.FechaModificacion = DateTime.Now;
                        ordenServicio.NumeroCotizacion = numeroCotizacion;
                        db.Set<OrdenServicio>().AddOrUpdate(ordenServicio);
                        db.SaveChanges();
                    }
                }

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
                comprobanteNEG.ListarSerieCorrelativo("FACTURA", ref serie, ref numero);

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
                    //Departamento = "LIMA",
                    Direccion = cliente.empresaDomicilio,
                    //Distrito = "SAN JUAN DE LURIGANCHO",
                    NombreComercial = cliente.empresaNombre,
                    NumeroDocumentoIdentidad = cliente.empresaNumeroDocumento,
                    PaginaWeb = "",
                    //Provincia = "LIMA",
                    RazonSocial = cliente.empresaNombre,
                    TipoDocumentoIdentidad = "6",
                    //Urbanizacion = ""
                };

                En_ComprobanteDetalleImpuestos comprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos
                {
                    AfectacionIGV = "10",
                    CodigoInternacionalTributo = "1000",
                    CodigoTributo = "VAT",
                    MontoBase = cotizacion.SubTotalFinal,
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
                    Descripcion = cotizacion.TipoDocumentoSolicitado + " - " + cotizacion.DescripcionProducto + " - " + "COT." + cotizacion.NumeroCotizacion, //Descripcion = cotizacion.DescripcionProducto, 
                    ImpuestoTotal = cotizacion.IGV,
                    Item = 1,
                    Total = cotizacion.SubTotalFinal,
                    UnidadMedida = "NIU",
                    ValorVentaUnitario = cotizacion.SubTotalFinal,
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
                        Total = cotizacion.SubTotalFinal,
                        GravadoIGV = new En_GrabadoIGV
                        {
                            MontoBase = cotizacion.SubTotalFinal,
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


                DateTime dt;
                DateTime fechaRegistro = DateTime.Now;

                try//19.08.2020 - 13L
                {
                    DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(DateTime.Now.ToString()), DateTimeKind.Utc);

                    var kind = convertedDate.Kind;
                    dt = convertedDate.ToLocalTime().AddHours(-9);
                    fechaRegistro = dt;
                }
                catch
                {
                    fechaRegistro = DateTime.Now;
                }

                En_ComprobanteElectronico comprobante = new En_ComprobanteElectronico
                {
                    FechaEmision = fechaRegistro.ToString("yyyy-MM-dd"), //DateTime.Now.ToString("yyyy-MM-dd"),
                    HoraEmision = fechaRegistro.ToString("HH:mm:ss"), //DateTime.Now.ToString("HH:mm:ss"),
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
                    TotalValorVenta = cotizacion.SubTotalFinal
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
                    comprobanteNEG.ActualizarSerieCorrelativo(serie, "FACTURA");
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

        public JsonResult RegistrarCobranza(EnCobranza cobranza)
        {
            try
            {
                string idUsuario = (string)Session["IdUser"];

                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                cobranza.IdUsuario = idUsuario;
                comprobanteNEG.RegistrarCobranza(cobranza);

                var mensaje = "Se grabó correctamente la cobranza.";
                var respuesta = new
                {
                    Codigo = "0",
                    Mensaje = mensaje
                };
                return Json(respuesta);
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Mensaje = e.Message.ToString()
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Ventas;
using Web.Utilitario;
using Datos;
using Negocio;
using Web.Models;
using Web.Models.Clientes;
using Web.Models.Producto;
using Web.Models.CambioMonedas;
using Web.Models.CorrelativoMaster;
using Web.Models.VentaDetalles;
using Microsoft.Reporting.WebForms;
using System.IO;
using Web.Models.IGVMaster;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;
using Microsoft.AspNet.Identity;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Web.Service.RC.FactElect;
using System.Data;

namespace Web.Controllers
{
    public class VentaController : BaseController
    {
        CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
        CO_TipoComprobanteNEG tipoComprobanteNEG = new CO_TipoComprobanteNEG();
        ClienteNEG clienteNEG = new ClienteNEG();
        WH_ProductoServicioNEG productoServicioNEG = new WH_ProductoServicioNEG();
        CO_MonedaNEG monedaNEG = new CO_MonedaNEG();
        CO_CorrelativoMastNEG correlativoMasterNEG = new CO_CorrelativoMastNEG();
        CO_ComprobanteDetalleNEG comprobanteDetalleNEG = new CO_ComprobanteDetalleNEG();
        CambioMonedaNEG cambioMonedaNEG = new CambioMonedaNEG();
        IGVMastNEG igvMastNEG = new IGVMastNEG();
        Conversiones conversion = new Conversiones();
        Attachment adjunto;

        public int temporal = 1;

        // GET: Venta
        #region Agregar Venta
        public ActionResult AddVenta()
        {
            return View();
        }

        public JsonResult GenerarComprobanteElectronico(long id)
        {
            long IdComprobante = id;

            // Generar factura electrónica
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();

                DataSet ds = new CO_ComprobanteNEG().CargarVenta(IdComprobante);

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
                        Correo = ds.Tables[0].Rows[0]["Correo"].ToString(),
                        Nombre = ds.Tables[0].Rows[0]["Nombre"].ToString(),
                        Telefono = ds.Tables[0].Rows[0]["Telefono"].ToString()
                    },
                    //Departamento = "LIMA",
                    Direccion = ds.Tables[0].Rows[0]["Direccion"].ToString(),
                    //Distrito = "SAN JUAN DE LURIGANCHO",
                    NombreComercial = ds.Tables[0].Rows[0]["NombreComercial"].ToString(),
                    NumeroDocumentoIdentidad = ds.Tables[0].Rows[0]["NumeroDocumentoIdentidad"].ToString(),
                    PaginaWeb = "",
                    //Provincia = "LIMA",
                    RazonSocial = ds.Tables[0].Columns["RazonSocial"].ToString(),
                    TipoDocumentoIdentidad = "6",
                    //Urbanizacion = ""
                };

                En_ComprobanteElectronico comprobante = new En_ComprobanteElectronico();

                string serieCorrelativo = ds.Tables[0].Rows[0]["SerieCorrelativo"].ToString();
                decimal total = decimal.Parse(ds.Tables[0].Rows[0]["Total"].ToString());
                decimal subTotal = decimal.Parse(ds.Tables[0].Rows[0]["SubTotal"].ToString());

                // Inicio de los detalles
                comprobante.ComprobanteDetalle = new En_ComprobanteDetalle[ds.Tables[1].Rows.Count];
                int indice = 0;
                decimal impuestoTotal = 0;
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    int cantidad = int.Parse(item["Cantidad"].ToString());
                    decimal precio = decimal.Parse(item["Precio"].ToString());
                    string nombreProducto = item["Descripcion"].ToString();
                    decimal valorVentaUnitario = cantidad * precio;
                    decimal impuestoTotalItem = valorVentaUnitario * 0.18M;
                    impuestoTotal += impuestoTotalItem;

                    En_ComprobanteDetalleImpuestos comprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos
                    {
                        AfectacionIGV = "10",
                        CodigoInternacionalTributo = "1000",
                        CodigoTributo = "VAT",
                        MontoBase = valorVentaUnitario,
                        MontoTotalImpuesto = Math.Round(impuestoTotalItem, 2),
                        NombreTributo = "IGV",
                        Porcentaje = 18.00M
                    };

                    En_ComprobanteDetalle detalle = new En_ComprobanteDetalle
                    {
                        Cantidad = cantidad,
                        Codigo = "0000",
                        CodigoSunat = "",
                        CodigoTipoPrecio = "01",
                        Descripcion = nombreProducto,
                        ImpuestoTotal = Math.Round(impuestoTotalItem, 2),
                        Item = (indice + 1),
                        Total = valorVentaUnitario,
                        UnidadMedida = "NIU",
                        ValorVentaUnitario = valorVentaUnitario / cantidad,
                        ValorVentaUnitarioIncIgv = (valorVentaUnitario + impuestoTotalItem) / cantidad
                    };
                    detalle.MultiDescripcion = new string[1];
                    detalle.MultiDescripcion.SetValue("-", 0);

                    detalle.ComprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos[1];
                    detalle.ComprobanteDetalleImpuestos.SetValue(comprobanteDetalleImpuestos, 0);

                    comprobante.ComprobanteDetalle.SetValue(detalle, indice);
                    indice++;
                }
                // Fin de los detalles

                En_Leyenda leyenda = new En_Leyenda
                {
                    Codigo = "1000",
                    Valor = Util.Enletras(total)
                };

                En_MontosTotales montos = new En_MontosTotales
                {
                    Gravado = new En_Gravado
                    {
                        Total = Math.Round(subTotal, 2),
                        GravadoIGV = new En_GrabadoIGV
                        {
                            MontoBase = subTotal,
                            MontoTotalImpuesto = Math.Round(impuestoTotal, 2),
                            Porcentaje = 18.00M
                        }
                    }
                };

                DateTime dt;
                DateTime fechaRegistro = DateTime.Now;

                try//23.08.2020 - 13L
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

                comprobante.FechaEmision = fechaRegistro.ToString("yyyy-MM-dd");   //DateTime.Now.ToString("yyyy-MM-dd");
                comprobante.HoraEmision = fechaRegistro.ToString("HH:mm:ss");      //DateTime.Now.ToString("HH:mm:ss");
                comprobante.Moneda = "PEN";
                comprobante.ImporteTotal = total;
                comprobante.Emisor = emisor;
                comprobante.Receptor = receptor;
                comprobante.MontoTotales = montos;
                comprobante.SerieNumero = serieCorrelativo;
                comprobante.TipoComprobante = "01";
                comprobante.TipoOperacion = "0101";
                comprobante.TotalImpuesto = Math.Round(impuestoTotal, 2);
                comprobante.TotalPrecioVenta = total;
                comprobante.TotalValorVenta = Math.Round(subTotal, 2);
                comprobante.Leyenda = new En_Leyenda[1];
                comprobante.Leyenda.SetValue(leyenda, 0);

                En_Respuesta respuesta = client.RegistroComprobante(comprobante);

                if (respuesta != null && respuesta.Codigo == "0")
                {
                    return Json(new { code_result = respuesta.Codigo, data = respuesta.Descripcion, result_description = respuesta.Descripcion }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception((respuesta != null) ? respuesta.Descripcion : "La entidad respuesta está vacía.");
                }
            }
            catch (Exception e)
            {
                return Json(new { code_result = 99, data = "", result_description = e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult guardarComprobante(VentaViewModels ventaViewModels, List<VentaDetalleViewModels> ventaDetalleViewModels, List<ProductoServicioViewModels> productoViewModels)
        {
            long IdComprobante = 0;
            var tipoComprobante = comprobanteNEG.DevolverTipoComprobante(ventaViewModels.idCorrelativo);
            string serie = "";
            long numero = 0;
            comprobanteNEG.ListarSerieCorrelativo(tipoComprobante, ref serie, ref numero);

            try
            {
                OperationResult resultado = null;
                if (ventaViewModels.idComprobante != 0)
                {
                    ventaViewModels.textoTotal = conversion.enletras(Convert.ToString(ventaViewModels.total));
                    ventaViewModels.serieCorrelativo = string.Format("{0}-{1}", serie, numero); //(comprobanteNEG.CodigoComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo))).ToString();
                    resultado = comprobanteNEG.actualizarComprobante(ventaViewModels.idComprobante, ventaViewModels.idCorrelativo, ventaViewModels.serieCorrelativo, ventaViewModels.descripcion, ventaViewModels.estado, ventaViewModels.subtotal, ventaViewModels.total, ventaViewModels.textoTotal, ventaViewModels.fechaRegistro, ventaViewModels.usuarioRegistro, IdUsuario(), ventaViewModels.idUsuario, ventaViewModels.idCliente, ventaViewModels.idMoneda, 1);
                }
                else
                {
                    ventaViewModels.textoTotal = conversion.enletras(Convert.ToString(ventaViewModels.total));
                    ventaViewModels.serieCorrelativo = string.Format("{0}-{1}", serie, numero); // comprobanteNEG.DevolverSerieComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo)) + "-" + (comprobanteNEG.CodigoComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo))).ToString();
                    resultado = comprobanteNEG.guardarComprobante(ventaViewModels.idCorrelativo, ventaViewModels.serieCorrelativo, ventaViewModels.descripcion, "ACTIVO", ventaViewModels.subtotal, ventaViewModels.total, ventaViewModels.textoTotal, IdUsuario(), IdUsuario(), ventaViewModels.idCliente, ventaViewModels.idMoneda, 1);
                    IdComprobante = resultado.data;

                    for (int i = 0; i < ventaDetalleViewModels.Count; i++)
                    {
                        ventaDetalleViewModels[i].montoUnitario = ventaDetalleViewModels[i].cantidad * ventaDetalleViewModels[i].precio;
                        resultado = comprobanteDetalleNEG.guardarComprobanteDetalle(ventaDetalleViewModels[i].idProducto, Convert.ToInt32(IdComprobante), ventaDetalleViewModels[i].cantidad, ventaDetalleViewModels[i].precio, ventaDetalleViewModels[i].montoUnitario);
                        for (int j = 0; j < productoViewModels.Count; j++)
                        {
                            if (productoViewModels[j].idProducto == ventaDetalleViewModels[i].idProducto)
                            {
                                productoViewModels[j].stock = (productoViewModels[j].stock - ventaDetalleViewModels[i].cantidad);
                                productoServicioNEG.actualizarProducto(productoViewModels[j].idProducto, productoViewModels[j].idTipoProductoServicio, productoViewModels[j].codigo, productoViewModels[j].nombre, productoViewModels[j].descripcion, productoViewModels[j].stock, productoViewModels[j].estado, productoViewModels[j].costo, productoViewModels[j].precio, productoViewModels[j].fechaRegistro, productoViewModels[j].usuarioRegistro, productoViewModels[j].usuarioActualizacion);
                            }
                        }
                    }
                }
                //imprimirVentas(Convert.ToString(IdComprobante));
                temporal = 1;
                Util.verificarError(resultado);
                comprobanteNEG.ActualizarSerieCorrelativo(serie, tipoComprobante);
                return Json(new { code_result = resultado.code_result, data = resultado.data, result_description = resultado.title }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string FecActual = DateTime.Today.ToString("MM/dd/yy");
                string ruta = Server.MapPath("~/Reportes/" + "Log.txt");

                System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta, true);
                string texto;
                texto = " MESSAGE: " + e.Message.ToString() + " SOURCE: " + e.Source + " STACKTRACE: " + e.StackTrace + "totGC:" + e.ToString();// + " pdfAbrir:" + pdfAbrir;
                sw.WriteLine(texto);
                sw.Close();

                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR")]
        #region Listar Venta
        public ActionResult ViewAllVentas()
        {
            return View();
        }

        public JsonResult listarComprobante()
        {
            try
            {
                List<CO_Comprobante> resultado = comprobanteNEG.listarComprobante();

                List<MostrarVentaViewModels> lista = MostrarVentaViewModels.convert(resultado.OrderByDescending(z => z.serieCorrelativo).ToList());

                return Json(lista.OrderByDescending(x => x.serieCorrelativo));
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Listar Cliente
        public JsonResult listarCliente()
        {
            try
            {
                List<Cliente> resultado = clienteNEG.listarCliente();

                List<MostrarClienteViewModel> lista = MostrarClienteViewModel.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
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

        [Authorize(Roles = "ADMINISTRADOR")]
        #region Listar Cambio de Moneda
        public JsonResult listarCambioMoneda()
        {
            try
            {
                List<CambioMoneda> resultado = cambioMonedaNEG.listarCambioMoneda();

                List<MostrarCambioMonedaViewModels> lista = MostrarCambioMonedaViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR")]
        #region Listar Correlativos
        public JsonResult listarCorrelativoMast()
        {
            try
            {
                List<CorrelativoMast> resultado = correlativoMasterNEG.listarCorrelativoMast();

                List<CorrelativoMastViewModels> lista = CorrelativoMastViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        #region Listar Detalle de Comprobante
        public JsonResult listarComprobanteDetalle(int idComprobante)
        {
            try
            {
                List<CO_ComprobanteDetalle> resultado = comprobanteDetalleNEG.listarComprobanteDetalle(idComprobante);

                List<MostrarVentaDetalleViewModels> lista = MostrarVentaDetalleViewModels.convert(resultado);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion

        #region Descargar PDF del Comprobante        
        [HttpGet]        
        public ActionResult imprimirVentas(string idComprobante)
        {
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            try
            {
                /*
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                Service1Client client = new Service1Client();
                */
                string Reporttype = "PDF";
                string fileExtension;
                string mimeType;
                string encodig;

                using (var db = new DBRouillonConsultinVentasEntities())
                {
                    int tmpIdCorrelativo = comprobanteNEG.returnIdCorrelativo(Convert.ToInt32(idComprobante));
                    var tmpComprobante = comprobanteNEG.tmpComprobante(Convert.ToInt32(idComprobante));
                    var tmpCorrelativo = comprobanteNEG.tmpCorrelativo(tmpIdCorrelativo);
                    var tmpTipoComprobante = comprobanteNEG.tmpTipoComprobante(Convert.ToInt32(tmpCorrelativo.idTipoComprobante));
                    string correoCliente = comprobanteNEG.tmpClienteCorreo(Convert.ToInt32(tmpComprobante.idCliente));

                    var serieCorrelativo = tmpComprobante.serieCorrelativo;
                    string serie = "", numero = "";
                    if(serieCorrelativo.Length > 0)
                    {
                        serie = serieCorrelativo.Split('-')[0];
                        numero = serieCorrelativo.Split('-')[1];
                    }
                    var tipoComprobante = (tmpTipoComprobante.idTipoComprobante == 2) ? "01" : "03";

                    //Directorio de almacenamiento
                    var path01 = Path.Combine(Server.MapPath("~/Reportes"), tmpTipoComprobante.tipoComprobante);
                    if (!Directory.Exists(path01))
                        Directory.CreateDirectory(path01);

                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/Report1.rdlc");   //Report1_Large.rdlc
                    reportViewer.LocalReport.Refresh();
                    reportViewer.LocalReport.EnableExternalImages = true;

                    ReportDataSource reportDataSource = new ReportDataSource();
                    reportDataSource.Name = "ComprobantesDataSet";
                    reportDataSource.Value = db.sp_ReporteComprobante(Convert.ToInt32(idComprobante)).ToList();
                    reportViewer.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer.LocalReport.Refresh();

                    fileExtension = "pdf";

                    string[] streams;
                    Warning[] warnings;
                    byte[] renderByte;
                    var fileName = tmpComprobante.serieCorrelativo;                    

                    renderByte = reportViewer.LocalReport.Render("PDF");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename =" + fileName + ".pdf");

                    //Guardamos el Archivo en la ruta asignada
                    string rutaArchivo = Server.MapPath("~/Reportes/" + tmpTipoComprobante.tipoComprobante + "/" + fileName + "." + fileExtension);
                    FileStream fs = System.IO.File.Create(path01 + "/" + fileName + "." + fileExtension);
                    fs.Write(renderByte, 0, renderByte.Length);
                    fs.Close();

                    /*if (temporal == 1)
                    {
                        Email(rutaArchivo, correoCliente, tmpTipoComprobante.tipoComprobante);
                    }*/

                    return File(renderByte, "pdf"); // fileExtension);                    
                }
            }
            catch (Exception e)
            {
                string FecActual = DateTime.Today.ToString("MM/dd/yy");
                string ruta = Server.MapPath("~/Reportes/" + "Log.txt");
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta, true);
                string texto;
                texto = "::" + "path01:" + Path.Combine(Server.MapPath("~/Reportes")) + " , ruta2:" + Server.MapPath("~/Reportes/Report1.rdlc") + " MESSAGE: " + e.Message.ToString() + " SOURCE: " + e.Source + " STACKTRACE: " + e.StackTrace + "tot:" + e.ToString();
                sw.WriteLine(texto);
                sw.Close();

                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
        #endregion


        [HttpGet]
        public void imprimirVentasExcel(string idComprobante)
        {
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();

            if (idComprobante == null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    Page pagina = new Page();
                    dynamic form = new HtmlForm();

                    //GrdPago.EnableViewState = false;
                    pagina.EnableEventValidation = false;
                    pagina.DesignerInitialize();
                    pagina.Controls.Add(form);
                    form.Controls.Add("tabla-correos");
                    pagina.RenderControl(htw);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=Pago_.xls");
                    Response.Charset = "UTF-8";

                    Response.ContentEncoding = Encoding.Default;
                    Response.Write(sb.ToString());
                    Response.End();

                }
                catch (Exception e)
                {
                    string FecActual = DateTime.Today.ToString("MM/dd/yy");
                    string ruta = Server.MapPath("~/Reportes/" + "Log.txt");
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta, true);
                    string texto;
                    texto = "::" + "path01:" + Path.Combine(Server.MapPath("~/Reportes")) + " , ruta2:" + Server.MapPath("~/Reportes/Report1.rdlc") + " MESSAGE: " + e.Message.ToString() + " SOURCE: " + e.Source + " STACKTRACE: " + e.StackTrace + "tot:" + e.ToString();
                    sw.WriteLine(texto);
                    sw.Close();

                    HttpContext.Response.StatusCode = 500;
                    //return Json(Util.errorJson(e));
                }
            }
        }


        #region Cargar Combos
        public JsonResult comboTipoComprobante()
        {
            try
            {
                List<CO_TipoComprobante> resultado = tipoComprobanteNEG.listarTipoComprobante();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idTipoComprobante, item.tipoComprobante);
                    lista.Add(combo);
                }

                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        public JsonResult comboTipoMoneda()
        {
            try
            {
                List<CO_Moneda> resultado = monedaNEG.listarMoneda();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idMoneda, item.descripcion);
                    lista.Add(combo);
                }

                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        public JsonResult comboIGV()
        {
            try
            {
                List<IGVMast> resultado = igvMastNEG.listarIGV();

                List<ComboGenericoViewModel> lista = new List<ComboGenericoViewModel>();
                foreach (var item in resultado)
                {
                    ComboGenericoViewModel combo = new ComboGenericoViewModel(item.idIGV, Convert.ToString(item.porcentaje));
                    lista.Add(combo);
                }
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        #endregion

        #region Envio de Email(comprobante) al Cliente
        public void Email(string ruta, string correoCliente, string tipoComprobante)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());

            if (ConfigurationManager.AppSettings["CCC"] != null)
            {
                try
                {
                    //msg.To.Add(new MailAddress(ConfigurationManager.AppSettings["CCC"].ToString()));
                    string cc = ConfigurationManager.AppSettings["CCC"].ToString();
                    string[] correos = cc.Split(',');

                    foreach (string correo in correos)
                    {
                        msg.To.Add(correo);
                    }
                }
                catch (Exception e)
                {

                }

            }

            msg.To.Add(new MailAddress(correoCliente));

            msg.Subject = tipoComprobante;

            adjunto = new Attachment(ruta);
            msg.Attachments.Add(adjunto);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
        #endregion
    }
}
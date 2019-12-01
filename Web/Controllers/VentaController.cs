using System;
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
        public int temporal=1;

        // GET: Venta
        #region Agregar Venta
        public ActionResult AddVenta()
        {
            return View();
        }

        public JsonResult guardarComprobante(VentaViewModels ventaViewModels, List<VentaDetalleViewModels> ventaDetalleViewModels, List<ProductoServicioViewModels> productoViewModels)
        {
            long IdComprobante = 0;
            string pdfAbrir="";
            try
            {
                OperationResult resultado = null;
                if (ventaViewModels.idComprobante != 0)
                {
                    ventaViewModels.textoTotal = conversion.enletras(Convert.ToString(ventaViewModels.total));
                    ventaViewModels.serieCorrelativo = (comprobanteNEG.CodigoComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo))).ToString();
                    resultado = comprobanteNEG.actualizarComprobante(ventaViewModels.idComprobante, ventaViewModels.idCorrelativo, ventaViewModels.serieCorrelativo, ventaViewModels.descripcion, ventaViewModels.estado, ventaViewModels.subtotal, ventaViewModels.total, ventaViewModels.textoTotal, ventaViewModels.fechaRegistro, ventaViewModels.usuarioRegistro, IdUsuario(), ventaViewModels.idUsuario, ventaViewModels.idCliente, ventaViewModels.idMoneda, 1);
                }
                else
                {
                    ventaViewModels.textoTotal = conversion.enletras(Convert.ToString(ventaViewModels.total));
                    ventaViewModels.serieCorrelativo = comprobanteNEG.DevolverSerieComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo)) +"-"+ (comprobanteNEG.CodigoComprobante(Convert.ToInt32(ventaViewModels.idCorrelativo))).ToString();
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
                
                imprimirVentas(Convert.ToString(IdComprobante));

                temporal = 1;
                Util.verificarError(resultado);

                //////////////////////////////////////////////////////////////////////////////////////////////
                int tmpIdCorrelativo = comprobanteNEG.returnIdCorrelativo(Convert.ToInt32(IdComprobante));
                var tmpComprobante = comprobanteNEG.tmpComprobante(Convert.ToInt32(IdComprobante));
                var tmpCorrelativo = comprobanteNEG.tmpCorrelativo(tmpIdCorrelativo);
                var tmpTipoComprobante = comprobanteNEG.tmpTipoComprobante(Convert.ToInt32(tmpCorrelativo.idTipoComprobante));

                pdfAbrir = Server.MapPath("~/Reportes/" + tmpTipoComprobante.tipoComprobante + "/" + ventaViewModels.serieCorrelativo + ".pdf");
                //pdfAbrir = "http://crouillon-001-site3.atempurl.com/Reportes/" + tmpTipoComprobante.tipoComprobante + "/" + ventaViewModels.serieCorrelativo + ".pdf";

                //System.Diagnostics.Process.Start(pdfAbrir);


                //string FilePath = pdfAbrir;// Server.MapPath("javascript1-sample.pdf");
                //WebClient User = new WebClient();            
                //Byte[] FileBuffer = User.DownloadData(FilePath);

                //if (FileBuffer != null)
                //{
                //    Response.ContentType = "application/pdf";
                //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //    Response.BinaryWrite(FileBuffer);
                //}

                //////////////////////////////////////////////////////////////////////////////////////////////

                /*En_Respuesta respuesta_wcf = new En_Respuesta();
                En_ComprobanteElectronico comprobante_wcf = new En_ComprobanteElectronico();
                Service.RC.FactElect.Service1Client client = new Service.RC.FactElect.Service1Client();

                En_Emisor emisor_wcf = new En_Emisor();
                En_Receptor receptor_wcf = new En_Receptor();
                En_Contacto contacto_wcf = new En_Contacto();
                
                emisor_wcf.NumeroDocumentoIdentidad = "20112811096";
                emisor_wcf.TipoDocumentoIdentidad = "6";
                emisor_wcf.CodigoDomicilioFiscal = "0002";
                emisor_wcf.RazonSocial = "Razon Social Emisor";
                emisor_wcf.PaginaWeb = "www.paginweb.com";
                emisor_wcf.NombreComercial = "Nombre Comercial";
                emisor_wcf.CodigoUbigeo = "020202";
                emisor_wcf.CodigoDomicilioFiscal = "0002";
                emisor_wcf.Urbanizacion = "Urbanizacion marquez 2";
                emisor_wcf.Provincia = "Lima";
                emisor_wcf.Distrito = "Lima";
                emisor_wcf.Departamento = "Lima";
                emisor_wcf.CodigoPais = "PE";
                emisor_wcf.Direccion = "Dirección Emisor";

                comprobante_wcf.Emisor = emisor_wcf;
           
                receptor_wcf.TipoDocumentoIdentidad = "6";
                receptor_wcf.NumeroDocumentoIdentidad = "20000000000";
                receptor_wcf.RazonSocial = "Razon Social Receptor";
                receptor_wcf.PaginaWeb = "www.receptor.com";
                receptor_wcf.NombreComercial = "Nombre Comercial Receptor";
                receptor_wcf.CodigoDomicilioFiscal = "0002";
                receptor_wcf.Provincia = "Lima";
                receptor_wcf.Departamento = "Lima";
                receptor_wcf.Distrito = "Lima";
                receptor_wcf.Direccion = "Direccion Receptor";
                receptor_wcf.CodigoPais = "PE";
                receptor_wcf.Urbanizacion = "";
                receptor_wcf.CodigoUbigeo = "020202";
                comprobante_wcf.Receptor = receptor_wcf;

                contacto_wcf.Correo = "rouilloncesar@gmail.com";
                contacto_wcf.Nombre = "";
                contacto_wcf.Telefono = "";
                emisor_wcf.Contacto = contacto_wcf;
                receptor_wcf.Contacto = contacto_wcf;

                List<En_Leyenda> leyenda_wcf1 = new List<En_Leyenda>();
                En_Leyenda leyenda_wcf = new En_Leyenda()
                {
                    Codigo = "1000",
                    Valor ="Valor"
                };
                leyenda_wcf1.Add(leyenda_wcf);                                

                comprobante_wcf.Leyenda = leyenda_wcf1.ToArray();                 

                comprobante_wcf.TipoComprobante = "01";
                comprobante_wcf.SerieNumero = ventaViewModels.serieCorrelativo;
                comprobante_wcf.FechaEmision = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
                comprobante_wcf.TotalImpuesto = ventaViewModels.total - ventaViewModels.subtotal;
                comprobante_wcf.TotalValorVenta = ventaViewModels.total;
                comprobante_wcf.TotalPrecioVenta = ventaViewModels.total;
                comprobante_wcf.TotalDescuento = ventaViewModels.total- ventaViewModels.total;
                comprobante_wcf.TotalCargo = ventaViewModels.total - ventaViewModels.total;
                comprobante_wcf.ImporteTotal = ventaViewModels.total;
                comprobante_wcf.FechaVencimiento = System.DateTime.Now.Date.ToString("yyyy-MM-dd");
                comprobante_wcf.HoraEmision = System.DateTime.Now.Hour.ToString("00")+":"+ System.DateTime.Now.Minute.ToString("00")+":"+ System.DateTime.Now.Second.ToString("00");
                comprobante_wcf.Moneda = "PEN";
                comprobante_wcf.TipoOperacion = "0101";

                En_ComprobanteDetalle ComprobanteDetalle_wcf = new En_ComprobanteDetalle();
                List<En_ComprobanteDetalle> ComprobanteDetalle_wcf1 = new List<En_ComprobanteDetalle>();
                En_ComprobanteDetalle ComprobanteDetalle_wcf2 = new En_ComprobanteDetalle()
                {
                    Cantidad = 1,
                    Codigo = "617000093",
                    CodigoSunat = "80161504",
                    CodigoTipoPrecio = "01",
                    Descripcion = "Descripcion 1",
                    ImpuestoTotal = Convert.ToDecimal(51.49),
                    Item = Convert.ToDecimal(1),
                    MultiDescripcion = new String[] { "Multidescripción" },
                    Total = ventaViewModels.subtotal,
                    ValorVentaUnitario = ventaViewModels.subtotal,
                    ValorVentaUnitarioIncIgv = ventaViewModels.total,
                    UnidadMedida="PEN"
                };

                List<En_ComprobanteDetalleImpuestos> ComprobanteDetalleImp_wcf1 = new List<En_ComprobanteDetalleImpuestos>();
                En_ComprobanteDetalleImpuestos ComprobanteDetalleImp_wcf = new En_ComprobanteDetalleImpuestos()
                {
                    AfectacionIGV = Convert.ToString(10),
                    CodigoInternacionalTributo = "1000",
                    CodigoTributo = "VAT",
                    MontoBase = ventaViewModels.subtotal,
                    MontoTotalImpuesto = ventaViewModels.total - ventaViewModels.subtotal,
                    NombreTributo = "IGV",
                    Porcentaje = ventaViewModels.idIGV
                };
                ComprobanteDetalleImp_wcf1.Add(ComprobanteDetalleImp_wcf);
                ComprobanteDetalle_wcf2.ComprobanteDetalleImpuestos = ComprobanteDetalleImp_wcf1.ToArray();

                ComprobanteDetalle_wcf1.Add(ComprobanteDetalle_wcf2);
                comprobante_wcf.ComprobanteDetalle = ComprobanteDetalle_wcf1.ToArray();


                En_MontosTotales MontosTotales_wcf = new En_MontosTotales();
                List<En_MontosTotales> MontosTotales_wcf1 = new List<En_MontosTotales>();

                List<En_GrabadoIGV> GrabadoIGV_wcf1 = new List<En_GrabadoIGV>();
                En_GrabadoIGV GrabadoIGV_wcf = new En_GrabadoIGV()
                {
                    MontoBase = ventaViewModels.total,
                    MontoTotalImpuesto= ventaViewModels.total - ventaViewModels.subtotal,
                    Porcentaje= ventaViewModels.total - ventaViewModels.subtotal
                };

                List<En_Gravado> Gravado_wcf1 = new List<En_Gravado>();
                En_Gravado Gravado_wcf = new En_Gravado()
                {
                    Total = ventaViewModels.total,
                };
                Gravado_wcf1.Add(Gravado_wcf);
                GrabadoIGV_wcf1.Add(GrabadoIGV_wcf);
                                
                Gravado_wcf.GravadoIGV = GrabadoIGV_wcf;
                MontosTotales_wcf.Gravado = Gravado_wcf;
                comprobante_wcf.MontoTotales = MontosTotales_wcf;

                respuesta_wcf = client.RegistroComprobante(comprobante_wcf);*/


                return Json(new { code_result = resultado.code_result, data = resultado.data, result_description = resultado.title }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string FecActual = DateTime.Today.ToString("MM/dd/yy");
                string ruta = Server.MapPath("~/Reportes/" + "Log.txt");

                System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta, true);
                string texto;
                texto = " MESSAGE: " + e.Message.ToString() + " SOURCE: " + e.Source + " STACKTRACE: " + e.StackTrace + "totGC:" + e.ToString() + " pdfAbrir:"+ pdfAbrir;
                sw.WriteLine(texto);
                sw.Close();

                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
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

        [Authorize(Roles = "ADMINISTRADOR")]
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

        [Authorize(Roles = "ADMINISTRADOR")]
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

        [Authorize(Roles = "ADMINISTRADOR")]
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

                    //Directorio de almacenamiento
                    var path01 = Path.Combine(Server.MapPath("~/Reportes"), tmpTipoComprobante.tipoComprobante);
                    if (!Directory.Exists(path01))
                        Directory.CreateDirectory(path01);
                    //Fin

                    reportViewer.ProcessingMode = ProcessingMode.Local;

                    //LocalReport localReport = new LocalReport();
                    reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/Report1.rdlc");   //Report1_Large.rdlc
                    //var reportStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(Server.MapPath("~/Reportes/Report1.rdlc"));
                    //localReport.LoadReportDefinition(reportStream);

                    reportViewer.LocalReport.Refresh();
                    reportViewer.LocalReport.EnableExternalImages = true;

                    ReportDataSource reportDataSource = new ReportDataSource();
                    reportDataSource.Name = "ComprobantesDataSet";
                    reportDataSource.Value = db.sp_ReporteComprobante(Convert.ToInt32(idComprobante)).ToList();
                    reportViewer.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer.LocalReport.Refresh();

                    //if (Reporttype == "Excel")
                    //{
                    //    fileName = "xlsx";
                    //}
                    //if (Reporttype == "PDF")
                    //{
                    //    fileExtension = "application/pdf";// "pdf";
                    //}
                    fileExtension = "pdf";

                    string[] streams;
                    Warning[] warnings;
                    byte[] renderByte;
                    var fileName = tmpComprobante.serieCorrelativo;

                    //string deviceInfo = "<DeviceInfo>" + "  <OutputFormat>EMF</OutputFormat>" + "  <PageWidth>8.5in</PageWidth>" + "  <PageHeight>11in</PageHeight>" + "  <MarginTop>1in</MarginTop>" + "  <MarginLeft>1.00in</MarginLeft>" + "  <MarginRight>1.00in</MarginRight>" + "  <MarginBottom>0.75in</MarginBottom>" + "</DeviceInfo>";
                    //renderByte = reportViewer.LocalReport.Render(Reporttype, deviceInfo, out mimeType, out encodig, out fileExtension, out streams, out warnings);
                    //Response.AddHeader("content-disposition", "attachment;filename =" + fileName + "." + fileExtension);

                    renderByte = reportViewer.LocalReport.Render("PDF");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename =" + fileName + ".pdf");

                    //Guardamos el Archivo en la ruta asignada
                    string rutaArchivo = Server.MapPath("~/Reportes/" + tmpTipoComprobante.tipoComprobante + "/" + fileName + "." + fileExtension);
                    FileStream fs = System.IO.File.Create(path01 + "/" + fileName + "." + fileExtension);
                    fs.Write(renderByte, 0, renderByte.Length);
                    fs.Close();

                    if (temporal == 1)
                    {
                        Email(rutaArchivo, correoCliente, tmpTipoComprobante.tipoComprobante);
                    }

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
        public void Email(string ruta,string correoCliente,string tipoComprobante)
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
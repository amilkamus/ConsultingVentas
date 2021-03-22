using Negocio;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Xml.XPath;
using Web.Models;
using Web.Service.RC.FactElect;
using Web.Utilitario;

namespace Web.Controllers
{
    public class ComprobanteController : BaseController
    {
        // GET: Comprobante
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, FACTURACION")]
        public JsonResult ListarComprobanteElectronicos(En_EntradaListarComprobante entrada)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();
                En_SalidaListarComprobante[] comprobantes = client.ListarComprobanteElectronicos(entrada);

                var respuesta = new
                {
                    Codigo = "0",
                    Mensaje = "Se procesó correctamente.",
                    Comprobantes = comprobantes
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Mensaje = "Ocurrió un error al procesar la información, error: " + e.Message.ToString()
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, FACTURACION")]
        public ActionResult Exportar(Web.Models.Comprobante.ComprobanteViewModel entrada)
        {
            int filaError = 0;
            int columnaError = 0;
            string nombreColumnaError = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();
                En_EntradaListarComprobante entradaWS = new En_EntradaListarComprobante
                {
                    Estado = entrada.Estado,
                    NumeroDocumentoIdentidadEmisor = "20602034675",
                    NumeroDocumentoIdentidadReceptor = entrada.NumeroDocumentoIdentidadReceptor,
                    FechaInicial = entrada.FechaInicial,
                    FechaFinal = entrada.FechaFinal,
                    TipoComprobante = entrada.TipoComprobante,
                    SerieNumero = entrada.SerieNumero
                };
                En_SalidaListarComprobante[] comprobantes = client.ListarComprobanteElectronicos(entradaWS);
                List<En_SalidaListarComprobante> listaComprobantes = new List<En_SalidaListarComprobante>();
                if (comprobantes != null && comprobantes.Length > 0)
                {
                    listaComprobantes = comprobantes.ToList();
                }

                IWorkbook wb = new NPOI.XSSF.UserModel.XSSFWorkbook();

                ISheet sheet = wb.CreateSheet("Reporte");
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
                columnasCabecera.Add("NumeroDocumentoIdentidad", "Número RUC");
                columnasCabecera.Add("RazonSocial", "Razon Social");
                columnasCabecera.Add("Estado", "Estado SUNAT");
                columnasCabecera.Add("TipoComprobante", "Tipo de comprobante");
                columnasCabecera.Add("SerieNumero", "Serie y número");
                columnasCabecera.Add("FechaEmision", "Fecha de emisión");
                columnasCabecera.Add("Moneda", "Moneda");
                columnasCabecera.Add("ComprobanteReferenciado", "Comprobante Referenciado");
                columnasCabecera.Add("TotalImpuesto", "IGV");
                columnasCabecera.Add("TotalValorVenta", "Total sin IGV");
                columnasCabecera.Add("TotalDescuento", "Descuento");
                columnasCabecera.Add("TotalPrecioVenta", "Total con IGV");

                foreach (KeyValuePair<string, string> cabecera in columnasCabecera)
                {
                    cell = row.CreateCell(columna);
                    cell.SetCellValue(cabecera.Value);
                    cell.CellStyle = styleNegritaBorde;
                    columna += 1;
                }

                rowIndex += 1;
                string[] columnasNumericas = new[] { "TotalPrecioVenta", "TotalImpuesto", "TotalValorVenta", "TotalDescuento" };
                string[] columnasFecha = new[] { "FechaEmision" };
                string[] columnasFechaHora = new[] { "" };
                filaError = rowIndex;
                foreach (var comprobante in listaComprobantes)
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
                            cell.SetCellValue(Convert.ToDouble(comprobante.GetType().GetProperty(cabecera.Key).GetValue(comprobante, null)));
                            cell.CellStyle = styleDecimal;
                        }
                        else if (columnasFecha.Contains(nombreColumna))
                        {
                            cell.SetCellValue(comprobante.GetType().GetProperty(cabecera.Key).GetValue(comprobante, null).ToString());
                            cell.CellStyle = styleFecha;
                        }
                        else
                            cell.SetCellValue(comprobante.GetType().GetProperty(cabecera.Key).GetValue(comprobante, null).ToString());

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
                return File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte.xlsx");
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

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, FACTURACION")]
        public JsonResult ObtenerDocumentoComprobante(long id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();
                En_SalidaArchivo documento = client.ObtenerDocumentoComprobante(id);

                return Json(documento, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Descripcion = "Ocurrió un error al procesar la información, error: " + e.Message.ToString()
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, FACTURACION")]
        public JsonResult ObtenerRepresentacionImpresa(long id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();
                En_SalidaArchivo documento = client.ObtenerRepresentacionImpresa(id);

                return Json(documento, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Descripcion = "Ocurrió un error al procesar la información, error: " + e.Message.ToString()
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, FACTURACION")]
        public JsonResult ObtenerRespuestaComprobante(long id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();
                En_SalidaArchivo documento = client.ObtenerRespuestaComprobante(id);

                return Json(documento, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var respuesta = new
                {
                    Codigo = "1",
                    Descripcion = "Ocurrió un error al procesar la información, error: " + e.Message.ToString()
                };

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, FACTURACION")]
        public JsonResult GenerarNotaCredito(ComprobanteViewModel parametro)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                Service1Client client = new Service1Client();

                En_SalidaArchivo respuesta = client.ObtenerDocumentoComprobante(parametro.IdComprobante);
                byte[] bytes = Convert.FromBase64String(respuesta.ContenidoArchivo);
                Stream stream = new MemoryStream(bytes);

                XmlDocument docXML = new XmlDocument();
                docXML.Load(stream);

                // Leemos el XML del comprobante para cargar la nueva entidad de la nota de crédito
                var nav = docXML.CreateNavigator();
                var ns = Util.ObtenerXmlNamespaces(nav);

                En_Emisor emisor = new En_Emisor
                {
                    NumeroDocumentoIdentidad = "20602034675"
                };

                XPathNavigator nodoDocumentoIdentidad = nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID", ns);
                En_Receptor receptor = new En_Receptor
                {
                    CodigoDomicilioFiscal = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:AddressTypeCode", ns), ""),
                    CodigoPais = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cac:Country/cbc:IdentificationCode", ns), ""),
                    CodigoUbigeo = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:ID", ns), ""),
                    Contacto = new En_Contacto
                    {
                        Correo = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:ElectronicMail", ns), ""),
                        Nombre = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Name", ns), ""),
                        Telefono = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Telephone", ns), "")
                    },
                    //Departamento = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:CountrySubentity", ns), ""),
                    Direccion = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cac:AddressLine/cbc:Line", ns), ""),
                    //Distrito = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:District", ns), ""),
                    NombreComercial = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyName/cbc:Name", ns), ""),
                    PaginaWeb = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cbc:WebsiteURI", ns), ""),
                    //Provincia = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:CityName", ns), ""),
                    RazonSocial = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName", ns), ""),
                    NumeroDocumentoIdentidad = Util.NodeValue(nodoDocumentoIdentidad, ""),
                    TipoDocumentoIdentidad = nodoDocumentoIdentidad.GetAttribute("schemeID", ""),
                    //Urbanizacion = Util.NodeValue(nav.SelectSingleNode("*/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cac:RegistrationAddress/cbc:CitySubdivisionName", ns), "")
                };

                En_ComprobanteElectronico comprobante = new En_ComprobanteElectronico();

                // Inicio de los detalles
                XPathNodeIterator detalles = nav.Select("*/cac:InvoiceLine", ns);
                comprobante.ComprobanteDetalle = new En_ComprobanteDetalle[detalles.Count];
                int indice = 0;
                foreach (XPathNavigator item in detalles)
                {
                    En_ComprobanteDetalle detalle = new En_ComprobanteDetalle
                    {
                        Cantidad = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cbc:InvoicedQuantity", ns), "0")),
                        Codigo = Util.NodeValue(item.SelectSingleNode("cac:Item/cac:SellersItemIdentification/cbc:ID", ns), ""),
                        CodigoSunat = Util.NodeValue(item.SelectSingleNode("cac:Item/cac:CommodityClassification/cbc:ItemClassificationCode", ns), ""),
                        CodigoTipoPrecio = Util.NodeValue(item.SelectSingleNode("cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceTypeCode", ns), ""),
                        Descripcion = Util.NodeValue(item.SelectSingleNode("cac:Item/cbc:Description", ns), ""),
                        ImpuestoTotal = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cac:TaxTotal/cbc:TaxAmount", ns), "0")),
                        Item = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cbc:ID", ns), "0")),
                        Total = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cbc:LineExtensionAmount", ns), "0")),
                        UnidadMedida = item.SelectSingleNode("cbc:InvoicedQuantity", ns).GetAttribute("unitCode", ""),
                        ValorVentaUnitario = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cac:Price/cbc:PriceAmount", ns), "0")),
                        ValorVentaUnitarioIncIgv = decimal.Parse(Util.NodeValue(item.SelectSingleNode("cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceAmount", ns), "0"))
                    };

                    detalle.MultiDescripcion = new string[1];
                    detalle.MultiDescripcion.SetValue("-", 0);

                    XPathNodeIterator impuestos = item.Select("cac:TaxTotal/cac:TaxSubtotal", ns);
                    detalle.ComprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos[impuestos.Count];
                    int indiceImpuesto = 0;
                    foreach (XPathNavigator impuesto in impuestos)
                    {
                        En_ComprobanteDetalleImpuestos comprobanteDetalleImpuestos = new En_ComprobanteDetalleImpuestos
                        {
                            AfectacionIGV = Util.NodeValue(impuesto.SelectSingleNode("cac:TaxCategory/cbc:TaxExemptionReasonCode", ns), ""),
                            CodigoInternacionalTributo = Util.NodeValue(impuesto.SelectSingleNode("cac:TaxCategory/cac:TaxScheme/cbc:ID", ns), ""),
                            CodigoTributo = Util.NodeValue(impuesto.SelectSingleNode("cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode", ns), ""),
                            MontoBase = decimal.Parse(Util.NodeValue(impuesto.SelectSingleNode("cbc:TaxableAmount", ns), "0")),
                            MontoTotalImpuesto = decimal.Parse(Util.NodeValue(impuesto.SelectSingleNode("cbc:TaxAmount", ns), "0")),
                            NombreTributo = Util.NodeValue(impuesto.SelectSingleNode("cac:TaxCategory/cac:TaxScheme/cbc:Name", ns), ""),
                            Porcentaje = decimal.Parse(Util.NodeValue(impuesto.SelectSingleNode("cac:TaxCategory/cbc:Percent", ns), "0"))
                        };
                        detalle.ComprobanteDetalleImpuestos.SetValue(comprobanteDetalleImpuestos, indiceImpuesto);
                        indiceImpuesto++;
                    }
                    comprobante.ComprobanteDetalle.SetValue(detalle, indice);
                    indice++;
                }
                // Fin de los detalles

                En_Leyenda leyenda = new En_Leyenda
                {
                    Codigo = nav.SelectSingleNode("*/cbc:Note", ns).GetAttribute("languageLocaleID", ""),
                    Valor = Util.NodeValue(nav.SelectSingleNode("*/cbc:Note", ns), "")
                };

                En_MontosTotales montos = new En_MontosTotales
                {
                    Gravado = new En_Gravado
                    {
                        Total = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:TaxTotal/cbc:TaxAmount", ns), "0")),
                        GravadoIGV = new En_GrabadoIGV
                        {
                            MontoBase = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxableAmount", ns), "0")),
                            MontoTotalImpuesto = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount", ns), "0")),
                            Porcentaje = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:TaxTotal/cac:TaxSubtotal/cbc:Percent", ns), "0"))
                        }
                    }
                };

                DateTime dt;
                DateTime fechaRegistro = DateTime.Now;

                try//21.08.2020 - 13L
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

                comprobante.FechaEmision = fechaRegistro.ToString("yyyy-MM-dd");   //DateTime.Now.ToString("yyyy-MM-dd"); //Util.NodeValue(nav.SelectSingleNode("*/cbc:IssueDate", ns), "");
                //comprobante.FechaVencimiento = Util.NodeValue(nav.SelectSingleNode("*/cbc:DueDate", ns), "");
                comprobante.HoraEmision = fechaRegistro.ToString("HH:mm:ss");    //DateTime.Now.ToString("HH:mm:ss");  //Util.NodeValue(nav.SelectSingleNode("*/cbc:IssueTime", ns), "");
                comprobante.Moneda = Util.NodeValue(nav.SelectSingleNode("*/cbc:DocumentCurrencyCode", ns), "");
                comprobante.ImporteTotal = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:LegalMonetaryTotal/cbc:PayableAmount", ns), "0"));
                comprobante.Emisor = emisor;
                comprobante.Receptor = receptor;
                comprobante.MontoTotales = montos;

                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                string serie = "";
                long numero = 0;
                comprobanteNEG.ListarSerieCorrelativo("NOTA DE CREDITO", ref serie, ref numero);

                comprobante.SerieNumero = string.Format("{0}-{1}", serie, numero);
                XPathNavigator nodeTipoComprobante = nav.SelectSingleNode("*/cbc:InvoiceTypeCode", ns);
                comprobante.TipoComprobante = "07"; // Util.NodeValue(nodeTipoComprobante, ""); // se va a cambiar por el tipo de nota 07: Credito, 08:Debito
                comprobante.TipoOperacion = nodeTipoComprobante == null ? "" : nodeTipoComprobante.GetAttribute("listID", "");
                comprobante.TotalCargo = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount", ns), "0"));
                comprobante.TotalDescuento = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount", ns), "0"));
                comprobante.TotalImpuesto = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:TaxTotal/cbc:TaxAmount", ns), "0"));
                comprobante.TotalPrecioVenta = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", ns), "0"));
                comprobante.TotalValorVenta = decimal.Parse(Util.NodeValue(nav.SelectSingleNode("*/cac:LegalMonetaryTotal/cbc:LineExtensionAmount", ns), "0"));

                comprobante.Leyenda = new En_Leyenda[1];
                comprobante.Leyenda.SetValue(leyenda, 0);

                // referenciado
                comprobante.DocumentoSustentoNota = new En_DocumentoSustentoNota()
                {
                    CodigoMotivoAnulacion = parametro.TipoEmision,
                    MotivoAnulacion = parametro.MotivoEmision,
                    SerieNumero = Util.NodeValue(nav.SelectSingleNode("*/cbc:ID", ns), "")
                };

                En_DocumentoReferenciaNota referencia = new En_DocumentoReferenciaNota()
                {
                    TipoDocumento = Util.NodeValue(nodeTipoComprobante, ""),
                    SerieNumero = Util.NodeValue(nav.SelectSingleNode("*/cbc:ID", ns), ""),
                    Fecha = Util.NodeValue(nav.SelectSingleNode("*/cbc:IssueDate", ns), "")
                };

                comprobante.DocumentoReferenciaNota = new En_DocumentoReferenciaNota[1];
                comprobante.DocumentoReferenciaNota.SetValue(referencia, 0);

                En_Respuesta resultado = client.RegistroComprobante(comprobante);
                if (resultado.Codigo == "0")
                {
                    respuesta.Descripcion += " Comprobante: " + comprobante.SerieNumero;
                    comprobanteNEG.ActualizarSerieCorrelativo(serie, "NOTA DE CREDITO");
                    comprobanteNEG.QuitarFacturaCotizacion(comprobante.DocumentoReferenciaNota[0].SerieNumero);
                }

                stream.Dispose();
                stream.Close();
                return Json(resultado);
            }
            catch (Exception e)
            {
                En_Respuesta resultado = new En_Respuesta
                {
                    Codigo = "99",
                    Descripcion = e.Message.ToString()
                };
                return Json(resultado);
            }
        }
    }
}
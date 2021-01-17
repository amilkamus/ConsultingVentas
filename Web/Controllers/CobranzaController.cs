using Entidad;
using Negocio;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Models.Cotizacion;
using Web.Utilitario;

namespace Web.Controllers
{
    public class CobranzaController : BaseController
    {
        // GET: Cobranza
        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR, PARAMETRIZADOR, FACTURACION")]
        public JsonResult ListarCobranzas(EnCobranzaIn cobranzaIn)
        {
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnCobranzaOut> lista = comprobanteNEG.ListarCobranzas(cobranzaIn);
                return Json(lista);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }

        [Authorize(Roles = "ADMINISTRADOR, OPERADOR")]
        public ActionResult Exportar(EnCobranzaIn cobranzaIn)
        {
            int filaError = 0;
            int columnaError = 0;
            string nombreColumnaError = "";
            try
            {
                CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
                List<EnCobranzaOut> lista = comprobanteNEG.ListarCobranzas(cobranzaIn);

                IWorkbook wb = new NPOI.XSSF.UserModel.XSSFWorkbook();

                ISheet sheet = wb.CreateSheet("Cobranzas");
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
                columnasCabecera.Add("FechaIngreso", "FECHA DOC");
                columnasCabecera.Add("SerieNumero", "FACTURA");
                columnasCabecera.Add("Solicitante", "RAZÓN SOCIAL");
                columnasCabecera.Add("CondicionPago_1", "CONDICIÓN DE PAGO 1");
                columnasCabecera.Add("CondicionPago_2", "CONDICIÓN DE PAGO 2");
                columnasCabecera.Add("Total", "MONTO");
                columnasCabecera.Add("Importe1", "PAGO1");
                columnasCabecera.Add("Importe2", "PAGO2");
                columnasCabecera.Add("Importe3", "PAGO3");
                columnasCabecera.Add("Detraccion", "DETRACCIÓN");
                columnasCabecera.Add("Saldo", "SALDO");
                columnasCabecera.Add("Observacion1", "OBSERVACIONES");
                columnasCabecera.Add("NroOperacion", "N° OPERACIÓN");
                columnasCabecera.Add("CodigoInterno", "CÓDIGO INTERNO");

                foreach (KeyValuePair<string, string> cabecera in columnasCabecera)
                {
                    cell = row.CreateCell(columna);
                    cell.SetCellValue(cabecera.Value);
                    cell.CellStyle = styleNegritaBorde;
                    columna += 1;
                }

                rowIndex += 1;
                string[] columnasNumericas = new[] { "Total", "Importe1", "Importe2", "Importe3" };
                string[] columnasFecha = new[] { "FechaIngreso" };
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
                return File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCobranzas.xlsx");
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

        private CotizacionViewModelOut ObtenerCotizacionCobranza(Cotizacion cotizacion, long id)
        {
            CotizacionViewModelOut modelo = new CotizacionViewModelOut();
            modelo.Cotizacion = cotizacion;

            CO_ComprobanteNEG comprobanteNEG = new CO_ComprobanteNEG();
            modelo.NombreUsuario = NombreUsuario(cotizacion.IdUsuarioRegistro);
            modelo.Cobranza = comprobanteNEG.ListarCobranzasPorCotizacion(id);
            if (modelo.Cobranza == null)
            {
                modelo.Cobranza = new EnCobranza();
                modelo.Cobranza.PagoDetraccion = "-1";
                modelo.Cobranza.Autodetraccion = false;
                if (cotizacion.Total > 700)
                {
                    modelo.Cobranza.Detraccion = cotizacion.Total * 0.12M;
                    modelo.Cobranza.Saldo = cotizacion.Total - modelo.Cobranza.Detraccion;
                }
                modelo.Cobranza.EjecutivoVenta = modelo.NombreUsuario;
            }
            return modelo;
        }

        public JsonResult ObtenerCotizacion(int id)
        {
            try
            {
                CotizacionContext db = new CotizacionContext();
                Cotizacion cotizacion = db.Cotizacions.Find(id);
                if (cotizacion == null)
                    throw new Exception("No se encontró la cotización.");

                CotizacionViewModelOut modelo = ObtenerCotizacionCobranza(cotizacion, id);
                return Json(modelo);
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(Util.errorJson(e));
            }
        }
    }
}
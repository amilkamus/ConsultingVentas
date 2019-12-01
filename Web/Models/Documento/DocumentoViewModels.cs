using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace Web.Models.Documento
{
    [Serializable]
    [DataContract]
    public class DocumentoViewModels
    {
        [DataMember]
        public int idTipoDocumento { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }

        public DocumentoViewModels()
        {

        }

        public DocumentoViewModels(TipoDocumento tipoDocumento)
        {
            this.idTipoDocumento = tipoDocumento.idTipoDocumento;
            this.nombre = tipoDocumento.tipoDocumento1;
            this.estado = tipoDocumento.estado;
        }

        public static List<DocumentoViewModels> convert(List<TipoDocumento> documento)
        {
            List<DocumentoViewModels> resultado = new List<DocumentoViewModels>();

            foreach (var item in documento)
            {
                resultado.Add(new DocumentoViewModels(item));
            }
            return resultado;
        }
    }
}
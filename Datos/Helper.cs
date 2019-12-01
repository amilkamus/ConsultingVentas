using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public static class Helper
    {
        public static DBRouillonConsultinVentasEntities getDBContext
        {
            get { return new DBRouillonConsultinVentasEntities(); }
        }
        /// <summary>
        /// Obtiene los errores que faltaron validar
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public static String GetValidationError(DBRouillonConsultinVentasEntities contexto)
        {
            String error = "";
            var listError = contexto.GetValidationErrors();
            foreach (var item in listError)
            {
                if (!item.IsValid)
                {
                    String tabla = item.Entry.Entity.ToString();
                    foreach (var iitem in item.ValidationErrors)
                    {
                        error += "tabla: " + tabla + ", Columna: " + iitem.PropertyName + ", Propiedad:" + iitem.ErrorMessage + "\r\n";
                    }

                }
            }
            return error;
        }
    }
}

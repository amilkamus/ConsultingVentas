using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Sidebar()
        {
            return PartialView(navbarItems().ToList());
        }

        public IEnumerable<NavBar> navbarItems()
        {
            var menu = new List<NavBar>();

            menu.Add(new NavBar { Id = 1, Nombre = "Gestion de Usuarios", Icono = "fa fa-user", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 2, Nombre = "Nuevo Usuario", Controlador = "Account", Accion = "Register", Icono = "fa fa-user-plus", Estado = true, EsPadre = false, IdPadre = 1 });
            menu.Add(new NavBar { Id = 3, Nombre = "Listar Usuarios", Controlador = "Account", Accion = "ViewUser", Icono = "fa fa-users", Estado = true, EsPadre = false, IdPadre = 1 });
            menu.Add(new NavBar { Id = 4, Nombre = "Nuevo Rol", Controlador = "Role", Accion = "Create", Icono = "fa fa-user-plus", Estado = true, EsPadre = false, IdPadre = 1 });
            menu.Add(new NavBar { Id = 5, Nombre = "Listar Roles", Controlador = "Role", Accion = "Index", Icono = "fa fa-users", Estado = true, EsPadre = false, IdPadre = 1 });

            menu.Add(new NavBar { Id = 6, Nombre = "Productos/Servicios", Icono = "fa fa-desktop", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 7, Nombre = "Nuevo Producto", Controlador = "Producto", Accion = "AddProducto", Icono = "fa  fa-plus", Estado = true, EsPadre = false, IdPadre = 6 });
            menu.Add(new NavBar { Id = 8, Nombre = "Listar Productos", Controlador = "Producto", Accion = "ViewAllProducto", Icono = "fa fa-list", Estado = true, EsPadre = false, IdPadre = 6 });

            menu.Add(new NavBar { Id = 9, Nombre = "Clientes", Icono = "fa fa-users", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 10, Nombre = "Nuevo Cliente", Controlador = "Cliente", Accion = "AddCliente", Icono = "fa fa-plus", Estado = true, EsPadre = false, IdPadre = 9 });
            menu.Add(new NavBar { Id = 11, Nombre = "Listar Cliente", Controlador = "Cliente", Accion = "ViewAllCliente", Icono = "fa fa-list", Estado = true, EsPadre = false, IdPadre = 9 });

            menu.Add(new NavBar { Id = 12, Nombre = "Cotizacion", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 13, Nombre = "Nueva Cotizacion", Controlador = "Cotizacion", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 12 });
            menu.Add(new NavBar { Id = 14, Nombre = "Listar Cotizacion", Controlador = "Cotizacion", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 12 });

            menu.Add(new NavBar { Id = 53, Nombre = "Orden de Servicio", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 54, Nombre = "Nueva Orden Servicio", Controlador = "OrdenServicio", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 53 });
            menu.Add(new NavBar { Id = 55, Nombre = "Listar Orden Servicio", Controlador = "OrdenServicio", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 53 });

            menu.Add(new NavBar { Id = 15, Nombre = "Ventas", Icono = "fa fa-line-chart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 16, Nombre = "Nueva Venta", Controlador = "Venta", Accion = "AddVenta", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 15 });
            menu.Add(new NavBar { Id = 17, Nombre = "Listar Venta", Controlador = "Venta", Accion = "ViewAllVentas", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 15 });

            menu.Add(new NavBar { Id = 18, Nombre = "Nosotros", Icono = "fa fa-building", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 19, Nombre = "Info. de la Empresa", Controlador = "Compania", Accion = "AddCompania", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 18 });

            menu.Add(new NavBar { Id = 20, Nombre = "Tipo de Documento", Icono = "fa fa-dashboard", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 21, Nombre = "Nuevo Tipo Documento", Controlador = "Documento", Accion = "AddDocumento", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 20 });
            menu.Add(new NavBar { Id = 22, Nombre = "Listar Tipos Documento", Controlador = "Documento", Accion = "ViewAllDocumento", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 20 });

            menu.Add(new NavBar { Id = 23, Nombre = "Tipo de Producto", Icono = "fa fa-dashboard", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 24, Nombre = "Nuevo Tipo producto", Controlador = "TipoProducto", Accion = "AddTipoProducto", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 23 });
            menu.Add(new NavBar { Id = 25, Nombre = "Listar Tipos producto", Controlador = "TipoProducto", Accion = "ViewAllTipoProducto", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 23 });

            menu.Add(new NavBar { Id = 26, Nombre = "Moneda", Icono = "fa fa-money", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 27, Nombre = "Nueva Moneda", Controlador = "Moneda", Accion = "AddMoneda", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 26 });
            menu.Add(new NavBar { Id = 28, Nombre = "Listar Monedas", Controlador = "Moneda", Accion = "ViewAllMoneda", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 26 });

            menu.Add(new NavBar { Id = 29, Nombre = "Cambio de Moneda", Icono = "fa fa-dollar", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 30, Nombre = "Nuevo Cambio de Moneda", Controlador = "CambioMoneda", Accion = "AddCambioMoneda", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 29 });
            menu.Add(new NavBar { Id = 31, Nombre = "Listar Cambio de Moneda", Controlador = "CambioMoneda", Accion = "ViewAllCambioMoneda", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 29 });

            menu.Add(new NavBar { Id = 32, Nombre = "Caja", Icono = "fa fa-dashboard", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 33, Nombre = "Nueva Caja", Controlador = "Caja", Accion = "AddCaja", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 32 });
            menu.Add(new NavBar { Id = 34, Nombre = "Listar Caja", Controlador = "Caja", Accion = "ViewAllCaja", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 32 });

            menu.Add(new NavBar { Id = 35, Nombre = "Egreso", Icono = "fa fa-share-square-o", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 36, Nombre = "Nuevo Egreso", Controlador = "Egreso", Accion = "AddEgreso", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 35 });
            menu.Add(new NavBar { Id = 37, Nombre = "Listar Egresos", Controlador = "Egreso", Accion = "ViewAllEgreso", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 35 });

            menu.Add(new NavBar { Id = 38, Nombre = "Tipo de Persona", Icono = "fa fa-dashboard", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 39, Nombre = "Nuevo Tipo Persona", Controlador = "TipoPersonas", Accion = "AddTipoPersona", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 38 });
            menu.Add(new NavBar { Id = 40, Nombre = "Listar Tipos Persona", Controlador = "TipoPersonas", Accion = "ViewAllTipoPersona", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 38 });

            menu.Add(new NavBar { Id = 41, Nombre = "Tipo de Comprobante", Icono = "fa fa-dashboard", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 42, Nombre = "Nuevo Tipo Comprobante", Controlador = "TipoComprobante", Accion = "AddTipoComprobante", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 41 });
            menu.Add(new NavBar { Id = 43, Nombre = "Listar Tipos Comprobante", Controlador = "TipoComprobante", Accion = "ViewAllTipoComprobante", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 41 });

            menu.Add(new NavBar { Id = 44, Nombre = "Correlativo Comprob", Icono = "fa fa-plus-square-o", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 45, Nombre = "Nuevo Correlativo", Controlador = "CorrelativoMast", Accion = "AddCorrelativo", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 44 });
            menu.Add(new NavBar { Id = 46, Nombre = "Listar Correlativos", Controlador = "CorrelativoMast", Accion = "ViewAllCorrelativoMast", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 44 });

            menu.Add(new NavBar { Id = 47, Nombre = "Parametros", Icono = "fa fa-table", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 48, Nombre = "Nuevo Parametro", Controlador = "Parametro", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 47 });
            menu.Add(new NavBar { Id = 49, Nombre = "Listar Parametros", Controlador = "Parametro", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 47 });

            menu.Add(new NavBar { Id = 50, Nombre = "Tipos Parametro", Icono = "fa fa-table", Estado = true, EsPadre = true, IdPadre = 0, Rol = 1 });
            menu.Add(new NavBar { Id = 51, Nombre = "Nuevo Tipos Parametro", Controlador = "TipoParametro", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 50 });
            menu.Add(new NavBar { Id = 52, Nombre = "Listar Tipos Parametro", Controlador = "TipoParametro", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 50 });

            //OPERADOR            
            menu.Add(new NavBar { Id = 600, Nombre = "Productos/Servicios", Icono = "fa fa-desktop", Estado = true, EsPadre = true, IdPadre = 0, Rol = 2 });
            menu.Add(new NavBar { Id = 700, Nombre = "Nuevo Producto", Controlador = "Producto", Accion = "AddProducto", Icono = "fa  fa-plus", Estado = true, EsPadre = false, IdPadre = 600 });
            menu.Add(new NavBar { Id = 800, Nombre = "Listar Productos", Controlador = "Producto", Accion = "ViewAllProducto", Icono = "fa fa-list", Estado = true, EsPadre = false, IdPadre = 600 });

            menu.Add(new NavBar { Id = 900, Nombre = "Clientes", Icono = "fa fa-users", Estado = true, EsPadre = true, IdPadre = 0, Rol = 2 });
            menu.Add(new NavBar { Id = 100, Nombre = "Nuevo Cliente", Controlador = "Cliente", Accion = "AddCliente", Icono = "fa fa-plus", Estado = true, EsPadre = false, IdPadre = 900 });
            menu.Add(new NavBar { Id = 110, Nombre = "Listar Cliente", Controlador = "Cliente", Accion = "ViewAllCliente", Icono = "fa fa-list", Estado = true, EsPadre = false, IdPadre = 900 });

            menu.Add(new NavBar { Id = 312, Nombre = "Cotizacion", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 2 });
            menu.Add(new NavBar { Id = 313, Nombre = "Nueva Cotizacion", Controlador = "Cotizacion", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 312 });
            menu.Add(new NavBar { Id = 314, Nombre = "Listar Cotizacion", Controlador = "Cotizacion", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 312 });

            menu.Add(new NavBar { Id = 353, Nombre = "Orden de Servicio", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 2 });
            menu.Add(new NavBar { Id = 354, Nombre = "Nueva Orden Servicio", Controlador = "OrdenServicio", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 353 });
            menu.Add(new NavBar { Id = 355, Nombre = "Listar Orden Servicio", Controlador = "OrdenServicio", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 353 });


            //PARAMETRIZADOR            
            menu.Add(new NavBar { Id = 120, Nombre = "Cotizacion", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 4 });
            menu.Add(new NavBar { Id = 140, Nombre = "Listar Cotizacion", Controlador = "Cotizacion", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 120 });

            menu.Add(new NavBar { Id = 153, Nombre = "Orden de Servicio", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 4 });
            menu.Add(new NavBar { Id = 155, Nombre = "Listar Orden Servicio", Controlador = "OrdenServicio", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 153 });

            menu.Add(new NavBar { Id = 147, Nombre = "Parametros", Icono = "fa fa-table", Estado = true, EsPadre = true, IdPadre = 0, Rol = 4 });
            menu.Add(new NavBar { Id = 148, Nombre = "Nuevo Parametro", Controlador = "Parametro", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 147 });
            menu.Add(new NavBar { Id = 149, Nombre = "Listar Parametros", Controlador = "Parametro", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 147 });

            menu.Add(new NavBar { Id = 150, Nombre = "Tipos Parametro", Icono = "fa fa-table", Estado = true, EsPadre = true, IdPadre = 0, Rol = 4 });
            menu.Add(new NavBar { Id = 151, Nombre = "Nuevo Tipos Parametro", Controlador = "TipoParametro", Accion = "Create", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 150 });
            menu.Add(new NavBar { Id = 152, Nombre = "Listar Tipos Parametro", Controlador = "TipoParametro", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 150 });


            //FACTURACION
            menu.Add(new NavBar { Id = 220, Nombre = "Cotizacion", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 5 });
            menu.Add(new NavBar { Id = 240, Nombre = "Listar Cotizacion", Controlador = "Cotizacion", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 220 });

            menu.Add(new NavBar { Id = 253, Nombre = "Orden de Servicio", Icono = "fa fa-shopping-cart", Estado = true, EsPadre = true, IdPadre = 0, Rol = 5 });
            menu.Add(new NavBar { Id = 255, Nombre = "Listar Orden Servicio", Controlador = "OrdenServicio", Accion = "Index", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 253 });

            menu.Add(new NavBar { Id = 244, Nombre = "Correlativo Comprob", Icono = "fa fa-plus-square-o", Estado = true, EsPadre = true, IdPadre = 0, Rol = 5 });
            menu.Add(new NavBar { Id = 245, Nombre = "Nuevo Correlativo", Controlador = "CorrelativoMast", Accion = "AddCorrelativo", Icono = "fa fa-cart-plus", Estado = true, EsPadre = false, IdPadre = 244 });
            menu.Add(new NavBar { Id = 246, Nombre = "Listar Correlativos", Controlador = "CorrelativoMast", Accion = "ViewAllCorrelativoMast", Icono = "fa fa-circle-o", Estado = true, EsPadre = false, IdPadre = 244 });


            return menu;
        }
    }
}
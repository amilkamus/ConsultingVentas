if OBJECT_ID('usp_CargarVenta', 'P') is not null
	drop procedure usp_CargarVenta
go
create procedure usp_CargarVenta
	@idComprobante bigint
as
begin
	select cont.Correo, 
		cont.nombre + ' ' + cont.apellidos Nombre, 
		cont.Telefono, 
		emp.domicilio Direccion, 
		emp.nombre NombreComercial, 
		emp.numeroDocumento NumeroDocumentoIdentidad, 
		emp.nombre RazonSocial,
		co.SubTotal,
		co.montoTotal Total,
		co.SerieCorrelativo
	from CO_Comprobante co 
	inner join Cliente cli on co.idCliente = cli.idCliente
	inner join PersonaMast emp on emp.idPersona = cli.idEmpresaCliente
	inner join PersonaMast cont on cli.idContacto = cont.idPersona
	where idComprobante = @idComprobante

	select cd.Cantidad, cd.Precio, p.Descripcion from CO_ComprobanteDetalle cd 
	inner join WH_ProductoServicio p on cd.idProducto = p.idProducto
	where idComprobante = @idComprobante
end
go
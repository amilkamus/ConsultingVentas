ALTER PROCEDURE ObtenerCotizacionImpresion
	@idCotizacion BIGINT
AS
BEGIN
	SELECT top 1 
		ID, IdCotizacion, TipoCotizacion, NumeroCotizacion, Solicitante, RUC, isnull(Contacto, '-') Contacto,
		isnull(Email, '-') Email, Fecha, TipoDocumentoSolicitado, DescripcionProducto, CantidadMuestra,
		isnull(Telefono, '-') Telefono, isnull(CorreoConfirmacion, '-') CorreoConfirmacion, 
		isnull(CondicionPago_1, '-') CondicionPago_1, isnull(CondicionPago_2, '-') CondicionPago_2, 
		isnull(Banco, '-') Banco, isnull(Moneda, '-') Moneda,
		isnull(CuentaCorriente, '-') CuentaCorriente, isnull(CuentaAhorro, '-') CuentaAhorro, isnull(CCI, '-') CCI, 
		isnull(Observaciones, '-') Observaciones, isnull(CondicionPago_De, '-') CondicionPago_De, isnull(Detracciones,'-') Detracciones,
		SerieNumero, isnull(DiasEntrega, 0) DiasEntrega, Correlativo, CorrelativoInicial, IdUsuarioRegistro, 
		FechaRegistro, IdUsuarioModificacion, FechaModificacion, SubTotal, PorcentajeDescuento,
		MontoDescuento, SubTotalFinal, IGV, Total, isnull(Proyecto, '-') Proyecto
	FROM dbo.Cotizacions where ID = @idCotizacion

	select 
		ps.nombre Analisis, pm.ParametroDescripcion Parametro, pm.Metodologia,
		tpm.TipoParametroDescripcion TipoParametro, cp.Cantidad NumeroMuestras, cp.Precio, cp.Cantidad * cp.Precio Subtotal,
		pm.LimiteDeteccion
	from CotizacionProductoes cp
		inner join WH_ProductoServicio ps on cp.IdProducto = ps.idProducto
		inner join Parametroes pm on pm.ID = cp.IdParametro
		inner join TipoParametroes tpm on tpm.ID = cp.IdTipoParametro
	where IdCotizacion = @idCotizacion

	select Documento, NormaReferencia, Precio, TipoServicio, Cantidad, Subtotal from CotizacionCertificadoes where IdCotizacion = @idCotizacion

	select 
		Actividad, Procedimiento, ReferenciaNormativa, ReferenciaMuestreo, PlanMuestreo, 
		LugarMuestreo, Precio, Cantidad, Subtotal, Producto, Documento, TipoServicio from CotizacionInspeccions 
	where IdCotizacion = @idCotizacion

	declare @resumen table (id int identity(1,1), resumen varchar(500), precio decimal(15,2), NumeroDias varchar(10), Total decimal(15,2))
	insert into @resumen
	select 	
		ps.descripcion Resumen, 
		sum(cp.Cantidad * cp.precio) Precio, '-' NumeroDias, sum(cp.Cantidad * cp.precio) Total from CotizacionProductoes cp inner join WH_ProductoServicio ps on cp.IdProducto = ps.idProducto
	where IdCotizacion = @idCotizacion
	group by ps.IdProducto, ps.descripcion
	union all
	select Documento, Precio, Cantidad NumeroDias, SubTotal Total
	from CotizacionCertificadoes where IdCotizacion = @idCotizacion
	union all
	select Actividad, Precio, cast(Cantidad as varchar(20)) NumeroDias, Subtotal Total from CotizacionInspeccions where IdCotizacion = @idCotizacion
	union all
	select Descripcion, Precio, case when NumeroDias = 0 then '-' else CAST(numerodias as varchar(20)) end NumeroDias, Total from CotizacionResumen where IdCotizacion = @idCotizacion          

	select CAST(id as varchar(20)) + '.- ' + Resumen Resumen, Precio, NumeroDias, Total from @resumen
	order by id asc
END
GO
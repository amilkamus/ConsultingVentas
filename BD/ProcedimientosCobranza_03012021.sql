if object_id('usp_ObtenerCertificadosCotizacion', 'P') is not null
	drop procedure usp_ObtenerCertificadosCotizacion
go
create procedure usp_ObtenerCertificadosCotizacion
	@IdCotizacion bigint
as
begin
	select ID, IdCotizacion, isnull(Documento, '') Documento, isnull(NormaReferencia, '') NormaReferencia, isnull(Precio, 0) Precio  
	from CotizacionCertificadoes 
	where IdCotizacion = @IdCotizacion
end
go

if object_id('usp_ObtenerInspeccionesCotizacion', 'P') is not null
	drop procedure usp_ObtenerInspeccionesCotizacion
go
create procedure usp_ObtenerInspeccionesCotizacion
	@IdCotizacion bigint
as
begin
	select ID, IdCotizacion, Actividad, 
		isnull(Procedimiento, '') Procedimiento,
		isnull(ReferenciaNormativa, '') ReferenciaNormativa, 
		isnull(ReferenciaMuestreo, '') ReferenciaMuestreo, 
		isnull(PlanMuestreo, '') PlanMuestreo, 
		isnull(LugarMuestreo, '') LugarMuestreo,
		Precio, Cantidad, Subtotal
	from CotizacionInspeccions 
	where IdCotizacion = @IdCotizacion
end
go

if object_id('usp_ObtenerResumenesCotizacion', 'P') is not null
	drop procedure usp_ObtenerResumenesCotizacion
go
create procedure usp_ObtenerResumenesCotizacion
	@IdCotizacion bigint
as
begin
	select ID, IdCotizacion,
		isnull(Descripcion, '') Descripcion,
		Precio, NumeroDias, Total
	from CotizacionResumen 
	where IdCotizacion = @IdCotizacion
end
go
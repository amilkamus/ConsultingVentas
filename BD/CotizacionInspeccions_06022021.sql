if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionInspeccions' and COLUMN_NAME = 'Producto')
	alter table CotizacionInspeccions	add Producto varchar(500)
go
if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionInspeccions' and COLUMN_NAME = 'Documento')
	alter table CotizacionInspeccions	add Documento varchar(500)
go
if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionInspeccions' and COLUMN_NAME = 'TipoServicio')
	alter table CotizacionInspeccions	add TipoServicio varchar(20)
go

update CotizacionInspeccions set producto = '' where producto is null
update CotizacionInspeccions set documento = '' where documento is null
update CotizacionInspeccions set tiposervicio = '' where tiposervicio is null
go

alter procedure usp_ObtenerInspeccionesCotizacion
	@IdCotizacion bigint
as
begin
	select ID, IdCotizacion, 
		Producto,
		Actividad, 
		isnull(Procedimiento, '') Procedimiento,
		Documento,
		isnull(ReferenciaNormativa, '') ReferenciaNormativa, 
		isnull(ReferenciaMuestreo, '') ReferenciaMuestreo, 
		isnull(PlanMuestreo, '') PlanMuestreo, 
		TipoServicio,
		isnull(LugarMuestreo, '') LugarMuestreo,
		Precio, Cantidad, Subtotal
	from CotizacionInspeccions 
	where IdCotizacion = @IdCotizacion
end
go
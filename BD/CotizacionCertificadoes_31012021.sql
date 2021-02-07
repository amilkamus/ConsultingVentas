if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionCertificadoes' and COLUMN_NAME = 'TipoServicio')
	alter table CotizacionCertificadoes	add TipoServicio varchar(20)
go

if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionCertificadoes' and COLUMN_NAME = 'Cantidad')
	alter table CotizacionCertificadoes	add Cantidad int
go

if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'CotizacionCertificadoes' and COLUMN_NAME = 'SubTotal')
	alter table CotizacionCertificadoes	add SubTotal decimal(15, 2)
go

update CotizacionCertificadoes set Cantidad = 0 where Cantidad is null
update CotizacionCertificadoes set SubTotal = 0 where SubTotal is null
update CotizacionCertificadoes set TipoServicio = '' where tiposervicio is null
go


alter procedure usp_ObtenerCertificadosCotizacion
	@IdCotizacion bigint
as
begin
	select ID, IdCotizacion, isnull(Documento, '') Documento, isnull(NormaReferencia, '') NormaReferencia, 
	isnull(Precio, 0) Precio , isnull(TipoServicio, '') TipoServicio, isnull(Cantidad, 0) Cantidad, isnull(SubTotal, 0) SubTotal
	from CotizacionCertificadoes 
	where IdCotizacion = @IdCotizacion
end
go
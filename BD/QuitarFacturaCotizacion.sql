if object_id('QuitarFacturaCotizacion', 'P') is not null	
	drop procedure QuitarFacturaCotizacion
go
create procedure QuitarFacturaCotizacion
	@serieNumeroReferenciado varchar(20)
as
begin
	update cotizacions set SerieNumero = '' where SerieNumero = @serieNumeroReferenciado
end
go
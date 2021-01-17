ALTER procedure [dbo].[usp_ObtenerCobranza]
	@IdCotizacion bigint
as
begin
	select c.IdCotizacion, EjecutivoVenta, convert(varchar(10), cast(com.FechaEmison as date), 103) FechaIngreso, FechaPago, Detraccion, FechaPago1, Importe1, 
			FechaPago2, Importe2, FechaPago3, Importe3, PagoDetraccion, Observacion1, Autodetraccion,  Saldo, NroOperacion, CodigoInterno
	from Cobranzas c
	inner join Cotizacions co on c.IdCotizacion = co.ID
	inner join FacturacionElectronicaDB.dbo.Comprobante com on com.SerieNumero = co.SerieNumero and com.TipoComprobante = '01'
	where c.IdCotizacion = @IdCotizacion
end
go
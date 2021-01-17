ALTER procedure [dbo].[usp_ObtenerCobranza]
	@IdCotizacion bigint
as
begin

	if exists(select 1 from Cobranzas where IdCotizacion = @IdCotizacion)
	begin

		select c.IdCotizacion, EjecutivoVenta, convert(varchar(10), cast(com.FechaEmison as date), 103) FechaIngreso, FechaPago, Detraccion, FechaPago1, Importe1, 
				FechaPago2, Importe2, FechaPago3, Importe3, PagoDetraccion, Observacion1, Autodetraccion,  Saldo, NroOperacion, CodigoInterno
		from Cobranzas c
		inner join Cotizacions co on c.IdCotizacion = co.ID
		inner join FacturacionElectronicaDB.dbo.Comprobante com on com.SerieNumero = co.SerieNumero and com.TipoComprobante = '01'
		where c.IdCotizacion = @IdCotizacion

	end
	else
	begin

		select c.ID IdCotizacion, '' EjecutivoVenta, convert(varchar(10), cast(com.FechaEmison as date), 103) FechaIngreso, '' FechaPago, 0.0 Detraccion, '' FechaPago1, 0.0 Importe1, 
				'' FechaPago2, 0.0 Importe2, '' FechaPago3, 0.0 Importe3, cast(0 as bit) PagoDetraccion, '' Observacion1, cast(0 as bit) Autodetraccion,  0.0 Saldo, '' NroOperacion, '' CodigoInterno
		from Cotizacions c 
		inner join FacturacionElectronicaDB.dbo.Comprobante com on com.SerieNumero = c.SerieNumero and com.TipoComprobante = '01'
		where c.ID = @IdCotizacion
		

	end
end
go
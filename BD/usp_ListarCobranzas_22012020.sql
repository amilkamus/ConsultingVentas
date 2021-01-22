alter procedure [dbo].[usp_ListarCobranzas]
	--@NumeroOrdenServicio varchar(20) = '',
	@NumeroCotizacion varchar(200) = '',
	@RucSolicitante varchar(20) = '',
	@NombreSolicitante varchar(200) = '',
	@FechaInicio varchar(10) = '01/11/2000',
	@FechaFin varchar(10) = '31/12/2099',
	@DescripcionProducto varchar(200) = '',
	@ObservacionesProducto varchar(200) = '',
	@NombreContacto varchar(200) = '',
	@SerieNumero varchar(12) = '', 
	@IdUsuarioRegistro  nvarchar(128) = '0'
as
begin
	set dateformat dmy
	set language 'spanish'
	select
		co.ID IdCotizacion,
		DATENAME(MONTH, cast(com.FechaEmison as date)) Mes,
		case co.TipoCotizacion when 'AMB' then 'Ambiental' else 'Alimentos' end TipoCotizacion,
		os.NumeroOrdenServicio,
		co.NumeroCotizacion,
		c.EjecutivoVenta,
		co.Solicitante,
		co.DescripcionProducto,
		co.Contacto,
		co.Email,
		co.Telefono,		
		co.SerieNumero,
		co.CondicionPago_1,
		co.CondicionPago_2,
		convert(varchar(10), cast(com.FechaEmison as date), 103) FechaIngreso,
		c.FechaPago,
		co.Total,
		c.Detraccion,		
		c.FechaPago1,
		c.Importe1,
		c.FechaPago2,
		c.Importe2,	
		c.FechaPago3,
		c.Importe3,	
		case c.PagoDetraccion when 1 then 'SI' when 0 then 'NO' else '-1' end PagoDetraccion,
		c.Saldo,
		c.Observacion1,		
		c.Autodetraccion,		
		c.NroOperacion,
		c.CodigoInterno,
		co.IGV,
		co.SubTotalFinal
	from 
	Cobranzas c 
	inner join Cotizacions co on c.IdCotizacion = co.ID
	inner join FacturacionElectronicaDB.dbo.Comprobante com on com.SerieNumero = co.SerieNumero and com.TipoComprobante = '01'	
	left join OrdenServicios os on os.NumeroCotizacion = co.NumeroCotizacion
	where
			--os.NumeroOrdenServicio like @NumeroOrdenServicio + '%' and 
			co.NumeroCotizacion like @NumeroCotizacion + '%'
		and RUC like @RucSolicitante + '%'
		and Solicitante like @NombreSolicitante + '%'
		and cast(c.FechaRegistro as date) between cast(@FechaInicio as date) and cast(@FechaFin as date)
		and DescripcionProducto like @DescripcionProducto + '%'
		and c.Observacion1 like @ObservacionesProducto + '%'
		and Contacto like @NombreContacto + '%'
		and co.SerieNumero like @SerieNumero + '%'
		and (c.IdUsuarioRegistro = @IdUsuarioRegistro or '0' = @IdUsuarioRegistro)
	order by c.FechaRegistro desc
end
go
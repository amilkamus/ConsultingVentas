if object_id('Cobranzas', 'U') is null
	create table Cobranzas
	(
		IdCobranza bigint identity(1, 1) primary key,
		IdCotizacion bigint,
		OS varchar(20),
		EjecutivoVenta varchar(400),
		FechaIngreso varchar(20),
		FechaPago varchar(20),
		Detraccion decimal(15, 2),
		TotalConDetraccion decimal(15,2),
		FechaPago1 varchar(20),
		Importe1 decimal(15,2),
		FechaPago2 varchar(20),
		Importe2 decimal(15,2),
		ImporteDebe2 decimal(15,2),
		PagoDetraccion bit,
		Observacion1 varchar(max),
		Observacion2 varchar(max),
		Autodetraccion varchar(20),
		Tipo varchar(20),
		Saldo varchar(20),
		IdUsuarioRegistro nvarchar(128),
		FechaRegistro datetime,
		IdUsuarioModificacion nvarchar(128),
		FechaModificacion datetime
	)
go

if object_id('usp_RegistrarCobranza', 'P') is not null
	drop procedure usp_RegistrarCobranza
go
create procedure usp_RegistrarCobranza
	@IdCotizacion bigint,
	@OS varchar(20) = '',
	@EjecutivoVenta varchar(400) = '',
	@FechaIngreso varchar(20) = '',
	@FechaPago varchar(20) = '',
	@Detraccion decimal(15, 2),
	@TotalConDetraccion decimal(15,2) = 0,
	@FechaPago1 varchar(20) = '',
	@Importe1 decimal(15,2) = 0,
	@FechaPago2 varchar(20) = '',
	@Importe2 decimal(15,2) = 0,
	@ImporteDebe2 decimal(15,2) = 0,
	@PagoDetraccion bit,
	@Observacion1 varchar(max) = '',
	@Observacion2 varchar(max) = '',
	@Autodetraccion varchar(20) = '',
	@Tipo varchar(20) = '',
	@Saldo varchar(20) = '',
	@IdUsuario nvarchar(128)
as
begin

	if not exists(select 1 from Cobranzas where IdCotizacion =  @IdCotizacion)
	begin
		insert into Cobranzas (IdCotizacion, OS, EjecutivoVenta, FechaIngreso, FechaPago, Detraccion, TotalConDetraccion, FechaPago1, Importe1, 
		FechaPago2, Importe2, ImporteDebe2, PagoDetraccion, Observacion1, Observacion2, Autodetraccion, Tipo, Saldo, IdUsuarioRegistro, FechaRegistro)
		values (@IdCotizacion, @OS, @EjecutivoVenta, @FechaIngreso, @FechaPago, @Detraccion, @TotalConDetraccion, @FechaPago1, @Importe1, 
		@FechaPago2, @Importe2, @ImporteDebe2, @PagoDetraccion, @Observacion1, @Observacion2, @Autodetraccion, @Tipo, @Saldo, @IdUsuario, GETDATE())
	end
	else
	begin
		update Cobranzas set			
			OS = @OS, 
			EjecutivoVenta = @EjecutivoVenta, 
			FechaIngreso = @FechaIngreso, 
			FechaPago = @FechaPago, 
			Detraccion = @Detraccion, 
			TotalConDetraccion = @TotalConDetraccion, 
			FechaPago1 = @FechaPago1, 
			Importe1 = @Importe1, 
			FechaPago2 = @FechaPago2, 
			Importe2 = @Importe2, 
			ImporteDebe2 = @ImporteDebe2, 
			PagoDetraccion = @PagoDetraccion, 
			Observacion1 = @Observacion1, 
			Observacion2 = @Observacion2, 
			Autodetraccion = @Autodetraccion, 
			Tipo = @Tipo, 
			Saldo = @Saldo, 
			IdUsuarioModificacion = @IdUsuario, 
			FechaModificacion = GETDATE() 
		where IdCotizacion = @IdCotizacion
	end
end
go

if object_id('usp_ObtenerCobranza', 'P') is not null
	drop procedure usp_ObtenerCobranza
go
create procedure usp_ObtenerCobranza
	@IdCotizacion bigint
as
begin
	select IdCotizacion, OS, EjecutivoVenta, FechaIngreso, FechaPago, Detraccion, TotalConDetraccion, FechaPago1, Importe1, 
			FechaPago2, Importe2, ImporteDebe2, PagoDetraccion, Observacion1, Observacion2, Autodetraccion, Tipo, Saldo
	from Cobranzas 
	where IdCotizacion = @IdCotizacion
end
go

if object_id('usp_ListarCobranzas', 'P') is not null
	drop procedure usp_ListarCobranzas
go
create procedure usp_ListarCobranzas
	@NumeroOrdenServicio varchar(20) = '',
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
		DATENAME(MONTH, c.FechaIngreso) Mes,
		case co.TipoCotizacion when 'AMB' then 'Ambiental' else 'Alimentos' end TipoCotizacion,
		os.NumeroOrdenServicio,
		co.NumeroCotizacion,
		c.EjecutivoVenta,
		co.Solicitante,
		co.DescripcionProducto,
		co.Contacto,
		co.Email,
		co.Telefono,
		c.OS,
		co.SerieNumero,
		co.CondicionPago_1,
		co.CondicionPago_2,
		c.FechaIngreso,
		c.FechaPago,
		co.Total,
		c.Detraccion,
		c.TotalConDetraccion,
		c.FechaPago1,
		c.Importe1,
		c.FechaPago2,
		c.Importe2,
		c.ImporteDebe2,
		case c.PagoDetraccion when 1 then 'SI' else 'NO' end PagoDetraccion,
		case c.Saldo when 'Cancelo' then 'Canceló' else 'En Proceso' end Saldo,		
		c.Observacion1,
		c.Observacion2,
		c.Autodetraccion,
		c.Tipo
	from 
	Cobranzas c 
	inner join Cotizacions co on c.IdCotizacion = co.ID
	left join OrdenServicios os on os.NumeroCotizacion = co.NumeroCotizacion
	where
			os.NumeroOrdenServicio like @NumeroOrdenServicio + '%'
		and co.NumeroCotizacion like @NumeroCotizacion + '%'
		and RUC like @RucSolicitante + '%'
		and Solicitante like @NombreSolicitante + '%'
		and cast(c.FechaRegistro as date) between cast(@FechaInicio as date) and cast(@FechaFin as date)
		and DescripcionProducto like @DescripcionProducto + '%'
		and Observaciones like @ObservacionesProducto + '%'
		and Contacto like @NombreContacto + '%'
		and SerieNumero like @SerieNumero + '%'
		and (c.IdUsuarioRegistro = @IdUsuarioRegistro or '0' = @IdUsuarioRegistro)
	order by c.FechaRegistro desc
end
go
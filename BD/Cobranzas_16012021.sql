if object_id('Cobranzas', 'U') is not null
	drop table Cobranzas
go
create table Cobranzas
(
	IdCobranza bigint identity(1, 1) primary key,
	IdCotizacion bigint,
	NroOperacion varchar(400),
	CodigoInterno varchar(400),
	EjecutivoVenta varchar(400),
	FechaIngreso varchar(20),
	FechaPago varchar(20),
	Detraccion decimal(15, 2),
	FechaPago1 varchar(20),
	Importe1 decimal(15,2),
	FechaPago2 varchar(20),
	Importe2 decimal(15,2),
	FechaPago3 varchar(20),
	Importe3 decimal(15,2),	
	PagoDetraccion bit,
	Observacion1 varchar(max),	
	Autodetraccion bit,	
	Saldo decimal(15,2),
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
	@NroOperacion varchar(400) = '',
	@CodigoInterno varchar(400) = '',
	@EjecutivoVenta varchar(400) = '',
	@FechaIngreso varchar(20) = '',
	@FechaPago varchar(20) = '',
	@Detraccion decimal(15, 2),	
	@FechaPago1 varchar(20) = '',
	@Importe1 decimal(15,2) = 0,
	@FechaPago2 varchar(20) = '',
	@Importe2 decimal(15,2) = 0,
	@FechaPago3 varchar(20) = '',
	@Importe3 decimal(15,2) = 0,	
	@PagoDetraccion bit,
	@Observacion1 varchar(max) = '',	
	@Autodetraccion bit,	
	@Saldo decimal(15,2) = 0,
	@IdUsuario nvarchar(128)
as
begin

	if @Importe1 = 0
		set @FechaPago1 = ''

	if @Importe2 = 0
		set @FechaPago2 = ''

	if @Importe3 = 0
		set @FechaPago3 = ''

	if not exists(select 1 from Cobranzas where IdCotizacion =  @IdCotizacion)
	begin
		insert into Cobranzas (IdCotizacion, NroOperacion, CodigoInterno, EjecutivoVenta, FechaIngreso, FechaPago, Detraccion, FechaPago1, Importe1, 
		FechaPago2, Importe2, FechaPago3, Importe3, PagoDetraccion, Observacion1, Autodetraccion, Saldo, IdUsuarioRegistro, FechaRegistro)
		values (@IdCotizacion, @NroOperacion, @CodigoInterno, @EjecutivoVenta, @FechaIngreso, @FechaPago, @Detraccion, @FechaPago1, @Importe1, 
		@FechaPago2, @Importe2, @FechaPago3, @Importe3, @PagoDetraccion, @Observacion1, @Autodetraccion, @Saldo, @IdUsuario, GETDATE())
	end
	else
	begin
		update Cobranzas set
			NroOperacion = @NroOperacion,
			CodigoInterno = @CodigoInterno,
			EjecutivoVenta = @EjecutivoVenta, 
			FechaIngreso = @FechaIngreso, 
			FechaPago = @FechaPago, 
			Detraccion = @Detraccion, 			
			FechaPago1 = @FechaPago1, 
			Importe1 = @Importe1, 
			FechaPago2 = @FechaPago2, 
			Importe2 = @Importe2, 
			FechaPago3 = @FechaPago3, 
			Importe3 = @Importe3, 
			PagoDetraccion = @PagoDetraccion, 
			Observacion1 = @Observacion1, 			
			Autodetraccion = @Autodetraccion, 			
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
	select IdCotizacion, EjecutivoVenta, FechaIngreso, FechaPago, Detraccion, FechaPago1, Importe1, 
			FechaPago2, Importe2, FechaPago3, Importe3, PagoDetraccion, Observacion1, Autodetraccion,  Saldo, NroOperacion, CodigoInterno
	from Cobranzas 
	where IdCotizacion = @IdCotizacion
end
go

if object_id('usp_ListarCobranzas', 'P') is not null
	drop procedure usp_ListarCobranzas
go
create procedure usp_ListarCobranzas
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
		DATENAME(MONTH, c.FechaIngreso) Mes,
		case co.TipoCotizacion when 'AMB' then 'Ambiental' else 'Alimentos' end TipoCotizacion,
		--os.NumeroOrdenServicio,
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
		c.FechaIngreso,
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
		c.CodigoInterno
	from 
	Cobranzas c 
	inner join Cotizacions co on c.IdCotizacion = co.ID
	--left join OrdenServicios os on os.NumeroCotizacion = co.NumeroCotizacion
	where
			--os.NumeroOrdenServicio like @NumeroOrdenServicio + '%' and 
			co.NumeroCotizacion like @NumeroCotizacion + '%'
		and RUC like @RucSolicitante + '%'
		and Solicitante like @NombreSolicitante + '%'
		and cast(c.FechaRegistro as date) between cast(@FechaInicio as date) and cast(@FechaFin as date)
		and DescripcionProducto like @DescripcionProducto + '%'
		and c.Observacion1 like @ObservacionesProducto + '%'
		and Contacto like @NombreContacto + '%'
		and SerieNumero like @SerieNumero + '%'
		and (c.IdUsuarioRegistro = @IdUsuarioRegistro or '0' = @IdUsuarioRegistro)
	order by c.FechaRegistro desc
end
go
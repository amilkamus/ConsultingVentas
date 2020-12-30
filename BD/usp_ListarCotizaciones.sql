if object_id('usp_ListarCotizaciones', 'P') is not null
	drop procedure usp_ListarCotizaciones
go
create procedure usp_ListarCotizaciones
	@TipoCotizacion varchar(20) = '0',
	@NumeroCotizacion varchar(200) = '',
	@RucSolicitante varchar(20) = '',
	@NombreSolicitante varchar(200) = '',
	@FechaInicio varchar(10) = '01/01/2000',
	@FechaFin varchar(10) = '31/12/2099',
	@DescripcionProducto varchar(200) = '',
	@ObservacionesProducto varchar(200) = '',
	@NombreContacto varchar(200) = '',
	@SerieNumero varchar(12) = '', 
	@IdUsuarioRegistro  nvarchar(128) = '0'
as
begin
	set nocount on
	set dateformat dmy

	select
		C.ID,
		case TipoCotizacion when 'ALI' then 'Alimentos' when 'AMB' then 'Ambiental' END TipoCotizacion, 
		NumeroCotizacion, 
		RUC, 
		Solicitante, 
		Fecha, 
		DescripcionProducto, 
		Observaciones, 
		Contacto, 
		isnull(SerieNumero, '') SerieNumero, 
		CONCAT(FirstName, ' ', LastName) UsuarioRegistro
	from Cotizacions C inner join AspNetUsers U on c.IdUsuarioRegistro = u.Id
	where
			(TipoCotizacion =  @TipoCotizacion or '0' = @TipoCotizacion)
		and NumeroCotizacion like @NumeroCotizacion + '%'
		and RUC like @RucSolicitante + '%'
		and Solicitante like @NombreSolicitante + '%'
		and cast(Fecha as date) between cast(@FechaInicio as date) and cast(@FechaFin as date)
		and DescripcionProducto like @DescripcionProducto + '%'
		and Observaciones like @ObservacionesProducto + '%'
		and Contacto like @NombreContacto + '%'
		and SerieNumero like @SerieNumero + '%'
		and (IdUsuarioRegistro = @IdUsuarioRegistro or '0' = @IdUsuarioRegistro)
	order by c.FechaRegistro desc
end
go
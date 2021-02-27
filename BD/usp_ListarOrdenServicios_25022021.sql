alter procedure usp_ListarOrdenServicios
	@NumeroOrdenServicio varchar(20) = '',
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
		O.ID,
		O.NumeroOrdenServicio,
		C.NumeroCotizacion, 
		RUC, 
		Solicitante, 
		Fecha, 
		DescripcionProducto, 
		Observaciones, 
		ISNULL(o.ObservacionesInforme, '') ObservacionesInforme,
		CONCAT(FirstName, ' ', LastName) UsuarioRegistro,
		C.Total TotalCotizacion
	from OrdenServicios O 
		inner join Cotizacions C on C.NumeroCotizacion = O.NumeroCotizacion
		inner join AspNetUsers U on C.IdUsuarioRegistro = U.Id
	where
			o.NumeroOrdenServicio like @NumeroOrdenServicio + '%'
		and C.NumeroCotizacion like @NumeroCotizacion + '%'
		and RUC like @RucSolicitante + '%'
		and Solicitante like @NombreSolicitante + '%'
		and cast(Fecha as date) between cast(@FechaInicio as date) and cast(@FechaFin as date)
		and isnull(DescripcionProducto,'') like @DescripcionProducto + '%'
		and isnull(Observaciones, '') like @ObservacionesProducto + '%'
		and isnull(Contacto,'') like @NombreContacto + '%'
		and isnull(SerieNumero, '') like @SerieNumero + '%'
		and (O.IdUsuarioRegistro = @IdUsuarioRegistro or '0' = @IdUsuarioRegistro)
	order by o.FechaRegistro desc
end
GO
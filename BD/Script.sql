IF OBJECT_ID('[dbo].[ParametroProducto]', 'U') IS NOT NULL
	DROP TABLE [dbo].[ParametroProducto]
GO
CREATE TABLE [dbo].[ParametroProducto](
	[idProducto] [bigint] NULL,
	[idParametro] [bigint] NULL
) ON [PRIMARY]
GO

IF OBJECT_ID('usp_InsertarParametrosProducto', 'P') IS NOT NULL
	DROP PROCEDURE usp_InsertarParametrosProducto
GO
IF OBJECT_ID('usp_ListarParametrosProducto', 'p') IS NOT NULL
	DROP PROCEDURE usp_ListarParametrosProducto
GO

IF EXISTS(SELECT 1 FROM SYS.TYPES WHERE [name] = 'typeParametroProducto')
	DROP TYPE typeParametroProducto
GO
CREATE TYPE [dbo].[typeParametroProducto] AS TABLE(
	[Activo] [bit] NULL,
	[ID] [bigint] NULL,
	[CodParametro] [bigint] NULL,
	[ParametroDescripcion] [varchar](500) NULL,
	[Metodologia] [varchar](500) NULL,
	[Precio] [decimal](18, 0) NOT NULL
)
GO

create procedure [dbo].[usp_InsertarParametrosProducto]
	@idProducto bigint,
	@parametroProducto typeParametroProducto readonly
as
begin
	delete ParametroProducto where idProducto = @idProducto
	
	insert into ParametroProducto(idProducto, idParametro)
	select @idProducto, ID from @parametroProducto where Activo = 1
end
GO

CREATE procedure [dbo].[usp_ListarParametrosProducto]
	@idProducto bigint
as
begin
	select 1 Activo, p.ID idParametro, CodParametro, ParametroDescripcion, Metodologia, Precio, tp.ID idTipoParametro, @idProducto idProducto
	from ParametroProducto pp 
	inner join Parametroes p on pp.idParametro = p.ID
	inner join TipoParametroes tp on tp.TipoParametroDescripcion = p.Estado
	where idProducto = @idProducto
	union all
	select 0 Activo, p.ID, CodParametro, ParametroDescripcion, Metodologia, Precio, tp.ID idTipoParametro, @idProducto idProducto
	from Parametroes P
	inner join TipoParametroes tp on tp.TipoParametroDescripcion = p.Estado
	where p.ID not in (select idParametro from ParametroProducto where idProducto = @idProducto)
	order by CodParametro
end
GO


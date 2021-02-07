if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'OrdenServicios' and COLUMN_NAME = 'TipoEmpaqueEnvase')
	alter table OrdenServicios	add TipoEmpaqueEnvase varchar(500)
go

update OrdenServicios set TipoEmpaqueEnvase = '' where TipoEmpaqueEnvase is null
go
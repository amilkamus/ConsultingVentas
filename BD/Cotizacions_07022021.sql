if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'DBO' and TABLE_NAME = 'Cotizacions' and COLUMN_NAME = 'LugarInspeccionMuestreo')
	alter table Cotizacions	add LugarInspeccionMuestreo varchar(500)
go
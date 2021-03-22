alter table Cotizacions add FacturacionRUC varchar(20)
go

alter table Cotizacions add FacturacionRazonSocial varchar(500)
go

alter table Cotizacions add FacturacionCorreo varchar(100)
go

select top 0 * from Cotizacions
go
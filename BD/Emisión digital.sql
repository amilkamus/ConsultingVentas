alter table Cotizacions add EmisionDigital bit 
go

update Cotizacions set EmisionDigital = ISNULL(EmisionDigital, 0)
go
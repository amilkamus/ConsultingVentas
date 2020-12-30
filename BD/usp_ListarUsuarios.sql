if object_id('usp_ListarUsuarios', 'P') is not null
	drop procedure usp_ListarUsuarios
go
create procedure usp_ListarUsuarios
as
begin
	select ID, CONCAT(FirstName, ' ', LastName) Nombre from AspNetUsers
	order by 2 asc
end
go
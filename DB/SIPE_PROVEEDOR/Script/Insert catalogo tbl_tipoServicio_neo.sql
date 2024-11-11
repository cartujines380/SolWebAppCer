declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_tipoServicio_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_tipoServicio_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'1','servicio 1','A',''),
		  (@v_max,'2','servicio 2','A','')		

end
	
go

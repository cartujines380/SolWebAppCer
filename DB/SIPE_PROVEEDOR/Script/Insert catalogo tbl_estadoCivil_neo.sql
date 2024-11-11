declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_estadoCivil_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_estadoCivil_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'2452','CASADO/A','A',''),
		  (@v_max,'2453','DIVORCIADO/A','A',''),
		  (@v_max,'2455','SOLTERO/A','A',''),
		  (@v_max,'2456','UNIDO/A','A',''),
		  (@v_max,'2457','VIUDO/A','A','')

end
	
go

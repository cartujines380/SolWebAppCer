declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_regimenMatrimonial_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_regimenMatrimonial_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'2159','SOCIEDAD CONYUGAL','A',''),
		  (@v_max,'2160','CAPITULACION MATRIMONIAL','A',''),	  
		  (@v_max,'2161','DISOLUCIÓN CONYUGAL','A','')

end
	
go

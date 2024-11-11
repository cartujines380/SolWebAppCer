declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_relacionDepenLaboral_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_relacionDepenLaboral_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'1949','Profesional Independiente','A',''),
		  (@v_max,'1950','Independiente con actividad comercial','A',''),	  
		  (@v_max,'4630','Dependiente','A',''),	  
		  (@v_max,'4632','No Labora','A','')

end
	
go

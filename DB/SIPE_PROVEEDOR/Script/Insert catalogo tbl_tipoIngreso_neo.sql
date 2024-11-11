declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_tipoIngreso_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_tipoIngreso_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'3046','Estudiante','A',''),
		  (@v_max,'4621','Independiente','A',''),	  
		  (@v_max,'4622','Jubilado/Pensionista o Estudiante','A',''),	  
		  (@v_max,'4623','Empleado Público','A',''),
		  (@v_max,'4624','Empleado Privado','A',''),
		  (@v_max,'4625','Ama de Casa','A',''),
		  (@v_max,'4626','Rentista','A',''),
		  (@v_max,'4627','Remesas del Exterior','A','')

end
	
go

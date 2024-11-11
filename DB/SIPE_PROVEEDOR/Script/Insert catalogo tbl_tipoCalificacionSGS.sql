declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_tipoCalificacionSGS') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_tipoCalificacionSGS','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'JC','Persona jurídica crítica','A',''),
		  (@v_max,'JNC','Persona jurídica no crítica','A',''),
		  (@v_max,'NC','Persona natural crítica','A',''),		  
		  (@v_max,'NNC','Persona natural no crítica','A','')

end
	
go

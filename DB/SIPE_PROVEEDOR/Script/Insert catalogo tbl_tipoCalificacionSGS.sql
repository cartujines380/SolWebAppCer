declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_tipoCalificacionSGS') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_tipoCalificacionSGS','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'JC','Persona jur�dica cr�tica','A',''),
		  (@v_max,'JNC','Persona jur�dica no cr�tica','A',''),
		  (@v_max,'NC','Persona natural cr�tica','A',''),		  
		  (@v_max,'NNC','Persona natural no cr�tica','A','')

end
	
go

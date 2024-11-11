declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_puntajeCalificacionSGS') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_puntajeCalificacionSGS','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'A','A','A',''),
		  (@v_max,'B-','B-','A',''),
		  (@v_max,'B+','B+','A',''),		  
		  (@v_max,'C','C','A',''),		  
		  (@v_max,'D','D','A','')

end
	
go

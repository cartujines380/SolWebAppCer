declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_procesoSoporte') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_procesoSoporte','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'P1','Intercambia información de datos sensibles, tales como: personales, tarjetahabientes, transaccionales u otros','A',''),
	      (@v_max,'P2','Presta un servicio que se conecta a la red del banco','A',''),		  
		  (@v_max,'P3','Presta su servicio en la nube (SaaS, IaaS, PaaS)','A',''),
		  (@v_max,'P4','Transportación de valores','A',''),
		  (@v_max,'P0','Otros','A','')

	

end
	
go

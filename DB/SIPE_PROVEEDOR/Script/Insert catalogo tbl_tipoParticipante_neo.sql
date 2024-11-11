declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_tipoParticipante_neo') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_tipoParticipante_neo','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'349','TITULAR','A',''),
		  (@v_max,'350','CONYUGUE','A',''),	  
		  (@v_max,'351','MENOR DE EDAD','A',''),	  
		  (@v_max,'352','FIRMA AUTORIZADA','A',''),
		  (@v_max,'353','REPRESENTANTE','A',''),
		  (@v_max,'354','GARANTE','A',''),
		  (@v_max,'355','CYG GARANTE','A',''),
		  (@v_max,'356','ACCIONISTA','A',''),
		  (@v_max,'357','REPRESENTANTE LEGAL','A',''),
		  (@v_max,'523','TODOS','A',''),
		  (@v_max,'3110','CYG REPRESENTANTE LEGAL','A',''),
		  (@v_max,'4737','SOCIO','A',''),
		  (@v_max,'9503','TITULAR ADICIONAL','A',''),
		  (@v_max,'19544','BENEFICIARIO','A',''),
		  (@v_max,'20506','COTITULAR','A','')

end
	
go

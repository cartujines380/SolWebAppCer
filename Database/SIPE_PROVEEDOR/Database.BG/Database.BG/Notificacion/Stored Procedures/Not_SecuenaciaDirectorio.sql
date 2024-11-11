


CREATE PROCEDURE [Notificacion].[Not_SecuenaciaDirectorio]
          @PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
    
	Declare @desDirectorio VARCHAR(100)
	Declare @codSap VARCHAR(18)
    select  @desDirectorio=nref.value('@desDirectorio','Varchar(100)'),
	        @codSap = nref.value('@codSap','Varchar(18)') 
    from @PI_ParamXML.nodes('/Root/SecDirectorio') AS R(nref) 
	Declare @idSecuencial bigint
	


		 SELECT @idSecuencial = [Secuencia]  
         FROM  [Notificacion].[Directorio] where [Descripcion] = @desDirectorio 


		 --@idSecuencial = @idSecuencial + 1
		 update [Notificacion].[Directorio]
		 set [Secuencia] = @idSecuencial +1 
		 where [Descripcion] = @desDirectorio

		 SELECT [Secuencia]  
         FROM  [Notificacion].[Directorio] where [Descripcion] = @desDirectorio 
		 
		 if @codSap != '' 
		 begin
		    select Pais, Region from Proveedor.pro_proveedor where CodProveedor = @codSap
		 end


		 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_SecuenaciaDirectorio]'
END CATCH




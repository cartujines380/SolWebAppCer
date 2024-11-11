


CREATE PROCEDURE [Notificacion].[Not_ConsultaClasificados]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @TipoLista varchar(1)
    

	select  @TipoLista=nref.value('@TipoLista','varchar(1)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
		
		SELECT ROW_NUMBER() OVER(ORDER BY FechaPublicacion desc) AS NumClasificado
      ,[Cargo]
      ,[Ciudad]
      ,[FechaPublicacion]
  FROM [SIPE_PROVEEDOR].[Notificacion].[Clasificado]
	    	
		
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaClasificados]'
END CATCH




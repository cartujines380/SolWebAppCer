



CREATE PROCEDURE [Notificacion].[Not_Agrupa_Notificacion]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @idNot1 int
	Declare @idNot2 int
	
    select  @idNot1=nref.value('@CodNotificacion1','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @idNot2=nref.value('@CodNotificacion2','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
	        update Notificacion.Notificacion
			     set CodAgrupacion = @idNot2, Estado = 'E'	, FecEnvio = GETDATE()
				 				    
					where Codigo = @idNot1					 

			update Notificacion.Notificacion
			     set CodAgrupacion = @idNot1, Estado = 'E'	, FecEnvio = GETDATE()					    
					where Codigo = @idNot2	
				
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_Agrupa_Notificacion]'
END CATCH




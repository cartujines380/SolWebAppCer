



CREATE PROCEDURE [Notificacion].[Not_Registra_Mensaje]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
    Declare @nombre varchar(60)
	Declare @correo varchar(60)
	Declare @telefono varchar(20)
	Declare @celular varchar(20)
	Declare @productos varchar(200)
	Declare @mensaje varchar(200)

	select  @nombre=nref.value('@Usuario','varchar(60)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

    select  @correo=nref.value('@Correo','varchar(60)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @telefono=nref.value('@Telefono','varchar(20)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @celular=nref.value('@Celular','varchar(20)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @productos=nref.value('@Productos','varchar(200)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @mensaje=nref.value('@Mensaje','varchar(200)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref)  

begin
	   
	  --insert into [Notificacion].[RegistroProveedor] values (@nombre,@correo,@telefono,@celular,@productos,GETDATE())			 
	  insert into [Notificacion].[Mensaje] values (@nombre,@correo,@productos,GETDATE(),@telefono)				
				
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_Registra_Mensaje]'
END CATCH








CREATE PROCEDURE [Notificacion].[Not_Actualiza_Notificacion]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @tipoNotificacion varchar(1)
	Declare @ruc varchar(13)
	Declare @codProv varchar(10)
	Declare @estado varchar(1)
	Declare @idNot int
	Declare @usuario varchar(20)
	
    select  @idNot=nref.value('@CodNotificacion','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @ruc=nref.value('@CodProveedor','varchar(13)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	select  @usuario=nref.value('@Usuario','varchar(20)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
	             SELECT @codProv = CodProveedor
				FROM  [Proveedor].[Pro_Proveedor] 
				where Ruc = @ruc

				 SELECT @estado = Estado
				FROM  [Notificacion].[Notificacion_Proveedor] 
				where Cod_proveedor = @codProv
				and Cod_Notificacion = @idNot
				and Usuario = @usuario	             
				 
				 select @tipoNotificacion = tipo
				 from Notificacion.Notificacion
				 where [Codigo] = @idNot

				 if @estado = 'L'
				   begin
				     update Notificacion.Notificacion_Proveedor
					  set estado = 'X',
					    FecAceptacion = getdate()
					 where Cod_notificacion = @idNot					 
					  and Cod_proveedor = @codProv and Usuario = @usuario
				   end   
				 else
				  begin
				 if @tipoNotificacion = 'T' or @tipoNotificacion = 'L'
				 begin
				    insert into Notificacion.Notificacion_Proveedor(Cod_notificacion, Cod_proveedor, FecAceptacion, Estado, Usuario) 
					values (@idNot, @codProv, getdate(), 'L' , @usuario)
					
				 end
				 if @tipoNotificacion = 'I'
				 begin
				    update Notificacion.Notificacion_Proveedor
					set estado = 'L',
					    FecAceptacion = getdate()
					where Cod_notificacion = @idNot					 
					and Cod_proveedor = @codProv and Usuario = @usuario
				 end 
				end
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_Actualiza_Notificacion]'
END CATCH





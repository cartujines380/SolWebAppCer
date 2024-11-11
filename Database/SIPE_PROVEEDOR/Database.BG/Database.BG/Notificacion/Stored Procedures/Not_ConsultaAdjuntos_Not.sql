



CREATE PROCEDURE [Notificacion].[Not_ConsultaAdjuntos_Not]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @idNot int
	Declare @tipoC varchar(1)
    select  @idNot=nref.value('@CodNotificacion','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	select @tipoC= TipoCorreo 
	from Notificacion.Notificacion 
	where   codigo = @idNot
	

begin
	  if @tipoC = 'G' 
	  begin
		   Select top 1	[Id_Notificacion]
		  ,[NomArchivo] 
		  FROM [Notificacion].[Adjunto] 
		  where [Id_Notificacion] = @idNot  and Comunicado = 'S'
		  order by [Comunicado] desc
	  end
	  else
	  begin
		  Select	[Id_Notificacion]
		  ,[NomArchivo] 
		  FROM [Notificacion].[Adjunto] 
		  where [Id_Notificacion] = @idNot 
		  order by [Comunicado] desc
	  end

	  Select	[Cod_Notificacion]
	  ,[Cod_Linea]
     
	  FROM [Notificacion].[Notificacion_LineaNegocio] 
	  where [Cod_Notificacion] = @idNot 
      
	  select Cod_Departamento 
	  from Notificacion.Notificacion_Departamento where  Cod_Notificacion = @idNot

	  select Cod_Zona as IdZona 
	  from Notificacion.Notificacion_Zona where  Cod_Notificacion = @idNot

	  select CodRol as CodRol 
	  from Notificacion.Not_Rol where  Cod_Notificacion = @idNot

	  select CodDepartamento ,
	         CodFuncion
	  from Notificacion.Not_DepFuncion where  Cod_Notificacion = @idNot
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaAdjuntos_Not]'
END CATCH





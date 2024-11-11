


CREATE PROCEDURE [Notificacion].[Not_ConsultaNotificacion]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @estadoNot varchar(1)
    

	select  @estadoNot=nref.value('@EstadoNotificacion','varchar(1)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
		if (@estadoNot ='T')
		begin    
		SELECT [Codigo] , [Titulo] ,[Comunicado] , CONVERT(VARCHAR(15),FechaVencimiento,101) as FechaVencimiento
             , (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8001 and codigo = [Categoria]) as Categoria ,
			 (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8002 and codigo = [Prioridad]) as Prioridad ,
			 Case [Obligatorio] When 'S' then 'Si' else 'No' End as Obligatorio, CONVERT(varchar(1), isnull(Corporativo,'0')) as Corporativo,
			 [Tipo] ,[UsrIngreso], TipoCorreo , MsjCorreo, CONVERT(VARCHAR(15),FechaPublicacion,101) as FechaPublicacion
           ,[FecIngreso] ,[UsrAprobacion] ,[FecAprobacion] ,[FecEnvio], 
		    (select detalle from [Proveedor].[Pro_Catalogo] a where a.tabla = 8003 and a.codigo = b.estado) as Estado, Ruta, Observacion
            FROM [Notificacion].[Notificacion] b with(nolock) 
			where b.UsrIngreso <> 'NOT_PEDIDOS'
			order by FecIngreso desc
	    end
		if (@estadoNot !='T')
		begin
		SELECT [Codigo] , [Titulo] ,[Comunicado] ,CONVERT(VARCHAR(15),FechaVencimiento,101) as FechaVencimiento
             , (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8001 and codigo = [Categoria]) as Categoria ,
			 (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8002 and codigo = [Prioridad]) as Prioridad ,
			 Case [Obligatorio] When 'S' then 'Si' else 'No' End as Obligatorio, CONVERT(varchar(1), isnull(Corporativo,'0')) as Corporativo,
			 [Tipo] ,[UsrIngreso],TipoCorreo , MsjCorreo, CONVERT(VARCHAR(15),FechaPublicacion,101) as FechaPublicacion
           ,[FecIngreso] ,[UsrAprobacion] ,[FecAprobacion] ,[FecEnvio], 
		    (select detalle from [Proveedor].[Pro_Catalogo] a where a.tabla = 8003 and a.codigo = b.estado) as Estado, Ruta, Observacion
            FROM [Notificacion].[Notificacion] b with(nolock) 
			where b.Estado = @estadoNot 
			and b.UsrIngreso <> 'NOT_PEDIDOS'
			order by FecIngreso desc
			
	    end
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaNotificacion]'
END CATCH




-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Notificacion].[Not_P_GrabaNotificacion] 
	 @PI_ParamXML xml
	
AS
BEGIN try
	begin tran
	DECLARE @string VARCHAR(max);
     Declare @IdSolicitud bigint

SET @string = CAST(@PI_ParamXML AS VARCHAR(max));
	SET @string  = REPLACE(@string,'¥',',');
	SET @PI_ParamXML = CONVERT(XML, @string);

select top 1  @IdSolicitud=nref.value('@Codigo','bigint')
from @PI_ParamXML.nodes('/Root/NewNotificacion') AS R(nref) 


  begin

	     if (@IdSolicitud != 0)
		 begin
		    update a set
			     Titulo  =  nref.value('@Titulo','VARCHAR(MAX)')                  ,
			     Comunicado  = nref.value('@Comunicado','VARCHAR(MAX)')  ,
				 FechaVencimiento	=	nref.value('@FechaVencimiento','DATE')	,
				 FechaPublicacion	=	nref.value('@FechaPublicacion','DATE')	,
				 Categoria   =   nref.value('@Categoria','VARCHAR(MAX)')          ,
				 Prioridad   =   nref.value('@Prioridad','VARCHAR(MAX)') ,
				 Obligatorio = nref.value('@Obligatorio','VARCHAR(MAX)'), 
				 Corporativo = Convert(bit, nref.value('@Corporativo','VARCHAR(1)')), 
				 Tipo = nref.value('@Tipo','VARCHAR(MAX)'),
				 UsrModifica = nref.value('@Usuario','VARCHAR(50)'),
				 Estado = 'I',				
				 MsjCorreo = nref.value('@MensajeCorreo','VARCHAR(MAX)'), 
				
				 FecModifica = getdate()

				from [Notificacion].[Notificacion] a 
				inner join  @PI_ParamXML.nodes('/Root/NewNotificacion') AS R(nref) on   a.Codigo= nref.value('@Codigo','bigint')

				delete [Notificacion].[Adjunto] where Id_Notificacion = @IdSolicitud

				delete [Notificacion].[Notificacion_Proveedor] where Cod_Notificacion = @IdSolicitud
				delete [Notificacion].[Notificacion_LineaNegocio] where Cod_Notificacion = @IdSolicitud
				delete [Notificacion].[Notificacion_Departamento] where Cod_Notificacion = @IdSolicitud
				delete [Notificacion].[Notificacion_Zona] where Cod_Notificacion = @IdSolicitud
				delete [Notificacion].[Not_DepFuncion] where Cod_Notificacion = @IdSolicitud
				delete [Notificacion].[Not_Rol] where Cod_Notificacion = @IdSolicitud
				insert into [Notificacion].[Adjunto](
						 Id_Notificacion                      ,NomArchivo , Comunicado  
						         	)

		select  @IdSolicitud, nref.value('@Adjunto','VARCHAR(100)'), nref.value('@Comunicado','VARCHAR(1)')
		FROM @PI_ParamXML.nodes('/Root/NewListaAdjuntos') AS R(nref) 
		insert into [Notificacion].[Notificacion_Proveedor](
						 Cod_Notificacion                      ,Cod_Proveedor, Estado, Usuario
						         	)

		select  @IdSolicitud, nref.value('@CodProveedor','VARCHAR(10)'), 'I','0000000000'
		FROM @PI_ParamXML.nodes('/Root/NewListaProveedores') AS R(nref) 

		insert into [Notificacion].[Notificacion_Departamento](
						 Cod_Notificacion                      ,Cod_Departamento, Fecha
						         	)
		select  @IdSolicitud, nref.value('@CodigoDepartamento','VARCHAR(10)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaDepartamentos') AS R(nref) 

		insert into [Notificacion].[Notificacion_Zona](
						 Cod_Notificacion                      ,Cod_Zona, Fecha
						         	)
		select  @IdSolicitud, nref.value('@CodigoZona','VARCHAR(12)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaZonas') AS R(nref) 

		insert into [Notificacion].[Notificacion_LineaNegocio](
						 Cod_Notificacion                      ,Cod_Linea, Fecha
						         	)
		select  @IdSolicitud, nref.value('@Codigo','VARCHAR(10)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/NewListaLinNegocios') AS R(nref) 
		select @IdSolicitud

		insert into [Notificacion].[Not_Rol](
						 Cod_Notificacion                      ,CodRol, FechaRegistro
						         	)
		
		select  @IdSolicitud, nref.value('@CodRol','VARCHAR(20)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaRoles') AS R(nref) 

		insert into [Notificacion].[Not_DepFuncion](
						 Cod_Notificacion , 
						 CodDepartamento, 
						 CodFuncion,
						 Fecha)	 
		 select  @IdSolicitud, 
		        nref.value('@CodDepartamento','VARCHAR(20)'), 
				nref.value('@CodFuncion','VARCHAR(20)'), 
				GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaDepFunciones') AS R(nref) 


		
		 end
		 
		 if (@IdSolicitud = 0)
		 begin
		 insert into [Notificacion].[Notificacion](
						 Titulo                      ,Comunicado    ,FechaVencimiento
						,Categoria               ,Prioridad     ,Obligatorio, Tipo, Estado,FecIngreso, Ruta, CodAgrupacion, UsrIngreso, 
						 TipoCorreo, MsjCorreo,   FechaPublicacion, Corporativo
						         	)

		select  nref.value('@Titulo','VARCHAR(MAX)'),nref.value('@Comunicado','VARCHAR(MAX)'), nref.value('@FechaVencimiento','DATE'),
				nref.value('@Categoria','VARCHAR(MAX)'),nref.value('@Prioridad','VARCHAR(MAX)'),nref.value('@Obligatorio','VARCHAR(MAX)'),
				nref.value('@Tipo','VARCHAR(MAX)'), 'I', getdate(), nref.value('@Ruta','VARCHAR(100)'), 0,nref.value('@Usuario','VARCHAR(50)'),
				nref.value('@TipoCorreo','VARCHAR(1)'), nref.value('@MensajeCorreo','VARCHAR(MAX)'), nref.value('@FechaPublicacion','DATE') ,
				Convert(bit, nref.value('@Corporativo','VARCHAR(1)')) 
		FROM @PI_ParamXML.nodes('/Root/NewNotificacion') AS R(nref) 
		select @IdSolicitud=@@IDENTITY
		insert into [Notificacion].[Adjunto](
						 Id_Notificacion                      ,NomArchivo  , Comunicado  
						         	)

		select  @IdSolicitud, nref.value('@Adjunto','VARCHAR(100)'), nref.value('@Comunicado','VARCHAR(1)')
		FROM @PI_ParamXML.nodes('/Root/NewListaAdjuntos') AS R(nref) 
		insert into [Notificacion].[Notificacion_Proveedor](
						 Cod_Notificacion                      ,Cod_Proveedor, Estado, Usuario
						         	)

		select  @IdSolicitud, nref.value('@CodProveedor','VARCHAR(10)'), 'I',  '0000000000'
		FROM @PI_ParamXML.nodes('/Root/NewListaProveedores') AS R(nref) 

		insert into [Notificacion].[Notificacion_LineaNegocio](
						 Cod_Notificacion                      ,Cod_Linea, Fecha
						         	)

		select  @IdSolicitud, nref.value('@Codigo','VARCHAR(10)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/NewListaLinNegocios') AS R(nref) 

		insert into [Notificacion].[Notificacion_Departamento](
						 Cod_Notificacion                      ,Cod_Departamento, Fecha
						         	)
         

		select  @IdSolicitud, nref.value('@CodigoDepartamento','VARCHAR(10)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaDepartamentos') AS R(nref) 
		

		insert into [Notificacion].[Notificacion_Zona](
						 Cod_Notificacion                      ,Cod_Zona, Fecha
						         	)
		select  @IdSolicitud, nref.value('@CodigoZona','VARCHAR(12)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaZonas') AS R(nref) 

		insert into [Notificacion].[Not_Rol](
						 Cod_Notificacion                      ,CodRol, FechaRegistro
						         	)
		
		select  @IdSolicitud, nref.value('@CodRol','VARCHAR(20)'), GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaRoles') AS R(nref) 

		insert into [Notificacion].[Not_DepFuncion](
						 Cod_Notificacion , 
						 CodDepartamento, 
						 CodFuncion,
						 Fecha)	 
		 select  @IdSolicitud, 
		        nref.value('@CodDepartamento','VARCHAR(20)'), 
				nref.value('@CodFuncion','VARCHAR(20)'), 
				GETDATE()
		FROM @PI_ParamXML.nodes('/Root/ListaDepFunciones') AS R(nref) 


		select @IdSolicitud

		end
	

end 


IF @@TRANCOUNT > 0
			COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Not_P_GrabaNotificacion]'
END CATCH



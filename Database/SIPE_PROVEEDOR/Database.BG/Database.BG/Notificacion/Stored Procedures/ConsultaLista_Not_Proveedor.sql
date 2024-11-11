

CREATE PROCEDURE [Notificacion].[ConsultaLista_Not_Proveedor](
@PI_ParamXML xml
)
AS

BEGIN TRY
	
	Declare @codProveedor varchar(10)
	Declare @ruc varchar(13)
	Declare @usr varchar(20)
    select  @ruc=nref.value('@Ruc','varchar(13)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	select  @usr=nref.value('@Usuario','varchar(20)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

	begin
		  Select  @codProveedor = [CodProveedor]
		  FROM [Proveedor].[Pro_Proveedor]
		  where [Ruc] = @ruc 
	  
		  SELECT [Codigo] , [Titulo] ,[Comunicado] ,
				  CASE CONVERT(VARCHAR(15),FechaVencimiento,103) 
				  WHEN '01/01/1900' THEN  CONVERT(VARCHAR(15),FecIngreso,103)
				  ELSE  CONVERT(VARCHAR(15),FechaVencimiento,103) END
				  as FechaVencimiento
				 , b.TipoCorreo as Categoria ,
				 (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8002 and codigo = [Prioridad]) as Prioridad ,
				 Case [Obligatorio] When 'S'  then 'Si' When 'X' then 'Oculto' else 'No' End as Obligatorio,			
				Case b.TipoCorreo  when 'G' then 'Leído' Else
				Case (select c.estado from Notificacion.Notificacion_Proveedor c where c.Cod_Notificacion = b.Codigo and c.Cod_proveedor = @codProveedor and c.Usuario = @usr) 
				when 'L' then 'Leído' When 'X' then 'Oculto' else 'Pendiente' End End
				as Estado, b.Ruta, b.FechaPublicacion
				FROM [Notificacion].[Notificacion] b 
				where b.Estado = 'E' and b.Tipo = 'T' 
				--validar fecha de creacion de proveedor
				--and 'X' = (select 'X' from Proveedor.Pro_Proveedor where CodProveedor = @codProveedor
				--           and FechaCertifica <= b.FecEnvio   )
				--validar fecha de creacion de usuario
				and 'X' = (select 'X' from Seguridad.Seg_Usuario where  Ruc = @ruc 
						  and Usuario = @usr 
						  --and FechaRegistro <= b.FechaPublicacion 
				)
				and b.TipoCorreo in ('N', 'G')
				and GETDATE()  >= b.FechaPublicacion
				-- Solo permitir consultar notificacion N dias atras
				and  Convert(date,b.FechaPublicacion,103) >= Convert(date, (select top 1 Case  codigo
																	When 'M' then  Getdate() - (Detalle * 30 )
																	When 'D' then Getdate()  - (Detalle * 1)
																	Else Getdate() - 1000
																	End as numDias
																	from Proveedor.Pro_Catalogo where Tabla = 8006), 103)
			  and b.UsrIngreso <> 'NOT_PEDIDOS'
		 union
		 SELECT [Codigo] , [Titulo] ,[Comunicado] ,
				 CASE CONVERT(VARCHAR(15),FechaVencimiento,103) 
				  WHEN '01/01/1900' THEN  CONVERT(VARCHAR(15),FecIngreso,103)
				  ELSE  CONVERT(VARCHAR(15),FechaVencimiento,103) END
				  as FechaVencimiento
				 , b.TipoCorreo as Categoria ,
				 (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8002 and codigo = [Prioridad]) as Prioridad ,
				 Case [Obligatorio] When 'S' then 'Si' else 'No' End as Obligatorio,			
			  Case b.TipoCorreo  when 'G' then 'Leído' Else
				Case c.Estado when 'L' then 'Leído' When 'X' then 'Oculto' else 'Pendiente' End End as Estado, b.Ruta, b.FechaPublicacion
				FROM [Notificacion].[Notificacion] b , Notificacion.Notificacion_Proveedor c
				where b.Estado = 'E' and b.Tipo = 'I'
				and b.Codigo = c.Cod_Notificacion			
				and c.Cod_proveedor = @codProveedor 
				and c.Usuario = @usr
				and b.TipoCorreo in ('N', 'G')
				and GETDATE()  >= b.FechaPublicacion
				-- Solo permitir consultar notificacion N dias atras
				and  Convert(date,b.FechaPublicacion,103) >= Convert(date, (select top 1 Case  codigo
																	When 'M' then  Getdate() - (Detalle * 30 )
																	When 'D' then Getdate()  - (Detalle * 1)
																	Else Getdate() - 1000
																	End as numDias
																	from Proveedor.Pro_Catalogo where Tabla = 8006), 103)
				and b.UsrIngreso <> 'NOT_PEDIDOS'
		 union
		 SELECT distinct [Codigo] , [Titulo] ,[Comunicado] ,
				CASE CONVERT(VARCHAR(15),FechaVencimiento,103) 
				  WHEN '01/01/1900' THEN  CONVERT(VARCHAR(15),FecIngreso,103)
				  ELSE  CONVERT(VARCHAR(15),FechaVencimiento,103) END
				  as FechaVencimiento
				 , b.TipoCorreo as Categoria ,
				 (select detalle from [Proveedor].[Pro_Catalogo] where tabla = 8002 and codigo = [Prioridad]) as Prioridad ,
				 Case [Obligatorio] When 'S'  then 'Si' When 'X' then 'Oculto' else 'No' End as Obligatorio,			
			  Case b.TipoCorreo  when 'G' then 'Leído' Else
				Case (select c.estado from Notificacion.Notificacion_Proveedor c where c.Cod_Notificacion = b.Codigo and c.Cod_proveedor = @codProveedor and c.Usuario = @usr) 
				when 'L' then 'Leído' When 'X' then 'Oculto' else 'Pendiente' End End
				as Estado, b.Ruta, b.FechaPublicacion
				FROM [Notificacion].[Notificacion] b , Notificacion.Notificacion_LineaNegocio l
				where b.Estado = 'E' and b.Tipo = 'L' 
				and b.Codigo = l.Cod_Notificacion
				and l.Cod_Linea in (select CodLineaNegocio from Proveedor.Proveedor_LineaNegocio
									where CodProveedor = @codProveedor   )
				--validar fecha de creacion de proveedor
				--and 'X' = (select 'X' from Proveedor.Pro_Proveedor where CodProveedor = @codProveedor
				--           and FechaCertifica <= b.FecEnvio   )
				--validar fecha de creacion de usuario
				and 'X' = (select 'X' from Seguridad.Seg_Usuario where  Ruc = @ruc 
						  and Usuario = @usr 
						  --and FechaRegistro <= b.FechaPublicacion 
				)
				and b.TipoCorreo in ('N', 'G')
				and GETDATE()  >= b.FechaPublicacion
				-- Solo permitir consultar notificacion N dias atras
				and  Convert(date,b.FechaPublicacion,103) >= Convert(date, (select top 1 Case  codigo
																	When 'M' then  Getdate() - (Detalle * 30 )
																	When 'D' then Getdate()  - (Detalle * 1)
																	Else Getdate() - 1000
																	End as numDias
																	from Proveedor.Pro_Catalogo where Tabla = 8006), 103)
				and b.UsrIngreso <> 'NOT_PEDIDOS'
				--and c.Estado not in ('X')
				--order by b.FechaPublicacion desc
		union
			select Id, Titulo, Mensaje, CONVERT(VARCHAR(15),FechaCreacion,103),'M','Normal','No',
			(case when Estado = 'L' then 'Leido' else 'Pendiente' end) Estado,
			'', FechaCreacion FechaPublicacion
			from Proveedor.Pro_MensajesFlash b
			where Identificacion = @ruc
			order by b.FechaPublicacion desc
	end

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[ConsultaLista_Not_Proveedor]'
END CATCH



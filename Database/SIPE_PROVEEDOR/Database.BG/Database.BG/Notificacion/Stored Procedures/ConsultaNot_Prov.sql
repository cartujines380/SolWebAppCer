



CREATE PROCEDURE [Notificacion].[ConsultaNot_Prov]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
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
	  
	  SELECT b.Codigo , b.Titulo , b.Comunicado, b.Ruta, b.Obligatorio, b.CodAgrupacion, b.Prioridad as prio, b.FechaPublicacion
FROM  Notificacion.Notificacion b
			where not exists (SELECt * FROM Notificacion.Notificacion_Proveedor a 
			             where a.Cod_Notificacion = b.Codigo and a.Cod_proveedor=@codProveedor and a.Usuario = @usr)
			and b.Estado = 'E'
			and b.Tipo = 'T'
			and (b.FechaVencimiento >= SYSDATETIME() or b.FechaVencimiento = '1900-01-01')		
			--and 'X' = (select 'X' from Proveedor.Pro_Proveedor where CodProveedor = @codProveedor 
			--           and FechaCertifica <= b.FecEnvio   )	
			--validar fecha de creacion de usuario
			and 'X' = (select 'X' from Seguridad.Seg_Usuario where  Ruc = @ruc 
			          and Usuario = @usr and FechaRegistro <= b.FechaPublicacion )
			and b.TipoCorreo = 'N'
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
SELECT b.Codigo , b.Titulo , b.Comunicado, b.Ruta, b.Obligatorio, b.CodAgrupacion, b.Prioridad as prio, b.FechaPublicacion
FROM  Notificacion.Notificacion b, Notificacion.Notificacion_Proveedor a 
			where a.Cod_Notificacion = b.Codigo
			and a.Cod_proveedor = @codProveedor
			and a.Usuario = @usr
			and a.Estado = 'I'
			and b.Estado = 'E'
			and b.Tipo = 'I'
			and (b.FechaVencimiento >= SYSDATETIME() or b.FechaVencimiento = '1900-01-01')
			and b.TipoCorreo = 'N'
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
SELECT distinct b.Codigo , b.Titulo , b.Comunicado, b.Ruta, b.Obligatorio, b.CodAgrupacion, b.Prioridad as prio, b.FechaPublicacion
FROM  Notificacion.Notificacion b, Notificacion.Notificacion_LineaNegocio l
			where not exists (SELECt * FROM Notificacion.Notificacion_Proveedor a 
			             where a.Cod_Notificacion = b.Codigo and a.Cod_proveedor=@codProveedor and a.Usuario = @usr)
			and b.Estado = 'E'
			and b.Tipo = 'L'
			and (b.FechaVencimiento >= SYSDATETIME() or b.FechaVencimiento = '1900-01-01')
			and b.Codigo = l.Cod_Notificacion
			and l.Cod_Linea in (select CodLineaNegocio from Proveedor.Proveedor_LineaNegocio
			                    where CodProveedor = @codProveedor   )
			--and 'X' = (select 'X' from Proveedor.Pro_Proveedor where CodProveedor = @codProveedor
			--           and FechaCertifica <= b.FecEnvio   )	
			--validar fecha de creacion de usuario
			and 'X' = (select 'X' from Seguridad.Seg_Usuario where  Ruc = @ruc 
			          and Usuario = @usr and FechaRegistro <= b.FechaPublicacion )
			and b.TipoCorreo = 'N'
			and GETDATE()  >= b.FechaPublicacion
			-- Solo permitir consultar notificacion N dias atras
			and  Convert(date,b.FechaPublicacion,103) >= Convert(date, (select top 1 Case  codigo
																When 'M' then  Getdate() - (Detalle * 30 )
																When 'D' then Getdate()  - (Detalle * 1)
																Else Getdate() - 1000
																End as numDias
																from Proveedor.Pro_Catalogo where Tabla = 8006), 103)
			and b.UsrIngreso <> 'NOT_PEDIDOS'
			order by prio , b.FechaPublicacion desc
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[ConsultaNot_Prov]'
END CATCH





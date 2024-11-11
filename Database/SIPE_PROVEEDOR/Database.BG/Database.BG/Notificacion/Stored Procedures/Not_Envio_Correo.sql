


CREATE PROCEDURE [Notificacion].[Not_Envio_Correo]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	Declare @l_accion varchar(1)
	Declare @idNot int
	Declare @tipoNot varchar(1)
	Declare @tipoCorreo varchar(1)
	Declare @usuario varchar(20)

begin
	   SELECT 
		 @l_accion = nref.value('@ACCION','char(1)'),
		 @idNot = nref.value('@CODIGO','int'),
		 @tipoNot = nref.value('@TIPO','char(1)'),
		 @tipoCorreo = nref.value('@CORREO','char(1)')
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 
	
	select @usuario = UsrIngreso from Notificacion.Notificacion
	where Codigo = @idNot
	   

	    --Accion = 'X' Actualizar estado a enviado correo 
	   if @l_accion = 'X'
	   begin
	      update Notificacion.Notificacion
		  set CorreoEnviado = 'S',
		      FechaEnvio = GETDATE()
		  where Codigo = @idNot
	   end

	   --Accion = 'N' listado de notificaciones a enviar
	   if @l_accion = 'N'
	   begin
	      select * from Notificacion.Notificacion
		  where GETDATE() >= FechaPublicacion 
		  and CorreoEnviado  = 'N' and Estado = 'E' and TipoCorreo != 'G'
	   end
	  -- Accion = 'A' listado de destinatarios por codigo y tipo de notificacion
	  if @l_accion = 'A'
	  begin
	    if @tipoCorreo = 'S'
	     begin
		    select NomArchivo from Notificacion.Adjunto where Id_Notificacion = @idNot		   
		 end
		else
		 begin
		  select NomArchivo from Notificacion.Adjunto where Id_Notificacion = @idNot and Comunicado = 'S'		
		 end
	  end

	   -- Accion = 'L' listado de destinatarios por codigo y tipo de notificacion
	  if @l_accion = 'L'
	  begin
	      --Notificacion --
	     if @tipoCorreo = 'N'
	     begin
		   -- Enviar a todos los proveedores
		   if @tipoNot = 'T'
		    begin
						--Usuario CER
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
						where a.CodProveedor = b.CodProveedor
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
						and b.EsAdmin = 1
						UNION
						--Por zonas
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
						where a.CodProveedor = b.CodProveedor
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     

						UNION
						--Por departamento
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
						where a.CodProveedor = b.CodProveedor
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						 AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )
						
					UNION	
						--Roles																	
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )

						order by a.CodProveedor, b.CorreoE;
		     end
		   -- Enviar individualmente
		   if @tipoNot = 'I'
		    begin					
						
						--Usuario CER
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
						where a.CodProveedor = b.CodProveedor
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
						and b.EsAdmin = 1
						and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
												where  Cod_Notificacion = @idNot )
						UNION
						--Por zonas
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b

						where a.CodProveedor = b.CodProveedor
						and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
												where  Cod_Notificacion = @idNot )
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     

						UNION
						--Por departamento
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
						where a.CodProveedor = b.CodProveedor
						and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
												where  Cod_Notificacion = @idNot )
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						 AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )
					UNION
							--Roles																	
					 Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
												where  Cod_Notificacion = @idNot )
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )
					
						order by a.CodProveedor, b.CorreoE;
						
						
						
						--Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
						--				a.NomComercial,  b.Departamento,
						--				(select Detalle from Proveedor.Pro_Catalogo 
						--				 where tabla = 1033 and Codigo = b.Departamento ) as desDepartamento
						--				 from Proveedor.Pro_Proveedor a, Proveedor.Pro_ProveedorContacto b
						-- where a.CodProveedor = b.CodProveedor
						-- and b.Departamento in (select Cod_Departamento from Notificacion.Notificacion_Departamento 
						--						 where Cod_Notificacion = @idNot)
						-- and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
						--						where  Cod_Notificacion = @idNot )
						-- order by b.CorreoE;
			 end
		   -- Enviar por linea de negocio
		   if @tipoNot = 'L'
			   begin
						
						--Usuario CER
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
						where a.CodProveedor = b.CodProveedor and b.EsAdmin = 1
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
						and c.CodProveedor  = b.CodProveedor
						and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
												   where Cod_Notificacion = @idNot)
						
						UNION
						--Por zonas
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
						where a.CodProveedor = b.CodProveedor
						and c.CodProveedor  = b.CodProveedor
						and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
												   where Cod_Notificacion = @idNot)
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     

						UNION
						--Por departamento
						Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento
						from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
						where a.CodProveedor = b.CodProveedor
						and c.CodProveedor  = b.CodProveedor
						and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
												   where Cod_Notificacion = @idNot)
						AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
						 AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )
						
						UNION
						--Roles																	
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
										a.NomComercial, b.UsrCargo,  
										(select Detalle from Proveedor.Pro_Catalogo 
														where tabla = 1033 and Codigo = b.UsrCargo ) 
														 as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
									 where a.CodProveedor = b.CodProveedor 
									 and c.CodProveedor  = b.CodProveedor
									 and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
												   where Cod_Notificacion = @idNot)
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )

						order by a.CodProveedor, b.CorreoE;

					
						--Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
						--				a.NomComercial, b.Departamento,
						--				(select Detalle from Proveedor.Pro_Catalogo 
						--				 where tabla = 1033 and Codigo = b.Departamento ) as desDepartamento
						--from Proveedor.Pro_Proveedor a, Proveedor.Pro_ProveedorContacto b, Proveedor.Proveedor_LineaNegocio c
						--where a.CodProveedor = b.CodProveedor
						--and c.CodProveedor  = b.CodProveedor
						--and b.Departamento in (select Cod_Departamento from Notificacion.Notificacion_Departamento 
						--					   where Cod_Notificacion = @idNot)
						--and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
						--						   where Cod_Notificacion = @idNot)
						--order by b.CorreoE;
			   end
		 end

	      --Notificacion tipo correo -- 
	     if @tipoCorreo = 'S'
	     begin
			-- Enviar a todos 
			if @tipoNot = 'T' 
					begin
					  --Trae el correo de usuario CER
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor and b.EsAdmin = 1
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
					  UNION 
					  --Trae todos los correos por Zonas
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     
					  UNION
					  --Trae todos los correos por Deparatamento Y Funcion
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									  AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )
					   UNION
					     --Trae todos los correos por ROLES		 
						 Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )
									 
									 
									 order by a.CodProveedor , b.CorreoE
									 
					  
					  --Select top 1  'fcastro@sipecom.com' as CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
							--					a.NomComercial, '' as Departamento, '' as desDepartamento 
					  --from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
							--		 where a.CodProveedor = b.CodProveedor               
							--		 order by a.CodProveedor , b.CorreoE

					end     
			-- Enviar individualmente
			if @tipoNot = 'I' 
					
					begin
					  
					  If @usuario = 'NOT_PEDIDOS'
					  Begin
					       --Trae todos los correos por Zonas
						  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
													a.NomComercial, '' as Departamento, '' as desDepartamento 
						  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Notificacion.Notificacion_Proveedor n
										 where a.CodProveedor = b.CodProveedor
										 and n.Cod_proveedor = b.CodProveedor
										 and n.Usuario = b.Usuario
										 and n.Cod_Notificacion = @idNot
					  End
					  Else
					  Begin

					   --Trae el correo de usuario CER
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor and b.EsAdmin = 1
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
									 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
															where  Cod_Notificacion = @idNot )
					  UNION 
					  --Trae todos los correos por Zonas
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor
									 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
															where  Cod_Notificacion = @idNot ) 
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     
					  UNION
					  --Trae todos los correos por Deparatamento
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
															where  Cod_Notificacion = @idNot )
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )							    
					  
					  UNION
					     --Trae todos los correos por ROLES		 
						 Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b
									 where a.CodProveedor = b.CodProveedor 
									 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
															where  Cod_Notificacion = @idNot )
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )				 
					 
					 order by a.CodProveedor , b.CorreoE

					   --Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
								--					a.NomComercial, '' as Departamento, '' as desDepartamento 
								--					 from Proveedor.Pro_Proveedor a, Proveedor.Pro_ProveedorContacto b
								--	 where a.CodProveedor = b.CodProveedor
                  
								--	 and a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
								--							where  Cod_Notificacion = @idNot )
								--	 order by b.CorreoE
						end
					end  
			-- Enviar por linea de negocio
			if @tipoNot = 'L' 
					
					begin

					     --Trae el correo de usuario CER
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
									 where a.CodProveedor = b.CodProveedor and b.EsAdmin = 1
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 1  ))
									 and c.CodProveedor  = b.CodProveedor
									 and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
								 						       where Cod_Notificacion = @idNot)
					  UNION 
					  --Trae todos los correos por Zonas
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
									 where a.CodProveedor = b.CodProveedor
									 and c.CodProveedor  = b.CodProveedor
									 and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
								 						       where Cod_Notificacion = @idNot)  
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND EXISTS (SELECT TOP 1 1 FROM 
									             Seguridad.Seg_UsuarioZona uz WHERE 
												 uz.Usuario = b.Usuario
												 and Convert(int,uz.Zona) in (select Convert(int,nz.Cod_Zona) 
												                              from Notificacion.Notificacion_Zona nz where nz.Cod_Notificacion = @idNot))     
					  UNION
					  --Trae todos los correos por Deparatamento
					  Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
									 where a.CodProveedor = b.CodProveedor 
									 and c.CodProveedor  = b.CodProveedor 
									 and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
								 						       where Cod_Notificacion = @idNot)
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									AND 						  
									    (	b.UsrCargo in (select CodDepartamento from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)
										  AND B.UsrFuncion IN (select CodFuncion from Notificacion.Not_DepFuncion where Cod_Notificacion = @idNot)  )
					  
					   UNION
					     --Trae todos los correos por ROLES		 
						 Select Distinct b.CorreoE, a.NomComercial, a.CodProveedor,  a.Ruc, 
												a.NomComercial, '' as Departamento, '' as desDepartamento 
					  from Proveedor.Pro_Proveedor a, Seguridad.Seg_Usuario b, Proveedor.Proveedor_LineaNegocio c
									 where a.CodProveedor = b.CodProveedor 
									  and c.CodProveedor  = b.CodProveedor 
									 and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
								 						       where Cod_Notificacion = @idNot)
									 AND ( EXISTS(SELECT TOP 1 1 									 
									             FROM Notificacion.Notificacion nt where nt.Codigo = @idNot and  nt.Corporativo = 0  ))
									 AND ( EXISTS(SELECT TOP 1 1 FROM SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ru 
										  inner join  SIPE_FRAMEWORK.Participante.Par_Participante  pa on ru.IdUsuario = pa.IdUsuario
										  where pa.IdParticipante = b.IdParticipante 
										  and ru.IdRol in (select nr.CodRol from Notificacion.Not_Rol nr where nr.Cod_Notificacion = @idNot)
										  )
									   )
					  
					  order by a.CodProveedor , b.CorreoE
					   --Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
								--				a.NomComercial, '' as Departamento, '' as desDepartamento 
	                                
								--from Proveedor.Pro_Proveedor a, Proveedor.Pro_ProveedorContacto b, Proveedor.Proveedor_LineaNegocio c
								--where a.CodProveedor = b.CodProveedor
								--and c.CodProveedor  = b.CodProveedor                   
								-- and c.CodLineaNegocio in (select Cod_Linea from Notificacion.Notificacion_LineaNegocio 
								--						   where Cod_Notificacion = @idNot)
								--order by b.CorreoE
					end  
		    
		 end
	

	  end
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_Envio_Correo]'
END CATCH




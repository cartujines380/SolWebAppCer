USE [SIPE_PROVEEDOR]
GO

IF EXISTS (select 1 from sysobjects where name = 'Not_Actualiza_Estado' and type='P')
begin
    drop procedure  Notificacion.Not_Actualiza_Estado
    if exists (select 1 from sysobjects where name = 'Not_Actualiza_Estado' and type = 'P')
      PRINT '<<< DROP PROCEDURE Not_Actualiza_Estado -- ERROR -- >>>'
    else
      PRINT '== DROP PROCEDURE Not_Actualiza_Estado *OK* =='
end
GO

CREATE PROCEDURE [Notificacion].[Not_Actualiza_Estado]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @idNot int
	Declare @estadoNot varchar(1)
	Declare @tipoNot varchar(1)
	Declare @usr varchar(50)
	Declare @observacion varchar(200)
	declare @tipoCorreo varchar(1)
	DECLARE @string VARCHAR(max);

	SET @string = CAST(@PI_ParamXML AS VARCHAR(max));
	SET @string  = REPLACE(@string,'¥',',');
	SET @PI_ParamXML = CONVERT(XML, @string);

    select  @idNot=nref.value('@CodNotificacion','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @estadoNot=nref.value('@EstadoNotificacion','varchar(1)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @usr=nref.value('@Usuario','varchar(50)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @observacion=nref.value('@Observacion','varchar(200)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref)
	

begin
	   
	if(@estadoNot = 'A')
	begin
		update [Notificacion].[Notificacion]
		set	UsrAprobacion = @usr, FecAprobacion = GETDATE()
		where [Codigo] = @idNot
	end
	
	if(@estadoNot = 'E')
	begin
	    update [Notificacion].[Notificacion]
		set	FecEnvio = GETDATE()
		where [Codigo] = @idNot

		select @tipoNot = Tipo,
				@tipoCorreo = TipoCorreo
		from Notificacion.Notificacion 
		where Codigo = @idNot

		if (@tipoNot = 'I' and @tipoCorreo = 'N')
		begin
			insert into Notificacion.Notificacion_Proveedor (Usuario,  Cod_proveedor,Estado, Cod_Notificacion )
			select Usuario, CodProveedor,'I', @idNot
			from Seguridad.Seg_Usuario 
			where CodProveedor in (select a.Cod_proveedor from  Notificacion.Notificacion_Proveedor a 
								   where a.Cod_Notificacion = @idNot)
			and Estado = 'A'

            delete from Notificacion.Notificacion_Proveedor 
			where Cod_Notificacion = @idNot and Usuario = '0000000000'

		end

		select q.Titulo, q.Ruta, b.NomArchivo
		from Notificacion.Notificacion q 
			inner join Notificacion.Adjunto b
			on q.Codigo = b.Id_Notificacion
		where q.Codigo = @idNot
		and b.Comunicado = 'S'

		if  @tipoNot = 'T'
		begin
			if @tipoCorreo = 'S'
			begin
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
                       a.NomComercial, '' as Departamento, '' as desDepartamento 
									
                from Proveedor.Pro_Proveedor a, Proveedor.Pro_ProveedorContacto b
                where a.CodProveedor = b.CodProveedor  AND a.CodProveedor = '-1'                
				order by b.CorreoE;
			end
			else
			begin
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
				       a.NomComercial, b.Departamento,  
					   (select Detalle 
					    from Proveedor.Pro_Catalogo 
						where tabla = 1033 and Codigo = b.Departamento)  as desDepartamento
				from Proveedor.Pro_Proveedor a 
				inner join Proveedor.Pro_ProveedorContacto b
				on a.CodProveedor = b.CodProveedor
				where b.Departamento in (select Cod_Departamento from Notificacion.Notificacion_Departamento 
									   where Cod_Notificacion = @idNot)
				order by b.CorreoE
			end
		end
		if  @tipoNot = 'I'
		begin
			if @tipoCorreo = 'S'
			begin
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
				       a.NomComercial, '' as Departamento, '' as desDepartamento 
				from Proveedor.Pro_Proveedor a
				inner join Proveedor.Pro_ProveedorContacto b
				on a.CodProveedor = b.CodProveedor
				where a.CodProveedor in (select Cod_proveedor from Notificacion.Notificacion_Proveedor 
									where  Cod_Notificacion = @idNot )  
				order by b.CorreoE
			end
			else
			begin					
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
								a.NomComercial,  b.Departamento,
								(select Detalle from Proveedor.Pro_Catalogo 
								 where tabla = 1033 and Codigo = b.Departamento ) as desDepartamento
			    from Proveedor.Pro_Proveedor a
				inner join Proveedor.Pro_ProveedorContacto b
				
				on a.CodProveedor = b.CodProveedor
				where b.Departamento in (select Cod_Departamento 
										 from Notificacion.Notificacion_Departamento 
										 where Cod_Notificacion = @idNot)
				and a.CodProveedor in (select Cod_proveedor 
										from Notificacion.Notificacion_Proveedor 
										where  Cod_Notificacion = @idNot )
                order by b.CorreoE
			end
		end

		if  @tipoNot = 'L'
		begin
			if  @tipoCorreo = 'S'
			begin
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
					    a.NomComercial, '' as Departamento, '' as desDepartamento 
	                                
                from Proveedor.Pro_Proveedor a
				inner join Proveedor.Pro_ProveedorContacto b
				on a.CodProveedor = b.CodProveedor
				inner join Proveedor.Proveedor_LineaNegocio c
				on c.CodProveedor  = b.CodProveedor
                where c.CodLineaNegocio in (select Cod_Linea 
											from Notificacion.Notificacion_LineaNegocio 
											where Cod_Notificacion = @idNot)
				order by b.CorreoE
			end
			else
			begin
				Select Distinct b.CorreoE, b.Nombre, a.CodProveedor,  a.Ruc, 
								a.NomComercial, b.Departamento,
								(select Detalle from Proveedor.Pro_Catalogo 
									where tabla = 1033 and Codigo = b.Departamento ) as desDepartamento
				from Proveedor.Pro_Proveedor a
				inner join Proveedor.Pro_ProveedorContacto b
				on a.CodProveedor = b.CodProveedor
				inner join Proveedor.Proveedor_LineaNegocio c
				on c.CodProveedor  = b.CodProveedor
				where b.Departamento in (select Cod_Departamento 
										from Notificacion.Notificacion_Departamento 
										where Cod_Notificacion = @idNot)
				and c.CodLineaNegocio in (select Cod_Linea 
										from Notificacion.Notificacion_LineaNegocio 
										where Cod_Notificacion = @idNot)
				order by b.CorreoE
			end
		end

        select Tipo,
				TipoCorreo,
				Titulo,
				MsjCorreo,
				Ruta
		from Notificacion.Notificacion 
		where Codigo = @idNot
		select NomArchivo from Notificacion.Adjunto where Id_Notificacion = @idNot
				

	end
				 
	update [Notificacion].[Notificacion]
	set	
	Observacion = @observacion,		     
	Estado = @estadoNot
	where [Codigo] = @idNot
	
end

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_Actualiza_Estado]'
END CATCH



GO

if exists(select 1 from sysobjects where name='Not_Actualiza_Estado' and type = 'P')
  PRINT '== CREATE PROCEDURE Not_Actualiza_Estado *OK* =='
 else
  PRINT '<<< CREATE PROCEDURE Not_Actualiza_Estado -- ERROR -- >>>'
GO

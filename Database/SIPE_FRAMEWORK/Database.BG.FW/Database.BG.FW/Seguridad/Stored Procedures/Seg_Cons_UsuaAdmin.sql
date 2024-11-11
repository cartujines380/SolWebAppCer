CREATE procedure [Seguridad].[Seg_Cons_UsuaAdmin]
	( @PI_ParamXML xml )
AS
BEGIN

	DECLARE
		 @criterio		CHAR(1)
		,@nombre		VARCHAR(100)
		,@usuario		VARCHAR(60)

	SELECT
		@criterio		= nref.value('@CRITERIO','CHAR(1)'),
		@nombre			= nref.value('@NOMBRE','VARCHAR(100)'),
		@usuario		= nref.value('@USUARIO','VARCHAR(60)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @criterio='R'
	begin		
		select v.*,ISNULL(se.Usuario,0) esIngresado  from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados  v
		left join SIPE_PROVEEDOR.Seguridad.Seg_Empleado se on v.IdUsuario=se.Usuario
		where v.IdUsuario like '%'+@nombre+'%'
	END

	IF @criterio='N'
	begin		
		select v.*,ISNULL(se.Usuario,0) esIngresado  from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados  v
		left join SIPE_PROVEEDOR.Seguridad.Seg_Empleado se on v.IdUsuario=se.Usuario
		where v.Apellido1+' '+v.Apellido2+' '+v.Nombre1+' '+v.Nombre2 like '%'+@nombre+'%'
	END

	IF @criterio='T'
	begin
		select v.*,ISNULL(se.Usuario,0) esIngresado  from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados  v
		left join SIPE_PROVEEDOR.Seguridad.Seg_Empleado se on v.IdUsuario=se.Usuario
	END

END





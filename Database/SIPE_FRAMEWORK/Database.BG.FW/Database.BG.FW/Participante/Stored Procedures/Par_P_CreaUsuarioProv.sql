
CREATE Procedure [Participante].[Par_P_CreaUsuarioProv]
	(
		@IdProveedor bigint,
		@IdUsuario varchar(50),
		@Clave varchar(50) = null
	)
AS
	
IF NOT EXISTS(SELECT 1 FROM Participante.Par_RegistroCliente
				WHERE IdUsuario = @IdUsuario )
BEGIN
	insert into Participante.Par_RegistroCliente(IdUsuario,IdParticipante,RolAsignado,
								FechaRegistro,OrigenRegistro,Estado)
		VALUES (@IdUsuario,@IdProveedor,1,getdate(),'I',1)

	IF (@@error <> 0)
	BEGIN
	  RETURN
	END
	-- Actualiza los campos de tiempo de expira
	UPDATE Participante.Par_Participante
	SET IdPais = '593', IdProvincia='10-04',IdCiudad='81',
		FechaExpira='01-01-2999',TiempoExpira=0
	where idparticipante = @IdProveedor

	exec Seguridad.seg_p_setRol @IdUsuario,3 --Rol de Registro
	-- si es relacionado a proveedor se le asigna el rol =13, si es a cliente rol = 14
	IF EXISTS(SELECT 1 FROM Proveedores.Pro_ClienteProveedor cp 
								INNER JOIN Proveedores.Pro_Calificacion c
								ON cp.IdCalificacion = c.IdCalificacion
				WHERE cp.TipoRelacion = 'P' AND c.IdParticipante = @IdProveedor )
		exec Seguridad.seg_p_setRol @IdUsuario,13 --Rol de Proveedor
	ELSE
		exec Seguridad.seg_p_setRol @IdUsuario,14 --Rol de Cliente Adm.
END
/*ELSE
BEGIN
	UPDATE Participante.Par_RegistroCliente
	SET Clave = @Clave
	WHERE IdParticipante = @IdProveedor AND IdUsuario = @IdUsuario
END
*/



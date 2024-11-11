CREATE procedure [Seguridad].[Seg_p_QuitaRol]
@PI_IdUsuario varchar(50),
@PI_IdRol int
as
IF exists(SELECT 1 FROM Seguridad.Seg_RolUsuario 
		WHERE IdUsuario = @PI_IdUsuario AND IdRol = @PI_IdRol)
BEGIN
	DELETE Seguridad.Seg_RolUsuario
	WHERE IdUsuario = @PI_IdUsuario AND IdROl = @PI_IdRol
END






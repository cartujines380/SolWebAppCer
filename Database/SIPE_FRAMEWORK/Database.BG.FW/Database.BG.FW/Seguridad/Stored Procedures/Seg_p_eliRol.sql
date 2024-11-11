CREATE  procedure [Seguridad].[Seg_p_eliRol]
@PI_IdUsuario varchar(50),
@PI_IdRol int
as
IF exists(SELECT 1 FROM Seguridad.Seg_RolUsuario 
		WHERE IdUsuario = @PI_IdUsuario AND IdRol = @PI_IdRol)
BEGIN
	delete from Seguridad.Seg_RolUsuario
	where IdUsuario = @PI_IdUsuario
		and IdRol = @PI_IdRol
		
END




CREATE  procedure [Seguridad].[Seg_p_setRol]
@PI_IdUsuario varchar(50),
@PI_IdRol int
as
IF not exists(SELECT 1 FROM Seguridad.Seg_RolUsuario 
		WHERE IdUsuario = @PI_IdUsuario AND IdRol = @PI_IdRol)
BEGIN
	insert into Seguridad.Seg_RolUsuario(IdUsuario,IdRol,IdHorario,Estado,FechaInicial,FechaFinal)
	Values (@PI_IdUsuario,@PI_IdRol,1,'ACTIVE',getdate(),'2999-01-01')
END






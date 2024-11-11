

CREATE procedure [Seguridad].[Seg_p_EliminaUsuario]
	@PV_IdUsuario                 varchar(20)
AS

--Inicia la trnasaccion
BEGIN TRAN

-- Elimina RolUsuario
Delete Seguridad.Seg_RolUsuario 
FROM Seguridad.Seg_RolUsuario 
WHERE IdUsuario = @PV_IdUsuario

-- Elimina AutorizacionesUsuarios
Delete Seguridad.Seg_AutorizacionUsuario
FROM Seguridad.Seg_AutorizacionUsuario
WHERE IdUsuario = @PV_IdUsuario

-- Elimina TransaccionesUsuarios
Delete Seguridad.Seg_TransUsuario
FROM Seguridad.Seg_TransUsuario 
WHERE IdUsuario = @PV_IdUsuario

--Elimina Usuario
delete Seguridad.Seg_usuario
WHERE IdUsuario = @PV_IdUsuario

IF (@@error <> 0)
  ROLLBACK TRAN
ELSE
  COMMIT TRAN












create procedure [Seguridad].[Seg_P_ELIMINA_ROL]
@PV_idRol                    int

AS
 
   IF exists( select idrol from Seguridad.Seg_ROLUSUARIO
   			where IdRol=@PV_idRol)
	raiserror('Existen usuarios asociados a este rol',16,1)
   
--Inicia la trnasaccion
BEGIN TRAN

--ELimina los roles por transaccion
   DELETE FROM Seguridad.Seg_OpcionTransRol
               WHERE idrol=@PV_idRol
    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END

-- Elimina el Rol   
   DELETE FROM Seguridad.Seg_ROL
               WHERE idrol=@PV_idRol
     IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END

COMMIT TRAN











create procedure [Seguridad].[Seg_P_ELIMINA_ROLUSUARIO]
@PV_idRol           int,
    @PV_idUsuario         VARCHAR(20),
    @PV_idhorario         int
AS
     DELETE FROM Seguridad.Seg_ROLUSUARIO
     WHERE idrol=@PV_idRol AND idusuario=@PV_idUsuario
           And idHorario =@PV_idhorario
     --COMMIT
 






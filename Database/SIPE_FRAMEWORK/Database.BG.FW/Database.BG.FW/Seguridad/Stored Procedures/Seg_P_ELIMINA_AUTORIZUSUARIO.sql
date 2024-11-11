



create procedure [Seguridad].[Seg_P_ELIMINA_AUTORIZUSUARIO]
@PV_idAutorizacion      int,
        @PV_idUsuario           VARCHAR(20 )
AS

     DELETE FROM Seguridad.Seg_AUTORIZACIONUSUARIO
     WHERE idautorizacion=@PV_idAutorizacion AND idusuario=@PV_idUsuario
     --COMMIT







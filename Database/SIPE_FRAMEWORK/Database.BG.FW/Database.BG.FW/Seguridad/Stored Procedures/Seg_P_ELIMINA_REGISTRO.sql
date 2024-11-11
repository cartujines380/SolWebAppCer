



create procedure [Seguridad].[Seg_P_ELIMINA_REGISTRO]
@PV_idUsuario           VARCHAR(20 ),
  @PV_fechaIngreso        DATETIME
AS
     DELETE FROM Seguridad.Seg_REGISTRO
     WHERE idusuario=@PV_idUsuario AND
           fechaIngreso=@PV_fechaIngreso
     --COMMIT
 






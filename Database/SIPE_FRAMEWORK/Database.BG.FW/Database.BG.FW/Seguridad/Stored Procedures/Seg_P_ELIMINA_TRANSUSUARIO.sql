



create procedure [Seguridad].[Seg_P_ELIMINA_TRANSUSUARIO]
@PV_idTransaccion      int,
      @PV_idOpcion           int,
      @PV_idUsuario          VARCHAR(20 ),
      @PV_Organizacion       int,
      @PV_idhorario          int
AS 
     DELETE FROM Seguridad.Seg_TRANSUSUARIO
     WHERE IDORGANIZACION  = @PV_Organizacion and
           idtransaccion=@PV_idTransaccion AND
           idopcion=@PV_idOpcion           AND
           idusuario=@PV_idUsuario         AND
           IDHORARIO=@PV_idhorario
     --COMMIT
 






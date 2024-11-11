



create procedure [Seguridad].[Seg_P_ELEMINA_USUARIOREEMP]
@PV_idRol             int,
      @PV_idUsuario              VARCHAR(20 ),
      @PV_usrReemplazo           VARCHAR(20 )
AS
              
         DELETE FROM Seguridad.Seg_ROLUSUARIO
         Where idrol                =@PV_idRol
              And idusuario         =@PV_idUsuario
              And USRREEMPLAZO      =@PV_usrReemplazo
     --COMMIT
  









create procedure [Seguridad].[Seg_P_ACTUALIZA_USUARIOREEMP]
@PV_idRol             int,
      @PV_idUsuario              VARCHAR(20 ),
      @PV_fechaIncial            DATETIME,
      @PV_fechaFinal             DATETIME,
      @PV_usrReemplazo           VARCHAR(20 )
AS
   declare @lv_opcion int
   SET @lv_opcion= 0 
 
   
   Select @lv_opcion = count(idrol) 
        From  Seguridad.Seg_ROLUSUARIO
        Where idrol =               @PV_idRol
              And idusuario =       @PV_usrReemplazo
                 
        If ISNULL(@lv_opcion,0) = 0 
         BEGIN
              Insert Into Seguridad.Seg_ROLUSUARIO(IDROL,IDUSUARIO,IDHORARIO,
           ESTADO,FECHAINICIAL,FECHAFINAL,USRREEMPLAZO)
              Select @PV_idRol, @PV_usrReemplazo,IDHORARIO,'ACTIVE',@PV_fechaIncial,
                     @PV_fechaFinal,@PV_idUsuario
                  From Seguridad.Seg_ROLUSUARIO
                  Where idrol           = @PV_idRol 
                        AND idusuario   = @PV_idUsuario
                      
           --Inactiva los roles del usuario que se va    
          UPDATE Seguridad.Seg_ROLUSUARIO
              SET estado='INACTIVE',fechainicial =@PV_fechaIncial,fechafinal=@PV_fechaFinal
         WHERE idrol=@PV_idRol AND idusuario=@PV_idUsuario
         
        END            

     --COMMIT
   






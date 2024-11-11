


create procedure [Seguridad].[Seg_P_INGRESA_USUARIO]
 @PV_idUsuario             VARCHAR(20 ),
      @PV_Nombre              VARCHAR(100 ),
      @PV_TipoUser            VARCHAR(5 ),
      @PV_idEmpresa           int,
      @PV_Descripcion         VARCHAR(100 ),
      @PV_notificacion        VARCHAR( 15),
      @PV_correo              VARCHAR(100 ),
      @PV_fax                 VARCHAR(30 ),
      @PV_estado              VARCHAR(10 ),
      @PV_FechaExpira         DATETIME,         
      @PV_TiempoExpira        int,
      @PV_ruc                 VARCHAR(30 ),
      @PV_CheqEquipo          CHAR,
      @PV_Oficial             CHAR,
      @PV_NivelSeg            int,
      @PV_NivelAut            int,
      @PV_FechaAct            DATETIME
AS
  DECLARE @lv_existe            int
   set  @lv_existe= 0
   
          
       Select @lv_existe = count(*)  
       From Seguridad.Seg_usuario
       Where idusuario = @PV_idUsuario
       --and idempresa = @PV_idEmpresa
       
       IF @lv_existe = 0 
        BEGIN
           Insert Into Seguridad.Seg_usuario(idusuario,nombre,tipousuario,idempresa,
             descripcion,notificacion,correo,fax,estado,
             fechaexpira,tiempoexpira,ruc,cheqequipo,
             oficial,nivelseg,nivelaut,fechaact)
             
           Values(@PV_idUsuario,@PV_Nombre,@PV_TipoUser,@PV_idEmpresa,@PV_Descripcion,
                  @PV_notificacion,@PV_correo,@PV_fax,@PV_estado,@PV_FechaExpira,@PV_TiempoExpira,
                  @PV_ruc,@PV_CheqEquipo,@PV_Oficial,@PV_NivelSeg,@PV_NivelAut,
                  @PV_FechaAct)
           
           --COMMIT
         END 
        else
           raiserror ('Usuario ya Existe',16,1)
 






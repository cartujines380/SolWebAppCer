



/*==============================================================*/
/* MANTENIMIENTO: Usuario      */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_ACTUALIZA_USUARIO]
@PV_idUsuario             VARCHAR(20 ),
      @PV_NivelSeg              int,
      @PV_NivelAut              int,
      @PV_Estado                VARCHAR(10 ),
      @PV_TiempoExpira          int,
      @PV_CheqEquipo            CHAR,
      @PV_Oficial               CHAR,
      @PV_Nombre                VARCHAR(100 ),
      @PV_TipoUsuario           VARCHAR(5 ),
      @PV_IdEmpresa             int,
      @PV_Descripcion           VARCHAR(100 ),
      @PV_Notificacion          VARCHAR(15 ),
      @PV_Correo                VARCHAR(100 ),
      @PV_Fax                   VARCHAR(30 ),
      @PV_FechaExpira           DATETIME,
      @PV_Ruc                   VARCHAR(30 ),
      @PV_FechaAct              DATETIME
AS
     UPDATE Seguridad.Seg_USUARIO 
           SET nivelseg         = @PV_NivelSeg,
                nivelaut        = @PV_NivelAut,
                estado          = @PV_Estado,
                tiempoexpira    = @PV_TiempoExpira,
                cheqequipo      = @PV_CheqEquipo,
                oficial         = @PV_Oficial,
                nombre          = @PV_Nombre,
                tipousuario     = @PV_TipoUsuario,
                idempresa       = @PV_IdEmpresa,
                descripcion     = @PV_Descripcion,
                notificacion    = @PV_Notificacion,
                correo          = @PV_Correo,
                fax             = @PV_Fax,
                fechaexpira     = @PV_FechaExpira,
                ruc             = @PV_Ruc,
                fechaact        = @PV_FechaAct
     WHERE idusuario = @PV_idUsuario 
     --COMMIT
  







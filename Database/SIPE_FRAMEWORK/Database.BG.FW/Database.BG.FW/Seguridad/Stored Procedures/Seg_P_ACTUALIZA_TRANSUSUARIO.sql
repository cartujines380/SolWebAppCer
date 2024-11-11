



create procedure [Seguridad].[Seg_P_ACTUALIZA_TRANSUSUARIO]
@PV_idTransaccion          int,
        @PV_idOpcion               int,
        @PV_estado                 CHAR,
        @PV_fechaInicial           DATETIME,
        @PV_fechaFinal             DATETIME,
        @PV_idHorario              int,
        @PV_tipoIdentificacion     CHAR,
        @PV_idIdentificacion       VARCHAR(100 ),
        @PV_usrReemplaza           VARCHAR(20 ),
        @PV_idUsuario              VARCHAR(20 ),
        @PV_Organizacion           int
AS 
     UPDATE Seguridad.Seg_TRANSUSUARIO
            SET estado=@PV_estado,
                fechainicial=@PV_fechaInicial,
                fechafinal=@PV_fechaFinal,
                idhorario=@PV_idHorario,
                tipoidentificacion=@PV_tipoIdentificacion,
                ididentificacion=@PV_idIdentificacion,
                usrreemplaza=@PV_usrReemplaza
     WHERE idtransaccion=@PV_idTransaccion AND
           idopcion=@PV_idOpcion           AND
           idusuario=@PV_idUsuario         AND 
           IdOrganizacion =@PV_Organizacion         
     --COMMIT







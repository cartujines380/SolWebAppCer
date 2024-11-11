






/*==============================================================*/
/* MANTENIMIENTO: AUTORIZACION    */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_AUTORIZACION]
@PV_idTransaccion        int,
      @PV_idOpcion             int,
      @PV_idOrganizacion       int,
      @PV_idHorario            int,
      @PV_parametro            VARCHAR(100 ),
      @PV_operador             VARCHAR(50 ),
      @PV_valorAutorizado      VARCHAR(50 ),
      @PV_idAutorizacion   int OUTPUT
AS

   DECLARE @ln_idAutorizacion      int
     Select @ln_idAutorizacion = ISNULL(max(idautorizacion),0) + 1
        From Seguridad.Seg_AUTORIZACION 
        
     INSERT INTO Seguridad.Seg_AUTORIZACION(idautorizacion,idtransaccion,idopcion,idorganizacion,idhorario,
parametro,operador,valorautorizado)
            VALUES(@ln_idAutorizacion,@PV_idTransaccion,@PV_idOpcion,@PV_idOrganizacion, @PV_idHorario,
                   @PV_parametro,@PV_operador,@PV_valorAutorizado)
     --COMMIT
     SET @PV_idAutorizacion=@ln_idAutorizacion
 










create procedure [Seguridad].[Seg_P_ACTUALIZA_HORARIODIA]
@PV_idHorarioDia      int,
      @PV_horaInicio        DATETIME,
      @PV_horaFin           DATETIME,
      @PV_idHorario         int,
      @PV_Dias              CHAR
AS
     UPDATE Seguridad.Seg_HORARIODIA
            SET horainicio = @PV_horaInicio,
                horafin=@PV_horaFin,
                idhorario=@PV_idHorario,
                dias=@PV_Dias
     WHERE idhorario=@PV_idHorario
     --COMMIT
 






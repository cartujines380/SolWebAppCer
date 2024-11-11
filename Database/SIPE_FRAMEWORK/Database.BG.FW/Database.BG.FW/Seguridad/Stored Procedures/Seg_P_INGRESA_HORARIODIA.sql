


/*==============================================================*/
/* MANTENIMIENTO: HORARIODIA      */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_HORARIODIA]
@PV_idHorarioDia  int,
@PV_horaInicio        DATETIME,
@PV_horaFin           DATETIME,
    @PV_idHorario         int,
    @PV_dias              VARCHAR(7 )
AS
       -- SELECT Seguridad.Seg_S_horario_dia.NEXTVAL INTO @ln_idHorarioDia FROM dual
     INSERT INTO Seguridad.Seg_HORARIODIA(idHorarioDia,horainicio,horafin,idhorario,dias)
     VALUES(@PV_idHorarioDia,@PV_horaInicio,@PV_horaFin,@PV_idHorario,@PV_dias)
     --COMMIT
 






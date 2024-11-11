


create procedure [Seguridad].[Seg_P_CONS_HORARIOOFIC_ID]
@PV_idHorarioOficina  int
AS
       SELECT idHorario,Descripcion
       FROM  Seguridad.Seg_HORARIO
       WHERE idHorario>=@PV_idHorarioOficina

  






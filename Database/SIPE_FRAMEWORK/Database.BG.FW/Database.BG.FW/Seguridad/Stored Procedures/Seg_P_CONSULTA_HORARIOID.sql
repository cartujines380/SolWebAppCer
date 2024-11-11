
create procedure [Seguridad].[Seg_P_CONSULTA_HORARIOID]
@PV_idHorario           int

AS
DECLARE @VL_maxHorarioDia as int 
     SELECT @VL_maxHorarioDia = isnull(max(IdHorarioDia),0)
     FROM Seguridad.Seg_HORARIODIA
     WHERE idhorario=@PV_idHorario

     SELECT Descripcion, DiasFeriados, @VL_maxHorarioDia as MaxHorarioDia
     FROM Seguridad.Seg_HORARIO
     WHERE idhorario=@PV_idHorario
  









create procedure [Seguridad].[Seg_P_ELIMINA_HORARIODIA]
@PV_idHorario           int
AS
     DELETE FROM Seguridad.Seg_HORARIODIA
     WHERE idhorario=@PV_idHorario
     --COMMIT







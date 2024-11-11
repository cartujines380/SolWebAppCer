


create procedure [Seguridad].[Seg_P_CONS_DESCRIPCION_HORARIO]
@PV_idOficina               int
AS
       SELECT hofi.idHorario, ho.descripcion
       FROM Seguridad.Seg_HORARIOOFICINA hofi, Seguridad.Seg_HORARIO  ho
       WHERE hofi.idOficina=@PV_idOficina AND
             hofi.idHorario=ho.idHorario
 










create procedure [Seguridad].[Seg_P_ELIMINA_HORARIO_OFICINA]
@PV_idOficina          int,
           @PV_idhorario          int
AS
     DELETE FROM Seguridad.Seg_HORARIOOFICINA
     WHERE idoficina=@PV_idOficina and idHorario=@PV_idhorario
     --COMMIT
 






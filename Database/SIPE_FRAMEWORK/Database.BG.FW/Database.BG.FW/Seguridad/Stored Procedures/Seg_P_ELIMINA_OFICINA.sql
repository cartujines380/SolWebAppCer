



create procedure [Seguridad].[Seg_P_ELIMINA_OFICINA]
@PV_idOficina           int
AS
     DELETE FROM Seguridad.Seg_HORARIOOFICINA
     WHERE idoficina=@PV_idOficina
     --COMMIT
  






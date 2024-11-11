



create procedure [Seguridad].[Seg_P_CONSULTA_HORARIOSEARCHID] 
@PV_horario          int
AS
         SELECT distinct IDHORARIO, Descripcion
         FROM Seguridad.Seg_HORARIO 
        -- WHERE IDHORARIO = @PV_horario
         WHERE IDHORARIO >= @PV_horario
         
  






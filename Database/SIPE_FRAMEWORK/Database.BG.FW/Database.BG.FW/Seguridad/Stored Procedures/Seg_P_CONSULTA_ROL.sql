



create procedure [Seguridad].[Seg_P_CONSULTA_ROL]
@PV_idRol           int
AS
        SELECT  * FROM Seguridad.Seg_ROL
        WHERE IdRol = @PV_idRol

  






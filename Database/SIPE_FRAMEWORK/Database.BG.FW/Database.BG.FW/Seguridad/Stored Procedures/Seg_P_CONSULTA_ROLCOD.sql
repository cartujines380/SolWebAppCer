



create procedure [Seguridad].[Seg_P_CONSULTA_ROLCOD] 
@PV_idRol              int
AS
         SELECT distinct IDROL, Descripcion, Nombre
         FROM Seguridad.Seg_Rol
        -- WHERE IDROL = @PV_idRol
         WHERE IDROL >= @PV_idRol
         
    






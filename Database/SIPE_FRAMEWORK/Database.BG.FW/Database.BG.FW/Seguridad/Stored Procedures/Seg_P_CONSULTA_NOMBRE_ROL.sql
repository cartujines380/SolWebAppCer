


create procedure [Seguridad].[Seg_P_CONSULTA_NOMBRE_ROL] 
@PV_idRol           int

AS
  select r.nombre 
   from Seguridad.Seg_rol r
  where r.idrol=@PV_idRol
   
 







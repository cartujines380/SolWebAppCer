


create procedure [Seguridad].[Seg_P_CONSULTA_ORGANIZACIONID]
@PV_idOrganizacion   int
AS 
       SELECT idorganizacion,descripcion
       FROM Seguridad.Seg_ORGANIZACION
       WHERE idorganizacion>=@PV_idOrganizacion
   






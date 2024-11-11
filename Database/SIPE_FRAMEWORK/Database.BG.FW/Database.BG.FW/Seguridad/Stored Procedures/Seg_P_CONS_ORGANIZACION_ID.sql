


create procedure [Seguridad].[Seg_P_CONS_ORGANIZACION_ID]
@PV_id  int
AS

       SELECT distinct a.IdOrganizacion as codigo, b.Descripcion as descripcion
       FROM Seguridad.Seg_HorarioTrans a, Seguridad.Seg_Organizacion b
       WHERE a.IdOrganizacion >= @PV_id
       AND a.IdOrganizacion = b.IdOrganizacion
   







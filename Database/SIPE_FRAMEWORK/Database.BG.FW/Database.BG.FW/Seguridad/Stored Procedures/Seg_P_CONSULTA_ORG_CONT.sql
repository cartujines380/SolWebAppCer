


create procedure [Seguridad].[Seg_P_CONSULTA_ORG_CONT]
@PV_desc             VARCHAR(100 )
AS
       SELECT idorganizacion,descripcion
       FROM Seguridad.Seg_ORGANIZACION
       WHERE UPPER(descripcion) like '%' + UPPER(@PV_desc) + '%'
 







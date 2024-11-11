


create procedure [Seguridad].[Seg_P_CONS_ORGANIZ_DESC]
@PV_desc      VARCHAR(100)
AS
    SELECT distinct a.IdOrganizacion as codigo, b.Descripcion as descripcion
       FROM Seguridad.Seg_OpcionTrans a, Seguridad.Seg_Organizacion b
       WHERE a.IdOrganizacion=b.IdOrganizacion AND
            UPPER(b.descripcion) like UPPER(@PV_desc) + '%'
 






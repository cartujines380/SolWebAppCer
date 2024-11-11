



create procedure [Seguridad].[Seg_P_CONSULTA_ROLCONT] 
@PV_Descripcion      VARCHAR(100 )
AS
         SELECT distinct IDROL, Descripcion, Nombre
         FROM Seguridad.Seg_Rol
         WHERE UPPER(Descripcion) like '%' + UPPER(@PV_Descripcion) + '%'
         
 






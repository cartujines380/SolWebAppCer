


create procedure [Seguridad].[Seg_P_CONSULTA_NOMBREROL] 
@PV_Nombre     VARCHAR( 100)
AS
         SELECT distinct IDROL, Descripcion, Nombre
         FROM Seguridad.Seg_Rol
         WHERE UPPER(Nombre) like UPPER(@PV_Nombre) + '%'
         
   







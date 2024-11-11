


create procedure [Seguridad].[Seg_P_CONSULTA_USUARIODESC] 
@PV_Nombre      VARCHAR(20 )
AS
         SELECT iDUSUARIO, Nombre
         FROM Seguridad.Seg_Usuario
         WHERE UPPER(Nombre) like UPPER(@PV_Nombre) + '%'
         
 






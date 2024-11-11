


create procedure [Seguridad].[Seg_P_CONSULTA_USUARIOCONT] 
@PV_Nombre      VARCHAR(100 )
AS
         SELECT iDUSUARIO, Nombre
         FROM Seguridad.Seg_Usuario
         WHERE UPPER(Nombre) like '%' + UPPER(@PV_Nombre) + '%' 
  






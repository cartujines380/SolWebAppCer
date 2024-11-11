



create procedure [Seguridad].[Seg_P_CONSULTA_HORARIODESC]
@PV_descripcion     VARCHAR(100 )
AS
         SELECT distinct IDHORARIO, Descripcion
         FROM Seguridad.Seg_HORARIO 
         WHERE UPPER(Descripcion) LIKE UPPER(@PV_descripcion) + '%'
         
 









create procedure [Seguridad].[Seg_P_CONS_HORARIOOFIC_DESC]
@PV_desc      VARCHAR(100 )
AS

       SELECT idhorario, descripcion
       FROM Seguridad.Seg_HORARIO
       WHERE UPPER(descripcion) like UPPER(@PV_desc) + '%'







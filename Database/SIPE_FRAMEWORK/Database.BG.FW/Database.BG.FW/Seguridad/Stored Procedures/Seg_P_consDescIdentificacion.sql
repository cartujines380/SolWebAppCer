
CREATE PROCEDURE [Seguridad].[Seg_P_consDescIdentificacion]
@PI_IdTipoIdentificacion char,
@PI_Identificacion varchar(100)
AS
	
	If @PI_IdTipoIdentificacion = 'E'
            Select Descripcion from Seguridad.Seg_Equipo  where IdIdentificacion1 = @PI_Identificacion
        Else
            Select Descripcion from Seguridad.Seg_Equipo  where Area = @PI_Identificacion







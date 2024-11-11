
/* Si es un dia feriado devuelve 1, caso contrario 0 */

CREATE FUNCTION [Seguridad].[Seg_F_DiaFeriado]
(@IdEmpresa int, @IdSucursal int, @IdHorario int)
RETURNS bit
BEGIN
	DECLARE @Retorno bit
	IF EXISTS (	SELECT 1
				FROM Seguridad.Seg_Horario  
				WHERE IdHorario = @IdHorario 
					AND DiasFeriados = 1 )
	BEGIN -- Revisar si es un dia feriado
		IF EXISTS (SELECT 1 FROM Seguridad.Seg_DiasFeriados
					WHERE IdEmpresa = @IdEmpresa 
					    AND (IdSucursal = @IdSucursal OR IdSucursal = 0)
						AND Dia = convert(char,getdate(),110) )
			SET @Retorno = 1
		ELSE
			SET @Retorno = 0 -- No es dia Feriado
	END
	ELSE -- No toma en cuenta si es feriado
		SET @Retorno = 0
RETURN @Retorno

END





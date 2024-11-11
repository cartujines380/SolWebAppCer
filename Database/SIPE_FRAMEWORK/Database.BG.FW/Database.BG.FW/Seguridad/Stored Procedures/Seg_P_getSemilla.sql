
CREATE Procedure [Seguridad].[Seg_P_getSemilla]
(
	@PO_Semilla varchar(100) output
)
AS
	SELECT @PO_Semilla = Datagrama
	FROM Seguridad.Seg_Aplicacion		
		WHERE IdAplicacion = 1 -- default es FrameWork de Seguridad
			


CREATE PROC  [Catalogo].[Ctl_P_ConPorcRetencion] 
	@PI_TipoImpuesto char(1) 
AS

SELECT distinct 
	CodImpuesto, 
	Porcentaje, 
	Descripcion, 
	IdTipoParticipante 
FROM 	Catalogo.Ctl_PorcRetencion 
WHERE 
	TipoImpuesto = @PI_TipoImpuesto 






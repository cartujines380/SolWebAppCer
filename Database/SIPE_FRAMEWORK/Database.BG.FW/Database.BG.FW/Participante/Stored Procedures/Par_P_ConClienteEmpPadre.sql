CREATE PROCEDURE [Participante].[Par_P_ConClienteEmpPadre]
@PI_IdParticipante int
AS
SELECT distinct e1.IdParticipante, e1.Nombre
FROM Participante.Par_Empresa e1, Participante.Par_Cliente c,Participante.Par_Empresa e2
WHERE e1.IdParticipante = c.IdParticipante
	and c.IdEmpresa = @PI_IdParticipante
	and e1.IdParticipante = e2.IdEmpresaPadre





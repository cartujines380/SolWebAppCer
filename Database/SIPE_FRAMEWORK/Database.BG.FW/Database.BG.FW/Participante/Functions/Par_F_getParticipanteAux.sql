CREATE function [Participante].[Par_F_getParticipanteAux](@TipoParticipante VARCHAR(10)='')
RETURNS TABLE
AS	

RETURN (SELECT p.IdParticipante, p.IdEmpresa, p.IdUsuario, 
		p.Identificacion,p.TipoParticipante,
		p.Nombre, p.Estado
	FROM Participante.Par_V_Participante p 
	WHERE p.TipoPart=@TipoParticipante or @TipoParticipante = '')
	





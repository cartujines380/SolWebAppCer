CREATE   VIEW [Participante].[Par_V_Empresa]
AS
	SELECT e.IdParticipante as Codigo,
    	e.Nombre as Descripcion, e.Nivel,
    	e.IdEmpresaPadre as "CODIGOPADRE", 
		p.IdUsuario,p.Estado,
		'TipoParticipante' = 'E',p.Identificacion
    FROM Participante.Par_Empresa e, Participante.Par_Participante p
    WHERE e.IdParticipante = p.IdParticipante








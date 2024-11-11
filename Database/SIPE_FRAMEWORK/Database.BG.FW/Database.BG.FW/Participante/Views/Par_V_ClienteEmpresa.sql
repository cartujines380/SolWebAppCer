create   VIEW [Participante].[Par_V_ClienteEmpresa]
AS
	SELECT c.IdParticipante, em.Nombre as EmpresaCliente
    FROM  Participante.Par_Cliente c, Participante.Par_Empresa em
    WHERE c.IdEmpresa = em.IdParticipante






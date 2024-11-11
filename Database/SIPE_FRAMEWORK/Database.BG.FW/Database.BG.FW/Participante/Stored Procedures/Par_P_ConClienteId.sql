
CREATE PROCEDURE [Participante].[Par_P_ConClienteId] 
@PI_IdParticipante int,
@PI_IdEmpresa int
AS	
	SELECT	c.IdVendedor,p.Identificacion as IdentVendedor, p.IdUsuario as IdUsuarioVendedor,
			Participante.Par_F_getNombreParticipante(IdVendedor) as NombreVendedor
	FROM Participante.Par_Cliente c inner join Participante.Par_Participante p on c.IdVendedor = p.IdParticipante  
	where   c.IdParticipante = @PI_IdParticipante
		AND c.IdEmpresa = @PI_IdEmpresa






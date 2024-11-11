CREATE VIEW [Participante].[Par_V_ClixEplEmpresa]
as

	SELECT  cli.IdEmpresa, cli.IdOficina,
			Participante.Par_F_getNombreParticipante(cli.IdOficina) NombreOficina,
			cli.IdVendedor,	cli.IdParticipante IdCliente, p.Identificacion,
			Participante.Par_F_getNombreParticipante(cli.IdParticipante) NombreCliente			
	FROM Participante.Par_Cliente cli INNER JOIN Participante.Par_Participante p 
			ON cli.IdParticipante = p.IdParticipante









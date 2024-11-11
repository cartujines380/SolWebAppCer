--exec Participante.Par_P_ConOficinaZona 2,1
CREATE PROCEDURE  [Participante].[Par_P_ConOficinaZona]
@PI_IdEmpresa int,
@PI_IdZona varchar(50)
AS
SELECT e2.IdParticipante, e2.Nombre 
	FROM Participante.Par_Empresa e1, Participante.Par_Oficina o, Participante.Par_Empresa e2
	WHERE e1.IdParticipante = @PI_IdEmpresa
	      	and e1.IdParticipante = o.IdEmpresa
			and o.IdZona = @PI_IdZona
	  		and o.IdOficina = e2.IdParticipante





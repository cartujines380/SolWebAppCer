


CREATE PROCEDURE [Participante].[Par_P_ConOrganigramaId] 
@PI_IdOrganigrama int,
@PI_IdEmpresa int
AS		
	

SELECT c.Descripcion,
	c.IdOrganigramaPadre,c.Nivel,c.IdEmpleado,
	cp.Descripcion AS DescOrganigramaPadre,
	Participante.Par_F_getIdentParticipante(c.IdEmpleado) as Identificacion, 
	Participante.Par_F_getNombreParticipante(c.IdEmpleado) as Nombre			
	FROM Participante.Par_Organigrama c, Participante.Par_Organigrama cp
	WHERE c.IdOrganigrama = @PI_IdOrganigrama 
			and c.IdEmpresa = @PI_IdEmpresa		
			and (c.IdEmpresa = cp.IdEmpresa or cp.IdEmpresa = 1)
			and c.IdOrganigramaPadre = cp.IdOrganigrama
			
		
		








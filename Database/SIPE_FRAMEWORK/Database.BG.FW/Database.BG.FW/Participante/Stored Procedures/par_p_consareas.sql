create procedure [Participante].[par_p_consareas]
@IdOrganigramaPadre int
AS
	SELECT IdOrganigrama, Descripcion from Participante.par_organigrama


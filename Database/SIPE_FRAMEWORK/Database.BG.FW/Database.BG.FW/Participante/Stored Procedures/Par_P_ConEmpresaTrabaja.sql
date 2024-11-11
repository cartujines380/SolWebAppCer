

create  procedure [Participante].[Par_P_ConEmpresaTrabaja]
	@IdUsuario varchar(50)
AS
IF EXISTS(SELECT 1 FROM Seguridad.Seg_RolUsuario
			WHERE IdRol = 1 AND idUsuario = @IdUsuario)
BEGIN
	/*SELECT distinct em.IdParticipante, em.Nombre
	FROM Sige_Participante..Par_Empresa em
			INNER JOIN Sige_Participante..Par_EntidadEmpresa ee
			  ON ee.IdEmpresa = em.IdParticipante
	UNION
	SELECT DISTINCT IdParticipante, Nombre
	FROM Sige_Participante..Par_Empresa
	WHERE IdCategoriaEmpresa = 8
	*/
	SELECT distinct em.IdParticipante, em.Nombre
	FROM Participante.Par_Empresa em
			INNER JOIN Participante.Par_Organigrama o
				ON em.IdParticipante = o.IdEmpresa
	
END
ELSE
BEGIN
	SELECT DISTINCT em.IdParticipante, em.Nombre
	FROM Participante.Par_RegistroCliente rc
			INNER JOIN Participante.Par_Empleado e
			ON rc.IdParticipante = e.IdParticipante
			INNER JOIN Participante.Par_Empresa em
			  ON em.IdParticipante = e.IdEmpresa
				INNER JOIN Participante.Par_Organigrama o
				ON em.IdParticipante = o.IdEmpresa
	WHERE rc.IdUsuario = @IdUsuario AND e.IdCargo in (10,8)
	
END



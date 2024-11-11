CREATE PROCEDURE [Participante].[Par_P_ConUsuariosRol]
(
	@IdRol int
)
AS
BEGIN
	SELECT
		 p.IdParticipante
		,p.IdUsuario
		,p.Identificacion
		,Nombre = ISNULL( pe.Nombre1 + ' ' + pe.Apellido1, p.IdUsuario ),
		mc.Valor 
	FROM Participante.Par_Participante p
		LEFT OUTER JOIN Participante.Par_Persona pe
			ON pe.IdParticipante = p.IdParticipante
		INNER JOIN Seguridad.Seg_RolUsuario ru
			ON ru.IdUsuario = p.IdUsuario AND ru.IdRol = @IdRol
			LEFT OUTER JOIN Participante.Par_MedioContacto mc
			on mc.IdParticipante=p.IdParticipante and mc.IdMedioContacto =1 
END


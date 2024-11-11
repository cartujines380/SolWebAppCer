

CREATE  VIEW [Seguridad].[Seg_V_USUARIO] AS
SELECT par.IdUsuario, (per.Nombre1 + ' '+ per.Nombre2 + ' ' + per.Apellido1 +' '+ per.Apellido2) Nombre, par.FechaExpira, par.ESTADO
FROM Participante.Par_Participante par, Participante.Par_Persona per
WHERE par.IdParticipante = per.IdParticipante







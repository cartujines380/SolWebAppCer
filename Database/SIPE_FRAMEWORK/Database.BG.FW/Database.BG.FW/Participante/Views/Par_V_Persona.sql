/*SELECT p.IdParticipante,p.Identificacion, 
		pe.Nombre1 + ' ' 
		+ pe.Nombre2+ ' ' 
		+ pe.Apellido1 + ' ' 
		+ pe.Apellido2 as Nombre, 
	(Select Nombre FROM Participante.Par_Empresa e1
		WHERE e1.IdParticipante = c.IdEmpresa ) AS EmpresaCliente
		
    FROM  Participante.Par_Persona pe, Participante.Par_Participante p,  Participante.Par_Cliente c
    WHERE pe.IdParticipante = p.IdParticipante
	  AND pe.IdParticipante *= c.IdParticipante*/
CREATE VIEW [Participante].[Par_V_Persona]
AS
SELECT     pe.IdParticipante, p.IdUsuario, p.Identificacion, ISNULL(pe.Apellido1, '') + ' ' + ISNULL(pe.Apellido2, '') + ' ' + ISNULL(pe.Nombre1, '') 
                      + ' ' + ISNULL(pe.Nombre2, '') AS Nombre, p.Estado, 'P' AS TipoParticipante, ISNULL(pe.Nombre1, '') + ' ' + ISNULL(pe.Nombre2, '') AS Nombres, 
                      ISNULL(pe.Apellido1, '') + ' ' + ISNULL(pe.Apellido2, '') AS Apellidos
FROM         Participante.Par_Persona AS pe INNER JOIN
                      Participante.Par_Participante AS p ON pe.IdParticipante = p.IdParticipante





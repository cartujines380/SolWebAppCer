CREATE VIEW [Participante].[Par_V_PersonaGestor]
AS
	SELECT p.IdParticipante as Codigo, e.IdEmpresa,p.Identificacion, 
		pe.Apellido1 + ' ' 
		+ pe.Apellido2 + ' ' 
		+ pe.Nombre1 + ' ' 
		+ pe.Nombre2 + ' ' 
		  as Nombre, p.IdUsuario
    FROM  Participante.Par_Empleado e, Participante.Par_Persona pe,Participante.Par_Participante p
    WHERE pe.IdParticipante = p.IdParticipante and e.IdParticipante = p.IdParticipante
	 and e.IdCargo in (SELECT IdCargo FROM Catalogo.ctl_V_Cargo where Jerarquia=1)	 








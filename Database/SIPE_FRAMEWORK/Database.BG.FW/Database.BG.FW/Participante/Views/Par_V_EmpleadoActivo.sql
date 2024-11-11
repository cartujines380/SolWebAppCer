
CREATE   VIEW [Participante].[Par_V_EmpleadoActivo]
--select * from Participante.Par_V_EmpleadoActivo
--Le agregue la Ultima columna Departamento del Empleado.. 
--por KLRI 19/01/2006
AS
SELECT e.IdParticipante, p.IdUsuario, e.IdEmpresa, p.Identificacion, 
		isnull(pe.Apellido1,'') + ' ' + isnull(pe.Apellido2,'') + ' '+ isnull(pe.Nombre1,'') + ' ' + isnull(pe.Nombre2,'') as Nombre, 
		e.IdCargo, Catalogo.Ctl_F_conCatalogo(207,e.IdCargo) Cargo,
		Catalogo.Ctl_F_conCatalogoDescAlt(207,e.IdCargo) TipoCargo,
		e.IdOficina,Participante.Par_F_getNombreParticipante(e.IdOficina) Oficina,
		e.Estado, 'TipoParticipante' = 'P'--, 
		--(select Participante.Par_F_getDepartamentoPartic(e.idempresa,e.idorganigrama)) as Departamento
    	FROM Participante.Par_Empleado e, Participante.Par_Persona pe, Participante.Par_Participante p
    	WHERE e.IdParticipante = pe.IdParticipante
		AND pe.IdParticipante = p.IdParticipante
		AND e.Estado = 'A'






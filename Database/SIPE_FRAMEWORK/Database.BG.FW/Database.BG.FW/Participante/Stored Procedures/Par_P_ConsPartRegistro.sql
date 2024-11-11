
-- Modificado por Administracion de Cuentas
CREATE  procedure [Participante].[Par_P_ConsPartRegistro]
@PI_IdUsuario varchar(50)
AS
declare @VL_TipoPart char
declare @VL_Telefono varchar(10), @VL_Correo varchar(100), @VL_DDI varchar(10), @VL_Direccion varchar(100)

	SELECT @VL_TipoPart = p.TipoParticipante
	FROM Participante.Par_RegistroCliente rc, Participante.Par_Participante p
	WHERE rc.IdUsuario = @PI_IdUsuario 
		AND rc.IdParticipante = p.IdParticipante

	IF @VL_TipoPart = 'P'
	BEGIN
		SELECT p.IdParticipante, p.TipoParticipante, p.IdTipoIdentificacion, p.Identificacion,d.IdPais IdPais,
			d.IdProvincia IdProvincia, d.IdCiudad IdCiudad,pe.Apellido1,pe.Apellido2, pe.Nombre1,
			pe.Nombre2, pe.Sexo,convert(varchar,pe.FechaNacimiento,110) as FechaNacimiento,pe.EstadoCivil, 
			r.PregSecreta, r.RespSecreta, r.FraseSecreta,r.AdmProducto,r.RolAsignado,
			isnull(d.Telefono,'') Telefono, isnull(d.DDI,'') DDI, isnull(d.Direccion,'') Direccion, isnull(d.correo,'') Correo, 
			cl.GastoAnual,p.Comentario, r.IdTipoLogin, p.Estado, p.TipoPartRegistro, 
			Catalogo.Ctl_F_conCatalogoDescAlt(24,p.TipoPartRegistro) as DesTipoPartRegistro
		FROM Participante.Par_RegistroCliente r
			INNER JOIN Participante.Par_Participante p 				
			ON r.IdParticipante = p.IdParticipante
				left outer join Participante.Par_V_TelefonoCorreoPart d
					on p.IdParticipante = d.IdParticipante
				INNER JOIN Participante.Par_Persona pe
				ON p.IdParticipante = pe.IdParticipante
				LEFT OUTER JOIN Participante.Par_Cliente cl
				ON p.IdParticipante = cl.IdParticipante
		WHERE r.IdUsuario = @PI_IdUsuario
	END
	IF @VL_TipoPart = 'E'
	BEGIN
		SELECT	p.IdParticipante, p.TipoParticipante, p.IdTipoIdentificacion, p.Identificacion,d.IdPais IdPais,
			d.IdProvincia IdProvincia, d.IdCiudad IdCiudad, Participante.Par_F_getNombreParticipante(p.IdParticipante) as Nombre,
				pc.IdTipoIdentificacion IdTipoIdentificacionR, pc.Identificacion IdentificacionR,pc.IdParticipante as IdParticipanteR,
				isnull(pe.Nombre1,'') as Nombre1, isnull(pe.Nombre2,'') as Nombre2,isnull(pe.Apellido1,'') as Apellido1,isnull(pe.Apellido2,'') as Apellido2,
				r.PregSecreta, r.RespSecreta, r.FraseSecreta,r.AdmProducto,r.RolAsignado,
			isnull(d.Telefono,'') Telefono, isnull(d.DDI,'') DDI, isnull(d.Direccion,'') Direccion, isnull(d.correo,'') Correo, 
			cl.GastoAnual,p.Comentario, r.IdTipoLogin, p.Estado, p.TipoPartRegistro, 
			Catalogo.Ctl_F_conCatalogoDescAlt(24,p.TipoPartRegistro) as DesTipoPartRegistro
		FROM Participante.Par_RegistroCliente r
			INNER JOIN Participante.Par_Participante p 				
			ON r.IdParticipante = p.IdParticipante
			LEFT OUTER JOIN Participante.Par_Cliente cl
			ON	p.IdParticipante = cl.IdParticipante
				left outer join Participante.Par_V_TelefonoCorreoPart d
					on p.IdParticipante = d.IdParticipante
				left outer join Participante.Par_Contacto c 
					on p.IdParticipante = c.IdParticipante AND c.IdTipoContacto = 1  -- Representante Legal			
				left outer JOIN Participante.Par_Participante pc
				ON c.IdPartContacto = pc.IdParticipante
				left outer JOIN Participante.Par_Persona pe
				ON pc.IdParticipante = pe.IdParticipante
		WHERE r.IdUsuario = @PI_IdUsuario				
	END





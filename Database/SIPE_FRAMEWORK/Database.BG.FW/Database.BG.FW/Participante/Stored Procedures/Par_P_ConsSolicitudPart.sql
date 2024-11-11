
create  procedure [Participante].[Par_P_ConsSolicitudPart]
@PI_IdParticipante int
AS
declare @VL_TipoPart char

	SELECT @VL_TipoPart = TipoParticipante
	FROM .Participante.Par_Participante
	WHERE IdParticipante = @PI_IdParticipante

	IF @VL_TipoPart = 'P'
	BEGIN
		SELECT p.IdParticipante, p.IdUsuario, p.TipoParticipante, p.IdTipoIdentificacion, p.Identificacion,d.IdPais,
			d.IdProvincia, d.IdCiudad,pe.Apellido1,pe.Apellido2, pe.Nombre1,
			pe.Nombre2, pe.Sexo,convert(varchar,pe.FechaNacimiento,110) as FechaNacimiento,pe.EstadoCivil, 
			mc1.Valor Telefono, mc1.ValorAlt DDI, d.Direccion, 
			mc2.Valor Correo, p.Comentario, p.TipoPartRegistro, 
			Catalogo.Ctl_F_conCatalogoDescAlt(24,p.TipoPartRegistro) as DesTipoPartRegistro
		FROM .Participante.Par_Participante p 
					left outer join .Participante.Par_Direccion d
						on  p.IdParticipante = d.IdParticipante 
					left outer join .Participante.Par_MedioContacto mc1
						on  p.IdParticipante = mc1.IdParticipante AND mc1.IdTipoMedioContacto = 1 --Telefono
					left outer join .Participante.Par_MedioContacto mc2 
						on p.IdParticipante = mc2.IdParticipante AND mc2.IdTipoMedioContacto = 3, --Correo
		.Participante.Par_Persona pe             
		WHERE p.IdParticipante = @PI_IdParticipante
		AND p.IdParticipante = pe.IdParticipante	
	END
	IF @VL_TipoPart = 'E'
	BEGIN
		SELECT p.IdParticipante, p.IdUsuario, p.TipoParticipante, p.IdTipoIdentificacion, p.Identificacion,d.IdPais,
			d.IdProvincia, d.IdCiudad, Participante.Par_F_getNombreParticipante(p.IdParticipante) as Nombre,
			pc.IdTipoIdentificacion IdTipoIdentificacionR, pc.Identificacion IdentificacionR,
			isnull(pe.Nombre1,'') as Nombre1, isnull(pe.Nombre2,'') as Nombre2,isnull(pe.Apellido1,'') as Apellido1,isnull(pe.Apellido2,'') as Apellido2,
			mc1.Valor Telefono, d.Direccion, 
			mc2.Valor Correo, p.Comentario, p.TipoPartRegistro, 
			Catalogo.Ctl_F_conCatalogoDescAlt(24,p.TipoPartRegistro) as DesTipoPartRegistro
		FROM .Participante.Par_Participante p
					left outer join .Participante.Par_Direccion d
						on  p.IdParticipante = d.IdParticipante 
					left outer join .Participante.Par_MedioContacto mc1
						on  p.IdParticipante = mc1.IdParticipante AND mc1.IdTipoMedioContacto = 1 --Telefono
					left outer join .Participante.Par_MedioContacto mc2 
						on p.IdParticipante = mc2.IdParticipante AND mc2.IdTipoMedioContacto = 3 --Correo
					left outer join .Participante.Par_Contacto c 
						on p.IdParticipante = c.IdParticipante AND c.IdTipoContacto = 1  -- Representante Legal			
					left outer join .Participante.Par_Participante pc
						on c.IdPartContacto = pc.IdParticipante
					left outer join .Participante.Par_Persona pe
						on pc.IdParticipante = pe.IdParticipante
		WHERE p.IdParticipante =  @PI_IdParticipante
		
	END





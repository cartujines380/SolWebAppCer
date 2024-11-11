CREATE PROCEDURE [Participante].[Par_P_ConParticipanteIdentificacion] 
@PI_Identificacion varchar(100),
@PI_TipoIdentificacion varchar(10)
AS
	SELECT p.IdParticipante,p.TipoParticipante,
		IdUsuario, Opident,
		convert(varchar,p.FechaRegistro,110) as FechaRegistro,
		p.IdPais, Catalogo.Ctl_F_conCatalogo(2,p.IdPais) as Pais,
		p.IdProvincia,Catalogo.Ctl_F_conCatalogo(3,p.IdProvincia) as Provincia,
		p.IdCiudad, Catalogo.Ctl_F_conCatalogo(4,p.IdCiudad) as Ciudad,
		convert(varchar,p.FechaExpira,110) as FechaExpira, 
		TiempoExpira,isnull(ChequeaEquipo,0) ChequeaEquipo, p.Comentario,
		p.IdNaturalezaNegocio,
		Catalogo.Ctl_F_conCatalogo(205,p.IdNaturalezaNegocio)as NaturalezaNegocio,
		p.Estado		
	FROM Participante.Par_Participante p
	WHERE   p.Identificacion = @PI_Identificacion
		and p.IdTipoIdentificacion = @PI_TipoIdentificacion








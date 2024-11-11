CREATE PROCEDURE [Participante].[Par_P_conReferencias]
@PI_IdParticipante int,
@PI_TipoParticipante char(1) --Empresa o Persona
AS
	
IF @PI_TipoParticipante = 'P'
   begin
	SELECT 	Catalogo.Ctl_F_conCatalogo(11,c.IdTipoContacto) as Referencia,
		p.IdParticipante,p.Identificacion,p.IdUsuario,
		Participante.Par_F_getNombreParticipante(p.IdParticipante) Nombre		
	FROM 	Participante.Par_Contacto c, Participante.Par_Participante p
	WHERE   c.IdPartContacto =  @PI_IdParticipante
		and c.IdParticipante = p.IdParticipante
   end
	
IF @PI_TipoParticipante = 'E'
   begin
	SELECT 	Participante.Par_F_getNombreCategoriaEmp(e.IdCategoriaEmpresa) as Referencia,
		p.IdParticipante,p.Identificacion,p.IdUsuario,
		Participante.Par_F_getNombreParticipante(p.IdParticipante) Nombre
	FROM 	Participante.Par_Empresa e, Participante.Par_Participante p
	WHERE   e.IdEmpresaPadre = @PI_IdParticipante
		and e.IdParticipante = p.IdParticipante
   end







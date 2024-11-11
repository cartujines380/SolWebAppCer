
CREATE   VIEW [Participante].[Par_V_Usuario]
AS
	SELECT rc.IdParticipante,rc.IdUsuario,p.Identificacion,
		 Participante.par_f_getNombreParticipante(rc.IdParticipante) as Nombre,
		 isnull(Catalogo.Ctl_F_conCatalogo(24,TipoPartRegistro),'') TipoPartRegistro
   	 FROM  Participante.Par_RegistroCliente rc INNER JOIN Participante.Par_Participante p
			ON rc.IdParticipante = p.IdParticipante



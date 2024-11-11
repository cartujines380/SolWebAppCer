CREATE PROCEDURE [Participante].[Par_P_ConsPregSecreta]
@PI_IdUsuario varchar(50),
@PO_PregSecreta varchar(100) output,
@PO_UrlAD varchar(200) output
AS
	SELECT @PO_PregSecreta = rc.PregSecreta 
	FROM Participante.Par_Participante p, Participante.Par_RegistroCliente rc
	WHERE p.IdUsuario = @PI_IdUsuario
	    AND p.IdParticipante = rc.IdParticipante
	    AND rc.Estado = 1

	-- Recupera el Url de donde se definio el usuario en el Active Directory
	SELECT @PO_UrlAD = pa.Valor
	FROM Participante.Par_RegistroCliente rc
			INNER JOIN Participante.Par_Participante p
			ON p.IdParticipante = rc.IdParticipante
			  INNER JOIN Catalogo.Ctl_Catalogo c
			  ON c.IdTabla = 24 AND c.Codigo = p.TipoPartRegistro
				INNER JOIN Seguridad.Seg_ParamAplicacion pa
				ON pa.IdAplicacion = convert(int,c.DescAlterno) AND pa.Parametro = 'Url'
	WHERE rc.IdUsuario = @PI_IdUsuario 



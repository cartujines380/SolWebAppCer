CREATE PROCEDURE [Participante].[Par_P_ConCargaFamiliar]
@PI_IdParticipante int
AS
	SELECT c.IdCargaFamiliar, c.IdTipoCarga
		,Catalogo.Ctl_F_conCatalogo(214,c.IdTipoCarga) as TipoCarga		
		,c.Nombre, convert(varchar,c.FechaNac,110) as FechaNac 
		,c.IdTipoIdentificacion, c.Identificacion
	FROM Participante.Par_CargaFamiliar c
	WHERE c.IdParticipante = @PI_IdParticipante







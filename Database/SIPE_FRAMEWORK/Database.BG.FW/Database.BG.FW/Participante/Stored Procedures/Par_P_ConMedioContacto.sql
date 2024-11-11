
CREATE PROCEDURE [Participante].[Par_P_ConMedioContacto]
@PI_IdParticipante int
AS
declare @VL_ValorAlt as varchar(25)

SELECT d.IdDireccion, d.IdMedioContacto, d.IdTipoMedioContacto, 
		t.Descripcion as TipoMedioContacto, d.ValorAlt, d.Valor
	FROM Participante.Par_MedioContacto d, Catalogo.ctl_V_TipoMedioContacto t
	WHERE d.IdParticipante = @PI_IdParticipante
		AND d.IdTipoMedioContacto = t.IdTipoMedioContacto
	



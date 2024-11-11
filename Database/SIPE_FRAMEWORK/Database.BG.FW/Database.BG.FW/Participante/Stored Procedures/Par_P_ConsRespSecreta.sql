CREATE PROCEDURE [Participante].[Par_P_ConsRespSecreta]
@PI_IdUsuario varchar(50),
@PI_PregSecreta varchar(100),
@PI_RespSecreta varchar(100),
@PO_Estado char output,
@PO_Correo varchar(100) output
AS
DECLARE @VL_RespSecreta varchar(100), @VL_IdParticipante int

	SELECT @VL_RespSecreta = rc.RespSecreta ,
	       @VL_IdParticipante = p.IdParticipante
	FROM Participante.Par_Participante p, Participante.Par_RegistroCliente rc
	WHERE p.IdUsuario = @PI_IdUsuario
	    AND p.IdParticipante = rc.IdParticipante
	    AND rc.Estado = 1
	    AND rc.PregSecreta = @PI_PregSecreta
	IF @PI_RespSecreta = @VL_RespSecreta
	BEGIN
		SET @PO_Estado = 1
		SELECT @PO_Correo = mc.Valor
		FROM Participante.Par_Participante p, Participante.Par_MedioContacto mc
		WHERE p.IdParticipante = @VL_IdParticipante
	    	    AND mc.IdParticipante = p.IdParticipante
	    	    AND mc.IdDireccion = 1
	    	    AND mc.IdTipoMedioContacto = 3 -- Correo
	END
	ELSE
		SET @PO_Estado = 0






CREATE PROCEDURE [Comunicacion].[Cmu_P_LeeMsgAviso]
@PI_IdAviso int,
@PI_IdUsuario varchar(50),
@PO_Mensaje varchar(max) output
AS
DECLARE @VL_IdUsuarioDestino varchar(50)
	
	SELECT @VL_IdUsuarioDestino = IdUsuarioDestino,
			@PO_Mensaje = Mensaje
	FROM Comunicacion.Cmu_Aviso
	WHERE IdAviso = @PI_IdAviso

	-- Acutualiza el estado del Aviso a Visto si es el usuario destino
	IF @PI_IdUsuario = @VL_IdUsuarioDestino
		UPDATE Comunicacion.Cmu_Aviso 
		SET Estado = 'V'
		WHERE  IdAviso = @PI_IdAviso


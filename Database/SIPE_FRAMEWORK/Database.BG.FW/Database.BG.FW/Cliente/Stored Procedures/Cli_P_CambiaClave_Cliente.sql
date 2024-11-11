CREATE procedure [Cliente].[Cli_P_CambiaClave_Cliente]
     @PI_IdUsuario    varchar(20),
     @PI_ClaveAnterior varchar(50),
     @PI_ClaveNueva   varchar(50),
     @PO_CambioClave    int out
     
AS 
   
   EXEC Cliente.Cli_P_VERIFICA_Cliente @PI_IdUsuario, @PI_ClaveAnterior, @PO_CambioClave OUT
   IF @PO_CambioClave = 1
   BEGIN
		UPDATE Cliente.Cli_UsuarioCliente SET 
		CLave = @PI_ClaveNueva ,
		Estado = 'AC'
		WHERE IdUsuario = @PI_IdUsuario
		
		SELECT @PO_CambioClave = 0 
   END
   ELSE 
   BEGIN
		SELECT @PO_CambioClave = -1
   END


create procedure [Cliente].[Cli_P_ResetClave_Cliente]
     @PI_IdUsuario    varchar(20),
     @PI_ClaveNueva   varchar(50),
     @PO_CambioClave    int out
     
AS 
   SET @PO_CambioClave = 0
   IF EXISTS ( SELECT TOP 1 1 FROM Cliente.Cli_UsuarioCliente where IdUsuario = @PI_IdUsuario )   
   BEGIN
		UPDATE Cliente.Cli_UsuarioCliente SET 
		CLave = @PI_ClaveNueva ,
		Estado = 'AC'
		WHERE IdUsuario = @PI_IdUsuario
		IF @@ROWCOUNT > 0
		SET @PO_CambioClave = 1
   END

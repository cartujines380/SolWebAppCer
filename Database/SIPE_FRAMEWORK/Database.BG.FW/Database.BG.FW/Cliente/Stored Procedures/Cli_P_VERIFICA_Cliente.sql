CREATE procedure [Cliente].[Cli_P_VERIFICA_Cliente]
     @PI_IdUsuario    varchar(20),
     @PI_Clave        varchar(50),
     @PO_EsCliente    int out     
AS    
   IF EXISTS( SELECT TOP 1 1 FROM
   Cliente.Cli_UsuarioCliente Where IdUsuario = @PI_IdUsuario 
   AND ( (  Clave = @PI_Clave ) OR  ( @PI_Clave = '' AND Estado = 'PE')))
   BEGIN
	SELECT @PO_EsCliente = 1
   END
   ELSE
   BEGIN
	SELECT @PO_EsCliente = 1
   END

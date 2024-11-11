CREATE PROC [Participante].[Par_P_RegistraCtaUsuario]
@PI_IdParticipante int,
@PI_IdEmpresa int,
@PI_IdUsuario varchar(50),
@PI_IdProducto int,
@PI_NumeroProducto varchar(50)
AS
	/*
		-- Y si se esta asociando una cuenta nueva del usuario que registra
		IF NOT EXISTS(SELECT 1 FROM Sige_Sas..Sas_ProdCliente 
						WHERE IdUsuario = @PI_IdUsuario 
							AND IdProducto = @PI_IdProducto 
							AND NumeroProducto = @PI_NumeroProducto)
		BEGIN
			INSERT Sige_Sas..Sas_ProdCliente(IdCliente,IdEmpresa,IdUsuario,
								IdProducto,NumeroProducto,Estado,TipoCliente)
			VALUES(@PI_IdParticipante,@PI_IdEmpresa,@PI_IdUsuario,@PI_IdProducto,
					@PI_NumeroProducto,'1','N')			
		END
*/





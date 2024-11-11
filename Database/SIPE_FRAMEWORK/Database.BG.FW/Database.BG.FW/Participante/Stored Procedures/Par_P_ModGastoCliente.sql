CREATE Procedure [Participante].[Par_P_ModGastoCliente]
@PI_IdCliente int,
@PI_Gasto money
AS
UPDATE Participante.Par_Cliente 
SET GastoAnual = @PI_Gasto
WHERE IdParticipante = @PI_IdCliente



CREATE PROCEDURE [Comunicacion].[Cmu_P_LeeAvisos]
@PI_IdUsuario varchar(50),
@PI_Opcion char
AS
	IF @PI_Opcion = 'E'
		exec Comunicacion.Cmu_P_LeeAvisosEnviados @PI_IdUsuario
	ELSE
		exec Comunicacion.Cmu_P_LeeAvisosRecibidos @PI_IdUsuario



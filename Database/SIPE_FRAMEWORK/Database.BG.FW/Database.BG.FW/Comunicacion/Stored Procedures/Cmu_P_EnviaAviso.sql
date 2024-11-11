CREATE PROCEDURE [Comunicacion].[Cmu_P_EnviaAviso]-- 'admin','vmunoz','prueba'
@PI_IdUsuarioOrigen varchar(50),
@PI_IdUsuarioDestino varchar(50),
@PI_Mensaje varchar(max)
AS
	INSERT Comunicacion.Cmu_Aviso(IdUsuarioOrigen,FechaEnvia,IdUsuarioDestino,Estado,Mensaje)
	VALUES(@PI_IdUsuarioOrigen,getdate(),@PI_IdUsuarioDestino,'N',@PI_Mensaje)


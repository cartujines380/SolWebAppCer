CREATE PROC [Participante].[Par_P_RegAdminUsuario]
@PI_IdUsuario varchar(50),
@PI_IdParticipante int,
@PI_RolAsignado int,
@PI_IdUsuarioRegistro varchar(50)
AS
	
-- Ingresar Registro del Cliente
	insert into Participante.Par_RegistroCliente(IdUsuario,IdParticipante,RolAsignado,IdUsuarioRegistro,
			FechaRegistro,OrigenRegistro,
			TipoTarjeta,NumeroTarjeta,TipoIdent,
			NumIdent,TipoPlanTrans, TipoCliente,Estado,PregSecreta,
			RespSecreta,FraseSecreta,AdmProducto)
		VALUES(@PI_IdUsuario,@PI_IdParticipante,@PI_RolAsignado,@PI_IdUsuarioRegistro,
			getdate(),'I',null,null,null,null,null,1,1,null,null,null,0)





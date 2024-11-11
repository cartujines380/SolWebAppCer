

CREATE PROCEDURE [Participante].[Par_P_ConEliLogicaCEP]
--Procedimiento que retorna 1 o 0
--para cambiar el estado a Inactivo del Cliente, Empleado o Proveedor
@PI_IdEmpresa int,
@PI_IdParticipante int,
@PI_TipoPart int,
@PO_Estado bit output
as
	
--zzz_F_ConEliLogicaCliente(@PI_IdEmpresa, @PI_IdParticipante)
	--zzz_F_ConEliLogicaEmpleado
	--zzz_F_ConEliLogicaProveedor
select @PO_Estado = 0








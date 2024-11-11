

/*

	exec Participante.Par_P_eliParticipante 1556

*/

CREATE PROCEDURE [Participante].[Par_P_eliParticipante]
@PI_IdParticipante as int
AS

SET NOCOUNT ON
-- Elimina Clientes
-- Hay que validar con Trrigger
DECLARE @IdCalificacion bigint
BEGIN TRY
	BEGIN TRAN
-- Eliminar Registro
	DELETE Seguridad.Seg_Registro
	FROM Seguridad.Seg_Registro ru INNER JOIN 
			Participante.Par_RegistroCliente rc ON ru.IdUsuario = rc.IdUsuario
	WHERE rc.IdParticipante = @PI_IdParticipante

-- Eliminar Roles
	DELETE Seguridad.Seg_RolUsuario
	FROM Seguridad.Seg_RolUsuario ru INNER JOIN 
			Participante.Par_RegistroCliente rc ON ru.IdUsuario = rc.IdUsuario
	WHERE rc.IdParticipante = @PI_IdParticipante

-- Eliminar permisos directos
	DELETE Seguridad.Seg_TransUsuario
	FROM Seguridad.Seg_TransUsuario tu INNER JOIN 
			Participante.Par_RegistroCliente rc ON tu.IdUsuario = rc.IdUsuario
	WHERE rc.IdParticipante = @PI_IdParticipante
	
--Elimino RegistroClientes
	delete Participante.Par_RegistroCliente WHERE IdParticipante = @PI_IdParticipante

--Elimino Organigrama
	delete Participante.Par_Organigrama WHERE IdEmpresa = @PI_IdParticipante

--Elimino Clientes
	delete Participante.Par_Cliente WHERE IdParticipante = @PI_IdParticipante

-- Elimino Empleados 
	delete Participante.Par_Empleado WHERE IdParticipante = @PI_IdParticipante

-- Elimino Proveedores
	delete Participante.Par_Proveedor WHERE IdParticipante = @PI_IdParticipante

-- Elimino Persona
	delete Participante.Par_Persona WHERE IdParticipante = @PI_IdParticipante

--Elimino Lotes de hacienda
	delete Participante.Par_LoteHac WHERE IdEmpresa = @PI_IdParticipante

--Elimina Oficinas
	delete Participante.Par_Oficina WHERE IdOficina = @PI_IdParticipante or IdEmpresa = @PI_IdParticipante


-- Elimino Empresa
	delete Participante.Par_Empresa WHERE IdParticipante = @PI_IdParticipante

--Elimina Contactos
	delete Participante.Par_Contacto WHERE IdParticipante = @PI_IdParticipante

--Elimina Documentos
	delete Participante.Par_DocumentoParticipante WHERE IdParticipante = @PI_IdParticipante

--Elimina MedioContacto
	delete Participante.Par_MedioContacto WHERE IdParticipante = @PI_IdParticipante

--Elimina Direcciones
	delete Participante.Par_Direccion WHERE IdParticipante = @PI_IdParticipante

-- Elimina Cuentas
--	delete Participante.Par_CuentaParticipante WHERE IdParticipante = @PI_IdParticipante

-- Elimina Parametros 
	delete Participante.Par_ParametroParticipante where IdParticipante = @PI_IdParticipante
	
-- Elimina Calificaciones
	--delete Proveedores.Pro_Avisos WHERE IdCliente = @PI_IdParticipante
	--delete Proveedores.Pro_DocumentosContrato WHERE IdCliente = @PI_IdParticipante
	--delete Proveedores.Pro_Contrato WHERE IdCliente = @PI_IdParticipante
	--delete Proveedores.Pro_ClienteProveedor WHERE IdCliente = @PI_IdParticipante
	--SELECT @IdCalificacion = IdCalificacion 
	--FROM Proveedores.Pro_Calificacion 
	--WHERE IdParticipante = @PI_IdParticipante
	--delete Proveedores.Pro_ClienteProveedor WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Requisitos WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Servicio WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_PropiedadesReferencias WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_RefBancarias WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_BalancesPer WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_BalancesEmp WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_CCalidadAmbiente WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Ciudades WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Distribucion WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Bodegas WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_MaqArendada WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_MaqPropia WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_ManufacEquipo WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_DetArea WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Compras WHERE IdCalificacion = @IdCalificacion
    --delete Proveedores.Pro_Seguimiento WHERE IdCalificacion = @IdCalificacion
	--delete Proveedores.Pro_Contacto WHERE IdCalificacion = @IdCalificacion
	      
	--delete Proveedores.Pro_Calificacion WHERE IdParticipante = @PI_IdParticipante


	delete seguridad.seg_semilla WHERE IdParticipante = @PI_IdParticipante

	delete seguridad.Seg_Clave WHERE IdParticipante = @PI_IdParticipante

-- Elimina Participante
	delete Participante.Par_Participante WHERE IdParticipante = @PI_IdParticipante

	IF XACT_STATE() = 1
		COMMIT TRAN
	
END TRY	
BEGIN CATCH
	--Preguntar si existe transaccion
	IF XACT_STATE() = 1
		ROLLBACK TRAN
	-- Produce un RAISERROR con el msg de error
	exec Participante.Par_P_Error 'Sol_P_EliParticipante'
END CATCH



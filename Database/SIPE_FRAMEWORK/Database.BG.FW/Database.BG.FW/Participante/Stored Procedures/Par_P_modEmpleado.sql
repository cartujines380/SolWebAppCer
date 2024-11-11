CREATE PROCEDURE [Participante].[Par_P_modEmpleado]
 @PI_docXML as varchar(max),
 @PI_IdUsuario varchar(50)
AS
declare @VL_idXML int
declare @VL_IdParticipante int
declare @VL_Cargo varchar(10)

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

SET NOCOUNT ON

Select @VL_IdParticipante = IdParticipante
FROM OPENXML (@VL_idXML, '/Participante') WITH (IdParticipante int)

-- Ingresa las Nuevas
insert into Participante.Par_Empleado(IdParticipante,IdTipoEmpleado,
		IdEmpresa,IdOficina,IdEmpresaPertenece,IdOrganigrama,
		IdCargo,Sueldo,HorasExtras,LibretaSeguro,
		FechaIngSeguro,FechaNotEgreso,IdMoneda,Estado)
	SELECT IdParticipante,IdTipoEmpleado,
		IdEmpresa,IdOficina,IdEmpresaPertenece,IdOrganigrama,
		IdCargo,Sueldo,HorasExtras,LibretaSeguro,
		FechaIngSeguro,FechaNotEgreso,IdMoneda,Estado
FROM   OPENXML (@VL_idXML, '/Participante/Empleados/Empleado_N') WITH Participante.Par_Empleado cx
  WHERE not exists (Select IdParticipante
		    FROM Participante.Par_Empleado c
			WHERE c.IdParticipante = cx.IdParticipante
				AND c.IdEmpresa = cx.IdEmpresa)

--Actualiza las existentes
UPDATE Participante.Par_Empleado
SET 	IdOficina = xe.IdOficina,
	IdTipoEmpleado = xe.IdTipoEmpleado,
	IdEmpresa = xe.IdEmpresa,
	IdEmpresaPertenece = xe.IdEmpresaPertenece,
	IdOrganigrama = xe.IdOrganigrama,
	IdCargo = xe.IdCargo,
	Sueldo = xe.Sueldo,
	IdMoneda = xe.IdMoneda,	
	HorasExtras = xe.HorasExtras,
	Estado = xe.Estado
FROM   Participante.Par_Empleado e, OPENXML (@VL_idXML, '/Participante/Empleados/Empleado_M') WITH Participante.Par_Empleado xe
WHERE e.IdParticipante = xe.IdParticipante
	AND e.IdEmpresa = xe.IdEmpresa

-- Elimina Cargas Familiares que no existan en el xml
DELETE Participante.Par_CargaFamiliar
FROM   Participante.Par_CargaFamiliar, OPENXML (@VL_idXML, '/Participante/CargaFamiliar/Carga_E') WITH Participante.Par_CargaFamiliar xc
WHERE Participante.Par_CargaFamiliar.IdParticipante = xc.IdParticipante
	AND Participante.Par_CargaFamiliar.IdCargaFamiliar = xc.IdCargaFamiliar


-- Elimina las que no existen en xml
DELETE Participante.Par_Empleado
FROM Participante.Par_Empleado, OPENXML (@VL_idXML, '/Participante/Empleados/Empleado_E') WITH Participante.Par_Empleado xe
WHERE Participante.Par_Empleado.IdParticipante = xe.IdParticipante 
	and Participante.Par_Empleado.IdEmpresa = xe.IdEmpresa

--Modifica Cargas Familiares
UPDATE Participante.Par_CargaFamiliar
SET IdTipoCarga = xc.IdTipoCarga,IdTipoIdentificacion = xc.IdTipoIdentificacion,
Identificacion = xc.Identificacion,Nombre = xc.Nombre,
FechaNac = xc.FechaNac
FROM  Participante.Par_CargaFamiliar c, OPENXML (@VL_idXML, '/Participante/CargaFamiliar/Carga_M') WITH Participante.Par_CargaFamiliar xc
WHERE c.IdParticipante = xc.IdParticipante AND c.IdCargaFamiliar = xc.IdCargaFamiliar

-- Ingresa Cargas Familiares
Insert into Participante.Par_CargaFamiliar(IdParticipante,IdCargaFamiliar,IdTipoCarga,IdTipoIdentificacion,
		Identificacion,Nombre,FechaNac)
	SELECT IdParticipante,IdCargaFamiliar,IdTipoCarga,IdTipoIdentificacion,
		Identificacion,Nombre,FechaNac
 FROM   OPENXML (@VL_idXML, '/Participante/CargaFamiliar/Carga_N') WITH Participante.Par_CargaFamiliar
  WHERE IdCargaFamiliar not in (Select IdCargaFamiliar
		FROM Participante.Par_CargaFamiliar 
		WHERE IdParticipante = @VL_IdParticipante)


	--Creacion de Rol de empleado, N = nuevo
	SELECT @VL_Cargo = IdCargo		
	FROM   OPENXML (@VL_idXML, '/Participante/Empleados/Empleado_N') WITH (IdCargo varchar(10))
	--Preguntar por cargo de empleado para asignar roles
	if (@VL_Cargo = '7')
		exec Seguridad.seg_p_setRol @PI_IdUsuario, 50 --Rol de administrador AFS
	if (@VL_Cargo = '8')
		exec Seguridad.seg_p_setRol @PI_IdUsuario, 51 --Rol de administrador Broker
	
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END

	--Creacion de Rol de empleado, M = Modificar
	SELECT @VL_Cargo = IdCargo		
	FROM   OPENXML (@VL_idXML, '/Participante/Empleados/Empleado_M') WITH (IdCargo varchar(10))
	--Preguntar por cargo de empleado para asignar roles
	if (@VL_Cargo = '10')
		exec Seguridad.seg_p_setRol @PI_IdUsuario, 11 --Rol de Coordinador
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 


--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML







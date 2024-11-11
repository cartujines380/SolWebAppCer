CREATE PROCEDURE [Participante].[Par_P_modProveedor]
 @PI_docXML as varchar(max)
AS
declare @VL_idXML int
declare @VL_IdParticipante int, @VL_IdUsuario varchar(50)
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

SET NOCOUNT ON

Select @VL_IdParticipante = IdParticipante
FROM OPENXML (@VL_idXML, '/Participante') WITH (IdParticipante int)


-- Asigna Rol de Cliente para los proveedores
SELECT @VL_IdUsuario = IdUsuario
 FROM Participante.Par_RegistroCliente
 WHERE IdParticipante = @VL_IdParticipante
IF len(isnull(@VL_IdUsuario,'')) > 0  
	exec Seguridad.seg_p_setRol @VL_IdUsuario,13 --Rol de proveedor

-- Ingresa las Nuevas
insert into Participante.Par_Proveedor(IdParticipante,IdEmpresa,IdOficina,
	ContribuyenteEspecial, Estado)
	SELECT IdParticipante,IdEmpresa,IdOficina,
	ContribuyenteEspecial, Estado
FROM   OPENXML (@VL_idXML, '/Participante/Proveedores/Proveedor_N') WITH Participante.Par_Proveedor  xp
WHERE not exists (Select IdParticipante
		    FROM Participante.Par_Proveedor c
			WHERE c.IdParticipante = xp.IdParticipante
				AND c.IdEmpresa = xp.IdEmpresa)

--Actualiza las existentes
UPDATE Participante.Par_Proveedor
SET 	IdOficina = xp.IdOficina,
	ContribuyenteEspecial = xp.ContribuyenteEspecial,
	Estado = xp.Estado	
FROM   Participante.Par_Proveedor p, OPENXML (@VL_idXML, '/Participante/Proveedores/Proveedor_M') WITH Participante.Par_Proveedor xp
WHERE p.IdParticipante = xp.IdParticipante
	AND p.IdEmpresa = xp.IdEmpresa

-- Elimina las que no existen en xml
DELETE Participante.Par_Proveedor
FROM Participante.Par_Proveedor, OPENXML (@VL_idXML, '/Participante/Proveedores/Proveedor_E') WITH Participante.Par_Proveedor xp
WHERE  Participante.Par_Proveedor.IdParticipante = xp.IdParticipante 
	and Participante.Par_Proveedor.IdEmpresa = xp.IdEmpresa


--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML







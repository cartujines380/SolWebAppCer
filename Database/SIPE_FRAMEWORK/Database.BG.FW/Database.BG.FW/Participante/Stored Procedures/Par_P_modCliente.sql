CREATE PROCEDURE [Participante].[Par_P_modCliente]
 @PI_docXML as varchar(max)
AS

declare @VL_idXML int, @VL_IdUsuario varchar(50)
declare @VL_IdParticipante int

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
	exec Seguridad.seg_p_setRol @VL_IdUsuario,12 --Rol de cliente

-- Ingresa las Nuevas
insert into Participante.Par_cliente(IdParticipante,IdEmpresa,IdOficina,IdOficinaGestion,PorcentajeDescuento,
	IdVendedor,
	ContribuyenteEspecial,IdCalificacion,Iva,Estado,GastoAnual)
	SELECT IdParticipante,IdEmpresa,IdOficina,IdOficinaGestion,PorcentajeDescuento,
	IdVendedor,
	ContribuyenteEspecial,IdCalificacion,Iva,Estado,GastoAnual
FROM   OPENXML (@VL_idXML, '/Participante/Clientes/Cliente_N') WITH Participante.Par_Cliente cx
  WHERE not exists (Select IdParticipante
		    FROM Participante.Par_Cliente c
			WHERE c.IdParticipante = cx.IdParticipante
				AND c.IdEmpresa = cx.IdEmpresa)

--Actualiza las existentes
UPDATE Participante.Par_Cliente
SET 	IdOficina = xe.IdOficina,
	IdOficinaGestion = xe.IdOficinaGestion,
	PorcentajeDescuento = xe.PorcentajeDescuento,
	IdVendedor = xe.IdVendedor,
	ContribuyenteEspecial = xe.ContribuyenteEspecial,
	IdCalificacion=xe.IdCalificacion,
	Iva = xe.Iva, Estado = xe.Estado,
	GastoAnual = isnull(xe.GastoAnual,c.GastoAnual)
FROM   Participante.Par_Cliente c, OPENXML (@VL_idXML, '/Participante/Clientes/Cliente_M') WITH Participante.Par_Cliente xe
WHERE c.IdParticipante = xe.IdParticipante AND c.IdEmpresa = xe.IdEmpresa

-- Elimina las que no existen en xml
DELETE Participante.Par_Cliente
FROM   Participante.Par_Cliente , OPENXML (@VL_idXML, '/Participante/Clientes/Cliente_E') WITH Participante.Par_Cliente xe
WHERE Participante.Par_Cliente.IdParticipante = xe.IdParticipante AND Participante.Par_Cliente.IdEmpresa = xe.IdEmpresa


--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML








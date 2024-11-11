
CREATE procedure [Participante].[Par_P_ModProductos]
@PI_IdUsuario varchar(50), 
@PI_docXML as varchar(max)
AS


declare @VL_IdParticipante int
declare @VL_idXML int

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

SELECT @VL_IdParticipante = IdParticipante FROM Participante.Par_Participante WHERE IdUsuario = @PI_IdUsuario
BEGIN TRAN

	UPDATE Participante.Par_Producto
	SET EstadoProducto = xp.EstadoProducto
	FROM Participante.Par_Producto p, OPENXML (@VL_idXML, '/ResultSet/Producto') WITH Participante.Par_Producto xp
	WHERE p.IdParticipante = @VL_IdParticipante
          AND p.TipoProducto = xp.TipoProducto
          AND p.NumeroProducto = xp.NumeroProducto

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
EXEC sp_xml_removedocument @VL_idXML

--setea permisos
if exists(SELECT 1 FROM  Participante.Par_Producto
	 WHERE TipoProducto between 1 and 2  and EstadoProducto = 1 and IdParticipante = @VL_IdParticipante)
	exec Seguridad.seg_p_setRol @PI_IdUsuario,4
else
	exec Seguridad.seg_p_eliRol @PI_IdUsuario,4

if exists(SELECT 1 FROM Participante.Par_Producto
	 WHERE TipoProducto between 3 and 5 and EstadoProducto = 1 and IdParticipante = @VL_IdParticipante)
	exec Seguridad.seg_p_setRol @PI_IdUsuario,5
else
	exec Seguridad.seg_p_eliRol @PI_IdUsuario,5


COMMIT TRAN






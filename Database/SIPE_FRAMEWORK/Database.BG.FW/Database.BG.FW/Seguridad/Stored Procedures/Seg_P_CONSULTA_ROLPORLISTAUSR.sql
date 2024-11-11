
-- EXEC [Seguridad].[Seg_P_CONSULTA_ROLPORLISTAUSR] '<Root><Usr Usuario="1790005739TesaliaU1" /><Usr Usuario="1790005739prueba1" /></Root>'

CREATE PROCEDURE [Seguridad].[Seg_P_CONSULTA_ROLPORLISTAUSR]
	@PI_XmlListaUsr	XML
AS
BEGIN
         SELECT distinct ur.idusuario, rl.idrol, rl.descripcion
         FROM Seguridad.Seg_RolUsuario ur
			INNER JOIN Seguridad.Seg_Rol rl
				ON rl.idrol = ur.idrol
			INNER JOIN @PI_XmlListaUsr.nodes('/Root/Usr') AS R(nref)
				ON ur.idusuario = nref.value('@Usuario','VARCHAR(20)')
END



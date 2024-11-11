
CREATE PROC [Catalogo].[Ctl_P_ActCatalogoEmpresa]
	@PI_xmlDoc varchar(8000)
AS
DECLARE @VL_idXML int, @VL_IdTabla int, @VL_IdEmpresa int, 
	@VL_IdRubro int, @VL_IdModulo int 
--Preparo el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_xmlDoc
--Obtengo los datos necesarios
SELECT @VL_IdTabla = IdTabla, @VL_IdEmpresa = IdEmpresa, 
		@VL_IdRubro = IdRubro, @VL_IdModulo = IdModulo 
	FROM OPENXML (@VL_idXML, '/Usuario') 
		WITH ( IdTabla int, IdEmpresa int, IdRubro int, IdModulo int ) 
IF ( NOT @VL_IdTabla IS NULL AND LEN(@VL_IdTabla) = 0 ) 
	SET @VL_IdTabla = NULL 
--Inicio la transaccion
BEGIN TRAN
--Ingresa los Nuevos Catalogos del Xml
INSERT INTO Catalogo.Ctl_CatalogoEmpresa( IdEmpresa, IdTabla, Codigo )
SELECT @VL_IdEmpresa, IdTabla, Codigo 
	FROM OPENXML (@VL_idXML, '/Usuario/Catalogo') WITH ( IdTabla int, Codigo varchar(50) ) cx 
  	WHERE not exists ( Select 1 FROM Catalogo.Ctl_CatalogoEmpresa c 
				WHERE c.IdEmpresa = @VL_IdEmpresa 
				AND c.IdTabla = cx.IdTabla 
				AND c.Codigo = cx.Codigo ) 
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END 
-- Elimino los que No existen en el Xml
DELETE Catalogo.Ctl_CatalogoEmpresa 
WHERE Catalogo.Ctl_CatalogoEmpresa.IdEmpresa = @VL_IdEmpresa 
	AND ( Catalogo.Ctl_CatalogoEmpresa.IdTabla = @VL_IdTabla OR @VL_IdTabla IS NULL) 
	AND not exists ( 
		Select 1 FROM OPENXML (@VL_idXML, '/Usuario/Catalogo') WITH ( IdTabla int, Codigo varchar(50) ) cx 
			WHERE Catalogo.Ctl_CatalogoEmpresa.IdEmpresa = @VL_IdEmpresa 
			AND Catalogo.Ctl_CatalogoEmpresa.IdTabla = cx.IdTabla 
			AND Catalogo.Ctl_CatalogoEmpresa.Codigo = cx.Codigo ) 
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END
-- PARTE DE RUBROS CONTABLES
IF ( NOT @VL_IdRubro IS NULL AND NOT @VL_IdModulo IS NULL )
BEGIN
	DELETE FROM  sige_Contabilidad.con_SegmentoModulo 
		WHERE IdEmpresa = @VL_IdEmpresa 
			AND IdRubro = @VL_IdRubro 
			AND IdModulo = @VL_IdModulo 
			AND IdCodigo IN (SELECT t.Codigo FROM Catalogo.Ctl_Catalogo t 
								WHERE t.IdTabla = @VL_IdTabla) 
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
	INSERT INTO sige_Contabilidad.con_SegmentoModulo
		(IdEmpresa, IdModulo, IdCodigo, IdSegmento, ValorSegmento, TipoSegmento, IdRubro)
	SELECT 
		@VL_IdEmpresa, @VL_IdModulo, cx.Codigo, cx.IdSegmento, cx.IdItem, cx.TipoSegmento, cx.IdRubro
	FROM OPENXML (@VL_idXML, '/Usuario/Catalogo/Segmento') 
		WITH ( Codigo int '../@Codigo', IdSegmento int, IdItem varchar(30), TipoSegmento char(10), IdRubro int ) cx 
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
END
--Finalizo la transaccion
COMMIT TRAN
--Libero el documento XML
EXEC sp_xml_removedocument @VL_idXML






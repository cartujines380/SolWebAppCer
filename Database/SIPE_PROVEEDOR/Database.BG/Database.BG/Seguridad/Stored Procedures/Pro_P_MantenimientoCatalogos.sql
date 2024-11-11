
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Fausto Silva
-- Create date: 03-06-2022
-- Description:	Procedure Pro_P_MantenimientoCatalogos
-- 851
-- =============================================

CREATE PROCEDURE [Seguridad].[Pro_P_MantenimientoCatalogos]
	@PI_ParamXML xml
AS
BEGIN
SET NOCOUNT ON;
	
	DECLARE
	 @Tipo					INT,
	 @W_IDCATALOGO			BIGINT,
	 @W_CodigoCatalogo		varchar(20),
	 @W_NombreCatalogo		varchar(100),
	 @W_Estado				varchar(2)


	SELECT 
		 @Tipo					= nref.value('@Tipo','int')
		 ,@W_CodigoCatalogo 	= nref.value('@IdCatalogo','varchar(20)')
		 ,@W_IDCATALOGO			= nref.value('@idCabecera','BIGINT')
		 ,@W_NombreCatalogo	    = nref.value('@NombreCatalogo','VARCHAR(100)')
		 ,@W_Estado				= nref.value('@Estado','varchar(2)')
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 


	
	IF (@W_CodigoCatalogo='')
	BEGIN
		SET @W_CodigoCatalogo=NULL
	END

	SET @W_NombreCatalogo='%'+@W_NombreCatalogo+'%'

	IF (@W_Estado='')
	BEGIN
		SET @W_Estado=NULL
	END

	--TIPO 1 = CATALOGO
	IF(@Tipo='1')
	BEGIN
		SELECT CD.Codigo as Codigo,CD.Detalle,DescAlterno
		FROM Proveedor.Pro_Tabla CC
		INNER JOIN Proveedor.Pro_Catalogo CD
			ON CC.Tabla=CD.Tabla
			WHERE CD.ESTADO='A'		
			AND CC.TablaNombre = @W_NombreCatalogo
			ORDER BY CD.Detalle 
	END
	IF(@Tipo='2')
	BEGIN

		INSERT INTO Proveedor.Pro_Tabla(Tabla,TablaNombre, Estado)
		SELECT nref.value('@IdCatalogo','int'),
		nref.value('@descripcion','VARCHAR(200)'),
		nref.value('@Estado','VARCHAR(2)')
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 

		--SELECT @W_IDCATALOGO=@@IDENTITY
		  print 'dddd'
			INSERT INTO Proveedor.Pro_Catalogo(
			Tabla,
			Codigo,
			Detalle,
			Estado,
			DescAlterno)
			SELECT 
			 @W_CodigoCatalogo
			,nref.value('@codigoDetalle','VARCHAR(10)')
			,nref.value('@descripcion','VARCHAR(200)')
			,nref.value('@Estado','VARCHAR(2)'),
			nref.value('@descAlterno','VARCHAR(20)')
			FROM @PI_ParamXML.nodes('/Root/CatalogoDetalle') as item(nref) 

			SELECT RETORNO =@W_IDCATALOGO,ESTADO ='INGRESADO' 

	END
	IF(@Tipo='3')
	BEGIN
		SELECT	Tabla as IdCabecera, Tabla as codigoCatalogo, TablaNombre as descripcion,'A' AS estado
		FROM	Proveedor.Pro_Tabla
		WHERE Tabla = ISNULL(@W_CodigoCatalogo,Tabla)	
		AND TablaNombre LIKE(@W_NombreCatalogo)
		--codigoCatalogo=ISNULL(@W_CodigoCatalogo,codigoCatalogo)
		--AND		Descripcion LIKE(@W_NombreCatalogo)
	END
	IF(@Tipo='4')
	BEGIN
		UPDATE	Proveedor.Pro_Catalogo set
		Detalle=nref.value('@descripcion','VARCHAR(200)'),
		Estado=nref.value('@Estado','VARCHAR(5)'),
		DescAlterno=nref.value('@descAlterno','VARCHAR(20)')
		FROM  @PI_ParamXML.nodes('/Root/CatalogoDetalle') as item(nref) 
		INNER JOIN Proveedor.Pro_Catalogo ON
		nref.value('@idDetalle','VARCHAR(10)')= Codigo
		AND Tabla=@W_IDCATALOGO

		INSERT INTO Proveedor.Pro_Catalogo(
			Tabla,
			Codigo,
			Detalle,
			Estado,
			DescAlterno)
		SELECT 
		@W_IDCATALOGO
		,nref.value('@codigoDetalle','VARCHAR(10)')
		,nref.value('@descripcion','VARCHAR(200)')
		,nref.value('@Estado','VARCHAR(2)')
		,nref.value('@descAlterno','VARCHAR(20)')
		FROM @PI_ParamXML.nodes('/Root/CatalogoDetalle') as item(nref) 
		WHERE nref.value('@idDetalle','VARCHAR(10)') NOT IN(
		SELECT Codigo
		FROM  @PI_ParamXML.nodes('/Root/CatalogoDetalle') as item(nref) 
		INNER JOIN Proveedor.Pro_Catalogo ON
		nref.value('@idDetalle','VARCHAR(10)')= Codigo
		AND Tabla=@W_IDCATALOGO)

		SELECT RETORNO ='',ESTADO ='MODIFICADO' 

	END
	IF(@Tipo='5')
	BEGIN


		SELECT  Tabla as IdCabecera, Tabla as codigoCatalogo, TablaNombre as descripcion
		FROM	Proveedor.Pro_Tabla CO
		WHERE CO.Tabla = @W_CodigoCatalogo

		SELECT PE.Codigo as CodElemento,PE.Codigo as codigoDetalle,pe.Detalle as Descripcion,pe.Estado,ISNULL(DescAlterno,'') DescAlterno
		FROM  Proveedor.Pro_Tabla CP
			INNER JOIN Proveedor.Pro_Catalogo PE
				ON CP.Tabla=PE.Tabla
			WHERE Cp.Tabla=@W_CodigoCatalogo

	END
	
END





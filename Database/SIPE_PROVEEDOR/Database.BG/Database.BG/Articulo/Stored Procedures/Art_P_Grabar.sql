-- =============================================
-- Author:		Jessica Navarrete
-- Create date: 15-06-2015
-- Description:	Ingreso, actualización de solicitudes de articulos
-- =============================================
CREATE PROCEDURE [Articulo].[Art_P_Grabar]
	@PI_ParamXML xml
AS
BEGIN TRY
	
	BEGIN TRAN

	SET NOCOUNT ON;
	DECLARE @l_accion CHAR(1), @l_codcabecera BIGINT, @l_CodSap VARCHAR(10);

	SELECT 
		 @l_accion = nref.value('@accion','char(1)'),
		 @l_CodSap = nref.value('@CodSapProveedor','varchar(10)'),
		 @l_codcabecera = nref.value('@IdSolArticulo','varchar(10)')
	FROM @PI_ParamXML.nodes('/Root/Datos') as item(nref) 

	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root/Datos') AS item(nref) where nref.value('@accion','char(1)') = 'I')
	BEGIN
		--Inserta Cabecera
		INSERT INTO Articulo.Art_SolArticulo
           (IdEmpresa
           ,TipoSolArticulo
           ,CodSapProveedor
           ,CodSapContacto
           ,Fecha
           ,EstadoSolicitud
           ,Usuario)
		SELECT 
				nref.value('@IdEmpresa','int'),
				nref.value('@TipoSolArticulo','varchar(10)'),
				nref.value('@CodSapProveedor','varchar(10)'),
				nref.value('@CodSapContacto','varchar(15)'),
				GETDATE(),
				nref.value('@EstadoSolicitud','varchar(10)'),
				nref.value('@Usuario','varchar(100)')
		FROM @PI_ParamXML.nodes('/Root/Datos') as item(nref)
		WHERE nref.value('@accion','char(1)') = 'I';

		SELECT @l_codcabecera = @@IDENTITY;
	END
	
	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root/Datos') AS item(nref) where nref.value('@accion','char(1)') = 'U')
	BEGIN

		UPDATE cab
		   SET EstadoSolicitud = nref.value('@EstadoSolicitud','varchar(10)')
		 FROM Articulo.Art_SolArticulo cab
			INNER JOIN @PI_ParamXML.nodes('/Root/Datos') item(nref) 
				ON (cab.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND nref.value('@accion','char(1)') = 'U');

		UPDATE det
		   SET EstadoSolicitud = nref.value('@EstadoSolicitud','varchar(10)')
		 FROM Articulo.Art_SolArtDetalle det
			INNER JOIN @PI_ParamXML.nodes('/Root/Detalle') item(nref) 
				ON (det.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND nref.value('@accion','char(1)') = 'U');
	END

	-- ============================================= DETALLE ==========================================
	DELETE det FROM Articulo.Art_SolArtDetalle det 
	INNER JOIN @PI_ParamXML.nodes('/Root/Detalle') item(nref) 
		ON (det.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND det.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
			AND nref.value('@accion','char(1)') = 'D');
	
	UPDATE det
		SET  Descripcion = nref.value('@Descripcion','varchar(100)')
			,Marca_pr = nref.value('@Marca_pr','varchar(300)')
			,TextoBreve = nref.value('@TextoBreve','varchar(200)')
			,Modelo_pr = nref.value('@Modelo_pr','varchar(300)')
			,Coleccion_pr = nref.value('@Coleccion_pr','varchar(300)')
			,Temporada_pr = nref.value('@Temporada_pr','varchar(300)')
			,Estacion_pr = nref.value('@Estacion_pr','varchar(300)')
			,LineaNegocio_pr = nref.value('@LineaNegocio_pr','varchar(300)')
			,TamArticulo_pr = nref.value('@TamArticulo_pr','varchar(200)')
			,GradoAlcohol_pr = nref.value('@GradoAlcohol_pr','varchar(18)')
			,ClacificacionFiscal = nref.value('@ClacificacionFiscal','varchar(10)')
			,Deducible = nref.value('@Deducible','varchar(10)')
		FROM Articulo.Art_SolArtDetalle det
		INNER JOIN @PI_ParamXML.nodes('/Root/Detalle') item(nref) 
			ON (det.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND det.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
				AND nref.value('@accion','char(1)') = 'U');

	INSERT INTO Articulo.Art_SolArtDetalle
        (IdSolArticulo
		,IdSolDetArticulo
        ,CodReferenciaProveedor
        ,Descripcion
        ,Marca_pr
        ,TextoBreve
        ,Modelo_pr
        ,Coleccion_pr
        ,Temporada_pr
        ,Estacion_pr
        ,LineaNegocio_pr
        ,TamArticulo_pr
        ,GradoAlcohol_pr
        ,ClacificacionFiscal
        ,Deducible
        ,EstadoSolicitud
        ,EstadoArticulo)
	SELECT 
		@l_codcabecera,
		nref.value('@IdSolDetArticulo','bigint'),
		@l_CodSap,
		nref.value('@Descripcion','varchar(100)'),
		nref.value('@Marca_pr','varchar(300)'),
		nref.value('@TextoBreve','varchar(200)'),
		nref.value('@Modelo_pr','varchar(300)'),
		nref.value('@Coleccion_pr','varchar(300)'),
		nref.value('@Temporada_pr','varchar(300)'),
		nref.value('@Estacion_pr','varchar(300)'),
		nref.value('@LineaNegocio_pr','varchar(300)'),
		nref.value('@TamArticulo_pr','varchar(200)'),
		nref.value('@GradoAlcohol_pr','varchar(18)'),
		nref.value('@ClacificacionFiscal','varchar(10)'),
		nref.value('@Deducible','varchar(10)'),
		nref.value('@EstadoSolicitud','varchar(10)'),
		nref.value('@EstadoArticulo','varchar(10)')
	FROM @PI_ParamXML.nodes('/Root/Detalle') as item(nref)
	WHERE nref.value('@accion','char(1)') = 'I';
	-- ============================================= DETALLE ==========================================
	
	-- ============================================= MEDIDAS ==========================================
	DELETE med FROM Articulo.Art_SolArtUnidadMedida med 
	INNER JOIN @PI_ParamXML.nodes('/Root/Medidas') item(nref) 
		ON (med.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND med.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
			AND med.UnidadCaja = nref.value('@UnidadCaja','varchar(20)') AND nref.value('@accion','char(1)') = 'D');
	
	UPDATE med
	   SET CodUnidadMedidaAlter = nref.value('@CodUnidadMedidaAlter','varchar(10)')
		  ,CodBarra = nref.value('@CodBarra','varchar(20)')
		  ,PesoNeto = nref.value('@PesoNeto','numeric(19,4)')
		  ,CostoSinIva = nref.value('@CostoSinIva','numeric(19,4)')
		  ,Descuento1 = nref.value('@Descuento1','numeric(19,4)')
		  ,Descuento2 = nref.value('@Descuento2','numeric(19,4)')
		  ,CostoFOB = nref.value('@CostoFOB','numeric(19,4)')
		  ,ImpuestoVerde = nref.value('@ImpuestoVerde','bit')
	FROM Articulo.Art_SolArtUnidadMedida med
		INNER JOIN @PI_ParamXML.nodes('/Root/Medidas') item(nref) 
			ON (med.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND med.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
				AND med.UnidadCaja = nref.value('@UnidadCaja','varchar(20)') AND nref.value('@accion','char(1)') = 'U');

	INSERT INTO Articulo.Art_SolArtUnidadMedida
        (IdSolDetArticulo
        ,IdSolArticulo
        ,UnidadCaja
        ,CodUnidadMedidaAlter
        ,CodBarra
        ,PesoNeto
        ,CostoSinIva
        ,Descuento1
        ,Descuento2
        ,CostoFOB
        ,ImpuestoVerde
        ,Estado)
	SELECT 
		nref.value('@IdSolDetArticulo','bigint'),
		@l_codcabecera,
		nref.value('@UnidadCaja','varchar(20)'),
		nref.value('@CodUnidadMedidaAlter','varchar(10)'),
		nref.value('@CodBarra','varchar(20)'),
		nref.value('@PesoNeto','numeric(19,4)'),
		nref.value('@CostoSinIva','numeric(19,4)'),
		nref.value('@Descuento1','numeric(19,4)'),
		nref.value('@Descuento2','numeric(19,4)'),
		nref.value('@CostoFOB','numeric(19,4)'),
		nref.value('@ImpuestoVerde','bit'),
		1
	FROM @PI_ParamXML.nodes('/Root/Medidas') as item(nref)
	WHERE nref.value('@accion','char(1)') = 'I';
	-- ============================================= MEDIDAS ==========================================
	
	-- ============================================= IMAGENES =========================================
	DELETE ima FROM Articulo.Art_SolArtDocAdjunto ima 
	INNER JOIN @PI_ParamXML.nodes('/Root/Imagenes') item(nref) 
		ON (ima.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND ima.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
			AND ima.NomArchivo = nref.value('@NomArchivo','varchar(255)') AND nref.value('@accion','char(1)') = 'D');
	
	UPDATE ima SET Archivo = nref.value('@Archivo','varbinary(MAX)')
	FROM Articulo.Art_SolArtDocAdjunto ima
		INNER JOIN @PI_ParamXML.nodes('/Root/Imagenes') item(nref) 
			ON (ima.IdSolArticulo= nref.value('@IdSolArticulo','bigint') AND ima.IdSolDetArticulo= nref.value('@IdSolDetArticulo','bigint') 
				AND ima.NomArchivo = nref.value('@NomArchivo','varchar(255)') AND nref.value('@accion','char(1)') = 'U');

	INSERT INTO Articulo.Art_SolArtDocAdjunto
        (IdSolDetArticulo
        ,IdSolArticulo
        ,CodDocumento
        ,NomArchivo
        ,Archivo
        ,FechaCarga
        ,Estado)
	SELECT 
		nref.value('@IdSolDetArticulo','bigint'),
		@l_codcabecera,
		nref.value('@CodDocumento','varchar(10)'),
		nref.value('@NomArchivo','varchar(255)'),
		nref.value('@Archivo','varbinary(MAX)'),
		GETDATE(), 1
	FROM @PI_ParamXML.nodes('/Root/Imagenes') as item(nref)
	WHERE nref.value('@accion','char(1)') = 'I';
	
	-- ============================================= IMAGENES =========================================
	
	
	-- ============================================= ESTADOS ==========================================
	INSERT INTO Articulo.Art_SolArtHistEstado
        (IdSolDetArticulo
        ,IdSolArticulo
        ,Motivo
        ,Observacion
        ,Usuario
        ,Fecha
        ,EstadoSolicitud)
	SELECT 
		nref.value('@IdSolDetArticulo','bigint'),
		@l_codcabecera,
		null,
		'Nuevo Artículo',
		nref.value('@Usuario','varchar(100)'),
		GETDATE(), 
		nref.value('@EstadoSolicitud','varchar(10)')
	FROM @PI_ParamXML.nodes('/Root/Detalle') as item(nref)
	WHERE nref.value('@accion','char(1)') = 'I' OR nref.value('@accion','char(1)') = 'U';
	-- ============================================= ESTADOS ==========================================

	SELECT @l_codcabecera AS IdSolDetArticulo;

IF @@TRANCOUNT > 0
		COMMIT	TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	exec SP_PROV_Error @sp='[Art_P_SolicitudArticulo]'
END CATCH



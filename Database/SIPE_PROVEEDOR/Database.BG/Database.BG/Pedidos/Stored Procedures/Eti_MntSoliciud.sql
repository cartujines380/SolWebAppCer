
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--IF EXISTS (
--		SELECT *
--		FROM sysobjects
--		WHERE name = 'Eti_MntSoliciud'
--		)
--	DROP PROC Pedidos.Eti_MntSoliciud
--GO

CREATE PROCEDURE [Pedidos].[Eti_MntSoliciud] (@PI_ParamXML XML)
AS
BEGIN
	DECLARE	@OPCION CHAR(3)
			,@IDSOLICITUD INT
			,@IDPEDIDO INT
			,@NUMPEDIDO VARCHAR(10)
			,@CODARTICULO VARCHAR(18)
			,@CANDESPACHAR INT
			,@FECHA VARCHAR(10)
			,@FECHA2 VARCHAR(10)
			,@ESTADO CHAR(2)
			,@CODSAP VARCHAR(20)
			,@TIPOETIQUETA VARCHAR(5)
			,@TIPOTRAMA VARCHAR(25)

	--INSERT INTO DT VALUES(@PI_ParamXML)
	SELECT	@OPCION			= nref.value('@OPCION', 'CHAR(3)')
			,@IDSOLICITUD	= nref.value('@IDSOLICITUD', 'INT')
			,@TIPOETIQUETA	= nref.value('@TIPOETIQUETA', 'VARCHAR(5)')
			,@IDPEDIDO		= nref.value('@IDPEDIDO', 'INT')
			,@NUMPEDIDO		= nref.value('@NUMPEDIDO', 'VARCHAR(10)')
			,@CODARTICULO	= nref.value('@CODARTICULO', 'VARCHAR(18)')
			,@CANDESPACHAR	= nref.value('@CANDESPACHAR', 'INT')
			,@FECHA			= nref.value('@FECHA', 'VARCHAR(10)')
			,@FECHA2		= nref.value('@FECHA2', 'VARCHAR(10)')
			,@ESTADO		= nref.value('@ESTADO', 'VARCHAR(2)')
			,@CODSAP		= nref.value('@CODSAP', 'VARCHAR(20)')
			,@TIPOTRAMA		= nref.value('@TIPOTRAMA', 'VARCHAR(25)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @dia VARCHAR(2)
			,@mes VARCHAR(2)
			,@anio VARCHAR(4)

		SET @dia	= substring(@FECHA, 1, 2)
		SET @mes	= substring(@FECHA, 4, 2)
		SET @anio	= substring(@FECHA, 7, 4)
		SET @FECHA	= @anio + '/' + @mes + '/' + @dia
		SET @dia	= substring(@FECHA2, 1, 2)
		SET @mes	= substring(@FECHA2, 4, 2)
		SET @anio	= substring(@FECHA2, 7, 4)
		SET @FECHA2 = @anio + '/' + @mes + '/' + @dia

		DECLARE @W_RETORNO VARCHAR(10)
		DECLARE @descripcion_vacia VARCHAR(25)

		IF (@OPCION = ('AP'))
		BEGIN
			SELECT RETORNO = COUNT(1)
			FROM Etiqueta.Eti_AsignacionProv
			WHERE CODPROVEEDOR = @CODSAP
				AND GENERAETIQUETA = 1
		END

		IF (@OPCION = ('C'))
		BEGIN
			SELECT x.*
				,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
				,y.DesArticulo
			FROM [Etiqueta].[Eti_Solicitud] x
			INNER JOIN [Etiqueta].[Eti_Solicitud_Detalle] xx ON x.IdSolEtiqueta = xx.IdSolEtiqueta
			INNER JOIN [Pedidos].[Ped_PedidoDetalle] y ON x.IdPedido = y.IdPedido
				AND xx.CodArticulo = y.CodArticulo
			WHERE x.IdPedido = @IDPEDIDO --AND xx.CodArticulo=@CODARTICULO	

			SELECT x.*
				,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
				,y.DesArticulo
			FROM [Etiqueta].[Eti_Solicitud] x
			INNER JOIN [Etiqueta].[Eti_Solicitud_Detalle] xx ON x.IdSolEtiqueta = xx.IdSolEtiqueta
			INNER JOIN [Pedidos].[Ped_PedidoDetalle] y ON x.IdPedido = y.IdPedido
				AND xx.CodArticulo = y.CodArticulo
		END

		IF (@OPCION = 'AWW')
		BEGIN TRY
			BEGIN TRAN

			UPDATE DT
			SET DT.Estado = @ESTADO
			FROM Etiqueta.Eti_Solicitud DT
			WHERE dt.IdSolEtiqueta = @IDSOLICITUD

			DELETE Etiqueta.Eti_Solicitud_Tipo
			FROM @PI_ParamXML.nodes('/Root/Etiquetas') AS R(nref)
			INNER JOIN Etiqueta.Eti_Solicitud_Tipo OJ ON OJ.IdSolEtiqueta = nref.value('@IDSOLICITUD', 'INT')
				AND OJ.CodArticulo = nref.value('@CODARTICULO', 'VARCHAR(18)')

			INSERT Etiqueta.Eti_Solicitud_Tipo(IdSolEtiqueta,TipoEtiqueta,CodArticulo,NumMaterialProv,GrupoArticulo,PVP,VNOAF)
			SELECT nref.value('@IDSOLICITUD', 'INT')
				,nref.value('@TIPOETIQUETA', 'VARCHAR(5)')
				,nref.value('@CODARTICULO', 'VARCHAR(18)')
				,nref.value('@NUM_MATERIAL_PROV', 'VARCHAR(35)')
				,nref.value('@GRUPO_ARTICULO', 'VARCHAR(9)')
				,nref.value('@PVP', 'NUMERIC(18,2)')
				,nref.value('@VNOAF', 'NUMERIC(18,2)')
			FROM @PI_ParamXML.nodes('/Root/Etiquetas') AS R(nref)

			--WHERE NOT IN(SELECT * FROM 
			IF @@ROWCOUNT > 0
			BEGIN
				COMMIT TRAN

				SELECT 'true' AS RETORNO
			END
			ELSE
			BEGIN
				ROLLBACK TRAN

				SELECT 'false' AS RETORNO
			END
		END TRY

		BEGIN CATCH
			IF @@ROWCOUNT > 0
				ROLLBACK TRAN

			SELECT 'false' AS RETORNO
				--exec SP_PROV_Error @sp='[Ped_P_CargaPedidoProveedor]'
		END CATCH

		IF (@OPCION = ('AW'))
		BEGIN TRY
			BEGIN TRAN

			UPDATE DT
			SET DT.Estado = @ESTADO
			FROM Etiqueta.Eti_Solicitud DT
			WHERE dt.IdSolEtiqueta = @IDSOLICITUD

			IF @@ROWCOUNT > 0
			BEGIN
				COMMIT TRAN

				SELECT 'true' AS RETORNO
			END
			ELSE
			BEGIN
				ROLLBACK TRAN

				SELECT 'false' AS RETORNO
			END
		END TRY

		BEGIN CATCH
			IF @@ROWCOUNT > 0
				ROLLBACK TRAN

			SELECT 'false' AS RETORNO
				--exec SP_PROV_Error @sp='[Ped_P_CargaPedidoProveedor]'
		END CATCH

		IF (@OPCION = ('F'))
		BEGIN
			IF (@ESTADO = ('T'))
			BEGIN
				SELECT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
				FROM [Etiqueta].[Eti_Solicitud] x
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega = @FECHA
				ORDER BY X.IdSolEtiqueta
			END
			ELSE
			BEGIN
				SELECT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
				FROM [Etiqueta].[Eti_Solicitud] x
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega = @FECHA
					AND x.estado = @ESTADO
				ORDER BY X.IdSolEtiqueta
			END
		END

		IF (@OPCION = ('R1'))
		BEGIN
			SELECT CONVERT(VARCHAR(15), ES.FechaEntrega, 103) AS fechaEntrega
				,ES.NumPedido AS numPedido
				,ED.CanDespachar AS etiquetaCarton
				,0 AS etiquetaAdhesivas
				,0 AS etiquetaRFID
				,EDT.TipoEtiqueta
			INTO #B
			FROM [Etiqueta].[Eti_Solicitud] ES
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle ED ON ES.IdSolEtiqueta = ED.IdSolEtiqueta
			INNER JOIN Etiqueta.Eti_Solicitud_Tipo EDT ON ED.IdSolEtiqueta = EDT.IdSolEtiqueta
				AND ED.CodArticulo = EDT.CodArticulo
			WHERE ES.CodigoSap = @CODSAP
				AND EDT.TipoEtiqueta = '02'
				AND ES.FechaEntrega BETWEEN @FECHA
					AND @FECHA2
			
			UNION
			
			SELECT CONVERT(VARCHAR(15), ES.FechaEntrega, 103) AS fechaEntrega
				,ES.NumPedido AS numPedido
				,0 AS etiquetaCarton
				,ED.CanDespachar AS etiquetaAdhesivas
				,0 AS etiquetaRFID
				,EDT.TipoEtiqueta
			FROM [Etiqueta].[Eti_Solicitud] ES
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle ED ON ES.IdSolEtiqueta = ED.IdSolEtiqueta
			INNER JOIN Etiqueta.Eti_Solicitud_Tipo EDT ON ED.IdSolEtiqueta = EDT.IdSolEtiqueta
				AND ED.CodArticulo = EDT.CodArticulo
			WHERE ES.CodigoSap = @CODSAP
				AND EDT.TipoEtiqueta IN ('04','05','06')
				AND ES.FechaEntrega BETWEEN @FECHA
					AND @FECHA2
			
			UNION
			
			SELECT CONVERT(VARCHAR(15), ES.FechaEntrega, 103) AS fechaEntrega
				,ES.NumPedido AS numPedido
				,0 AS etiquetaCarton
				,0 AS etiquetaAdhesivas
				,ED.CanDespachar AS etiquetaRFID
				,EDT.TipoEtiqueta
			FROM [Etiqueta].[Eti_Solicitud] ES
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle ED ON ES.IdSolEtiqueta = ED.IdSolEtiqueta
			INNER JOIN Etiqueta.Eti_Solicitud_Tipo EDT ON ED.IdSolEtiqueta = EDT.IdSolEtiqueta
				AND ED.CodArticulo = EDT.CodArticulo
			WHERE ES.CodigoSap = @CODSAP
				AND EDT.TipoEtiqueta = '03'
				AND ES.FechaEntrega BETWEEN @FECHA
					AND @FECHA2
			
			If Exists(select top 1 1 from #B where TipoEtiqueta in('05','06'))
				Begin
					SELECT fechaEntrega
						,numPedido
						,SUM(etiquetaCarton) AS etiquetaCarton
						,SUM(etiquetaAdhesivas) AS etiquetaAdhesivas
						,SUM(etiquetaRFID) AS etiquetaRFID
						,isnull(CD.CodElemento, '00') AS codTipoEtiqueta
						,isnull(CD.Descripcion, '') AS desTipoEtiqueta
					FROM #B 
					LEFT JOIN [Seguridad].[Seg_Catalogo_Detalle] CD ON CD.CodElemento = #B.TipoEtiqueta
					LEFT JOIN [Seguridad].[Seg_Catalogo_Cabecera] CC ON CC.IdCabecera = CD.IdCabecera
					WHERE CodigoCatalogo = 'tbl_etiqueta_zebra'
					GROUP BY fechaEntrega, numPedido, TipoEtiqueta
					ORDER BY fechaEntrega
				End
			Else
				Begin
					SELECT fechaEntrega
						,numPedido
						,SUM(etiquetaCarton) AS etiquetaCarton
						,SUM(etiquetaAdhesivas) AS etiquetaAdhesivas
						,SUM(etiquetaRFID) AS etiquetaRFID
						,'00' AS codTipoEtiqueta
						,@descripcion_vacia AS desTipoEtiqueta
					FROM #B
					GROUP BY fechaEntrega, numPedido
					ORDER BY fechaEntrega
			End

			DROP TABLE #A
		END

		IF (@OPCION = ('R'))
		BEGIN
			Declare @tb_cat_etiqueta Table(
					CodElemento	varchar(10),
					Descripcion	varchar(200)
				)

			INSERT INTO @tb_cat_etiqueta(CodElemento,Descripcion)
				SELECT CD.CodElemento, CD.Descripcion 
				FROM [Seguridad].[Seg_Catalogo_Detalle] CD
				INNER JOIN [Seguridad].[Seg_Catalogo_Cabecera] CC ON CC.IdCabecera = CD.IdCabecera 
																	AND CC.CodigoCatalogo = 'tbl_etiqueta_zebra'

			IF (@ESTADO = ('T'))
			BEGIN
				--SELECT x.*,y.codArticulo,yi.CanDespachar,CONVERT(VARCHAR(10),x.FechaEntrega,103) FechaEntregaFormato, y.DesArticulo,0 as CanReimpresionFormato,'' as FechaReimpresionFormato,
				SELECT DISTINCT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
					,isnull(c.CodElemento, '00') AS codTipoEtiqueta
					,isnull(c.Descripcion, '') AS desTipoEtiqueta
				FROM [Etiqueta].[Eti_Solicitud] x WITH(NOLOCK) 
				LEFT JOIN [Etiqueta].[Eti_Solicitud_Tipo] t on x.IdSolEtiqueta = t.IdSolEtiqueta
				LEFT JOIN @tb_cat_etiqueta c on c.CodElemento = t.TipoEtiqueta	
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega BETWEEN @FECHA AND @FECHA2
				ORDER BY X.IdSolEtiqueta
			END
			ELSE
			BEGIN
				SELECT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
				FROM [Etiqueta].[Eti_Solicitud] x
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega BETWEEN @FECHA
						AND @FECHA2
					AND x.estado = @ESTADO
				ORDER BY X.IdSolEtiqueta
			END
		END

		IF (@OPCION = ('O'))
		BEGIN

			Declare @tb_cat_etiqueta2 Table(
					CodElemento	varchar(10),
					Descripcion	varchar(200)
				)

			INSERT INTO @tb_cat_etiqueta2(CodElemento,Descripcion)
				SELECT CD.CodElemento, CD.Descripcion 
				FROM [Seguridad].[Seg_Catalogo_Detalle] CD
				INNER JOIN [Seguridad].[Seg_Catalogo_Cabecera] CC ON CC.IdCabecera = CD.IdCabecera 
																	AND CC.CodigoCatalogo = 'tbl_etiqueta_zebra'

			SELECT DISTINCT x.*
				,'' AS codArticulo
				,'0' AS CanDespachar
				,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
				,'' AS DesArticulo
				,0 AS CanReimpresionFormato
				,'' AS FechaReimpresionFormato
				,(
					CASE X.Estado
						WHEN 'S'
							THEN 'Solicitud de Impresión'
						WHEN 'C'
							THEN 'Cancelada'
						WHEN 'G'
							THEN 'Generando Impresión'
						WHEN 'R'
							THEN 'Recibida por el CD'
						WHEN 'E'
							THEN 'Enviada al CD'
						ELSE ''
						END
					) EstadoFormato
				,'' AS MotivoFormato
				,x.estado
				,isnull(c.CodElemento, '00') AS codTipoEtiqueta
				,isnull(c.Descripcion, '') AS desTipoEtiqueta
			FROM [Etiqueta].[Eti_Solicitud] x
			LEFT JOIN [Etiqueta].[Eti_Solicitud_Tipo] t on x.IdSolEtiqueta = t.IdSolEtiqueta
			LEFT JOIN @tb_cat_etiqueta c on c.CodElemento = t.TipoEtiqueta	
			WHERE x.codigosap = @CODSAP
				AND x.NumPedido = @NUMPEDIDO
			ORDER BY X.IdSolEtiqueta
		END

		IF (@OPCION = ('I'))
		BEGIN
			DECLARE @W_FECHA DATETIME

			SELECT TOP 1 @W_FECHA = PD.FecEntregaFinal
			FROM Pedidos.Ped_Pedido PE
			INNER JOIN Pedidos.Ped_PedidoDetalle PD ON PE.IdPedido = PD.IdPedido
			WHERE PE.IdPedido = @IDPEDIDO

			INSERT INTO Etiqueta.Eti_Solicitud (
				IdPedido
				,NumPedido
				,FechaEntrega
				,Estado
				,CodigoSap
				,fecha_entregaproveedor
				)
			VALUES (
				@IDPEDIDO
				,@NUMPEDIDO
				,@FECHA
				,'S'
				,@CODSAP
				,GETDATE()
				) -- select * from Etiqueta.Eti_Solicitud		

			SELECT @W_RETORNO = @@IDENTITY

			INSERT INTO Etiqueta.Eti_Solicitud_Detalle (
				IdSolEtiqueta
				,CodArticulo
				,CanDespachar
				,CanOriginal
				)
			SELECT @W_RETORNO
				,nref.value('@CODARTICULO', 'VARCHAR(18)')
				,nref.value('@CANDESPACHAR', 'INT')
				,nref.value('@CANDORIGINAL', 'INT')
			FROM @PI_ParamXML.nodes('/Root/EtiquetaDet') AS R(nref)

			SELECT @W_RETORNO AS retorno
		END

		IF (@OPCION = ('U'))
		BEGIN
			UPDATE Etiqueta.Eti_Solicitud_Detalle
			SET CanDespachar = nref.value('@CANDESPACHAR', 'INT')
			FROM Etiqueta.Eti_Solicitud_Detalle DT
			INNER JOIN @PI_ParamXML.nodes('/Root/EtiquetaDet') AS R(nref) ON (
					CASE 
						WHEN (ISNUMERIC(DT.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, DT.CodArticulo))
						ELSE DT.CodArticulo
						END
					) = (
					CASE 
						WHEN (ISNUMERIC(nref.value('@CODARTICULO', 'VARCHAR(18)')) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, nref.value('@CODARTICULO', 'VARCHAR(18)')))
						ELSE nref.value('@CODARTICULO', 'VARCHAR(18)')
						END
					)
			WHERE dt.IdSolEtiqueta = @IDSOLICITUD

			INSERT INTO Etiqueta.Eti_Solicitud_Detalle (
				IdSolEtiqueta
				,CodArticulo
				,CanDespachar
				)
			SELECT @IDSOLICITUD
				,nref.value('@CODARTICULO', 'VARCHAR(18)')
				,nref.value('@CANDESPACHAR', 'INT')
			FROM @PI_ParamXML.nodes('/Root/EtiquetaDet') AS R(nref)
			WHERE (
					CASE 
						WHEN (ISNUMERIC(nref.value('@CODARTICULO', 'VARCHAR(18)')) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, nref.value('@CODARTICULO', 'VARCHAR(18)')))
						ELSE nref.value('@CODARTICULO', 'VARCHAR(18)')
						END
					) NOT IN (
					SELECT (
							CASE 
								WHEN (ISNUMERIC(CodArticulo) = 1)
									THEN CONVERT(VARCHAR, CONVERT(BIGINT, CodArticulo))
								ELSE CodArticulo
								END
							)
					FROM Etiqueta.Eti_Solicitud_Detalle DT
					WHERE dt.IdSolEtiqueta = @IDSOLICITUD
					)
				AND nref.value('@CANDESPACHAR', 'INT') <> 0

			DELETE Etiqueta.Eti_Solicitud_Detalle
			FROM @PI_ParamXML.nodes('/Root/EtiquetaDet') AS R(nref)
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle DT ON (
					CASE 
						WHEN (ISNUMERIC(nref.value('@CODARTICULO', 'VARCHAR(18)')) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, nref.value('@CODARTICULO', 'VARCHAR(18)')))
						ELSE nref.value('@CODARTICULO', 'VARCHAR(18)')
						END
					) = (
					CASE 
						WHEN (ISNUMERIC(CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, CodArticulo))
						ELSE CodArticulo
						END
					)
			WHERE dt.IdSolEtiqueta = @IDSOLICITUD
				AND nref.value('@CANDESPACHAR', 'INT') = 0

			SELECT @W_RETORNO = @IDSOLICITUD
		END

		IF (@OPCION = ('L'))
		BEGIN
			SELECT SC.NumPedido AS pNumPedido
				,PD.Item AS pItem
				,PD.CantPedido AS pCantPedido
				,PD.CodArticulo AS pCodArticulo
				,PD.Tamano AS pTamano
				,PD.TamanoCaja AS pTamanoCaja
				,PD.CodEAN AS pCodEAN
				,PD.DesArticulo AS pDesArticulo
				,PD.PrecioCosto AS pPrecioCosto
				,SD.CanDespachar AS catDesp
				,PD.CantPedido - sd.canDespachar AS saldo
			FROM Etiqueta.Eti_Solicitud SC
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle SD ON SC.IdSolEtiqueta = SD.IdSolEtiqueta
			INNER JOIN Pedidos.Ped_PedidoDetalle PD ON SC.IdPedido = PD.IdPedido
				AND (
					CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, PD.CodArticulo))
						ELSE pd.CodArticulo
						END
					) = (
					CASE 
						WHEN (ISNUMERIC(SD.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, SD.CodArticulo))
						ELSE SD.CodArticulo
						END
					)
			WHERE SC.IdSolEtiqueta = @IDSOLICITUD
			
			UNION
			
			SELECT SC.NumPedido AS pNumPedido
				,PD.Item AS pItem
				,PD.CantPedido AS pCantPedido
				,PD.CodArticulo AS pCodArticulo
				,PD.Tamano AS pTamano
				,PD.TamanoCaja AS pTamanoCaja
				,PD.CodEAN AS pCodEAN
				,PD.DesArticulo AS pDesArticulo
				,PD.PrecioCosto AS pPrecioCosto
				,0 AS catDesp
				,PD.CantPedido - 0 AS saldo
			FROM Etiqueta.Eti_Solicitud SC
			INNER JOIN Pedidos.Ped_Pedido PE ON SC.IdPedido = PE.IdPedido
			INNER JOIN Pedidos.Ped_PedidoDetalle PD ON PE.IdPedido = PD.IdPedido
			WHERE SC.IdSolEtiqueta = @IDSOLICITUD
				AND (
					PE.estadobloqueo IS NULL
					OR PE.estadobloqueo = '0'
					)
				AND (
					CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, PD.CodArticulo))
						ELSE pd.CodArticulo
						END
					) NOT IN (
					SELECT (
							CASE 
								WHEN (ISNUMERIC(CodArticulo) = 1)
									THEN CONVERT(VARCHAR, CONVERT(BIGINT, CodArticulo))
								ELSE CodArticulo
								END
							)
					FROM Etiqueta.Eti_Solicitud e
					INNER JOIN Etiqueta.Eti_Solicitud_Detalle ed ON e.IdSolEtiqueta = ed.IdSolEtiqueta
					WHERE e.NumPedido = pe.NumPedido
						AND e.CodigoSap = pe.CodProveedor
					)
			ORDER BY Item
		END

		IF (@OPCION = 'W')
		BEGIN
			--SELECT
			--	p.Origen, --p.IdEmpresa, p.EsDescargado, p.EsImpreso,
			--	p.CodAlmacen,
			--	p.CodAlmDestino,
			--	p.CodProveedor,
			--	p.IdPedido, p.NumPedido, p.CodAlmacenOriginal as CodAlmacen, p.NomAlmacen,
			--	p.FechaPedido, p.CodAlmDestinoOriginal as CodAlmDestino, p.CodProveedorOriginal as CodProveedor,
			--	p.NomProveedor, p.ZonaOrigen,
			--	pd.IndIva1,
			--	CASE WHEN (ISNUMERIC(pd.Item) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.Item)) ELSE pd.Item END as Item,
			--	--CASE WHEN (ISNUMERIC(pd.CodArticulo) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo)) ELSE pd.CodArticulo END  as CodArticulo,
			--	 pd.CodArticulo  as CodArticulo,
			--	pd.CodArticulo as CodArticulo,
			--	pd.DesArticulo, pd.Tamano, pd.CantPedido, pd.PrecioCosto,
			--	pd.UndPorCaja, pd.Descuento1, pd.Descuento2, pd.IndIva1Original as IndIva1, pd.TamanoCaja, pd.CodEAN,p.EsDescargado,p.EsImpreso
			--FROM [Pedidos].[Ped_Pedido] p
			--	INNER JOIN Pedidos.Ped_Pedido x
			--		ON p.IdEmpresa = 1 AND p.IdPedido = x.IdPedido
			--	INNER JOIN [Pedidos].[Ped_PedidoDetalle] pd
			--		ON p.IdEmpresa = pd.IdEmpresa AND p.IdPedido = pd.IdPedido
			--	WHERE P.IdPedido=@IDSOLICITUD
			--	AND (P.estadobloqueo IS NULL OR P.estadobloqueo='0')
			--	and CASE WHEN (ISNUMERIC(pd.CodArticulo) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo)) ELSE pd.CodArticulo END  not in
			--	(select CASE WHEN (ISNUMERIC(dt.CodArticulo) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, dt.CodArticulo)) ELSE dt.CodArticulo END from Etiqueta.Eti_Solicitud c inner join Etiqueta.Eti_Solicitud_Detalle dt on c.IdSolEtiqueta=dt.IdSolEtiqueta and c.IdPedido=@IDSOLICITUD WHERE C.Estado<>'C')
			--	ORDER BY p.FechaPedido desc, p.NumPedido,cast( pd.Item as int)
			DECLARE @W_TIPOPEDIDOS VARCHAR(10)

			SELECT @W_TIPOPEDIDOS = p.TipoPedido
			FROM Pedidos.Ped_Pedido p
			WHERE IdPedido = @IDSOLICITUD

			DECLARE @W_ESTADO VARCHAR(5)

			IF (@W_TIPOPEDIDOS = 2)
			BEGIN
				SELECT TOP 1 @W_ESTADO = Estado
				FROM Pedidos.Ped_DetalleDistribucion PD
				WHERE PD.IdPedido = @IDSOLICITUD

				SET @W_ESTADO = ISNULL(@W_ESTADO, 2)

				SELECT IdPedido
					,PD.CodArticulo
					,CANTIDAD = SUM(CanRealDistribucion)
				INTO #A
				FROM Pedidos.Ped_DetalleDistribucion PD
				WHERE PD.IdPedido = @IDSOLICITUD
				GROUP BY IdPedido
					,CodArticulo

				SELECT (
						CASE 
							WHEN (ISNUMERIC(dt.CodArticulo) = 1)
								THEN CONVERT(VARCHAR, CONVERT(BIGINT, dt.CodArticulo))
							ELSE dt.CodArticulo
							END
						) AS CodArticulo
				INTO #tmpleft
				FROM Etiqueta.Eti_Solicitud c
				INNER JOIN Etiqueta.Eti_Solicitud_Detalle dt ON c.IdPedido = @IDSOLICITUD
					AND c.IdSolEtiqueta = dt.IdSolEtiqueta
				WHERE C.Estado <> 'C'

				SELECT p.Origen
					,--p.IdEmpresa, p.EsDescargado, p.EsImpreso,
					p.CodAlmacen
					,p.CodAlmDestino
					,p.CodProveedor
					,p.IdPedido
					,p.NumPedido
					,p.CodAlmacenOriginal AS CodAlmacen
					,p.NomAlmacen
					,p.FechaPedido
					,p.CodAlmDestinoOriginal AS CodAlmDestino
					,p.CodProveedorOriginal AS CodProveedor
					,p.NomProveedor
					,p.ZonaOrigen
					,pd.IndIva1
					,CASE 
						WHEN (ISNUMERIC(pd.Item) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.Item))
						ELSE pd.Item
						END AS Item
					,
					--CASE WHEN (ISNUMERIC(pd.CodArticulo) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo)) ELSE pd.CodArticulo END  as CodArticulo,
					pd.CodArticulo AS CodArticulo
					,pd.CodArticulo AS CodArticulo
					,pd.DesArticulo
					,pd.Tamano
					,pd.CantPedido
					,pd.PrecioCosto
					,pd.UndPorCaja
					,pd.Descuento1
					,pd.Descuento2
					,pd.IndIva1Original AS IndIva1
					,pd.TamanoCaja
					,pd.CodEAN
					,p.EsDescargado
					,p.EsImpreso
					,catDesp = ISNULL(A.CANTIDAD, 0)
					,estadodistri = @W_ESTADO
					,tipoPedido = @W_TIPOPEDIDOS
				FROM [Pedidos].[Ped_Pedido] p
				INNER JOIN Pedidos.Ped_Pedido x ON p.IdEmpresa = 1
					AND p.IdPedido = x.IdPedido
				INNER JOIN [Pedidos].[Ped_PedidoDetalle] pd ON p.IdEmpresa = pd.IdEmpresa
					AND p.IdPedido = pd.IdPedido
				LEFT JOIN #A A ON P.IdPedido = A.IdPedido
					AND PD.CodArticulo = A.CodArticulo
				WHERE P.IdPedido = @IDSOLICITUD
					AND (
						P.estadobloqueo IS NULL
						OR P.estadobloqueo = '0'
						)
					AND CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo))
						ELSE pd.CodArticulo
						END NOT IN (
						SELECT CASE 
								WHEN (ISNUMERIC(dt.CodArticulo) = 1)
									THEN CONVERT(VARCHAR, CONVERT(BIGINT, dt.CodArticulo))
								ELSE dt.CodArticulo
								END
						FROM #tmpleft dt
						)
				ORDER BY p.FechaPedido DESC
					,p.NumPedido
					,cast(pd.Item AS INT)

				DROP TABLE #A

				DROP TABLE #tmpleft
			END

			IF (@W_TIPOPEDIDOS = 3)
			BEGIN
				SELECT TOP 1 @W_ESTADO = Estado
				FROM Pedidos.Ped_Cab_DetalleDistribucion_Flow PD
				WHERE PD.IdPedido = @IDSOLICITUD

				SET @W_ESTADO = ISNULL(@W_ESTADO, 2)

				SELECT IdPedido
					,PD.CodArticulo
					,CANTIDAD = SUM(CanRealDistribucion)
				INTO #B3
				FROM Pedidos.Ped_Cab_DetalleDistribucion_Flow PD
				WHERE PD.IdPedido = @IDSOLICITUD
				GROUP BY IdPedido
					,CodArticulo

				SELECT p.Origen
					,--p.IdEmpresa, p.EsDescargado, p.EsImpreso,
					p.CodAlmacen
					,p.CodAlmDestino
					,p.CodProveedor
					,p.IdPedido
					,p.NumPedido
					,p.CodAlmacenOriginal AS CodAlmacen
					,p.NomAlmacen
					,p.FechaPedido
					,p.CodAlmDestinoOriginal AS CodAlmDestino
					,p.CodProveedorOriginal AS CodProveedor
					,p.NomProveedor
					,p.ZonaOrigen
					,pd.IndIva1
					,CASE 
						WHEN (ISNUMERIC(pd.Item) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.Item))
						ELSE pd.Item
						END AS Item
					,
					--CASE WHEN (ISNUMERIC(pd.CodArticulo) = 1) THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo)) ELSE pd.CodArticulo END  as CodArticulo,
					pd.CodArticulo AS CodArticulo
					,pd.CodArticulo AS CodArticulo
					,pd.DesArticulo
					,pd.Tamano
					,pd.CantPedido
					,pd.PrecioCosto
					,pd.UndPorCaja
					,pd.Descuento1
					,pd.Descuento2
					,pd.IndIva1Original AS IndIva1
					,pd.TamanoCaja
					,pd.CodEAN
					,p.EsDescargado
					,p.EsImpreso
					,catDesp = ISNULL(A.CANTIDAD, 0)
					,estadodistri = @W_ESTADO
					,tipoPedido = @W_TIPOPEDIDOS
				FROM [Pedidos].[Ped_Pedido] p
				INNER JOIN Pedidos.Ped_Pedido x ON p.IdEmpresa = 1
					AND p.IdPedido = x.IdPedido
				INNER JOIN [Pedidos].[Ped_PedidoDetalle] pd ON p.IdEmpresa = pd.IdEmpresa
					AND p.IdPedido = pd.IdPedido
				LEFT JOIN #B3 A ON P.IdPedido = A.IdPedido
					AND PD.CodArticulo = A.CodArticulo
				WHERE P.IdPedido = @IDSOLICITUD
					AND (
						P.estadobloqueo IS NULL
						OR P.estadobloqueo = '0'
						)
					AND CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, pd.CodArticulo))
						ELSE pd.CodArticulo
						END NOT IN (
						SELECT CASE 
								WHEN (ISNUMERIC(dt.CodArticulo) = 1)
									THEN CONVERT(VARCHAR, CONVERT(BIGINT, dt.CodArticulo))
								ELSE dt.CodArticulo
								END
						FROM Etiqueta.Eti_Solicitud c
						INNER JOIN Etiqueta.Eti_Solicitud_Detalle dt ON c.IdSolEtiqueta = dt.IdSolEtiqueta
							AND c.IdPedido = @IDSOLICITUD
						WHERE C.Estado <> 'C'
						)
				ORDER BY p.FechaPedido DESC
					,p.NumPedido
					,cast(pd.Item AS INT)

				DROP TABLE #B3
			END
		END

		IF (@OPCION = ('A'))
		BEGIN
			SELECT 1
				--UPDATE Etiqueta.Eti_Solicitud SET CanDespachar=@CANDESPACHAR, FechaEntrega=@FECHA WHERE IdSolEtiqueta=@IDSOLICITUD
				--select @@TRANCOUNT retorno
		END

		IF (@OPCION = ('E'))
		BEGIN
			--		DELETE Etiqueta.Eti_Solicitud WHERE IdSolEtiqueta=@IDSOLICITUD
			UPDATE Etiqueta.Eti_Solicitud
			SET Estado = rtrim(ltrim(@ESTADO))
			WHERE IdSolEtiqueta = @IDSOLICITUD

			SELECT @@TRANCOUNT retorno
		END

		IF (@OPCION = ('TR'))
		BEGIN
			EXEC Etiqueta.Consulta_Trama_Impresora @PI_IDPedido = @IDPEDIDO
				,@PI_CantImpri = @CANDESPACHAR
				,@PI_TipoEti = @TIPOTRAMA
		END

		IF (@OPCION = ('RF'))
		BEGIN			
			
				SELECT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
				FROM [Etiqueta].[Eti_Solicitud] x
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega = @FECHA					
				ORDER BY X.NumPedido
			
		END

		IF (@OPCION = ('RR'))
		BEGIN
				SELECT x.*
					,'' AS codArticulo
					,'0' AS CanDespachar
					,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
					,'' AS DesArticulo
					,0 AS CanReimpresionFormato
					,'' AS FechaReimpresionFormato
					,(
						CASE X.Estado
							WHEN 'S'
								THEN 'Solicitud de Impresión'
							WHEN 'C'
								THEN 'Cancelada'
							WHEN 'G'
								THEN 'Generando Impresión'
							WHEN 'R'
								THEN 'Recibida por el CD'
							WHEN 'E'
								THEN 'Enviada al CD'
							ELSE ''
							END
						) EstadoFormato
					,'' AS MotivoFormato
					,x.estado
				FROM [Etiqueta].[Eti_Solicitud] x
				WHERE x.codigosap = @CODSAP
					AND x.FechaEntrega BETWEEN @FECHA
						AND @FECHA2
					--rAND x.estado = @ESTADO
				ORDER BY X.NumPedido		
		END

		IF (@OPCION = ('RO'))
		BEGIN

			SELECT DISTINCT x.*
				,'' AS codArticulo
				,'0' AS CanDespachar
				,CONVERT(VARCHAR(10), x.FechaEntrega, 103) FechaEntregaFormato
				,'' AS DesArticulo
				,0 AS CanReimpresionFormato
				,'' AS FechaReimpresionFormato
				,(
					CASE X.Estado
						WHEN 'S'
							THEN 'Solicitud de Impresión'
						WHEN 'C'
							THEN 'Cancelada'
						WHEN 'G'
							THEN 'Generando Impresión'
						WHEN 'R'
							THEN 'Recibida por el CD'
						WHEN 'E'
							THEN 'Enviada al CD'
						ELSE ''
						END
					) EstadoFormato
				,'' AS MotivoFormato
				,x.estado
			FROM [Etiqueta].[Eti_Solicitud] x			
			WHERE x.codigosap = @CODSAP
				AND x.NumPedido = @NUMPEDIDO
			ORDER BY X.NumPedido
		END

		IF (@OPCION = ('RL'))
		BEGIN

			Declare @tb_cat_etiquetaD Table(
					CodElemento	varchar(10),
					Descripcion	varchar(200)
				)

			INSERT INTO @tb_cat_etiquetaD(CodElemento,Descripcion)
				SELECT CD.CodElemento, CD.Descripcion 
				FROM [Seguridad].[Seg_Catalogo_Detalle] CD
				INNER JOIN [Seguridad].[Seg_Catalogo_Cabecera] CC ON CC.IdCabecera = CD.IdCabecera 
																	AND CC.CodigoCatalogo = 'tbl_etiqueta_zebra'

			SELECT SC.NumPedido AS pNumPedido
				,PD.Item AS pItem
				--,PD.CantPedido AS pCantPedido
				,(PD.CantPedido * PD.UndPorCaja) AS pCantPedido
				,PD.CodArticulo AS pCodArticulo
				,PD.Tamano AS pTamano
				,PD.TamanoCaja AS pTamanoCaja
				,PD.CodEAN AS pCodEAN
				,PD.DesArticulo AS pDesArticulo
				,PD.PrecioCosto AS pPrecioCosto
				,0 AS catDesp
				,PD.CantPedido - sd.canDespachar AS saldo
				,SC.IdPedido as idPedido
				,isnull(c.CodElemento, '00') AS codTipoEtiqueta
				,isnull(c.Descripcion, '') AS desTipoEtiqueta
			FROM Etiqueta.Eti_Solicitud SC WITH(NOLOCK) 
			INNER JOIN Etiqueta.Eti_Solicitud_Detalle SD ON SC.IdSolEtiqueta = SD.IdSolEtiqueta
			INNER JOIN Pedidos.Ped_PedidoDetalle PD ON SC.IdPedido = PD.IdPedido
				AND (
					CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, PD.CodArticulo))
						ELSE pd.CodArticulo
						END
					) = (
					CASE 
						WHEN (ISNUMERIC(SD.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, SD.CodArticulo))
						ELSE SD.CodArticulo
						END
					)
			LEFT JOIN [Etiqueta].[Eti_Solicitud_Tipo] t on SC.IdSolEtiqueta = t.IdSolEtiqueta
			LEFT JOIN @tb_cat_etiquetaD c on c.CodElemento = t.TipoEtiqueta	
			WHERE SC.IdSolEtiqueta = @IDSOLICITUD
			
			UNION
			
			SELECT SC.NumPedido AS pNumPedido
				,PD.Item AS pItem
				--,PD.CantPedido AS pCantPedido
                                ,(PD.CantPedido * PD.UndPorCaja) AS pCantPedido
				,PD.CodArticulo AS pCodArticulo
				,PD.Tamano AS pTamano
				,PD.TamanoCaja AS pTamanoCaja
				,PD.CodEAN AS pCodEAN
				,PD.DesArticulo AS pDesArticulo
				,PD.PrecioCosto AS pPrecioCosto
				,0 AS catDesp
				,PD.CantPedido - 0 AS saldo
				,SC.IdSolEtiqueta as IdSolEtiqueta
				,isnull(c.CodElemento, '00') AS codTipoEtiqueta
				,isnull(c.Descripcion, '') AS desTipoEtiqueta
			FROM Etiqueta.Eti_Solicitud SC WITH(NOLOCK) 
			INNER JOIN Pedidos.Ped_Pedido PE ON SC.IdPedido = PE.IdPedido
			INNER JOIN Pedidos.Ped_PedidoDetalle PD ON PE.IdPedido = PD.IdPedido
			LEFT JOIN [Etiqueta].[Eti_Solicitud_Tipo] t on SC.IdSolEtiqueta = t.IdSolEtiqueta
			LEFT JOIN @tb_cat_etiquetaD c on c.CodElemento = t.TipoEtiqueta
			WHERE SC.IdSolEtiqueta = @IDSOLICITUD
				AND (
					PE.estadobloqueo IS NULL
					OR PE.estadobloqueo = '0'
					)
				AND (
					CASE 
						WHEN (ISNUMERIC(pd.CodArticulo) = 1)
							THEN CONVERT(VARCHAR, CONVERT(BIGINT, PD.CodArticulo))
						ELSE pd.CodArticulo
						END
					) NOT IN (
					SELECT (
							CASE 
								WHEN (ISNUMERIC(CodArticulo) = 1)
									THEN CONVERT(VARCHAR, CONVERT(BIGINT, CodArticulo))
								ELSE CodArticulo
								END
							)
					FROM Etiqueta.Eti_Solicitud e
					INNER JOIN Etiqueta.Eti_Solicitud_Detalle ed ON e.IdSolEtiqueta = ed.IdSolEtiqueta
					WHERE e.NumPedido = pe.NumPedido
						AND e.CodigoSap = pe.CodProveedor
					)
			ORDER BY Item
		END

		IF (@OPCION = ('RTR'))
		BEGIN
			EXEC Etiqueta.ConsultaArticulo_Trama_Impresora @PI_IDPedido = @IDPEDIDO
				,@PI_CantImpri = @CANDESPACHAR
				,@PI_TipoEti = @TIPOTRAMA
				,@PI_CodArticulo = @CODARTICULO
		END

		IF @@TRANCOUNT > 0
			COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
	END CATCH
END

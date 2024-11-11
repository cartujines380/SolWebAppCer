USE [SIPE_PROVEEDOR]
GO

/****** Object:  StoredProcedure [Transporte].[Tra_P_ConChoferVehiculo]    Script Date: 01/04/2020 17:48:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 20-07-2015
-- Description:	Consulta Chofer y Vehiculos
-- 306
-- =============================================

CREATE PROCEDURE [Transporte].[Tra_P_ConChoferVehiculo]
	@PI_ParamXML xml
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Tipo INT,
	@CodProveedor	varchar(15),
	@CodPedido		varchar(10)

DECLARE @W_TIEMPO INT

DECLARE @W_TABLAESTADO VARCHAR(50)

	SET @W_TABLAESTADO='tbl_EstadoGeneral'

	SELECT 
		 @Tipo				= nref.value('@Tipo','int'),
		 @CodProveedor		= nref.value('@CodProveedor','varchar(15)')
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 


	

					SELECT @W_TIEMPO= isnull( cast( b.Detalle as int),15)
		FROM [Proveedor].[Pro_Tabla] a
		INNER JOIN [Proveedor].[Pro_Catalogo] b on a.Tabla=b.Tabla and b.Estado= A.Estado
		WHERE a.TablaNombre='tbl_DiasVigenciaPedidos' and a.Estado='A' and b.Codigo='DIA'

SET @W_TIEMPO=20

	--TIPO 1 = CHOFER
   IF(@Tipo='1')
   BEGIN


	SELECT  codigo		=TRACHO.IdChofer,
			detalle		=ISNULL(TRACHO.NOMBRE1,'') +' '+ ISNULL(TRACHO.APELLIDO1,''),
			DescAlterno	=''
	FROM Transporte.Tra_Chofer TRACHO
		INNER JOIN Proveedor.Pro_Catalogo CA
			ON	CA.Codigo=TRACHO.CodEstado
		INNER JOIN Proveedor.Pro_Tabla	TB
			ON	TB.Tabla=CA.Tabla
			AND TB.TablaNombre=@W_TABLAESTADO
		INNER JOIN Transporte.Tra_ProveedorChofer TRPC
			ON TRACHO.IdChofer=TRPC.IdChofer
	WHERE TRACHO.CodEstado='A'
	AND		TRPC.CodProveedor=@CodProveedor
	ORDER BY detalle DESC

				
			
		

	END

	--TIPO 2 = VEHICULO
		IF(@Tipo='2')
   BEGIN

   SELECT  codigo		=TRAVE.IdVehiculo,
			detalle		=TRAVE.PlacaVehiculo,
			DescAlterno	=''
	FROM Transporte.Tra_Vehiculo TRAVE
		INNER JOIN Proveedor.Pro_Catalogo CA
			ON	CA.Codigo=TRAVE.CodEstado
		INNER JOIN Proveedor.Pro_Tabla	TB
			ON	TB.Tabla=CA.Tabla
			AND TB.TablaNombre=@W_TABLAESTADO
		INNER JOIN Transporte.Tra_ProveedorVehiculo TRPV
			ON TRAVE.IdVehiculo=TRPV.IdVehiculo
	WHERE TRAVE.CodEstado='A'
	AND		TRPV.CodProveedor=ISNULL(@CodProveedor,TRPV.CodProveedor)
	ORDER BY detalle DESC

	
	END

	--TIPO 3 = PEDIDOS
   IF(@Tipo='3')
   BEGIN

		
		
		
		--SELECT @W_TIEMPO=DESCRIPCION FROM Transporte.TRA_PARAMETROS  TP WHERE TP.NOMBREPARAMETRO='CaducidadPedidos'

		SELECT DISTINCT NUMPEDIDO INTO #A
		FROM	Transporte.Tra_Consolidacion TACO 
			INNER JOIN Transporte.Tra_ConsolidacionPedidos TACP
			ON	TACO.IdConsolidacion=TACP.IdConsolidacion
			WHERE TACO.FecCadConsolidacion>=CAST(GETDATE() AS DATE)

		SELECT	NumPedido			=PPP.NumPedido,
				FechaPedido			=CONVERT(VARCHAR(15),PPP.FechaPedido,103),
				RineRide			='',
				AlmacenSolicitante	=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmacen),
				AlmacenDestino		=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmDestino),
				FechaCaducidad		=CONVERT(VARCHAR(15),PPP.FechaPedido+@W_TIEMPO,103),
				ValorPedido			=PPP.SubTotSinImp,
				ValorFactura        ='0.00',
				NumeroBulto			='0',
				NumeroPalet 		='0',
				IDPalet 			='',
				TipoPedido          =PPP.TipoPedido,
				IsCross             =isnull(ppp.IsCross,CONVERT(bit,0)),
			    Estado              =ppp.Estado,
				EstadoDesc          =ct.Detalle
		FROM    Pedidos.Ped_Pedido PPP
		LEFT JOIN Proveedor.Pro_Catalogo ct on ct.Tabla = 1008 and ct.Codigo = ppp.Estado
		WHERE	PPP.CodProveedor=@CodProveedor
		AND		CONVERT(VARCHAR(15),PPP.FechaPedido,112)>=CONVERT(VARCHAR(15),GETDATE()-@W_TIEMPO,112)
		AND		CONVERT(VARCHAR(15),PPP.FechaPedido,112)<=CONVERT(VARCHAR(15),GETDATE(),112)
		AND		PPP.NumPedido NOT IN(SELECT * FROM #A)
		ORDER BY CONVERT(VARCHAR(15),PPP.FechaPedido,103)

		
	
	END

	--TIPO 4 = DETALLE PEDIDOS
   IF(@Tipo='4')
   BEGIN
		DECLARE @WIDPEDIDO INT,
		@W_NUMPEDIDO	VARCHAR(15)

		SELECT @W_NUMPEDIDO=nref.value('@NumPedido','varchar(15)')
		FROM @PI_ParamXML.nodes('/Root') as item(nref)



		SELECT @WIDPEDIDO=PPP.IdPedido
		FROM Pedidos.Ped_Pedido PPP
		WHERE PPP.NumPedido=@W_NUMPEDIDO

		SELECT	Item				=ppd.Item,
		        NumPedido			=@W_NUMPEDIDO,
				CodigoProducto		=ppd.CodArticulo,
				Factura				='',
				FechaFactura		='',
				Descripcion			=ppd.DesArticulo,
				CantidadPedido		=cast(ppd.CantPedido as int),
				PrecioUnitario		=ppd.PrecioCosto,
				UnidadCaja			=ppd.UndPorCaja,
				Descuento1			=ppd.Descuento1,
				Descuento2			=ppd.Descuento2,
				Iva					=ppd.IndIva1,
				Subtotal			=ppd.CantPedido*ppd.PrecioCosto,
				Total				=ppd.CantPedido*ppd.PrecioCosto,
				CantidadxDespachar	=cast(ppd.CantPedido as int),
				CantidadDespachada  ='0',
				CantidadPediente	=ppd.CantPendiente
		FROM	Pedidos.Ped_PedidoDetalle PPD
		WHERE	PPD.IdPedido=@WIDPEDIDO

	
	END

	--TIPO 5 = Consulta Pedido
   IF(@Tipo='5')
   BEGIN
   	SELECT 
		@CodPedido			= nref.value('@IdConsolidacion','bigint')
	FROM @PI_ParamXML.nodes('/Root') as item(nref)

		

		SELECT DISTINCT NUMPEDIDO INTO #AT5
		FROM	Transporte.Tra_Consolidacion TACO 
			INNER JOIN Transporte.Tra_ConsolidacionPedidos TACP
			ON	TACO.IdConsolidacion=TACP.IdConsolidacion
			WHERE TACO.FecEmiConsolidacion>=CAST(GETDATE() AS DATE)

	IF(@CodProveedor='')
		BEGIN
			SELECT @CodProveedor=CodProveedor FROM Transporte.Tra_Consolidacion WHERE IdConsolidacion=@CodPedido
		END

		SELECT	
			IDCONSOLIDACION				=TRCO.IdConsolidacion,
			ESTADO						=CA.Detalle,
			FECHAEMISION				=CONVERT(VARCHAR(15),TRCO.FecEmiConsolidacion,101),
			FECHACADUCIDAD				=CONVERT(VARCHAR(15),TRCO.FecCadConsolidacion,101),
			ALMACENDESTINO				=(SELECT NomAlmacen FROM Pedidos.Ped_Almacen WHERE CodAlmacen=TRCO.CodAlmaDestino),
			IDALMACENDESTINO			=TRCO.CodAlmaDestino,
			IDCHOFER					=TRCO.IdProvChofer,
			IDAYUDANTE					=TRCO.IdProvChofer2,
			IDVEHICULO					=TRCO.IdProvVehiculo,
			COSRAPIDO					=TRCO.CosRapido
		FROM Transporte.Tra_Consolidacion TRCO
			INNER JOIN Proveedor.Pro_Catalogo CA ON TRCO.CodEstado=CA.Codigo AND Tabla = 1
		WHERE TRCO.IdConsolidacion=@CodPedido


		SELECT 
			NumPedido			=TCPE.NumPedido,
			FechaPedido			=CONVERT(VARCHAR(15),PPP.FechaPedido,103),
			RineRide			='',
			AlmacenSolicitante	=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmacen),
			AlmacenDestino		=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmDestino),
			FechaCaducidad		=CONVERT(VARCHAR(15),PPP.FechaPedido+@W_TIEMPO,103),
			ValorPedido			=PPP.SubTotSinImp,
			ValorFactura        ='0.00',
			NumeroBulto			=TCPE.NumBulto,
			NumeroPalet 		=TCPE.NumPallets,
			IDPalet 			=TCPE.CodigoPallets,
			chk					='true'
		FROM  Transporte.Tra_ConsolidacionPedidos TCPE
		  INNER JOIN Pedidos.Ped_Pedido PPP ON TCPE.NumPedido=PPP.NumPedido
		  AND  PPP.CodProveedor=@CodProveedor
		WHERE TCPE.IdConsolidacion=@CodPedido
		UNION
		SELECT	top 10 NumPedido			=PPP.NumPedido,
				FechaPedido			=CONVERT(VARCHAR(15),PPP.FechaPedido,103),
				RineRide			='',
				AlmacenSolicitante	=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmacen),
				AlmacenDestino		=(SELECT PPA.NomAlmacen FROM Pedidos.Ped_Almacen PPA WHERE PPA.CodAlmacen=PPP.CodAlmDestino),
				FechaCaducidad		=CONVERT(VARCHAR(15),PPP.FechaPedido+@W_TIEMPO,103),
				ValorPedido			=PPP.SubTotSinImp,
				ValorFactura        ='0.00',
				NumeroBulto			='0',
				NumeroPalet 		='0',
				IDPalet 			='',
				chk					='false'
		FROM    Pedidos.Ped_Pedido PPP
		WHERE	PPP.CodProveedor=@CodProveedor
		AND		CONVERT(VARCHAR(15),PPP.FechaPedido,112)>=CONVERT(VARCHAR(15),GETDATE()-@W_TIEMPO,112)
		AND		CONVERT(VARCHAR(15),PPP.FechaPedido,112)<=CONVERT(VARCHAR(15),GETDATE(),112)
		AND		PPP.NumPedido NOT IN(SELECT * FROM #AT5)
		ORDER BY NumPedido desc


		DECLARE @CODIGOS TABLE (CODIGO VARCHAR(10) PRIMARY KEY,PEDIDO VARCHAR(20))

		INSERT INTO @CODIGOS
		SELECT TCPE.IdConsoPedido,TCPE.NumPedido
		FROM  Transporte.Tra_ConsolidacionPedidos TCPE
		INNER JOIN Pedidos.Ped_Pedido PPP ON TCPE.NumPedido=PPP.NumPedido
		AND  PPP.CodProveedor=@CodProveedor
		WHERE TCPE.IdConsolidacion=@CodPedido
		
		 SELECT 
				Item				=TDP.CodItem,
				NumPedido			=e.PEDIDO,
				CodigoProducto		=TDP.CodProducto,
				Factura				=TDP.NumFactura,
				FechaFactura		=CONVERT(VARCHAR(15),TDP.FechaFactura,101),
				Descripcion			=TDP.Descripcion,
				CantidadPedido		=TDP.CantidadPedido,
				PrecioUnitario		=TDP.PrecioUnitario,
				UnidadCaja			=TDP.UndxCaja,
				Descuento1			=TDP.Desc1,
				Descuento2			=TDP.Desc2,
				Iva					=TDP.IVA,
				Subtotal			=TDP.CantidadPedido*TDP.PrecioUnitario,
				Total				=TDP.CantidadPedido*TDP.PrecioUnitario,
				CantidadxDespachar	=TDP.CantDespachar,
				CantidadDespachada  =TDP.CantDespachada,
				CantidadPediente	=TDP.CantPendiente
		 FROM  Transporte.Tra_Detalle_Pedido TDP
			INNER JOIN @CODIGOS e ON (e.CODIGO = TDP.IdConsoPedido)
		
		
   END


   --TIPO 6 = BODEGA
	IF(@Tipo='6')
   BEGIN


   SELECT	codigo		=AL.CodAlmacen,
			detalle		=AL.NomAlmacen,
			DescAlterno	=''
   FROM		Pedidos.Ped_Almacen AL
   ORDER BY AL.NomAlmacen
   


	
	END
	IF(@Tipo='7')
   BEGIN
		Select codigo=idSociedad, detalle=nombreSociedad, DescAlterno='', Licencia=Licencia from Proveedor.Pro_Sociedad
		where Activar='1'
		order by nombresociedad

	END
END




GO



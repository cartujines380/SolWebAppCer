-- =============================================
-- Author:		Jessica Navarrete
-- Create date: 25-01-2015
-- Description:	Consulta Pedidos pendientes de consulta/descarga/impresion y genera una notificacion al proveedor
-- insert [Proveedor].[Pro_Catalogo] values(1011,'DIATOLERANCIA',6,'A',NULL)
-- =============================================
CREATE PROCEDURE [Pedidos].[Ped_P_PedidosEncolados]
	( @PI_ParamXML xml )
AS
BEGIN

	DECLARE @Tipo INT, @IdSolicitud BIGINT
	DECLARE @Dias INT, @Tolerancia INT

	SELECT @Tipo = nref.value('@Tipo','int'),
		   @Dias = nref.value('@Dias','int'),
		   @Tolerancia = nref.value('@Tolerancia','int')
	FROM @PI_ParamXml.nodes('/Root') AS R(nref)

	--Ingresa una notificacion con estado 'E' de enviado con la lista de proveedores que tengan pedidos encolados
	IF @Tipo = 1 
		BEGIN
		return 0
		
		----	SELECT p.CodProveedor,FechaPedido into #a
		----	FROM [Pedidos].[Ped_Pedido] p
		----		INNER JOIN Seguridad.Seg_Usuario US ON
		----			P.CodProveedor=US.CodProveedor
		----			AND US.Estado='A'
		----	WHERE  P.FechaPedido BETWEEN CONVERT(DATETIME,GETDATE()-@Dias,103) AND CONVERT(DATETIME,GETDATE()-@Tolerancia,103)
		----	AND    p.EsDescargado = 0 
		----	AND	   p.EsImpreso = 0
		----	AND    P.Estado='PE'
		----	group by p.CodProveedor,FechaPedido
		----	order by p.CodProveedor,FechaPedido



		----	select CodProveedor,cantidad=COUNT(1) into #b
		----	from #A
		----	group by CodProveedor
		----	having COUNT(1)>=1
			
		----	DECLARE @W_CODSAP VARCHAR(30),@W_FECHAPEDIDO VARCHAR(30),@W_NUMPEDIDO  VARCHAR(30),@W_DATOS VARCHAR(max),@W_NOMBREPROVEEDOR VARCHAR(100),@W_RUC VARCHAR(15),@W_NOMBREALMACEN VARCHAR(50),@W_CIUDADALMACEN VARCHAR(50)

		----	DECLARE USUARIOPEDIDOS CURSOR FOR 
		----	SELECT B.CODPROVEEDOR
		----	FROM	#b B

		----	OPEN USUARIOPEDIDOS

		----	FETCH NEXT FROM USUARIOPEDIDOS 
		----	INTO @W_CODSAP

		----	WHILE @@FETCH_STATUS = 0
		----	BEGIN

		----	SET @W_DATOS=''

		----	DECLARE USUARIOPEDIDOSDET CURSOR FOR 
		----	SELECT CONVERT(DATE,P.FechaPedido,103) AS FECHAPEDIDO,P.NumPedido,PS.NomAlmacen,CS.NomCiudad
		----	FROM #b b 
		----		INNER JOIN [Pedidos].[Ped_Pedido] p
		----			ON B.CodProveedor=P.CodProveedor
		----		INNER JOIN Pedidos.Ped_AlmacenSAP Ps
		----			ON P.CodAlmacen=PS.CodAlmacen
		----		INNER JOIN Pedidos.Ped_CiudadSAP cs
		----			ON PS.CodCiudad=CS.CodCiudad
		----	WHERE  P.FechaPedido BETWEEN CONVERT(DATE,GETDATE()-@Dias,103) AND CONVERT(DATE,GETDATE()-@Tolerancia,103)
		----	AND    p.EsDescargado = 0 
		----	AND	   p.EsImpreso = 0
		----	AND    P.Estado='PE'
		----	AND    B.CodProveedor=@W_CODSAP
		----	ORDER BY P.FechaPedido
		----	SET @W_DATOS='<div><table border="1" WIDTH=100% style="border: thin solid #000000; padding: inherit;"><TR><TD style="font-weight: bold;text-align: center">CIUDAD</TD><TD style="font-weight: bold;text-align: center">ALMACEN</TD><TD style="font-weight: bold;text-align: center">FECHA/ORDEN</TD><TD style="font-weight: bold;text-align: center">NUMERO/ORDEN</TD></TR>'
		----		OPEN USUARIOPEDIDOSDET

		----		FETCH NEXT FROM USUARIOPEDIDOSDET 
		----		INTO @W_FECHAPEDIDO,@W_NUMPEDIDO,@W_NOMBREALMACEN,@W_CIUDADALMACEN

		----		WHILE @@FETCH_STATUS = 0
		----		BEGIN

		----		  SELECT @W_DATOS=@W_DATOS+'<TR><TD>'+@W_CIUDADALMACEN+'</TD><TD>'+@W_NOMBREALMACEN+'</TD><TD style="text-align: center">'+@W_FECHAPEDIDO+'</TD><TD style="text-align: center">'+@W_NUMPEDIDO+'</TD></TR>'		

		----		FETCH NEXT FROM USUARIOPEDIDOSDET 
		----		INTO  @W_FECHAPEDIDO,@W_NUMPEDIDO,@W_NOMBREALMACEN,@W_CIUDADALMACEN
		----		END 
		----		CLOSE USUARIOPEDIDOSDET;
		----		DEALLOCATE USUARIOPEDIDOSDET;

		----		SET @W_DATOS=@W_DATOS+'</table></DIV><br>'

		----		    SELECT @W_NOMBREPROVEEDOR=PO.NomComercial,@W_RUC=PO.Ruc FROM PROVEEDOR.Pro_Proveedor PO WHERE PO.CodProveedor=@W_CODSAP
				
		----			INSERT INTO [Notificacion].[Notificacion]( -- select * from [Notificacion].[Notificacion]
		----			Titulo,
		----			Comunicado,
		----			FechaVencimiento,
		----			Categoria,
		----			Prioridad,
		----			Obligatorio, 
		----			Tipo, 
		----			Estado,
		----			FecIngreso, 
		----			Ruta, 
		----			CodAgrupacion, 
		----			UsrIngreso, 
		----			TipoCorreo, 
		----			MsjCorreo,
		----			FechaPublicacion,
		----			Corporativo)
		----			VALUES ('Ordenes de Compra Encoladas',
		----			'', 
		----			'1900-01-01',
		----			'C',
		----			'N',
		----			'N',
		----			'I',
		----			'E', GETDATE(), 
		----			'', 
		----			0,
		----			'NOT_PEDIDOS', --Usuario Generico
		----			'S', 
		----			'Estimado(a)<div>'+@W_NOMBREPROVEEDOR+'<br></div><div>RUC:'+@W_RUC+'</div><br><div>Le notificamos que Ud. tiene Órdenes de Compra emitidas hace más de '+ cast((@Tolerancia-1) as varchar(5))+ ' días aún pendientes de "Descargar" o "Imprimir" en nuestro Portal de Proveedores:<br></div><br>'+@W_DATOS+'<div><br></div><div>Le recomendamos acceda al portal y proceda a descargarlas a la brevedad posible.</div>', 
		----			GETDATE(),
		----			0)

					
		
		----			--Obtiene el id de la notificacion
		----			SELECT @IdSolicitud = @@IDENTITY;

		----		INSERT INTO [Notificacion].[Notificacion_Proveedor](Cod_Notificacion,Cod_Proveedor, Estado,USUARIO)
		----		SELECT DISTINCT @IdSolicitud,us.CodProveedor, 'I',US.Usuario
		----		FROM  Seguridad.Seg_Usuario US 
		----		inner join #b b
		----		on us.CodProveedor=b.CodProveedor
		----		inner join SIPE_FRAMEWORK.Participante.Par_Participante pa
		----			on us.IdParticipante=pa.IdParticipante
		----		INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario RU
		----			ON PA.IdUsuario=RU.IdUsuario AND RU.IdRol=27
		----		WHERE  US.Estado='A'
		----		AND    US.CodProveedor=@W_CODSAP
		----		UNION
		----		SELECT DISTINCT @IdSolicitud,us.CodProveedor, 'I',US.Usuario
		----		FROM  Seguridad.Seg_Usuario US 
		----		inner join #b b
		----		on us.CodProveedor=b.CodProveedor
		----		inner join SIPE_FRAMEWORK.Participante.Par_Participante pa
		----			on us.IdParticipante=pa.IdParticipante
		----		INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario RU
		----			ON PA.IdUsuario=RU.IdUsuario AND US.UsrCargo='0003' AND US.UsrFuncion='03'
		----		WHERE  US.Estado='A'
		----		AND    US.CodProveedor=@W_CODSAP


				


		----		INSERT  Notificacion.Notificacion_Departamento 
		----		SELECT @IdSolicitud,CODIGO,GETDATE()
		----		FROM Proveedor.Pro_Catalogo CA
		----		WHERE CA.Tabla=1033

		----	FETCH NEXT FROM USUARIOPEDIDOS 
		----	INTO @W_CODSAP
		----	END 
		----	CLOSE USUARIOPEDIDOS;
		----	DEALLOCATE USUARIOPEDIDOS;

		
		------Rertorna el ID
		----SELECT @IdSolicitud

		----drop table #a
		----drop table #b

	END
END
--IF @@TRANCOUNT > 0
--			COMMIT	TRAN
--END TRY
--BEGIN CATCH
--	IF @@TRANCOUNT > 0
--		ROLLBACK TRAN
		
--		exec SP_PROV_Error @sp='[Ped_P_PedidosEncolados]'
--END CATCH


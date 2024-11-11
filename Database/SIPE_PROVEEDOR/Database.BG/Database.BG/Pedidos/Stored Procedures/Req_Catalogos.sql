-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 05-02-2017
-- Description:	Catalogo Requeimientos
-- 900
-- =============================================

CREATE PROCEDURE [Pedidos].[Req_Catalogos]
	@PI_ParamXML xml
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @Tipo VARCHAR(10);
	DECLARE
	 @CodCategoria			VARCHAR(10)
	,@CodEmpresa			VARCHAR(10)
	,@W_FECHA1				datetime
	,@W_FECHA2				datetime
	,@W_FECHAF1				datetime
	,@W_FECHAF2				datetime
	,@w_id					VARCHAR(10)
	,@PROVEEDOR				VARCHAR(10)

	DECLARE @ID			BIGINT
	
	DECLARE @ARTICULOS TABLE (CodArticulo VARCHAR(30) PRIMARY KEY)
	DECLARE @ALMACENES TABLE(CodAlmacen VARCHAR(10))

	DECLARE @REGSPROV	TABLE(codProveedor varchar(10), NomComercial varchar(35), correo varchar(241), mensaje varchar(max), titulo varchar(40))
	DECLARE @MENSAJE	VARCHAR(MAX),
			@fechaFin	varchar(10),
			@hoFin		varchar(10),
			@coLici		varchar(10)


	SELECT 
		 @Tipo		    = nref.value('@Tipo','VARCHAR(10)'),
		 @W_FECHA1		= convert(date,nref.value('@fecha','VARCHAR(10)'),103),
		 @CodCategoria	= nref.value('@codCategoria','VARCHAR(10)'),
		 @CodEmpresa	= nref.value('@codEmpresa','VARCHAR(10)'),
		 @ID			= nref.value('@id','BIGINT'),
		 @PROVEEDOR		= nref.value('@Proveedor','VARCHAR(10)')
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 


	

	IF(@W_FECHA1='')
		SET @W_FECHA1=NULL


	IF(@CodCategoria='')
		SET @CodCategoria=NULL

	IF(@CodEmpresa='')
		SET @CodEmpresa=NULL

	IF(@Tipo='EMP')
	BEGIN
		SELECT 
			c.Codigo as codEmpresa, 
			c.Detalle as descripcion
		FROM [Proveedor].[Pro_Tabla] as t
		INNER JOIN [Proveedor].[Pro_Catalogo] as c
			ON t.tabla=c.tabla
		WHERE t.TablaNombre = 'tbl_RedEmpresa'
		--select IdSociedad as codEmpresa, NombreSociedad as descripcion 
		--from SIPE_PROVEEDOR.Proveedor.Pro_sociedad with(nolock)
		--where Estado='A' and Activar=1
	END

	IF(@Tipo='CAT')
	BEGIN
		SELECT 
			c.Codigo as codEmpresa, 
			c.Detalle as descripcion
		FROM [Proveedor].[Pro_Tabla] as t
		INNER JOIN [Proveedor].[Pro_Catalogo] as c
			ON t.tabla=c.tabla
		WHERE t.TablaNombre = 'tbl_LineaNegocio'
	END

	IF(@Tipo='ELILI')
	BEGIN
		UPDATE	Pedidos.Red_Adquisicion SET estadoLicitacion='E'
		WHERE idrequerimiento=@ID

		SET @MENSAJE = 'La Licitación con <strong>código: </strong>'+cast(@ID as varchar(10))+'ha sido eliminada.'+
					'<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					Para mas información visite <a href="http://34.221.215.16:9999/">www.portalproveedores.com</a>'

		insert @REGSPROV
		SELECT PO.CodProveedor,PO.NomComercial,PO.CorreoE,@MENSAJE,'ELIMINACIÓN DE LICITACIÓN' 
		FROM Pedidos.Red_Adquisicion AD
		INNER JOIN Proveedor.Pro_Proveedor PO
		ON AD.codproveedor=PO.CodProveedor
		WHERE idrequerimiento=@ID

		select *
		from @REGSPROV
	END

	IF(@Tipo='GRE')
	BEGIN
	
		INSERT INTO Pedidos.Red_Requerimientos
		SELECT convert(date,nref.value('@fecha','VARCHAR(10)'),103),
		nref.value('@codCategoria','VARCHAR(10)'),
		nref.value('@codEmpresa','VARCHAR(10)'),
		nref.value('@monto','VARCHAR(20)'),
		nref.value('@descripcion','VARCHAR(8000)'),
		'I',
		nref.value('@usuario','VARCHAR(10)'),
		GETDATE(),
		nref.value('@titulo','VARCHAR(100)')
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 

		SELECT @w_id=@@IDENTITY  

		INSERT Pedidos.Red_RequerimientosAdjunto 
		SELECT @w_id,nref.value('@archivo','VARCHAR(100)'),nref.value('@descripcion','VARCHAR(100)'),'R'
	FROM @PI_ParamXML.nodes('/Root/Archivo') AS R(nref)

	SELECT MENSAJE= @w_id

	END

	IF(@Tipo='URE')
	BEGIN
		UPDATE Pedidos.Red_Requerimientos SET fecharequerimiento= convert(date,nref.value('@fecha','VARCHAR(10)'),103),
			codcategoria=nref.value('@codCategoria','VARCHAR(10)'),
			codempresa=nref.value('@codEmpresa','VARCHAR(10)'),
			descripcion=nref.value('@descripcion','VARCHAR(max)'),
			monto=nref.value('@monto','VARCHAR(20)'),
			titulo=nref.value('@titulo','VARCHAR(100)')
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 
		INNER JOIN Pedidos.Red_Requerimientos RE
		ON RE.idrequerimiento=nref.value('@id','BIGINT')

		SELECT @w_id=nref.value('@id','BIGINT')
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 
		
		DELETE Pedidos.Red_RequerimientosAdjunto
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 
		INNER JOIN Pedidos.Red_RequerimientosAdjunto RE
		ON RE.idrequerimiento=nref.value('@id','BIGINT')

		INSERT Pedidos.Red_RequerimientosAdjunto 
		SELECT @w_id,nref.value('@archivo','VARCHAR(100)'),nref.value('@descripcion','VARCHAR(100)'),'R'
	FROM @PI_ParamXML.nodes('/Root/Archivo') AS R(nref)

	END
	IF(@Tipo='BRLI')
	BEGIN
		SELECT 
			 @ID			= nref.value('@cod','BIGINT'),
			 @W_FECHA1		= convert(date,nref.value('@fechadesde','VARCHAR(10)'),103),
			 @W_FECHA2		= convert(date,nref.value('@fechahasta','VARCHAR(10)'),103),
			 @W_FECHAF1		=  convert(date,nref.value('@fechadesdeF','VARCHAR(10)'),103), 
			 @W_FECHAF2		= convert(date,nref.value('@fechahastaF','VARCHAR(10)'),103)
		FROM @PI_ParamXML.nodes('/Root') as item(nref) 

	IF	@ID=''
		SET @ID=null

	IF	@W_FECHA1=''
		SET @W_FECHA1=null
	IF	@W_FECHA2=''
		SET @W_FECHA2=null
	IF	@W_FECHAF1=''
		SET @W_FECHAF1=null
	IF	@W_FECHAF2=''
		SET @W_FECHAF2=null

		SELECT DISTINCT AD.nombrePub as nombre,CONVERT(VARCHAR(10),ad.feIniLicitacion,103) AS fe_empieza,
				CONVERT(VARCHAR(10),ad.feFinLicitacion,103) AS fe_exp,CONVERT(VARCHAR(10),AD.HOFINLICITACION,108) AS ho_exp,
				CONVERT(VARCHAR(10),RE.fecharequerimiento,103) AS feRequerimiento,re.monto as montoRequerimiento,re.idrequerimiento as codRequerimiento,
				re.titulo as desRequerimiento,cat_e.Detalle as empRequerimiento
		FROM Pedidos.Red_Requerimientos as RE		
		INNER JOIN Pedidos.Red_Adquisicion ad
			on RE.idrequerimiento=AD.idrequerimiento
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = RE.codempresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		WHERE  RE.estado='P'
		AND   AD.estadoparticipandp='E'
		AND	  RE.fecharequerimiento>=ISNULL(@W_FECHA1, RE.fecharequerimiento)
		AND  RE.fecharequerimiento<=ISNULL(@W_FECHA2, RE.fecharequerimiento)
		AND	  AD.feFinLicitacion>=ISNULL(@W_FECHAF1,AD.feFinLicitacion)
		AND  AD.feFinLicitacion<=ISNULL(@W_FECHAF2, AD.feFinLicitacion)
		AND   RE.idrequerimiento=ISNULL(@ID,RE.idrequerimiento)
		AND  getdate()>=AD.feFinLicitacion + ' '+ AD.hoFinLicitacion
		order by re.idrequerimiento desc

		SELECT DISTINCT RA.idrequerimiento as id,RA.nombrearchivo as nombre,ra.descripcion
		FROM	Pedidos.Red_Requerimientos RE		
		INNER JOIN Pedidos.Red_Adquisicion ad
		ON RE.idrequerimiento=AD.idrequerimiento
		INNER JOIN Pedidos.Red_RequerimientosAdjunto RA
		ON RE.idrequerimiento=RA.idrequerimiento
		WHERE RE.estado='P'
		AND AD.estadoparticipandp='E'
		AND	RE.fecharequerimiento>=ISNULL(@W_FECHA1, RE.fecharequerimiento)
		AND RE.fecharequerimiento<=ISNULL(@W_FECHA2, RE.fecharequerimiento)
		AND	AD.feFinLicitacion>=ISNULL(@W_FECHAF1,AD.feFinLicitacion)
		AND AD.feFinLicitacion<=ISNULL(@W_FECHAF2, AD.feFinLicitacion)
		AND RE.idrequerimiento=ISNULL(@ID,RE.idrequerimiento)
		AND getdate()>=AD.feFinLicitacion + ' '+ AD.hoFinLicitacion
		order by RA.idrequerimiento desc

		SELECT RE.idrequerimiento as id,po.NomComercial as empresa,isNull(ad.monto,0)monto,ad.codproveedor as codproveedor
		FROM Pedidos.Red_Requerimientos RE		
		INNER JOIN Pedidos.Red_Adquisicion ad
		ON RE.idrequerimiento=AD.idrequerimiento
		INNER JOIN Proveedor.Pro_Proveedor PO
		ON AD.codproveedor=PO.CodProveedor
		WHERE RE.estado='P'
		AND AD.estadoparticipandp='E'
		AND	RE.fecharequerimiento>=ISNULL(@W_FECHA1, RE.fecharequerimiento)
		AND RE.fecharequerimiento<=ISNULL(@W_FECHA2, RE.fecharequerimiento)
		AND	AD.feFinLicitacion>=ISNULL(@W_FECHAF1,AD.feFinLicitacion)
		AND AD.feFinLicitacion<=ISNULL(@W_FECHAF2, AD.feFinLicitacion)
		AND RE.idrequerimiento=ISNULL(@ID,RE.idrequerimiento)
		AND getdate()>=AD.feFinLicitacion + ' '+ AD.hoFinLicitacion
		order by RE.idrequerimiento desc
	END

	IF(@Tipo='BGRE')
	BEGIN
		SELECT 
			re.idrequerimiento AS id,
			CONVERT(VARCHAR(10),re.fecharequerimiento,103) AS fecha,
			cat.Detalle AS categoria,
			cat_e.Detalle AS empresa,
			RE.titulo AS descripcion,
			CASE 
				WHEN re.ESTADO ='I' THEN 'Ingresado'
				WHEN re.ESTADO ='D' THEN 'Directo'
				WHEN re.ESTADO ='L' THEN 'Licitación' 
				WHEN re.ESTADO ='P' THEN 'Publicado' 
				WHEN re.ESTADO ='A' THEN 'Adjudicada' 
			END AS estado,
			RE.titulo AS titulo
		FROM Pedidos.Red_Requerimientos RE
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat
			on cat.Codigo = RE.codcategoria
		INNER JOIN [Proveedor].[Pro_Tabla] as t
			on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = RE.codempresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		WHERE   re.estado<>'C'
		AND RE.fecharequerimiento=ISNULL(@W_FECHA1,RE.fecharequerimiento)
		AND RE.codcategoria=ISNULL(@CodCategoria,RE.codcategoria)
		AND RE.codempresa=ISNULL(@CodEmpresa,RE.codempresa)
		ORDER BY re.idrequerimiento desc
	END

	IF(@Tipo='BASI')
	BEGIN
		SELECT	
			re.idrequerimiento AS id,
			CONVERT(VARCHAR(10),re.fecharequerimiento,103) AS fecha,
			cat.Detalle AS categoria,
			cat_e.Detalle AS empresa,
			RE.titulo AS descripcion,
			CASE 
				WHEN re.ESTADO ='I' THEN 'Ingresado'
				WHEN re.ESTADO ='D' THEN 'Directo'
				WHEN re.ESTADO ='L' THEN 'Licitación' 
				WHEN re.ESTADO ='A' THEN 'Adjudicada' 
			END AS estado
		FROM Pedidos.Red_Requerimientos RE
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat
			on cat.Codigo = RE.codcategoria
		INNER JOIN [Proveedor].[Pro_Tabla] as t
			on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = RE.codempresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		INNER JOIN Pedidos.Red_Adquisicion ra
			ON re.idrequerimiento=ra.idrequerimiento
			AND RA.codproveedor=@PROVEEDOR
		WHERE re.estado<>'C'
		AND re.ESTADO IN('D','A')
		AND RA.estadoparticipandp='A'
		AND RE.fecharequerimiento=ISNULL(@W_FECHA1,RE.fecharequerimiento)
		AND RE.codcategoria=ISNULL(@CodCategoria,RE.codcategoria)
		AND RE.codempresa=ISNULL(@CodEmpresa,RE.codempresa)

		UNION
			
		SELECT	
			re.idrequerimiento AS id,
			CONVERT(VARCHAR(10),re.fecharequerimiento,103) AS fecha,
			cat.Detalle AS categoria,
			cat_e.Detalle AS empresa,
			RE.titulo AS descripcion,
			CASE 
				WHEN re.ESTADO ='I' THEN 'Ingresado'
				WHEN re.ESTADO ='D' THEN 'Directo'
				WHEN re.ESTADO ='L' THEN 'Licitación' 
				WHEN re.ESTADO ='A' THEN 'Adjudicada' 
			END AS estado
		FROM Pedidos.Red_Requerimientos RE
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat
			on cat.Codigo = RE.codcategoria
		INNER JOIN [Proveedor].[Pro_Tabla] as t
			on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = RE.codempresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		INNER JOIN Pedidos.Red_Adquisicion ra
			ON re.idrequerimiento=ra.idrequerimiento
			AND RA.codproveedor=@PROVEEDOR
		WHERE re.estado<>'C'
		AND re.ESTADO IN('D')
		AND RE.fecharequerimiento=ISNULL(@W_FECHA1,RE.fecharequerimiento)
		AND RE.codcategoria=ISNULL(@CodCategoria,RE.codcategoria)
		AND RE.codempresa=ISNULL(@CodEmpresa,RE.codempresa)
		ORDER BY re.idrequerimiento desc
	END

	IF(@Tipo='ELI')
	BEGIN
		UPDATE Pedidos.Red_Requerimientos SET estado='C'
		WHERE idrequerimiento=@ID
	END	

	IF(@Tipo='UREF')
	BEGIN
		UPDATE Pedidos.Red_Requerimientos SET estado='A'
		WHERE idrequerimiento=@ID

		UPDATE Pedidos.Red_Adquisicion SET estadoparticipandp='A'
		FROM Pedidos.Red_Adquisicion
		WHERE idrequerimiento=@ID
		AND codproveedor=@CodEmpresa

		select @coLici=convert(varchar(10),idrequerimiento), @fechaFin = convert(varchar(10), feFinLicitacion, 103), @hoFin = convert(varchar(10),hoFinLicitacion) from Pedidos.Red_Adquisicion where idrequerimiento=@ID and codproveedor=@CodEmpresa

		SET @MENSAJE = 'La presente es para indicarle que se le ha adjudicado Licitación de Sipecom S.A. con <strong>Código: </strong>'+@coLici+'.'+
							'<br/>
							<br/>
							<strong>Fecha de Cierre de la Licitación:</strong> '+@fechaFin+' <strong>Hora: </strong>'+@hoFin+'
							<br/>
							<br/>
							Por favor comuníquese con <strong>Katiuska Criollo</strong> al correo <a href="mailto:kcriollo@sipecom.com" target="_top">kcriollo@sipecom.com</a>.
							<br/>
							<br/>
							<br/>
							Para mas información visite <a href="http://34.221.215.16:9999/">www.portalproveedores.com</a>'

		insert into @REGSPROV
		select CodProveedor, NomComercial, CorreoE, @MENSAJE, 'ADJUDICACIÓN DE LICITACIÓN' 
		from Proveedor.Pro_Proveedor pp where CodProveedor = @CodEmpresa

		SET @MENSAJE = 'La presente es para agradecerle por su participación en la Licitación de Sipecom S.A. con <strong>Código: </strong>_IDPUBLI.'+
							'<br/>
							<br/>
							<strong>Fecha de Cierre de la Licitación:</strong> '+@fechaFin+' <strong>Hora: </strong>'+@hoFin+'
							<br/>							
							<br/>
							<br/>
							<br/>
							Para mas información visite <a href="http://34.221.215.16:9999/">www.portalproveedores.com</a>'

		insert into @REGSPROV
		select pp.CodProveedor, NomComercial, CorreoE, REPLACE(@MENSAJE,'_IDPUBLI',(select top 1 idrequerimiento from Pedidos.Red_Adquisicion where codproveedor = pp.CodProveedor and idrequerimiento=@ID)), 'AGRADECIMIENTO DE LICITACIÓN'
		from Proveedor.Pro_Proveedor pp 
		INNER JOIN Pedidos.Red_Adquisicion pa on pa.codproveedor=pp.CodProveedor
		where pp.CodProveedor <> @CodEmpresa and idrequerimiento = @ID and pa.estadoparticipandp in ('P','E')

		select * from @REGSPROV
	END	
	
	IF(@Tipo='BSEL')
	BEGIN
		SELECT  RE.idrequerimiento as id,
				CONVERT(VARCHAR(10),re.fecharequerimiento,103) as fecha,
				re.codcategoria as categoria,
				re.codempresa as empresa,
				re.descripcion AS descripcion,
				re.monto as monto,
				RE.titulo AS titulo
		FROM	Pedidos.Red_Requerimientos RE
		where idrequerimiento=@ID

		SELECT RTRIM(LTRIM(A.nombrearchivo)) as nombrearchivo ,RTRIM(LTRIM(a.descripcion)) as descripcion
		FROM Pedidos.Red_RequerimientosAdjunto A
		WHERE A.idrequerimiento=@ID and tipo='R'
	END
END



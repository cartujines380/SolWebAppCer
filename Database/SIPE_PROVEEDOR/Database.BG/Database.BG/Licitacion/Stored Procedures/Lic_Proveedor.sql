CREATE PROCEDURE [Licitacion].[Lic_Proveedor]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
--insert into tmp values(@PI_ParamXML)
	BEGIN TRAN
	DECLARE
		 @ACCION	 CHAR(2)
		,@CRITERIO	 CHAR(1)
		,@FEINI		DATETIME
		,@FEFIN		DATETIME
		,@REQ		VARCHAR(8)
		,@CAT		VARCHAR(8)
		,@EMP		VARCHAR(8)
		,@IDPROV	VARCHAR(8)
		,@IDUSU		VARCHAR(8)
		,@DESC		VARCHAR(400)
		,@NOM		VARCHAR(40)
		,@HOFIN		TIME
		,@VER		INT
		,@PROV		VARCHAR(10)
		,@IDPUBLI	INT
		,@TIEM		INT
		,@MONT		VARCHAR(20)

	SELECT
		@ACCION		= nref.value('@ACCION', 'CHAR(2)'),
		@CRITERIO	= nref.value('@CRITERIO', 'CHAR(1)'),
		@FEINI		= CONVERT(Datetime, nref.value('@FEINI', 'VARCHAR(10)'), 103),
		@FEFIN		= CONVERT(Datetime, nref.value('@FEFIN', 'VARCHAR(10)'), 103),
		@REQ		= nref.value('@REQ', 'VARCHAR(8)'),
		@CAT		= nref.value('@CAT', 'VARCHAR(8)'),
		@EMP		= nref.value('@EMPRESA', 'VARCHAR(8)'),
		@IDPROV		= nref.value('@IDPROV', 'VARCHAR(8)'),
		@IDUSU		= nref.value('@USUARIO', 'VARCHAR(8)'),
		@DESC		= nref.value('@DESC', 'VARCHAR(400)'),
		@NOM		= nref.value('@NOMBRE', 'VARCHAR(40)'),
		@HOFIN		= CONVERT(TIME, nref.value('@HOFIN', 'VARCHAR(10)')),
		@PROV		= nref.value('@PROV', 'VARCHAR(10)'),
		@IDPUBLI	= nref.value('@IDPUBLI', 'INT'),
		@TIEM		= nref.value('@TIEM', 'INT'),
		@MONT		= nref.value('@MONT', 'VARCHAR(20)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @ACCION = 'CO'
	BEGIN
		IF @CRITERIO = 'T'
		BEGIN
			SELECT 
				ra.idAdquisicion as idPubli,
				ra.version,
				req.idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				req.monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
				convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
				ra.hoFinLicitacion,
				ra.nombrePub,
				ra.descripcionPub,
				ra.estadoLicitacion,
				estadoparticipandp,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Pedidos].[Red_adquisicion] as ra 
				on ra.idrequerimiento = req.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE estadopublicado='P' 
			AND ra.codproveedor=@PROV
			AND estadoLicitacion<>'E'
			and ra.feFinLicitacion + ' '+ ra.hoFinLicitacion>=getdate()
			AND estadoparticipandp is null
			ORDER BY ra.idAdquisicion DESC
		END

		IF @CRITERIO = 'F'
		BEGIN
			SELECT 
				ra.idAdquisicion as idPubli,
				ra.version,
				req.idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				req.monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
				convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
				ra.hoFinLicitacion,
				ra.nombrePub,
				ra.descripcionPub,
				ra.estadoLicitacion,
				cat.Detalle as categoria,
				cat_e.Detalle empresa 
			FROM [Pedidos].[Red_Requerimientos] req
			INNER JOIN [Pedidos].[Red_adquisicion] as ra 
				on ra.idrequerimiento = req.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'		  
			WHERE ra.fechacreacion between @FEINI AND @FEFIN 
			AND estadopublicado = 'P' AND ra.codproveedor=@PROV
			AND estadoparticipandp is null
			AND estadoLicitacion <> 'E'
			AND ra.feFinLicitacion + ' '+ ra.hoFinLicitacion >= getdate()
			ORDER BY ra.idAdquisicion DESC
		END

		IF @CRITERIO = 'R'
		BEGIN
			SELECT 
				ra.idAdquisicion as idPubli,
				ra.version,
				req.idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				req.monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
				convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
				ra.hoFinLicitacion,
				ra.nombrePub,
				ra.estadoLicitacion,
				ra.descripcionPub,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Pedidos].[Red_adquisicion] ra 
				on ra.idrequerimiento = req.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'		
			WHERE req.idrequerimiento = @REQ 
			AND estadopublicado = 'P' 
			AND ra.codproveedor = @PROV
			AND estadoparticipandp is null
			AND estadoLicitacion <> 'E'
			AND ra.feFinLicitacion + ' '+ ra.hoFinLicitacion >= getdate()
			ORDER BY ra.idAdquisicion DESC
		END

		IF @CRITERIO = 'E'
		BEGIN
			SELECT 
				ra.idAdquisicion as idPubli,
				ra.version,
				req.idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				req.monto,
				ra.estadoLicitacion,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
				convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
				ra.hoFinLicitacion,
				ra.nombrePub,
				ra.descripcionPub,
				cat.Detalle categoria,
				cat_e.Detalle as empresa
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Pedidos].[Red_adquisicion] as ra 
				on ra.idrequerimiento = req.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE cat_e.Codigo = @EMP 
			AND estadopublicado = 'P' 
			AND ra.codproveedor = @PROV
			AND estadoparticipandp is null
			AND estadoLicitacion <> 'E'
			AND ra.feFinLicitacion + ' '+ ra.hoFinLicitacion >= getdate()
			ORDER BY ra.idAdquisicion DESC
		END

		IF @CRITERIO = 'C'
		BEGIN
			select 
			ra.idAdquisicion as idPubli,
			ra.version,
			req.idrequerimiento,
			codcategoria,
			cat_e.Codigo as codEmpresa,
			req.monto,
			req.titulo,
			req.descripcion,
			ra.estadoLicitacion,
			req.estado,
			usuariocreacion,
			convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
			convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
			ra.hoFinLicitacion,
			ra.nombrePub,
			ra.descripcionPub,
			cat.Detalle as categoria, 
			cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Pedidos].[Red_adquisicion] as ra 
				on ra.idrequerimiento=req.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE codcategoria = @CAT 
			AND estadopublicado = 'P' 
			AND ra.codproveedor = @PROV
			AND estadoparticipandp is null
			AND estadoLicitacion <> 'E'
			AND ra.feFinLicitacion + ' '+ ra.hoFinLicitacion>=getdate()
			ORDER BY ra.idAdquisicion DESC
		END

		SELECT * 
		FROM [Pedidos].Red_RequerimientosAdjunto as rad
		INNER JOIN [Pedidos].Red_Adquisicion as ra 
			on ra.idrequerimiento = rad.idrequerimiento
		WHERE estadopublicado='P' 
		and ra.codproveedor=@PROV 
		--and rad.tipo='P'
	END

	IF @ACCION = 'PA'
	BEGIN
		UPDATE Pedidos.Red_Adquisicion set estadoparticipandp='P' where idAdquisicion=@IDPUBLI AND codproveedor = @PROV
	END

	IF @ACCION = 'CP'
	BEGIN		
		SELECT 
			ra.idAdquisicion as idPubli,
			ra.version,
			req.idrequerimiento,
			codcategoria,
			cat_e.Codigo as codEmpresa,
			req.monto,
			req.titulo,
			req.descripcion,
			req.estado,
			usuariocreacion,
			convert(varchar(10),feIniLicitacion, 103) as feIniLicitacion,
			convert(varchar(10),feFinLicitacion, 103) as feFinLicitacion,
			ra.hoFinLicitacion,
			ra.nombrePub,
			ra.descripcionPub,
			ra.estadoLicitacion,
			CASE estadoLicitacion 
				WHEN 'L' THEN 'EN LICITACIÓN'
				WHEN 'T' THEN 'ENVIADA' 
			END as estadoLicDesc,
			ra.estadopublicado,
			estadoparticipandp,
			CASE estadoparticipandp 
				WHEN 'P' THEN 'PARTICIPANDO'
				WHEN 'E' THEN 'ENVIADA' 
			END as estadoParticipandoDesc,
			estadoparticipandp,
			cat.Detalle as categoria
		FROM [Pedidos].[Red_Requerimientos] as req
		INNER JOIN [Pedidos].[Red_adquisicion] as ra	
			on ra.idrequerimiento = req.idrequerimiento
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
			on cat.Codigo = req.codcategoria 
		INNER JOIN [Proveedor].[Pro_Tabla] as t
			on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
		WHERE estadopublicado = 'P' 
		AND ra.codproveedor = @PROV
		AND estadoparticipandp in ('P','E')
		AND estadoLicitacion <> 'E'
		AND ra.feFinLicitacion >= getdate()
		ORDER BY ra.idAdquisicion DESC

		SELECT * 
		FROM [Pedidos].Red_RequerimientosAdjunto as rad
		INNER JOIN [Pedidos].Red_Adquisicion as ra 
			on ra.idrequerimiento = rad.idrequerimiento
		WHERE estadopublicado = 'P' 
		AND ra.codproveedor = @PROV
		--and rad.tipo='P'
		ORDER BY ra.idAdquisicion desc
	END

	IF @ACCION = 'CA'
	BEGIN		
		select idAdquisicion, codproveedor, isNull(monto,0) monto, isNull(tiempoEjecucion,0) tiempoEjecucion, convert(varchar(10),fechaEnvio, 103)fechaEnvio, rtrim(estadoparticipandp) estado 
		from Pedidos.Red_Adquisicion 
		where codproveedor=@PROV AND idAdquisicion=@IDPUBLI

		select * from [Pedidos].[Red_OfertaAdjunto]
		where codProveedor=@PROV AND idAdquisicion=@IDPUBLI
	END

	IF @ACCION = 'GO'
	BEGIN
		
		INSERT INTO [Pedidos].[Red_OfertaAdjunto](idAdquisicion, codProveedor, nombrearchivo, descripcion)
		SELECT
			@IDPUBLI,
			@PROV,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)')
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		delete from [Pedidos].[Red_OfertaAdjunto] 
		where idAdquisicion=@IDPUBLI and codProveedor = @PROV and idadjunto not in(
			SELECT
				nref.value('@id', 'INT')
			FROM @PI_ParamXML.nodes('/Root/Docqueda') AS R(nref)
		)
		
		INSERT INTO [Pedidos].[Red_OfertaAdjunto](idAdquisicion, codProveedor, nombrearchivo, descripcion)
		SELECT
			@IDPUBLI,
			@PROV,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)')
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)
		
		select 1 retorno
	END

	IF @ACCION = 'EO'
	BEGIN
		
		INSERT INTO [Pedidos].[Red_OfertaAdjunto](idAdquisicion, codProveedor, nombrearchivo, descripcion)
		SELECT
			@IDPUBLI,
			@PROV,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)')
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		delete from [Pedidos].[Red_OfertaAdjunto] 
		where idAdquisicion=@IDPUBLI and codProveedor = @PROV and idadjunto not in(
			SELECT
				nref.value('@id', 'INT')
			FROM @PI_ParamXML.nodes('/Root/Docqueda') AS R(nref)
		)
		
		INSERT INTO [Pedidos].[Red_OfertaAdjunto](idAdquisicion, codProveedor, nombrearchivo, descripcion)
		SELECT
			@IDPUBLI,
			@PROV,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)')
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		UPDATE Pedidos.Red_Adquisicion set monto = @MONT, tiempoEjecucion = @TIEM, estadoparticipandp='E', fechaEnvio = GETDATE()
		WHERE idAdquisicion=@IDPUBLI AND codproveedor = @PROV
		
		select 1 retorno
	END



	IF @@TRANCOUNT > 0
			COMMIT	TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
END CATCH



 

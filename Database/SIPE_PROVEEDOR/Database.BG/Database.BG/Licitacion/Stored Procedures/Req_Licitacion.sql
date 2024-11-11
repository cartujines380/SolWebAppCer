CREATE PROCEDURE [Licitacion].[Req_Licitacion]
	@PI_ParamXML xml
AS

SET ARITHABORT ON;
BEGIN TRY
	BEGIN TRAN
	DECLARE
		 @ACCION	 CHAR(5)
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
		,@TIPO		CHAR(1)
		,@IDPUBLI	INT
		,@MENSAJE	VARCHAR(MAX)
		,@MONTO		VARCHAR(10)

		DECLARE @REGSPROV TABLE
		(
			codProveedor varchar(10),
			NomComercial varchar(500), 
			correo varchar(241), 
			mensaje varchar(max), 
			titulo varchar(500)
		)

	SELECT
		@ACCION		= nref.value('@ACCION', 'CHAR(5)'),
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
		@TIPO		= nref.value('@TIPO', 'CHAR(1)'),
		@IDPUBLI	= nref.value('@IDPUBLI', 'INT')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @ACCION = 'CO'
	BEGIN
		IF @CRITERIO = 'T'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				monto,
				req.titulo,
				req.descripcion,
				cat.Estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) fechacreacion,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE req.estado='L'
			ORDER BY idrequerimiento DESC
		END
		IF @CRITERIO = 'F'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle as categoria, 
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE fecharequerimiento between @FEINI AND @FEFIN
			ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'R'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle as categoria, 
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE idrequerimiento = @REQ
			ORDER BY idrequerimiento desc
		END

		IF @CRITERIO = 'E'
		BEGIN
			select 
				idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa
			FROM [Pedidos].[Red_Requerimientos] req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE cat_e.Codigo = @EMP
			ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'C'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				cat_e.Codigo as codEmpresa,
				monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE req.codcategoria = @CAT
			ORDER BY req.idrequerimiento DESC
		END
	END

	IF @ACCION = 'CA'
	BEGIN
		select * from [Pedidos].[Red_RequerimientosAdjunto] where idrequerimiento=@REQ
	END

	IF @ACCION = 'CP'
	BEGIN		

		SELECT  
			rr.idrequerimiento 
		INTO #A
		FROM  [Pedidos].[Red_Adquisicion] ra 
		INNER JOIN [Pedidos].[Red_Requerimientos] rr 
			on ra.idrequerimiento=rr.idrequerimiento
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = rr.codEmpresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		WHERE 
			estadopublicado='P'
			AND estadoparticipandp='A'

		SELECT 
			DISTINCT 
			ra.fechacreacion, 
			rr.idrequerimiento, 
			version, 
			rr.titulo, 
			rr.descripcion as descReq, 
			nombrePub, 
			Convert(varchar(10),feIniLicitacion,103) as feIniLicitacion, 
			Convert(varchar(10),feFinLicitacion,103) as feFinLicitacion, 
			hoFinLicitacion, 
			ra.estadoLicitacion, 
			cat_e.Detalle as descEmp, 
			cat_e.Codigo as codEmpresa,
			rr.monto, 
			ra.descripcionPub
		FROM [Pedidos].[Red_Adquisicion] as ra 
		INNER JOIN [Pedidos].[Red_Requerimientos] as rr 
			on ra.idrequerimiento = rr.idrequerimiento
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = rr.codEmpresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		WHERE 
			estadopublicado='P'
			AND estadoparticipandp NOT IN('A') 
			OR estadoparticipandp IS NULL
			AND  RR.idrequerimiento NOT IN(SELECT * FROM #A	)
			AND version IS NOT NULL
		ORDER BY
			ra.fechacreacion desc, 
			idrequerimiento desc

		SELECT 
			pp.CodProveedor, 
			pp.NomComercial, 
			ra.idrequerimiento
		FROM [Pedidos].[Red_Adquisicion] as ra
		INNER JOIN [Proveedor].[Pro_Proveedor] as pp 
			on pp.codProveedor=ra.codproveedor
		WHERE 
			estadopublicado='P' 
			AND estadoparticipandp in ('P','E','A')  
			AND RA.idrequerimiento NOT IN(SELECT * FROM #A	)

		SELECT * 
		FROM [Pedidos].Red_RequerimientosAdjunto 
		WHERE idrequerimiento in (
			SELECT 
				DISTINCT rr.idrequerimiento
			FROM [Pedidos].[Red_Adquisicion] as ra 
			INNER JOIN [Pedidos].[Red_Requerimientos] as rr 
				on ra.idrequerimiento = rr.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = rr.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			WHERE estadopublicado='P'
		)
	END

	IF @ACCION = 'CPB'
	BEGIN		
		SELECT DISTINCT ra.fechacreacion, rr.idrequerimiento, version, rr.titulo, rr.descripcion descReq, nombrePub, Convert(varchar(10),feIniLicitacion,103)feIniLicitacion, Convert(varchar(10),feFinLicitacion,103)feFinLicitacion, hoFinLicitacion, ra.estadoLicitacion, cat_e.Detalle as descEmp, cat_e.Codigo as codEmpresa, rr.monto, ra.descripcionPub
		FROM [Pedidos].[Red_Adquisicion] ra 
		INNER JOIN [Pedidos].[Red_Requerimientos] as rr 
			on ra.idrequerimiento=rr.idrequerimiento
		INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
			on cat_e.Codigo = rr.codEmpresa
		INNER JOIN [Proveedor].[Pro_Tabla] as t_e
			on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
		WHERE estadopublicado='P' and estadoparticipandp in ('A')
		order by ra.fechacreacion desc, idrequerimiento desc

		SELECT pp.CodProveedor, pp.NomComercial, ra.idrequerimiento
		FROM [Pedidos].[Red_Adquisicion] ra
		INNER JOIN [Proveedor].[Pro_Proveedor] pp on pp.codProveedor=ra.codproveedor
		WHERE estadopublicado='P' and estadoparticipandp in ('A')

		SELECT * FROM [Pedidos].Red_RequerimientosAdjunto where idrequerimiento in (
			SELECT DISTINCT rr.idrequerimiento
			FROM [Pedidos].[Red_Adquisicion] ra 
			INNER JOIN [Pedidos].[Red_Requerimientos] rr 
				on ra.idrequerimiento=rr.idrequerimiento
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = rr.codEmpresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			WHERE estadopublicado='P'
		)
	END

	IF @ACCION = 'PL'
	BEGIN
		IF @TIPO='P'
		BEGIN			
			INSERT INTO [Pedidos].[Red_Adquisicion] 
			(idrequerimiento, codproveedor, estadopublicado, estadoLicitacion, version, usuariocionrecion, fechacreacion, nombrePub, descripcionPub, feIniLicitacion, feFinLicitacion, hoFinLicitacion)
			SELECT	@REQ,
					nref.value('@codProv', 'VARCHAR(10)'),
					'P',
					'L',
					1,
					'',
					GETDATE(),
					@NOM,
					@DESC,
					@FEINI,
					@FEFIN,
					@HOFIN
			FROM @PI_ParamXML.nodes('/Root/provs') AS R(nref)

			SELECT @MONTO = monto from Pedidos.Red_Requerimientos where idrequerimiento = @REQ
			
			SET @MENSAJE = 'Ha recibido una invitación para participar en un Proceso de Licitación de Sipecom S.A
								<br/>
								<br/>
								<strong>Código: </strong>_IDPUBLI
								<br/>
								<br/>
								<strong>Fecha de cierre de licitación:</strong> '+CONVERT(VARCHAR(10), @FEFIN, 103)+' <strong>Hora:</strong> '+CONVERT(VARCHAR(10),@HOFIN)+
								'<br/>
								<br/>
								<strong>Monto:</strong> $'+@MONTO+
								'<br/>
								<br/>
								<br/>
								Para mas información visite <a href="http://34.221.215.16:9999/">www.portalproveedores.com</a>'

			insert into @REGSPROV
			SELECT pp.CodProveedor, pp.NomComercial, pp.CorreoE, REPLACE(@MENSAJE,'_IDPUBLI',(select top 1 idrequerimiento from Pedidos.Red_Adquisicion where codproveedor = pp.CodProveedor and idrequerimiento=@REQ)), 'INVITACIÓN A LICITACIÓN'
			FROM @PI_ParamXML.nodes('/Root/provs') AS R(nref)
			INNER JOIN Proveedor.Pro_Proveedor pp on pp.CodProveedor=nref.value('@codProv', 'VARCHAR(10)')

		END
		ELSE
		BEGIN
			INSERT INTO [Pedidos].[Red_Adquisicion] 
			(idrequerimiento, codproveedor, estadopublicado, estadoLicitacion, version, usuariocionrecion, fechacreacion, nombrePub, descripcionPub, feIniLicitacion, feFinLicitacion, hoFinLicitacion)
			SELECT	@REQ,
					cp.codProveedor,
					'P',
					'L',
					1,
					'',
					GETDATE(),
					@NOM,
					@DESC,
					@FEINI,
					@FEFIN,
					@HOFIN
			FROM Proveedor.CateProveedor as cp
			INNER JOIN @PI_ParamXML.nodes('/Root/cats') as R(nref) 
				on nref.value('@codCat', 'VARCHAR(10)') = cp.codCategoria
				
			SELECT @MONTO = monto from Pedidos.Red_Requerimientos where idrequerimiento = @REQ

			SET @MENSAJE = 'Ha recibido una invitación para participar en un Proceso de Licitación de Sipecom S.A.
								<br/>
								<br/>
								<strong>Código: </strong>_IDPUBLI
								<br/>
								<br/>
								<strong>Fecha de cierre de licitación:</strong> '+CONVERT(VARCHAR(10), @FEFIN, 103)+' <strong>Hora:</strong> '+CONVERT(VARCHAR(10),@HOFIN)+
								'<br/>
								<br/>
								<strong>Monto:</strong> $'+@MONTO+
								'<br/>
								<br/>
								<br/>
								Para mas información visite <a href="http://34.221.215.16:9999/">www.portaldeproveedores.com</a>'
				
			INSERT INTO @REGSPROV
			SELECT 
				pp.CodProveedor, 
				pp.NomComercial, 
				pp.CorreoE, 
				REPLACE(@MENSAJE,'_IDPUBLI',(select top 1 idAdquisicion 
											 from Pedidos.Red_Adquisicion 
											 where codproveedor = pp.CodProveedor and idrequerimiento = @REQ
											 )
				), 
				'INVITACIÓN A LICITACIÓN'
			FROM Proveedor.CateProveedor as cp 
			INNER JOIN @PI_ParamXML.nodes('/Root/cats') as R(nref) 
				on nref.value('@codCat', 'VARCHAR(10)') = cp.codCategoria
			INNER JOIN Proveedor.Pro_Proveedor as pp 
				on pp.CodProveedor = rtrim(cp.codProveedor)
		END

		INSERT INTO [Pedidos].[Red_RequerimientosAdjunto](idrequerimiento, nombrearchivo, descripcion, tipo)
		SELECT
			@REQ,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)'),
			'P'
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		delete from [Pedidos].[Red_RequerimientosAdjunto] 
		where idrequerimiento=@REQ and idadjunto not in(
			SELECT
				nref.value('@id', 'INT')
			FROM @PI_ParamXML.nodes('/Root/Docqueda') AS R(nref)
		)

		INSERT INTO [Pedidos].[Red_RequerimientosAdjunto](idrequerimiento, nombrearchivo, descripcion, tipo)
		SELECT
			@REQ,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)'),
			'P'
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		UPDATE [Pedidos].[Red_Requerimientos] SET ESTADO='P' WHERE idrequerimiento=@REQ

		select * from @REGSPROV
	END

	IF @ACCION = 'EL'
	BEGIN
		SELECT TOP 1 @VER=ra.version FROM [Pedidos].[Red_Adquisicion] ra WHERE idrequerimiento=@REQ
		SET @VER=@VER+1;

		update [Pedidos].[Red_Adquisicion] set version=@VER, nombrePub=@NOM, descripcionPub = @DESC, feIniLicitacion = @FEINI, feFinLicitacion=@FEFIN, hoFinLicitacion=@HOFIN
		where idrequerimiento=@req

		delete from [Pedidos].[Red_RequerimientosAdjunto] 
		where idrequerimiento=@REQ and idadjunto not in(
			SELECT
				nref.value('@id', 'INT')
			FROM @PI_ParamXML.nodes('/Root/Docqueda') AS R(nref)
		)

		INSERT INTO [Pedidos].[Red_RequerimientosAdjunto](idrequerimiento, nombrearchivo, descripcion, tipo)
		SELECT
			@REQ,
			nref.value('@nombre', 'VARCHAR(200)'),
			nref.value('@descDoc', 'VARCHAR(200)'),
			'P'
		FROM @PI_ParamXML.nodes('/Root/doc') AS R(nref)

		SET @MENSAJE = 'Se ha modificado la Licitación de Sipecom S.A con <strong>Código: </strong>'+cast(@req as varchar(10))+'.'+
							'<br/>
							<br/>
							<br/>
							<br/>
							<br/>
							Para mas información visite <a href="http://34.221.215.16:9999/">www.portalproveedores.com</a>'


			insert @REGSPROV
	   SELECT PO.CodProveedor,PO.NomComercial,PO.CorreoE,@MENSAJE,'MODIFICACIÓN DE LICITACIÓN' 
	   FROM Pedidos.Red_Adquisicion AD
	   INNER JOIN Proveedor.Pro_Proveedor PO
	   ON AD.codproveedor=PO.CodProveedor
		WHERE idrequerimiento=@req

		select *
		from @REGSPROV

	END

	IF @ACCION = 'CD'
	BEGIN
		SELECT rtrim(ra.codproveedor)codproveedor, isnull(monto,0)monto, convert(varchar(10),fechaEnvio,103)fechaEnvio, isnull(tiempoEjecucion,0)tiempoEjecucion, rtrim(estadoparticipandp)estadoparticipandp, CASE estadoparticipandp 
																															WHEN 'P' THEN 'PARTICIPANDO' END estadoparticipandodesc,
		pp.NomComercial,
		ra.estadoselecionado,
		ra.idAdquisicion
		FROM Pedidos.Red_Adquisicion ra 
		INNER JOIN Proveedor.Pro_Proveedor pp on ra.codproveedor = pp.CodProveedor
		WHERE idrequerimiento = @REQ AND estadoparticipandp IN ('E','P','A')

		SELECT ra.idadjunto, ra.descripcion, ra.idAdquisicion, rtrim(ra.codProveedor)codProveedor, ra.nombreArchivo FROM Pedidos.red_ofertaAdjunto ra 
		INNER JOIN Pedidos.red_adquisicion rad on rad.idAdquisicion=ra.idadquisicion 
		where idrequerimiento=@REQ AND rad.estadoparticipandp IN ('E','P','A')
	END

	IF @@TRANCOUNT > 0
			COMMIT	TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
END CATCH



 

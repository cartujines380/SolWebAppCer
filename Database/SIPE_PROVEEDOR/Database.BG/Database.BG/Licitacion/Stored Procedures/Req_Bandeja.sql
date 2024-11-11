CREATE PROCEDURE [Licitacion].[Req_Bandeja]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY

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
		,@MENSAJE	VARCHAR(MAX)

	SELECT
		@ACCION		= nref.value('@ACCION', 'CHAR(2)'),
		@CRITERIO	= nref.value('@CRITERIO', 'CHAR(1)'),
		@FEINI		= CONVERT(Datetime, nref.value('@FEINI', 'VARCHAR(10)'), 103),
		@FEFIN		= CONVERT(Datetime, nref.value('@FEFIN', 'VARCHAR(10)'), 103),
		@REQ		= nref.value('@REQ', 'VARCHAR(8)'),
		@CAT		= nref.value('@CAT', 'VARCHAR(8)'),
		@EMP		= nref.value('@EMPRESA', 'VARCHAR(8)'),
		@IDPROV		= nref.value('@IDPROV', 'VARCHAR(8)'),
		@IDUSU		= nref.value('@USUARIO', 'VARCHAR(8)')		
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @ACCION = 'CO'
	BEGIN
		IF @CRITERIO = 'T'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				req.codempresa,
				CASE monto 
					WHEN '' THEN '0' 
					ELSE monto 
				END as monto,
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
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE req.estado = 'I' 
			ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'F'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				req.codempresa,
				CASE monto WHEN '' THEN '0' ELSE monto END monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.detalle as categoria,
				cat_e.Detalle as empresa
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			 WHERE fecharequerimiento between @FEINI AND @FEFIN 
			 AND req.estado='I'
			 ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'R'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				req.codempresa,
				CASE monto 
					WHEN '' THEN '0' 
					ELSE monto 
				END as monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle as categoria,
				cat_e.Detalle as empresa 
			from [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE idrequerimiento = @REQ 
			AND req.estado='I'
			ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'E'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				req.codempresa,
				CASE monto 
					WHEN '' THEN '0' 
					ELSE monto 
				END as monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) as fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) as fechacreacion,
				cat.Detalle categoria,
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] as req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE req.codempresa = @EMP 
			AND req.estado='I'
			ORDER BY idrequerimiento DESC
		END

		IF @CRITERIO = 'C'
		BEGIN
			SELECT 
				idrequerimiento,
				codcategoria,
				req.codempresa,
				CASE monto WHEN '' THEN '0' ELSE monto END monto,
				req.titulo,
				req.descripcion,
				req.estado,
				usuariocreacion,
				convert(varchar(10),fecharequerimiento, 103) fecharequerimiento,
				convert(varchar(10),fechacreacion, 103) fechacreacion,
				cat.Detalle categoria,
				cat_e.Detalle as empresa 
			FROM [Pedidos].[Red_Requerimientos] req
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat_e
				on cat_e.Codigo = req.codempresa
			INNER JOIN [Proveedor].[Pro_Tabla] as t_e
				on t_e.Tabla = cat_e.Tabla and t_e.TablaNombre = 'tbl_RedEmpresa'
			INNER JOIN [Proveedor].[Pro_Catalogo] as cat 
				on cat.Codigo = req.codcategoria 
			INNER JOIN [Proveedor].[Pro_Tabla] as t
				on t.Tabla = cat.Tabla and t.TablaNombre = 'tbl_LineaNegocio'
			WHERE codcategoria = @CAT 
			AND req.estado='I'
			ORDER BY idrequerimiento DESC
		END
	END

	IF @ACCION = 'CA'
	BEGIN
		IF @CRITERIO ='E'
		BEGIN
			SELECT 
				c.Codigo as id, 
				c.Detalle as descripcion
			FROM [Proveedor].[Pro_Tabla] as t
			INNER JOIN [Proveedor].[Pro_Catalogo] as c
				ON t.tabla=c.tabla
			WHERE t.TablaNombre = 'tbl_RedEmpresa'
		END

		IF @CRITERIO ='C'
		BEGIN
			SELECT 
				c.Codigo as id, 
				c.Detalle as descripcion
			FROM [Proveedor].[Pro_Tabla] as t
			INNER JOIN [Proveedor].[Pro_Catalogo] as c
				ON t.tabla=c.tabla
			WHERE t.TablaNombre = 'tbl_LineaNegocio'
		END

		IF @CRITERIO ='P'
		BEGIN
			select po.CodProveedor id, po.NomComercial descripcion from Proveedor.Pro_Proveedor po
			 INNER JOIN Seguridad.Seg_Usuario us
			on po.CodProveedor=us.CodProveedor
			and us.EsAdmin=1
			and us.Estado='A'
		END
	END

	IF @ACCION = 'AD'	
	BEGIN		
		insert into [Pedidos].[Red_Adquisicion] (idrequerimiento, codproveedor, estadoselecionado, idUsuarioAsignacion, fechaSeleccionado)
		values(@REQ, @IDPROV, 1, @IDUSU,GETDATE())

		update [Pedidos].Red_Requerimientos set estado='D' where idrequerimiento = @REQ

		
		SET @MENSAJE = 'La presente es para indicarle que se le ha adjudicado Licitación de Sipecom S.A <strong>Código: </strong>'+@REQ+'.'+
							'<br/>
							<br/>
							Por favor comuníquese con Katiuska Criollo al correo kcriollo@sipecom.com.
							<br/>
							<br/>
							<br/>
							Para mas información visite <a href="http://34.221.215.16:9999/">www.portaldeproveedores.com</a>'
		
		select NomComercial, @MENSAJE mensaje, CorreoE correo, 'ADJUDICACIÓN DE LICITACIÓN' titulo from Proveedor.Pro_Proveedor where CodProveedor=@IDPROV
	END

	IF @ACCION = 'LI'
	BEGIN
		update [Pedidos].[Red_Requerimientos] set estado = 'L' where idrequerimiento = @REQ

		select 1 retorno
	END



	IF @@TRANCOUNT > 0
			COMMIT	TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
END CATCH



 

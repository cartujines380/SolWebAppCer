-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 01-05-2016
-- Description:	REPORTE DE LOG DE COMUNICACION
-- 325
-- =============================================

CREATE PROCEDURE [Seguridad].[Seg_LogComunicacion]
	@PI_ParamXML xml
AS
BEGIN
	DECLARE @W_RUC VARCHAR(20) 
	DECLARE @W_CODSAP VARCHAR(15) 
	DECLARE @W_FECHA1 DATE, @W_FECHA2 DATE
	SET NOCOUNT ON;

	SELECT	
					
			    @W_RUC=nref.value('@ruc','VARCHAR(20)'),
				@W_CODSAP=nref.value('@CodSap','VARCHAR(15)'),
				@W_FECHA1= CASE WHEN nref.value('@Fecha1','VARCHAR(10)') = '' THEN NULL ELSE CONVERT(DATETIME, nref.value('@Fecha1','VARCHAR(10)'), 103) END,
				@W_FECHA2= CASE WHEN nref.value('@Fecha2','VARCHAR(10)') = '' THEN NULL ELSE CONVERT(DATETIME, nref.value('@Fecha2','VARCHAR(10)') + ' 23:59', 103) END
				FROM  @PI_ParamXML.nodes('/Root') as item(nref)

IF(@W_CODSAP='')
	SET @W_CODSAP=NULL

IF(@W_RUC='')
	SET @W_RUC=NULL

IF(@W_FECHA1 IS NULL)
	SET @W_FECHA1=CONVERT(DATE,GETDATE()-30,103)

IF(@W_FECHA2 IS NULL)
	SET @W_FECHA2=CONVERT(DATE,GETDATE(),103)


		SELECT DISTINCT NTI.CODIGO,NTI.Titulo,NTI.Comunicado,
				(SELECT  detalle FROM [Proveedor].[Pro_Catalogo] C INNER JOIN [Proveedor].[Pro_Tabla] T ON  C.tabla = T.tabla WHERE T.TablaNombre = 'tbl_Categorias' AND C.Codigo=NTI.Categoria)AS categoria,
				(SELECT  detalle FROM [Proveedor].[Pro_Catalogo] C INNER JOIN [Proveedor].[Pro_Tabla] T ON  C.tabla = T.tabla WHERE T.TablaNombre = 'tbl_Prioridad' AND C.Codigo=NTI.Prioridad)AS prioridad,
				Obligatorio,UsrIngreso,convert(date,nti.FechaPublicacion,103)as FechaPublicacion INTO #A
		FROM Notificacion.Notificacion NTI
		WHERE	NTI.FechaPublicacion BETWEEN @W_FECHA1 AND @W_FECHA2
		--AND		NP.Cod_proveedor=ISNULL(@W_CODSAP,NP.Cod_proveedor)
		--AND		PO.Ruc=ISNULL(@W_RUC,PO.Ruc)	
		ORDER BY NTI.CODIGO



		SELECT DISTINCT NTI.CODIGO,NTI.Titulo,PO.CodProveedor,PO.Ruc,PO.NomComercial,PO.CorreoE,PO.DirCallePrinc INTO #B
		FROM Notificacion.Notificacion NTI
			INNER JOIN notificacion.Notificacion_Proveedor NP
				ON NTI.Codigo=NP.Cod_Notificacion
			INNER JOIN Proveedor.Pro_Proveedor PO
				ON NP.Cod_proveedor=PO.CodProveedor 
		WHERE	NTI.FechaPublicacion BETWEEN @W_FECHA1 AND @W_FECHA2
		AND		NP.Cod_proveedor=ISNULL(@W_CODSAP,NP.Cod_proveedor)
		AND		PO.Ruc=ISNULL(@W_RUC,PO.Ruc)	
		AND     NTI.Codigo IN(SELECT CODIGO FROM #A)
		ORDER BY PO.NomComercial



		SELECT PO.Cod_Notificacion,PO.Cod_proveedor,US.USUARIO,ISNULL(USD.Identificacion,'')AS Identificacion,ISNULL(USD.Apellido1,'')AS Apellido1,
				ISNULL(USD.Apellido2,'') AS Apellido2,ISNULL(USD.Nombre1,'') AS Nombre1,ISNULL(USD.Nombre2,'')Nombre2,US.CorreoE,
				(SELECT  detalle FROM [Proveedor].[Pro_Catalogo] C INNER JOIN [Proveedor].[Pro_Tabla] T ON  C.tabla = T.tabla WHERE T.TablaNombre = 'tbl_FuncionContacto' AND C.Codigo=US.UsrFuncion)AS funcion,
				(SELECT  detalle FROM [Proveedor].[Pro_Catalogo] C INNER JOIN [Proveedor].[Pro_Tabla] T ON  C.tabla = T.tabla WHERE T.TablaNombre = 'tbl_DepartaContacto' AND C.Codigo=US.UsrCargo)AS departamento,
				0 as rolAdmin,0 as rolComercial,0 as rolContable,0 as rolLogistico
				INTO #C
		FROM	Seguridad.Seg_Usuario US
		INNER JOIN Notificacion.Notificacion_Proveedor PO
			ON US.Usuario=PO.Usuario AND US.CodProveedor=PO.Cod_proveedor
		INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA
			ON US.IdParticipante=PA.IdParticipante
		LEFT  JOIN Seguridad.Seg_UsuarioAdicional USD
			ON US.Usuario=USD.Usuario
		WHERE  PO.Cod_Notificacion IN(SELECT CODIGO FROM #A) 
		ORDER BY Cod_proveedor,US.Usuario


			DECLARE @W_USUARIO  VARCHAR(20),@W_CODNOTI VARCHAR(5)

			DECLARE USUARIOROL CURSOR FOR 
			SELECT C.Cod_proveedor,C.Usuario,C.Cod_Notificacion
			FROM	#C C

			OPEN USUARIOROL

			FETCH NEXT FROM USUARIOROL 
			INTO @W_CODSAP,@W_USUARIO,@W_CODNOTI

			WHILE @@FETCH_STATUS = 0
			BEGIN
			DECLARE	@W_COD INT

				SELECT	@W_COD=COUNT(1)
				FROM	Seguridad.Seg_Usuario US
				INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA
					ON	US.IdParticipante=PA.IdParticipante
				INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ROL
					ON	PA.IdUsuario=ROL.IdUsuario AND ROL.IdRol IN(24)
				WHERE	US.CodProveedor=@W_CODSAP
				AND		US.Usuario=@W_USUARIO

				UPDATE #C SET rolAdmin=@W_COD
				WHERE	Cod_Notificacion=@W_CODNOTI
				AND		Cod_proveedor=@W_CODSAP
				AND		Usuario=@W_USUARIO

				SELECT	@W_COD=COUNT(1)
				FROM	Seguridad.Seg_Usuario US
				INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA
					ON	US.IdParticipante=PA.IdParticipante
				INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ROL
					ON	PA.IdUsuario=ROL.IdUsuario AND ROL.IdRol IN(25)
				WHERE	US.CodProveedor=@W_CODSAP
				AND		US.Usuario=@W_USUARIO

				UPDATE #C SET rolContable=@W_COD
				WHERE	Cod_Notificacion=@W_CODNOTI
				AND		Cod_proveedor=@W_CODSAP
				AND		Usuario=@W_USUARIO

				SELECT	@W_COD=COUNT(1)
				FROM	Seguridad.Seg_Usuario US
				INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA
					ON	US.IdParticipante=PA.IdParticipante
				INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ROL
					ON	PA.IdUsuario=ROL.IdUsuario AND ROL.IdRol IN(26)
				WHERE	US.CodProveedor=@W_CODSAP
				AND		US.Usuario=@W_USUARIO

				UPDATE #C SET rolLogistico=@W_COD
				WHERE	Cod_Notificacion=@W_CODNOTI
				AND		Cod_proveedor=@W_CODSAP
				AND		Usuario=@W_USUARIO

				SELECT	@W_COD=COUNT(1)
				FROM	Seguridad.Seg_Usuario US
				INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA
					ON	US.IdParticipante=PA.IdParticipante
				INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario ROL
					ON	PA.IdUsuario=ROL.IdUsuario AND ROL.IdRol IN(27)
				WHERE	US.CodProveedor=@W_CODSAP
				AND		US.Usuario=@W_USUARIO

				UPDATE #C SET rolComercial=@W_COD
				WHERE	Cod_Notificacion=@W_CODNOTI
				AND		Cod_proveedor=@W_CODSAP
				AND		Usuario=@W_USUARIO
			
			
			FETCH NEXT FROM USUARIOROL 
			INTO @W_CODSAP,@W_USUARIO,@W_CODNOTI
			END 
			CLOSE USUARIOROL;
			DEALLOCATE USUARIOROL;



		SELECT * FROM #A 	
		SELECT * FROM #B ORDER BY CodProveedor
		SELECT * FROM #C

		SELECT A.Codigo,A.categoria,A.Comunicado,A.FechaPublicacion,A.Obligatorio,A.prioridad,A.Titulo,A.UsrIngreso,
			  B.CodProveedor,B.CorreoE,B.DirCallePrinc,B.NomComercial,B.Ruc,
			  C.Apellido1,C.Apellido2,C.CorreoE AS CORREOUSUARIO,C.departamento,C.funcion,C.Identificacion,C.Nombre1,
			  C.Nombre2,C.Usuario,C.rolAdmin,C.rolComercial,C.rolContable,C.rolLogistico
		FROM	#A A
		INNER JOIN #B B
			ON A.Codigo=B.Codigo
		INNER JOIN #C C
			ON A.Codigo=C.Cod_Notificacion
			AND  B.CodProveedor=C.Cod_proveedor

		DROP TABLE #A
		DROP TABLE #B
		DROP TABLE #C
END

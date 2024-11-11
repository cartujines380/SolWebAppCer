
CREATE PROCEDURE [Seguridad].[Seg_P_ConsLogAuditoria] @PI_ParamXML xml AS
BEGIN TRY
    BEGIN TRAN
	    DECLARE @V_Trx       varchar(25)
		DECLARE @V_FechaIni  varchar(25)
		DECLARE @V_FechaFin  varchar(25)
		DECLARE @Msg         varchar(25)

    SELECT
	       @V_Trx             = nref.value('@Trx', 'VARCHAR(100)')
         , @V_FechaIni        = nref.value('@FechaIni', 'VARCHAR(25)')
		 , @V_FechaFin	      = nref.value('@FechaFin', 'VARCHAR(25)')
		 --convert(datetime, nref.value('@FechaFin', 'VARCHAR(100)'), 105)

    FROM
           @PI_ParamXML.nodes('/Root') AS R (nref);


	IF (@V_FechaIni IS NULL OR @V_FechaIni = '')
		BEGIN
			SELECT @V_FechaIni = CONVERT(DATETIME, '01-01-1900', 105)
		END
	Else
		BEGIN
			SELECT @V_FechaIni = CONVERT(DATETIME, @V_FechaIni, 105)
		END

	IF (@V_FechaFin IS NULL OR @V_FechaFin = '')
		BEGIN
			SELECT @V_FechaFin = CONVERT(DATETIME, '31-12-2999 23:59', 105)
		END
	Else
		BEGIN
			SELECT @V_FechaFin = CONVERT(DATETIME, Concat(@V_FechaFin, ' ', '23:59'), 105)
		END

--CONSULTA: Reporte de Roles: que roles existen y que opciones/acciones están asignadas a los Roles
    IF @V_Trx = 'ReporteRol'
    BEGIN
        Select 
		H.CodAD, 
		T.Descripcion, 
		H.FechaCreacion as Fecha 
		from SIPE_FRAMEWORK.[Seguridad].[Seg_HomologacionRoles] H
			inner join SIPE_FRAMEWORK.Seguridad.Seg_OpcionTransRol OTR
			on H.IdRol = OTR.IdRol
			inner join SIPE_FRAMEWORK.Seguridad.Seg_Transaccion T
			on OTR.IdOrganizacion = T.IdOrganizacion AND OTR.IdTransaccion = T.IdTransaccion
			WHERE H.Estado = 'A'
			AND   T.Estado = 'A'
			  AND H.FechaCreacion BETWEEN @V_FechaIni AND @V_FechaFin 
			 order by H.FechaCreacion desc   

   
    END;

--CONSULTA: Logs de Acceso a Roles: Quien modificó, eliminó o creó un nuevo Rol con sus respectivas fechas.
    IF @V_Trx = 'AccesoRol'
    BEGIN
        SELECT 
		IdUsuario as Usuario,
		FechaMovi as Fecha,
		CAST(
             CASE
				  WHEN CHARINDEX('P3="1"',CAST(txtTransaccion AS VARCHAR(MAX))) > 0
                     THEN 'INSERTA'
                  WHEN CHARINDEX('P3="2"',CAST(txtTransaccion AS VARCHAR(MAX))) > 0
                     THEN 'CONSULTA'
				  WHEN CHARINDEX('P3="3"',CAST(txtTransaccion AS VARCHAR(MAX))) > 0
                     THEN 'ELIMINA'
                  WHEN CHARINDEX('P3="4"',CAST(txtTransaccion AS VARCHAR(MAX))) > 0
                     THEN 'MODIFICA'
             END AS varchar) as Accion
						 --, *
			FROM SIPE_FRAMEWORK.Seguridad.Seg_Auditoria A
			WHERE IdTransaccion = 302
			and idorganizacion = 1
			  AND A.FechaMovi BETWEEN @V_FechaIni AND @V_FechaFin 
			order by FechaMovi desc   
    END;

--CONSULTA: Logs de Acceso de Usuarios: cuando accedió, quien accedió y a que opción accedió.
    IF @V_Trx = 'AccesoUsuario'
    BEGIN
		Select A.IdUsuario IdUsuario,
			   A.IdUsuario Usuario,
			   A.FechaMovi Fecha,
			   A.IdTransaccion,
			   T.Descripcion AS DescTransaccion,
			   A.IdOrganizacion,
			   O.Descripcion AS DescOrganizacion,
			   A.IdAplicacion,
			   APL.Nombre AS DescAplicativo,
			   A.IdIdentificacion
		FROM SIPE_FRAMEWORK.Seguridad.Seg_Auditoria A
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Transaccion T
				ON A.IdTransaccion = T.IdTransaccion
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Aplicacion APL
				ON APL.IdAplicacion = A.IdAplicacion
				   AND A.IdOrganizacion = T.IdOrganizacion
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Organizacion O
				ON O.IdOrganizacion = A.IdOrganizacion
		where T.Menu = 1
			  AND T.Estado = 'A'
			  AND A.FechaMovi BETWEEN @V_FechaIni AND @V_FechaFin 

		UNION ALL

		SELECT A.IdUsuario,
			   (
				   SELECT T.c.value('@P0', 'nvarchar(max)')
				   FROM txtTransaccion.nodes('/T') T(c)
			   ) as Usuario,
			   A.FechaMovi Fecha,
			   A.IdTransaccion,
			   T.Descripcion AS DescTransaccion,
			   A.IdOrganizacion,
			   O.Descripcion AS DescOrganizacion,
			   A.IdAplicacion,
			   APL.Nombre AS DescAplicativo,
			   A.IdIdentificacion
		FROM SIPE_FRAMEWORK.Seguridad.Seg_Auditoria A
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Transaccion T
				ON A.IdTransaccion = T.IdTransaccion
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Aplicacion APL
				ON APL.IdAplicacion = A.IdAplicacion
				   AND A.IdOrganizacion = T.IdOrganizacion
			INNER JOIN SIPE_FRAMEWORK.Seguridad.Seg_Organizacion O
				ON O.IdOrganizacion = A.IdOrganizacion
		WHERE A.IdTransaccion = 136
			  --AND A.idorganizacion = 1
			   AND A.FechaMovi BETWEEN @V_FechaIni AND @V_FechaFin 

			  
			  order by A.FechaMovi desc   		
    END;


    IF @@TRANCOUNT > 0
        COMMIT TRAN
    SELECT @Msg = 'OK';

    SELECT @Msg AS 'Mensaje';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN
    SELECT @Msg = ERROR_MESSAGE()
    RAISERROR(   @Msg, -- Message text.
                 16,   -- Severity.
                 1     -- State.
             )
END CATCH


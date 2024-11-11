CREATE PROCEDURE [Seguridad].[Seg_P_Ejecuta_SSIS_Auditoria]
AS
	--DECLARE @Query varchar(max)
	--SET @Query = 'msdb..sp_start_job N''' + @Tarea + ''''
	EXEC msdb..sp_start_job N'SSIS_Auditoria'


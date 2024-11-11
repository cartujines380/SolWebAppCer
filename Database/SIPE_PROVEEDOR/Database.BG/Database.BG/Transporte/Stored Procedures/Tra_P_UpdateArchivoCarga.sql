-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 18-09-2015
-- Description:	ACTUALIZAR CARGA DE ARCHIVO
-- 319
-- =============================================

CREATE PROCEDURE [Transporte].[Tra_P_UpdateArchivoCarga]
	@PI_ParamXML xml
AS
BEGIN
	SET NOCOUNT ON;
	

	--SUBIR ARCHIVO EN ROTACION DE PRODUCTO
   	IF NOT EXISTS(SELECT 1 FROM Notificacion.Rotacion_Productos RPRO INNER JOIN @PI_ParamXML.nodes('/Root') as item(nref)  ON nref.value('@ANIO','VARCHAR(4)')=RPRO.ANIO  AND nref.value('@MESNUMERO','VARCHAR(4)')=RPRO.MESNUMERO AND nref.value('@RUC','VARCHAR(13)')=RPRO.RUC)
   BEGIN

   INSERT INTO Notificacion.Rotacion_Productos(
					ANIO
					,MES
					,ARCHIVO
					,RUC
					,MESNUMERO)
				SELECT	
						nref.value('@ANIO','VARCHAR(4)')
						,CASE  nref.value('@MESNUMERO','INT') 
							WHEN 1 THEN 'Enero'
							WHEN 2 THEN 'Febrero'
							WHEN 3 THEN 'Marzo'
							WHEN 4 THEN 'Abril'
							WHEN 5 THEN 'Mayo'
							WHEN 6 THEN 'Junio'
							WHEN 7 THEN 'Julio'
							WHEN 8 THEN 'Agosto'
							WHEN 9 THEN 'Septiembre'
							WHEN 10 THEN 'Octubre'
							WHEN 11 THEN 'Noviembre'
							WHEN 12 THEN 'Diciembre'
							END
						,nref.value('@ARCHIVO','VARCHAR(50)')
						,nref.value('@RUC','VARCHAR(13)')
						,nref.value('@MESNUMERO','INT')
				FROM  @PI_ParamXML.nodes('/Root') as item(nref)
	

	END
	ELSE
	BEGIN
			UPDATE Notificacion.Rotacion_Productos SET
					ARCHIVO=nref.value('@ARCHIVO','VARCHAR(50)')
					FROM  Notificacion.Rotacion_Productos RPRO
					INNER JOIN @PI_ParamXML.nodes('/Root') as item(nref)	
					ON nref.value('@ANIO','VARCHAR(4)')=RPRO.ANIO  
						AND nref.value('@MESNUMERO','VARCHAR(4)')=RPRO.MESNUMERO 
						AND nref.value('@RUC','VARCHAR(13)')=RPRO.RUC

	END
	RETURN
END



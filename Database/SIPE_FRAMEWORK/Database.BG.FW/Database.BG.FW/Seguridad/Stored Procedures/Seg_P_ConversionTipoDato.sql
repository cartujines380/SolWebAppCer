CREATE PROCEDURE [Seguridad].[Seg_P_ConversionTipoDato]
 @i_dato varchar(20) output
AS
		IF NOT EXISTS ( SELECT TOP 1 1 FROM Systypes where name = @i_dato AND uid=4)
		BEGIN
			DECLARE @xtype tinyint
			SELECT @xtype = [xtype] FROM   systypes WHERE  name = @i_dato
			SELECT @i_dato = name from Systypes where [xtype] = @xtype AND uid = 4
		END				


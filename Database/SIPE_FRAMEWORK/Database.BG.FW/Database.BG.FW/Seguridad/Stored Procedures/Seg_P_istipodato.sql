CREATE PROCEDURE [Seguridad].[Seg_P_istipodato] 
 @i_dato varchar(20),
 @o_estado tinyint output
AS
		if exists(select 1 FROM systypes where name = @i_dato)
			SELECT @o_estado = 1
		else
			SELECT @o_estado = 0





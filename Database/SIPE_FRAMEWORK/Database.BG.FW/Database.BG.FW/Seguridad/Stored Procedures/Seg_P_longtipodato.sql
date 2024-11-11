CREATE PROCEDURE [Seguridad].[Seg_P_longtipodato]
 @i_dato varchar(20),
 @o_long int  output
AS
		SELECT @o_long = isnull(length,0)
		FROM systypes 
		WHERE name = @i_dato
	





CREATE TYPE [dbo].[udMovimientoId]
    FROM BIGINT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el identificador de un movimiento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udMovimientoId';


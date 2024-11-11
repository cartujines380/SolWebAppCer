CREATE TYPE [dbo].[udSecuenciaCortaId]
    FROM TINYINT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenan una secuencia corta', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udSecuenciaCortaId';


CREATE TYPE [dbo].[udSecuencia]
    FROM SMALLINT NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que manejen valores numéricos cortos de secuencias, no trx', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udSecuencia';


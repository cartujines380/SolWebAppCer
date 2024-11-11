CREATE TYPE [dbo].[udSecuenciaLarga]
    FROM BIGINT NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que manejen valores numéricos largos de secuencias, si trx', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udSecuenciaLarga';


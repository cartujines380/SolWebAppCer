CREATE TYPE [dbo].[udObservacion]
    FROM VARCHAR (1000) NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen descripciones u observaciones mas detalladas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udObservacion';


CREATE TYPE [dbo].[udDescripcion]
    FROM VARCHAR (255) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen descripciones u observaciones cortas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udDescripcion';


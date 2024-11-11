CREATE TYPE [dbo].[udEntidadCortaId]
    FROM SMALLINT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el identificador de una entidad o maestro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udEntidadCortaId';


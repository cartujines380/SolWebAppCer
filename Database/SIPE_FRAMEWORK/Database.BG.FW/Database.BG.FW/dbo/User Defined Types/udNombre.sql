CREATE TYPE [dbo].[udNombre]
    FROM VARCHAR (100) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el nombre de una entidad, persona o empresa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udNombre';


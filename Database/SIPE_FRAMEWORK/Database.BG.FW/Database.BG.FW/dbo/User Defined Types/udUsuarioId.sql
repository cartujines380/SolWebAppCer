CREATE TYPE [dbo].[udUsuarioId]
    FROM VARCHAR (15) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el código de un usuario registrado en al AD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udUsuarioId';


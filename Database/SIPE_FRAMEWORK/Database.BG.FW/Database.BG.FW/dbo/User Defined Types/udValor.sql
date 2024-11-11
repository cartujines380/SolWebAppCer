CREATE TYPE [dbo].[udValor]
    FROM DECIMAL (20, 6) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas de valor que requieran decimales ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udValor';


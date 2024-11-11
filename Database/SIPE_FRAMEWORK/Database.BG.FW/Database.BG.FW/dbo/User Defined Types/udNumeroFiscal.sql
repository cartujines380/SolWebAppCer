CREATE TYPE [dbo].[udNumeroFiscal]
    FROM VARCHAR (10) NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el número fiscal de un documento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udNumeroFiscal';


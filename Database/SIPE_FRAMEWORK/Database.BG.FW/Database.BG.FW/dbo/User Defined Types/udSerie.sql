CREATE TYPE [dbo].[udSerie]
    FROM VARCHAR (3) NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que almacenen el código de la serie para formar el codigo de un comprobante con el formato SRI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udSerie';


CREATE TYPE [dbo].[udCedulaRUCPasaporte]
    FROM VARCHAR (15) NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para las columnas que indiquen la identificación de una persona o empresa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udCedulaRUCPasaporte';


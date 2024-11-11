CREATE TYPE [dbo].[udTCNumero]
    FROM VARCHAR (10) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero de identificacion impreso en la tarjeta de credito con que se realiza la compra', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udTCNumero';


CREATE TYPE [dbo].[udTCAutorizacion]
    FROM VARCHAR (10) NOT NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de dato utilizado para definir el numero de autorizacion de la transaccion de compra con Tarjeta Credito
Es el numero que da el operador de TC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'udTCAutorizacion';


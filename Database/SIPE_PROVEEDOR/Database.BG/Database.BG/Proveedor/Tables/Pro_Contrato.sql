CREATE TABLE [Proveedor].[Pro_Contrato] (
    [IdContrato]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [CodProveedor]       VARCHAR (10) NOT NULL,
    [NumContrato]        VARCHAR (10) NOT NULL,
    [NomArchivo]         VARCHAR (60) NOT NULL,
    [FechaRegistro]      DATETIME     NOT NULL,
    [FechaActualizacion] DATETIME     NULL,
    [Estado]             VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_Pro_Contrato] PRIMARY KEY CLUSTERED ([IdContrato] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contrato registrado por el proveedor', @level0type = N'SCHEMA', @level0name = N'Proveedor', @level1type = N'TABLE', @level1name = N'Pro_Contrato';


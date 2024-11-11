CREATE TABLE [Proveedor].[Pro_Direccion] (
    [IdDireccion]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [CodProveedor]    VARCHAR (10)  NOT NULL,
    [Pais]            VARCHAR (10)  NULL,
    [Provincia]       VARCHAR (10)  NULL,
    [Ciudad]          VARCHAR (12)  NULL,
    [CallePrincipal]  VARCHAR (100) NULL,
    [CalleSecundaria] VARCHAR (100) NULL,
    [PisoEdificio]    VARCHAR (10)  NULL,
    [CodPostal]       VARCHAR (30)  NULL,
    [Solar]           VARCHAR (30)  NULL,
    [Estado]          BIT           NULL,
    PRIMARY KEY CLUSTERED ([IdDireccion] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_IdDireccion_01]
    ON [Proveedor].[Pro_Direccion]([IdDireccion] ASC);


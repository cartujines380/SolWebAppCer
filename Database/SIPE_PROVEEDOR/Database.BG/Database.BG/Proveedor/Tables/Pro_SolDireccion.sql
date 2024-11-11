CREATE TABLE [Proveedor].[Pro_SolDireccion] (
    [IdDireccion]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]     BIGINT        NOT NULL,
    [Pais]            VARCHAR (10)  NULL,
    [Provincia]       VARCHAR (10)  NULL,
    [Ciudad]          VARCHAR (12)  NULL,
    [CallePrincipal]  VARCHAR (100) NULL,
    [CalleSecundaria] VARCHAR (100) NULL,
    [PisoEdificio]    VARCHAR (10)  NULL,
    [CodPostal]       VARCHAR (30)  NULL,
    [Solar]           VARCHAR (30)  NULL,
    [Estado]          BIT           NULL,
    CONSTRAINT [PK_PRO_SOLDIRECCION] PRIMARY KEY CLUSTERED ([IdDireccion] ASC)
);


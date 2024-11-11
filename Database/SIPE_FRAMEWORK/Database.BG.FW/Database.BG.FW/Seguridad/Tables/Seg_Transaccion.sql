CREATE TABLE [Seguridad].[Seg_Transaccion] (
    [IdOrganizacion] INT           NOT NULL,
    [IdTransaccion]  INT           NOT NULL,
    [Descripcion]    VARCHAR (100) NULL,
    [Estado]         CHAR (1)      NULL,
    [PerfilContable] INT           NULL,
    [Parametros]     VARCHAR (MAX) NULL,
    [Auditable]      CHAR (1)      NULL,
    [CostoNormal]    MONEY         NULL,
    [CostoEspecial]  MONEY         NULL,
    [IdServidor]     INT           NULL,
    [NombreBase]     VARCHAR (100) NULL,
    [NombreSP]       VARCHAR (100) NULL,
    [IdServidorExec] INT           NULL,
    [Menu]           BIT           NULL,
    [Monitor]        BIT           NULL,
    [XmlEntrada]     VARCHAR (MAX) NULL,
    [XmlSalida]      VARCHAR (MAX) NULL,
    [XmlValidador]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Transaccion] PRIMARY KEY CLUSTERED ([IdOrganizacion] ASC, [IdTransaccion] ASC) ON [Seguridad],
    CONSTRAINT [FK_Organizacion_Transaccion] FOREIGN KEY ([IdOrganizacion]) REFERENCES [Seguridad].[Seg_Organizacion] ([IdOrganizacion])
) ON [Seguridad] TEXTIMAGE_ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Transaccion_001]
    ON [Seguridad].[Seg_Transaccion]([IdOrganizacion] ASC)
    ON [Indices];


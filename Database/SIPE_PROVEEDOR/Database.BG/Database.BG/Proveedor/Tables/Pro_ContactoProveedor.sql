CREATE TABLE [Proveedor].[Pro_ContactoProveedor] (
    [IdContacto]         INT          IDENTITY (1, 1) NOT NULL,
    [CodProveedor]       INT          NULL,
    [TipoIdentificacion] VARCHAR (2)  NULL,
    [Identificacion]     VARCHAR (20) NULL,
    [Nombre1]            VARCHAR (60) NULL,
    [Nombre2]            VARCHAR (60) NULL,
    [Apellido1]          VARCHAR (60) NULL,
    [Apellido2]          VARCHAR (60) NULL,
    [Prefijo]            VARCHAR (12) NULL,
    [Estado]             CHAR (1)     NULL,
    [TelfFijo]           VARCHAR (10) NULL,
    [TelfFijoEXT]        VARCHAR (5)  NULL,
    [TelfMovil]          VARCHAR (10) NULL,
    [email]              VARCHAR (60) NULL,
    [NotElectronica]     SMALLINT     NULL,
    [NotTransBancaria]   SMALLINT     NULL,
    [RecActas]           SMALLINT     NULL,
    [RepLegal]           SMALLINT     NULL,
    [FechaRegistro]      DATETIME     NULL,
    [FechaActualizacion] DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([IdContacto] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoProveedor_01]
    ON [Proveedor].[Pro_ContactoProveedor]([CodProveedor] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoProveedor_02]
    ON [Proveedor].[Pro_ContactoProveedor]([Identificacion] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoProveedor_03]
    ON [Proveedor].[Pro_ContactoProveedor]([Apellido1] ASC, [Apellido2] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoProveedor_04]
    ON [Proveedor].[Pro_ContactoProveedor]([Nombre1] ASC, [Nombre2] ASC);


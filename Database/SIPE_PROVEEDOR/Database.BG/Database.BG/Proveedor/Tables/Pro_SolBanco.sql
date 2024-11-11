CREATE TABLE [Proveedor].[Pro_SolBanco] (
    [IdSolBanco]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]        BIGINT        NOT NULL,
    [Extrangera]         BIT           NULL,
    [CodSapBanco]        VARCHAR (15)  NULL,
    [Pais]               VARCHAR (10)  NULL,
    [TipoCuenta]         VARCHAR (3)   NULL,
    [NumeroCuenta]       VARCHAR (18)  NULL,
    [TitularCuenta]      VARCHAR (60)  NULL,
    [ReprCuenta]         VARCHAR (200) NULL,
    [CodSwift]           VARCHAR (10)  NULL,
    [CodBENINT]          VARCHAR (10)  NULL,
    [CodABA]             VARCHAR (10)  NULL,
    [Principal]          BIT           NULL,
    [Provincia]          VARCHAR (10)  NULL,
    [DirBancoExtranjero] VARCHAR (250) NULL,
    [Estado]             BIT           NULL,
    [BancoExtranjero]    VARCHAR (250) NULL,
    CONSTRAINT [PK_PRO_SOLBANCO] PRIMARY KEY CLUSTERED ([IdSolBanco] ASC),
    CONSTRAINT [FK_PRO_SOLB_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);


CREATE TABLE [Seguridad].[Seg_Usuario] (
    [IdEmpresa]         INT          NOT NULL,
    [Ruc]               VARCHAR (13) NOT NULL,
    [Usuario]           VARCHAR (20) NOT NULL,
    [IdParticipante]    INT          NOT NULL,
    [CodProveedor]      VARCHAR (10) NULL,
    [CorreoE]           VARCHAR (50) NOT NULL,
    [Telefono]          VARCHAR (50) NULL,
    [Celular]           VARCHAR (50) NULL,
    [EsAdmin]           BIT          NOT NULL,
    [Estado]            VARCHAR (1)  NOT NULL,
    [UsrAutorizador]    VARCHAR (20) NOT NULL,
    [FechaRegistro]     DATETIME     NOT NULL,
    [FechaModificacion] DATETIME     NULL,
    [CodLegacy]         VARCHAR (10) NULL,
    [UsrLegacy]         VARCHAR (20) NULL,
    [UsrCargo]          VARCHAR (5)  NULL,
    [UsrFuncion]        VARCHAR (5)  DEFAULT ('12') NULL,
    [UsrSubido]         INT          DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Seg_Usuario] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC) ON [Proveedor],
    CONSTRAINT [FK_SegUsu_Prov] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
) ON [Proveedor];


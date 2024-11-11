CREATE TABLE [Participante].[Par_CuentaParticipante] (
    [IdParticipante]  INT           NOT NULL,
    [Cuenta]          VARCHAR (50)  NOT NULL,
    [IdTipoCuenta]    VARCHAR (10)  NULL,
    [IdBanco]         INT           NULL,
    [OficialCuenta]   VARCHAR (100) NULL,
    [Telefono]        VARCHAR (100) NULL,
    [DescCuentaBanco] VARCHAR (500) NULL,
    CONSTRAINT [PK_CuentaParticipante] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [Cuenta] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_CuentaParticipante] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];


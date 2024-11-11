CREATE TABLE [Seguridad].[Seg_HorarioTrans] (
    [IdOrganizacion] INT NOT NULL,
    [IdTransaccion]  INT NOT NULL,
    [IdOpcion]       INT NOT NULL,
    [IdHorario]      INT NOT NULL,
    CONSTRAINT [PK_HorarioTrans] PRIMARY KEY CLUSTERED ([IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC, [IdHorario] ASC) ON [Seguridad],
    CONSTRAINT [FK_Horario_HorarioTrans] FOREIGN KEY ([IdHorario]) REFERENCES [Seguridad].[Seg_Horario] ([IdHorario]),
    CONSTRAINT [FK_OpcionTrans_HorarioTrans] FOREIGN KEY ([IdOrganizacion], [IdTransaccion], [IdOpcion]) REFERENCES [Seguridad].[Seg_OpcionTrans] ([IdOrganizacion], [IdTransaccion], [IdOpcion])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_HorarioTrans_001]
    ON [Seguridad].[Seg_HorarioTrans]([IdHorario] ASC)
    ON [Indices];


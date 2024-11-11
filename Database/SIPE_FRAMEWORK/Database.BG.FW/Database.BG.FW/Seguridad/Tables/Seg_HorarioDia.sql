CREATE TABLE [Seguridad].[Seg_HorarioDia] (
    [IdHorarioDia] INT         NOT NULL,
    [IdHorario]    INT         NOT NULL,
    [Dias]         VARCHAR (7) NOT NULL,
    [HoraInicio]   DATETIME    NOT NULL,
    [HoraFin]      DATETIME    NOT NULL,
    CONSTRAINT [PK_HorarioDia] PRIMARY KEY CLUSTERED ([IdHorario] ASC, [IdHorarioDia] ASC) ON [Seguridad],
    CONSTRAINT [FK_Horario_HorarioDia] FOREIGN KEY ([IdHorario]) REFERENCES [Seguridad].[Seg_Horario] ([IdHorario])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_HorarioDia_001]
    ON [Seguridad].[Seg_HorarioDia]([IdHorario] ASC)
    ON [Indices];


CREATE TABLE [Seguridad].[Seg_Horario] (
    [IdHorario]    INT           NOT NULL,
    [Descripcion]  VARCHAR (100) NOT NULL,
    [DiasFeriados] BIT           NULL,
    CONSTRAINT [PK_Horario] PRIMARY KEY CLUSTERED ([IdHorario] ASC) ON [Seguridad]
) ON [Seguridad];


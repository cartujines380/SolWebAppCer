CREATE TABLE [Seguridad].[Seg_DiasFeriados] (
    [IdEmpresa]  INT      NOT NULL,
    [IdSucursal] INT      NOT NULL,
    [Dia]        DATETIME NOT NULL,
    CONSTRAINT [PK_DiasFeriados] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdSucursal] ASC, [Dia] ASC) ON [Seguridad]
) ON [Seguridad];


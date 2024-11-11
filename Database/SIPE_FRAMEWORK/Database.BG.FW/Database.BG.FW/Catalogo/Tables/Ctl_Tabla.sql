CREATE TABLE [Catalogo].[Ctl_Tabla] (
    [IdTabla]     INT           NOT NULL,
    [Nombre]      VARCHAR (50)  NOT NULL,
    [Descripcion] VARCHAR (150) NULL,
    PRIMARY KEY CLUSTERED ([IdTabla] ASC) ON [Catalogo]
) ON [Catalogo];


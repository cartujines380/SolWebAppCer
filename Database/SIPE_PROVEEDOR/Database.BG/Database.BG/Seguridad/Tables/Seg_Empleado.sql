CREATE TABLE [Seguridad].[Seg_Empleado] (
    [IdEmpresa]      INT           NOT NULL,
    [Ruc]            VARCHAR (13)  NOT NULL,
    [Usuario]        VARCHAR (20)  NOT NULL,
    [IdParticipante] INT           NOT NULL,
    [TipoIdent]      VARCHAR (10)  NOT NULL,
    [Identificacion] VARCHAR (13)  NOT NULL,
    [Apellido1]      VARCHAR (100) NOT NULL,
    [Apellido2]      VARCHAR (100) NULL,
    [Nombre1]        VARCHAR (100) NOT NULL,
    [Nombre2]        VARCHAR (100) NULL,
    [Cargo]          VARCHAR (10)  NOT NULL,
    [CorreoE]        VARCHAR (50)  NOT NULL,
    [Celular]        VARCHAR (50)  NULL,
    [Estado]         VARCHAR (1)   NOT NULL,
    CONSTRAINT [PK_Seg_Empleado] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC) ON [Proveedor]
) ON [Proveedor];


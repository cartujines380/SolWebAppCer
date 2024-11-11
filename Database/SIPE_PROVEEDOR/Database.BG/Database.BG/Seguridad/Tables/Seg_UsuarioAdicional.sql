CREATE TABLE [Seguridad].[Seg_UsuarioAdicional] (
    [IdEmpresa]      INT           NOT NULL,
    [Ruc]            VARCHAR (13)  NOT NULL,
    [Usuario]        VARCHAR (20)  NOT NULL,
    [TipoIdent]      VARCHAR (10)  NOT NULL,
    [Identificacion] VARCHAR (13)  NOT NULL,
    [Apellido1]      VARCHAR (100) NOT NULL,
    [Apellido2]      VARCHAR (100) NULL,
    [Nombre1]        VARCHAR (100) NOT NULL,
    [Nombre2]        VARCHAR (100) NULL,
    [EstadoCivil]    VARCHAR (10)  NOT NULL,
    [Genero]         VARCHAR (10)  NOT NULL,
    [Pais]           VARCHAR (10)  NOT NULL,
    [Provincia]      VARCHAR (10)  NOT NULL,
    [Ciudad]         VARCHAR (10)  NOT NULL,
    [Direccion]      VARCHAR (300) NOT NULL,
    [RecibeActas]    BIT           NULL,
    CONSTRAINT [PK_Seg_UsuarioAdicional] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC) ON [Proveedor]
) ON [Proveedor];


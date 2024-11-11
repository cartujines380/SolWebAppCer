CREATE TABLE [Proveedor].[Pro_ProveedorContacto] (
    [CodProveedor] VARCHAR (10)  NOT NULL,
    [CodContacto]  VARCHAR (15)  NOT NULL,
    [Tratamiento]  VARCHAR (30)  NULL,
    [NomPila]      VARCHAR (35)  NULL,
    [Nombre]       VARCHAR (35)  NULL,
    [DepCliente]   VARCHAR (12)  NULL,
    [Departamento] VARCHAR (4)   NULL,
    [Funcion]      VARCHAR (2)   NULL,
    [Telefono1]    VARCHAR (16)  NULL,
    [Telefono2]    VARCHAR (30)  NULL,
    [CorreoE]      VARCHAR (241) NULL,
    CONSTRAINT [PK_Pro_Pro_ProveedorContacto] PRIMARY KEY CLUSTERED ([CodProveedor] ASC, [CodContacto] ASC) ON [Proveedor]
) ON [Proveedor];


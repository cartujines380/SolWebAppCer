CREATE TABLE [Proveedor].[CMPro_Contacto_tmp] (
    [CodProveedor]       VARCHAR (25)  NULL,
    [CodContacto]        VARCHAR (2)   NULL,
    [TipoIdentificacion] VARCHAR (10)  NULL,
    [Identificacion]     VARCHAR (2)   NULL,
    [Nombre2]            VARCHAR (150) NULL,
    [Nombre1]            VARCHAR (150) NULL,
    [Apellido2]          VARCHAR (150) NULL,
    [Apellido1]          VARCHAR (150) NULL,
    [PreFijo]            VARCHAR (2)   NULL,
    [Departamento]       VARCHAR (2)   NULL,
    [Funcion]            VARCHAR (2)   NULL,
    [RepLegal]           BIT           DEFAULT ((1)) NULL,
    [Estado]             BIT           DEFAULT ((1)) NULL,
    [Telefono]           VARCHAR (45)  NULL,
    [Ext]                VARCHAR (15)  NULL,
    [Celular]            VARCHAR (60)  NULL,
    [Correo]             VARCHAR (150) NULL
);


GO
CREATE NONCLUSTERED INDEX [Idx_CodProveedor_01]
    ON [Proveedor].[CMPro_Contacto_tmp]([CodProveedor] ASC);


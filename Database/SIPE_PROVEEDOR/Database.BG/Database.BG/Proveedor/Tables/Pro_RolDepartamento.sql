CREATE TABLE [Proveedor].[Pro_RolDepartamento] (
    [IdRol]            INT          NOT NULL,
    [IdDepartamento]   VARCHAR (4)  NOT NULL,
    [DescDepartamento] VARCHAR (50) NOT NULL,
    [IdFuncion]        VARCHAR (2)  NOT NULL,
    [DescFuncion]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Pro_RolDepartamento] PRIMARY KEY CLUSTERED ([IdRol] ASC)
);


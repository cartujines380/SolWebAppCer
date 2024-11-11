CREATE TABLE [Proveedor].[Pro_ContactoDepartamento] (
    [IdContDep]       INT          IDENTITY (1, 1) NOT NULL,
    [IdContacto]      INT          NULL,
    [Identificacion]  VARCHAR (20) NULL,
    [CodDepartamento] VARCHAR (5)  NULL,
    [CodFuncion]      VARCHAR (5)  NULL,
    [Estado]          INT          NULL,
    PRIMARY KEY CLUSTERED ([IdContDep] ASC),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto])
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoDepartamento_01]
    ON [Proveedor].[Pro_ContactoDepartamento]([Identificacion] ASC);


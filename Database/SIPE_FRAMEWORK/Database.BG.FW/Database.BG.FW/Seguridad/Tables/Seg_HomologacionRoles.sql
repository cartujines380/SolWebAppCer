CREATE TABLE [Seguridad].[Seg_HomologacionRoles] (
    [Id]                  INT          IDENTITY (1, 1) NOT NULL,
    [IdRol]               INT          NOT NULL,
    [CodAD]               VARCHAR (50) NULL,
    [FechaCreacion]       DATETIME     NULL,
    [UsuarioCreacion]     VARCHAR (15) NULL,
    [FechaModificacion]   DATETIME     NULL,
    [UsuarioModificacion] VARCHAR (15) NULL,
    [Estado]              CHAR (1)     NOT NULL,
    CONSTRAINT [PK_ID] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IDROL] FOREIGN KEY ([IdRol]) REFERENCES [Seguridad].[Seg_Rol] ([IdRol])
);


GO
CREATE NONCLUSTERED INDEX [idx_codigo_ad_1]
    ON [Seguridad].[Seg_HomologacionRoles]([CodAD] ASC);


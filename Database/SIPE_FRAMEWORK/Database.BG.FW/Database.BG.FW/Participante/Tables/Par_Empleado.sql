CREATE TABLE [Participante].[Par_Empleado] (
    [IdParticipante]     INT          NOT NULL,
    [IdTipoEmpleado]     VARCHAR (10) NULL,
    [IdOrganigrama]      INT          NULL,
    [IdEmpresa]          INT          NOT NULL,
    [IdOficina]          INT          NOT NULL,
    [IdEmpresaPertenece] INT          NULL,
    [IdCargo]            VARCHAR (10) NULL,
    [Sueldo]             MONEY        NULL,
    [HorasExtras]        BIT          NULL,
    [LibretaSeguro]      VARCHAR (50) NULL,
    [FechaIngSeguro]     DATETIME     NULL,
    [FechaNotEgreso]     DATETIME     NULL,
    [IdMoneda]           VARCHAR (10) NULL,
    [Estado]             VARCHAR (10) NULL,
    CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdEmpresa] ASC) ON [Participante],
    CONSTRAINT [FK_Organigrama_Empleado] FOREIGN KEY ([IdOrganigrama], [IdEmpresa]) REFERENCES [Participante].[Par_Organigrama] ([IdOrganigrama], [IdEmpresa]),
    CONSTRAINT [FK_Persona_Empleado] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Persona] ([IdParticipante])
) ON [Participante];


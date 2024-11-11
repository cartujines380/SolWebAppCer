USE SIPE_PROVEEDOR
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Proveedor].[Pro_sociedad](
	[IdSociedad] [varchar](5) NOT NULL,
	[NombreSociedad] [varchar](100) NOT NULL,
	[Activar] [bit] NOT NULL,
	[Estado] [varchar](2) NOT NULL,
	[url] [varchar](100) NULL,
	[Licencia] [varchar](100) NULL,
	[RepresentanteLegal] [varchar](100) NULL,
	[RucSociedad] [varchar](13) NULL,
 CONSTRAINT [PK_pro_sociedad] PRIMARY KEY CLUSTERED 
(
	[IdSociedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1001', N'CORPORACION EL ROSADO S.A', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1021', N'PRUEBAS 1021', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1022', N'FRECUENTO S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1064', N'SUPERCINES S.A.', 1, N'A', N'', N'5Bjc4hKn16GGS8q0HpGBN1cmohIb7TaxR5nI49LrhwF6mvIEhuJfF5CaQllCrHwZUvUlsCiecmoGlXWD95ZJ9Q==', N'', N'')
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1068', N'COMDERE S.A.', 1, N'A', N'', N'LyNT6udzU2W8Q2ukYXHIopOe57WNrqoCnobK9xyB/FGnNbPUsUZv8bre0PSeVb/r0GpOJx1JKe6Sezey0XASrA==', N'', N'')
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'1072', N'XXXXXXXXX', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'2002', N'Alimentos del Ecuador Cia', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'2037', N'Briko S.A.', 1, N'A', N'', N'Kuxi0LBYPdERM1kAW4QjD2ZJ0zvhVXOpxgztGfC22 ZXlJGgBky9pRWU9BiTl eDn JYxfYxtDFZsLGx8SWJXQ==', N'', N'')
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'2071', N'PANADERIA DEL PACIFICO SA', 1, N'A', N'', N'TgaPhnGykk0g q6vXvPTi6znlkw6STv41H/YLfwKUJSTUZ7jA0CkykTAVv2CX9PKS42WQoC0qhN6j2XJNAjB6A==', N'', N'')
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'2072', N'Prokarnitas', 1, N'A', N'', N'yZLaW SyQXt8nYnr/KqI2rAWQmazErzVMWaUYHxKSV5x4tKwZwOG95Jd4gV HnxUcPoODJGkbYzcKp7Cs/I6hw==', N'', N'')
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3004', N'Com. Inmobiliaria SA CISA', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3025', N'Inmobiliaria Lavie S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3031', N'Inmobiliaria Motke S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3063', N'UBESAIR S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3065', N'Inmobiliaria Columbia S.A', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'3069', N'Inmobiliaria MeridionalSA', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4003', N'Alim. Superba Cia. Ltda.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4005', N'SECONAUNSA', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4006', N'Quitumbe S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4007', N'HAEMEK S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4008', N'Garmor S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4009', N'Comis. Roca Corocsa S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4010', N'BOCESA', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4011', N'Mejia S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4012', N'SANRASA', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4013', N'Comichala S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4014', N'Panglo S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4015', N'Comiduran S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4016', N'Kenort S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4017', N'AIDENP S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4018', N'Bacelli S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4020', N'Odescalchi S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4044', N'Inseg C. Ltda.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4046', N'Serviuno C.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4047', N'Ferconlisin S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4051', N'Serviser Cia. Ltda.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4060', N'Constructora Abacam S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4061', N'Constructora Abidi S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4067', N'Administradora', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'4070', N'Super Puntos S.A. (SPSA)', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'5052', N'Radio Concierto Guayaquil', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'5053', N'Radio Concierto Quito', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'5054', N'Radio Concierto Cuenca', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'5066', N'Entret. del Pacifico S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'6062', N'Ecosan S.A.', 0, N'A', NULL, NULL, NULL, NULL)
INSERT [Proveedor].[Pro_sociedad] ([IdSociedad], [NombreSociedad], [Activar], [Estado], [url], [Licencia], [RepresentanteLegal], [RucSociedad]) VALUES (N'9999', N'Ficticia Grupo ElRosad.HR', 0, N'A', NULL, NULL, NULL, NULL)

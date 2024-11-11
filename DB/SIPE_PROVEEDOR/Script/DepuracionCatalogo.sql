use SIPE_PROVEEDOR

declare @v_max int

select top 1 @v_max = t.Tabla from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_GrupoCuenta'
delete from Proveedor.Pro_Catalogo where Tabla in (@v_max) 
and Codigo not in ('ZPVA')

select * from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_GrupoCuenta'
-------------------------------------------------------------------------------------------------------------------------------------------

select top 1 @v_max = t.Tabla from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_LineaNegocio'
delete from Proveedor.Pro_Catalogo where Tabla in (@v_max) 

insert into Proveedor.Pro_Catalogo values
(@v_max, 'AF', 'ACTIVOS FIJOS', 'A', NULL),
(@v_max, 'AD', 'ADMINISTRACION', 'A', NULL),
(@v_max, 'AU', 'AUDITORIA', 'A', NULL),
(@v_max, 'BP', 'BANCA PERSONAS Y CANALES', 'A', NULL),
(@v_max, 'FI', 'FINANCIERO', 'A', NULL),
(@v_max, 'GC', 'GOBIERNO CORPORATIVO', 'A', NULL),
(@v_max, 'IN', 'INNOVACION', 'A', NULL),
(@v_max, 'LE', 'LEGAL', 'A', NULL),
(@v_max, 'ME', 'MERCADEO', 'A', NULL),
(@v_max, 'NE', 'NEGOCIOS ESPECIALIZADOS', 'A', NULL),
(@v_max, 'OP', 'OPERACIONES', 'A', NULL),
(@v_max, 'PV', 'PROVEEDURIA', 'A', NULL),
(@v_max, 'RN', 'REGIONAL NORTE', 'A', NULL),
(@v_max, 'RP', 'RIESGO DE PORTAFOLIO', 'A', NULL),
(@v_max, 'RI', 'RIESGO INTEGRAL', 'A', NULL),
(@v_max, 'SG', 'SEGURIDAD', 'A', NULL),
(@v_max, 'SI', 'SEGURIDAD DE LA INFORMACIÓN', 'A', NULL),
(@v_max, 'SE', 'SEGURIDAD INTEGRAL', 'A', NULL),
(@v_max, 'SS', 'SERVICIOS GENERALES.', 'A', NULL),
(@v_max, 'TA', 'TALENTO Y CULTURA', 'A', NULL),
(@v_max, 'TJ', 'TARJETAS', 'A', NULL),
(@v_max, 'TC', 'TARJETAS DE CREDITO', 'A', NULL),
(@v_max, 'TE', 'TECNOLOGIA', 'A', NULL)

select top 1 @v_max = t.Tabla from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_ProvGrupoCompras'

delete from Proveedor.Pro_Catalogo where Tabla in (@v_max)

insert into Proveedor.Pro_Catalogo values
(@v_max, 'AF', 'ACTIVOS FIJOS', 'A', 'AF'),
(@v_max, 'AD', 'ADMINISTRACION', 'A', 'AD'),
(@v_max, 'AU', 'AUDITORIA', 'A', 'AU'),
(@v_max, 'BP', 'BANCA PERSONAS Y CANALES', 'A', 'BP'),
(@v_max, 'FI', 'FINANCIERO', 'A', 'FI'),
(@v_max, 'GC', 'GOBIERNO CORPORATIVO', 'A', 'GC'),
(@v_max, 'IN', 'INNOVACION', 'A', 'IN'),
(@v_max, 'LE', 'LEGAL', 'A', 'LE'),
(@v_max, 'ME', 'MERCADEO', 'A', 'ME'),
(@v_max, 'NE', 'NEGOCIOS ESPECIALIZADOS', 'A', 'NE'),
(@v_max, 'OP', 'OPERACIONES', 'A', 'OP'),
(@v_max, 'PV', 'PROVEEDURIA', 'A', 'PV'),
(@v_max, 'RN', 'REGIONAL NORTE', 'A', 'RN'),
(@v_max, 'RP', 'RIESGO DE PORTAFOLIO', 'A', 'RP'),
(@v_max, 'RI', 'RIESGO INTEGRAL', 'A', 'RI'),
(@v_max, 'SG', 'SEGURIDAD', 'A', 'SG'),
(@v_max, 'SI', 'SEGURIDAD DE LA INFORMACIÓN', 'A', 'SI'),
(@v_max, 'SE', 'SEGURIDAD INTEGRAL', 'A', 'SE'),
(@v_max, 'SS', 'SERVICIOS GENERALES', 'A', 'SS'),
(@v_max, 'TA', 'TALENTO Y CULTURA', 'A', 'TA'),
(@v_max, 'TJ', 'TARJETAS', 'A', 'TJ'),
(@v_max, 'TC', 'TARJETAS DE CREDITO', 'A', 'TC'),
(@v_max, 'TE', 'TECNOLOGIA', 'A', 'TE')


select top 1 @v_max = t.Tabla from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_tipoParticipante_neo'

update Proveedor.Pro_Catalogo set Detalle = 'GARANTE' where Tabla = @v_max and Codigo = '354'





select top 1 @v_max = t.Tabla from Proveedor.Pro_Tabla t inner join Proveedor.Pro_Catalogo c on c.Tabla = t.Tabla  where t.TablaNombre = 'tbl_EstadosSolicitudProveedor'

delete from Proveedor.Pro_Catalogo where Tabla in (@v_max) and Codigo in ('AI','AS')

insert into Proveedor.Pro_Catalogo values
(@v_max, 'AI', 'PARCIALMENTE APROBADO', 'A', NULL),
(@v_max, 'AS', 'PARCIALMENTE APROBADO', 'A', NULL)






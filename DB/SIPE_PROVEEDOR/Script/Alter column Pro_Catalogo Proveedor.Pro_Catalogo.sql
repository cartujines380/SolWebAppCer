

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Catalogo]
	ALTER COLUMN detalle varchar(max);
------------------------------------------------------------------
  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD FechaNacimiento datetime;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD Nacionalidad varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD PaisResidencia varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD EstadoCivil varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeTipoIdentificacion varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeIdentificacion varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeNombres varchar(100);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeApellidos varchar(100);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeFechaNac datetime;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD ConyugeNacionalidad varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD RegimenMatrimonial varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD RelacionDependencia varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD AntiguedadLaboral varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD TipoIngreso varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD IngresoMensual varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_SolContacto]
	ADD TipoParticipante varchar(10);
------------------------------------------------------------------
  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD FechaNacimiento datetime;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD Nacionalidad varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD PaisResidencia varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD EstadoCivil varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeTipoIdentificacion varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeIdentificacion varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeNombres varchar(100);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeApellidos varchar(100);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeFechaNac datetime;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD ConyugeNacionalidad varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD RegimenMatrimonial varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD RelacionDependencia varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD AntiguedadLaboral varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD TipoIngreso varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD IngresoMensual varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_Contacto]
	ADD TipoParticipante varchar(10);
-----------------------------------------------------------------  
----------------------------------------------------------------
 ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD CodActividadEconomica varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD IdProveedor varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [TipoCalificacion] varchar(5);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [TipoServicio] varchar(10);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [RelacionBanco] bit;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [RelacionIdentificacion] varchar(20);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [RelacionNombres] varchar(100);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [RelacionArea] varchar(50);

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [PersonaExpuesta] bit;

  ALTER TABLE [SIPE_PROVEEDOR].[Proveedor].[Pro_ProveedorDetalle]
	ADD [EleccionPopular] bit;
---------------------------------------------------------------

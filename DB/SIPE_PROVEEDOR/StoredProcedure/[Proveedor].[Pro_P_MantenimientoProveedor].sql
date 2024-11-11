USE [SIPE_PROVEEDOR]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [Proveedor].[Pro_P_MantenimientoProveedor] 
GO
CREATE PROCEDURE [Proveedor].[Pro_P_MantenimientoProveedor] 
	 @PI_ParamXML xml
AS
BEGIN try
	begin tran
Declare @IdSolicitud bigint
Declare @IdProveedor varchar(10)
Declare @Ruc varchar(13)
Declare @Identificacion varchar(20)
Declare @Estado varchar(10)
Declare @Accion varchar(10)
Declare @CodProveedor varchar(10)

select top 1  @IdSolicitud=nref.value('@IdSolicitud','bigint')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 

select top 1  @IdProveedor=nref.value('@CodSapProveedor','varchar(10)')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 

select top 1  @Identificacion=nref.value('@Identificacion','varchar(20)')
from @PI_ParamXML.nodes('/Root/Identificacion') AS R(nref) 

select top 1 @Ruc = ruc
from Proveedor.Pro_Proveedor where CodProveedor = @IdProveedor

select top 1  @Estado=nref.value('@Estado','varchar(20)')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 


select top 1  @Accion=nref.value('@ACCION','varchar(20)')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 
print 'I'
if @IdProveedor is not NULL
BEGIN
print '1'
	update a set		
	Ruc						=nref.value('@Identificacion','VARCHAR(13)'),
	NomComercial             =upper(nref.value('@NomComercial','VARCHAR(100)')),		
	TipoProveedor			=nref.value('@GrupoCuenta','VARCHAR(10)'),
	--Idioma                   =nref.value('@Idioma','VARCHAR(2)'),	
	GenDocElec               =case when nref.value('@GenDocElec','VARCHAR(5)')='True' then '01' else ''end,
	DirCalleNum				=nrefT.value('@CallePrincipal','VARCHAR(100)'),	
	DirPisoEdificio			=nrefT.value('@PisoEdificio','VARCHAR(100)'),
	DirCallePrinc			=nrefT.value('@CalleSecundaria','VARCHAR(100)'),			
	DirCodPostal			=nrefT.value('@CodPostal','varchar(30)'),
	Pais					=nrefT.value('@Pais','VARCHAR(100)'),
	Telefono				=nref.value('@TelfFijo','VARCHAR(40)'),
	Extension				=nref.value('@TelfFijoEXT','VARCHAR(4)'),
	CodClaseContribuyente	=nref.value('@ClaseContribuyente','VARCHAR(10)'),
	ApoderadoIdFiscal		=nref.value('@Identificacion','VARCHAR(13)'),
	CorreoE				=nref.value('@EMAILCorp','VARCHAR(100)')
from [Proveedor].[Pro_Proveedor] a 
inner join  @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) on  nref.value('@ACCION','VARCHAR(1)')='U' and a.CodProveedor = nref.value('@CodSapProveedor','VARCHAR(10)')
INNER JOIN  @PI_ParamXML.nodes('/Root/SolDireccion') AS T(nrefT) ON nref.value('@CodSapProveedor','VARCHAR(10)')=nrefT.value('@CodProveedor','VARCHAR(10)')
where nref.value('@ACCION','VARCHAR(1)')='U'

print '2'
	if exists(SELECT top 1 1 from [Proveedor].[Pro_ProveedorDetalle] where IdProveedor = @IdProveedor)
	begin
	print '3'
		update a set
			EsCritico                =nref.value('@EsCritico','CHAR(1)'),
			ProcesoBrindaSoporte	 =nref.value('@ProcesoBrindaSoporte','VARCHAR(850)'),
			Sgs						 =nref.value('@Sgs','VARCHAR(10)'),
			TipoCalificacion		 =nref.value('@TipoCalificacion','VARCHAR(5)'),
			Calificacion             =nref.value('@Calificacion','VARCHAR(5)'),
			FecTermCalificacion	     =nref.value('@FecTermCalificacion','VARCHAR(20)'),
			FechaCreacion			 =nref.value('@FechaCreacion','datetime'),
			CodActividadEconomica    =nref.value('@CodActividadEconomica','VARCHAR(10)'),
			TipoServicio			 =nref.value('@TipoServicio','VARCHAR(10)'),
			RelacionBanco			 =nref.value('@Relacion','bit'),
			RelacionIdentificacion	 =nref.value('@IdentificacionR','VARCHAR(20)'),
			RelacionNombres 		 =nref.value('@NomCompletosR','VARCHAR(100)'),
			RelacionArea			 =nref.value('@AreaLaboraR','VARCHAR(50)'),
			PersonaExpuesta			 =nref.value('@PersonaExpuesta','bit'),
			EleccionPopular			 =nref.value('@EleccionPopular','bit')
		from [Proveedor].[Pro_ProveedorDetalle] a
		inner join  @PI_ParamXML.nodes('/Root/SolProveedorDetalle') AS R(nref) on a.IdProveedor = nref.value('@CodProveedor','VARCHAR(10)')

	end
	else
	begin
	print '4'
		insert into [Proveedor].[Pro_ProveedorDetalle]
		(IdProveedor, EsCritico, ProcesoBrindaSoporte, Sgs, TipoCalificacion, Calificacion, FecTermCalificacion, FechaCreacion
		,CodActividadEconomica, TipoServicio, RelacionBanco, RelacionIdentificacion, RelacionNombres, RelacionArea
		,PersonaExpuesta, EleccionPopular)

		select distinct
			@IdProveedor,
			EsCritico                =nref.value('@EsCritico','CHAR(1)'),
			ProcesoBrindaSoporte	 =nref.value('@ProcesoBrindaSoporte','VARCHAR(850)'),
			Sgs						 =nref.value('@Sgs','VARCHAR(10)'),
			TipoCalificacion		 =nref.value('@TipoCalificacion','VARCHAR(5)'),
			Calificacion             =nref.value('@Calificacion','VARCHAR(5)'),
			FecTermCalificacion	     =nref.value('@FecTermCalificacion','VARCHAR(20)'),
			FechaCreacion			 =nref.value('@FechaCreacion','datetime'),
			CodActividadEconomica    =nref.value('@CodActividadEconomica','VARCHAR(10)'),
			TipoServicio			 =nref.value('@TipoServicio','VARCHAR(10)'),
			RelacionBanco			 =nref.value('@Relacion','bit'),
			RelacionIdentificacion	 =nref.value('@IdentificacionR','VARCHAR(20)'),
			RelacionNombres 		 =nref.value('@NomCompletosR','VARCHAR(100)'),
			RelacionArea			 =nref.value('@AreaLaboraR','VARCHAR(50)'),
			PersonaExpuesta			 =nref.value('@PersonaExpuesta','bit'),
			EleccionPopular			 =nref.value('@EleccionPopular','bit')
		from @PI_ParamXML.nodes('/Root/SolProveedorDetalle') R(nref)
	end
	print '5'
	delete a  from [Proveedor].[Pro_Contacto] a
	where a.CodProveedor=@IdProveedor
	print '6'
	insert into [Proveedor].[Pro_Contacto]
	(
	   CodProveedor, TipoIdentificacion, Identificacion,   Nombre2, Nombre1
	,Apellido2     ,Apellido1          ,CodSapContacto   ,PreFijo, DepCliente
	,Departamento  ,Funcion            ,RepLegal         ,Estado,NotElectronica, NotTransBancaria,
	ext, telefono , celular, correo--)
	, FechaNacimiento, Nacionalidad, PaisResidencia, EstadoCivil, ConyugeTipoIdentificacion
	, ConyugeIdentificacion, ConyugeNombres, RegimenMatrimonial, RelacionDependencia
	, AntiguedadLaboral, TipoIngreso, IngresoMensual, ConyugeApellidos, ConyugeFechaNac
	, ConyugeNacionalidad, TipoParticipante)
	select distinct
		@IdProveedor,
		TipoIdentificacion   =nref.value('@TipoIdentificacion','varchar(10)'),
		Identificacion       =nref.value('@Identificacion','varchar(13)'),
		Nombre2              =upper(nref.value('@Nombre2','varchar(35)')),
		Nombre1              =upper(nref.value('@Nombre1','varchar(35)')),
		Apellido2            =upper(nref.value('@Apellido2','varchar(35)')),
		Apellido1            =upper(nref.value('@Apellido1','varchar(35)')),
		CodSapContacto       =nref.value('@CodSapContacto','varchar(15)'),
		PreFijo              =nref.value('@PreFijo','varchar(30)'),
		DepCliente           =nref.value('@DepCliente','varchar(12)'),
		Departamento         =nref.value('@Departamento','varchar(4)'),
		Funcion              =nref.value('@Funcion','varchar(2)'),
		RepLegal             =convert(bit,nref.value('@RepLegal','varchar(10)')),
		Estado               =nref.value('@Estado','bit'),
		NotElectronica       =nref.value('@NotElectronica','bit'),
		NotTransBancaria     =nref.value('@NotTransBancaria','bit'),
		ext                  =nref.value('@TelfFijoEXT','varchar(5)'),
		Telefono					=nref.value('@TelfFijo','varchar(35)'),
		Celular						=nref.value('@Celular','varchar(35)'),
		Correo						=nref.value('@EMAIL','varchar(35)'),
		FechaNacimiento				=nref.value('@FechaNacimiento','datetime'),
		Nacionalidad				=nref.value('@Nacionalidad','varchar(10)'),
		PaisResidencia				=nref.value('@Residencia','varchar(20)'),
		EstadoCivil					=nref.value('@EstadoCivil','varchar(20)'),
		ConyugeTipoIdentificacion	=nref.value('@ConyugeTipoIdentificacion','varchar(10)'),
		ConyugeIdentificacion		=nref.value('@ConyugeIdentificacion','varchar(20)'),
		ConyugeNombres				=nref.value('@ConyugeNombres','varchar(100)'),
		RegimenMatrimonial			=nref.value('@RegimenMatrimonial','varchar(20)'),
		RelacionDependencia			=nref.value('@RelacionDependencia','varchar(20)'),
		AntiguedadLaboral			=nref.value('@AntiguedadLaboral','varchar(10)'),
		TipoIngreso					=nref.value('@TipoIngreso','varchar(20)'),
		IngresoMensual				=nref.value('@IngresoMensual','varchar(10)'),
		ConyugeApellidos            =nref.value('@ConyugeApellidos','varchar(100)'),
		ConyugeFechaNac             =nref.value('@ConyugeFechaNac','datetime'),
		ConyugeNacionalidad         =nref.value('@ConyugeNacionalidad','varchar(10)'),
		TipoParticipante		    =nref.value('@TipoParticipante','varchar(10)')
	from @PI_ParamXML.nodes('/Root/SolContacto') R(nref)
	print '7'
	delete a  from [Proveedor].[Pro_ContactoDepartamento] a
	inner join [Proveedor].[Pro_ContactoProveedor] c on c.IdContacto = a.IdContacto
	where c.CodProveedor=@IdProveedor
	print '8'
	delete a  from [Proveedor].[Pro_ContactoProveedor] a
	where a.CodProveedor=@IdProveedor
	print '9'
	insert into [Proveedor].[Pro_ContactoProveedor]
	(
	   CodProveedor  ,TipoIdentificacion ,Identificacion  ,Nombre2,  Nombre1
	  ,Apellido2     ,Apellido1          ,PreFijo         ,Estado   ,TelfFijo
	  ,TelfFijoEXT,   TelfMovil          ,email           ,NotElectronica
	  ,NotTransBancaria,  RecActas,       RepLegal,        FechaRegistro )
	select distinct
		@IdProveedor,
		TipoIdentificacion   =nref.value('@TipoIdentificacion','varchar(10)'),
		Identificacion       =nref.value('@Identificacion','varchar(13)'),
		Nombre2              =upper(nref.value('@Nombre2','varchar(35)')),
		Nombre1              =upper(nref.value('@Nombre1','varchar(35)')),
		Apellido2            =upper(nref.value('@Apellido2','varchar(35)')),
		Apellido1            =upper(nref.value('@Apellido1','varchar(35)')),	
		PreFijo              =nref.value('@PreFijo','varchar(30)'),
		Estado               ='1',
		Telefono		     =nref.value('@TelfFijo','varchar(35)'),
		ext                  =nref.value('@TelfFijoEXT','varchar(5)'),
		Celular				 =nref.value('@TelfMovil','varchar(35)'),
		Correo				 =nref.value('@EMAIL','varchar(35)'),
		NotElectronica       =nref.value('@NotElectronica','bit'),
		NotTransBancaria     =nref.value('@NotTransBancaria','bit'),
		0,
		RepLegal             =convert(bit,nref.value('@RepLegal','varchar(10)')),
		FechaRegistro        =getdate()
	from @PI_ParamXML.nodes('/Root/SolContacto') R(nref)
	print '10'

	insert into [Proveedor].[Pro_ContactoDepartamento]
	(IdContacto, Identificacion, CodDepartamento, CodFuncion, Estado)
	select distinct
	IdContacto           =a.IdContacto,
	Identificacion       = nref.value('@Identificacion','varchar(13)'),
	Departamento         =nref.value('@Departamento','varchar(4)'),
	Funcion              =nref.value('@Funcion','varchar(2)'),
	Estado               ='1'
	from [Proveedor].Pro_ContactoProveedor a
	inner  join @PI_ParamXML.nodes('/Root/SolContacto')  R(nref) on a.CodProveedor= @IdProveedor and a.Identificacion= nref.value('@Identificacion','varchar(13)') 
	print '11'
	delete a  from [Proveedor].[Pro_Direccion] a
	where a.CodProveedor=@IdProveedor
	print '12'
	insert into [Proveedor].Pro_Direccion
	(
	CodProveedor,     Pais,          Provincia,   Ciudad, CallePrincipal,
	CalleSecundaria, PisoEdificio,  CodPostal,   Solar,  Estado)
	select distinct
		@IdProveedor,
		Pais                =nref.value('@Pais','varchar(10)'),
		Provincia			=nref.value('@Provincia','varchar(10)'),
		Ciudad				=nref.value('@Ciudad','varchar(12)'),
		CallePrincipal		=upper(nref.value('@CallePrincipal','varchar(100)')),
		CalleSecundaria		=upper(nref.value('@CalleSecundaria','varchar(100)')),
		PisoEdificio		=nref.value('@PisoEdificio','varchar(10)'),
		CodPostal			=nref.value('@CodPostal','varchar(30)'),
		Solar				=nref.value('@Solar','varchar(30)'),	
		Estado				=nref.value('@Estado','bit')
	from @PI_ParamXML.nodes('/Root/SolDireccion') R(nref)
	print '13'
	delete a  from [Proveedor].[Pro_DocAdjunto] a
	where a.CodProveedor=@IdProveedor
	print '14'
	insert into [Proveedor].[Pro_DocAdjunto]
		(CodProveedor, CodDocumento, NomArchivo,Archivo,FechaCarga,Estado)
		select 
			distinct
			@IdProveedor,
			CodDocumento =nref.value('@CodDocumento','varchar(10)'),
			NomArchivo   =nref.value('@NomArchivo','varchar(255)'),
			Archivo      =nref.value('@Archivo','varchar(255)'),
			FechaCarga   =getdate(),
			Estado       =nref.value('@Estado','bit')
		from @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref)
		print '15'
		delete a  from [Proveedor].[Pro_MedioContacto] a
		where a.CodProveedor=@IdProveedor
		print '16'
		insert into [Proveedor].[Pro_MedioContacto]
		(CodProveedor,IdContacto,TipMedioContacto,ValorMedioContacto,Estado)
		select distinct
			@IdProveedor,
			null,
			TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
			ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
			Estado                  =nref.value('@Estado','bit')	
		from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
		where nref.value('@Contacto','varchar(1)')='N'  and
		not exists(select top 1 1 from [Proveedor].[Pro_MedioContacto] M where m.CodProveedor=@IdProveedor and  isnull(m.IdContacto,0)=0 and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))
		print '17'
		insert into [Proveedor].[Pro_MedioContacto]
		(CodProveedor,IdContacto,TipMedioContacto,ValorMedioContacto,Estado)
		select distinct
			@IdProveedor,
			c.Id,
			TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
			ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
			Estado                  =nref.value('@Estado','bit')	
		from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
		  inner join Proveedor.Pro_Contacto c on  nref.value('@Identificacion','varchar(13)')= c.Identificacion and  c.CodProveedor=@IdProveedor
		where nref.value('@Contacto','varchar(1)')='S' and
		not exists(select top 1 1 from [Proveedor].[Pro_MedioContacto] M where m.CodProveedor=c.CodProveedor and  c.Id=m.IdContacto and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))
		print '18'


		delete a  from [Proveedor].[Pro_ProveedorFrmPago] a
	    where a.CodProveedor=@IdProveedor


		Insert into [Proveedor].[Pro_ProveedorFrmPago]
		(CodProveedor,  Extrangera     ,CodSapBanco   ,Pais      ,TipoCuenta
		,NumeroCuenta, TitularCuenta,  ReprCuenta    ,CodSwift , CodBENINT
		,CodABA        ,Principal     ,Estado,Provincia,DirBancoExtranjero,BancoExtranjero, FormaPago)

		select distinct
		@IdProveedor,
		nref.value('@Extrangera','bit'),  nref.value('@CodSapBanco','varchar(15)'), nref.value('@Pais','varchar(10)'), nref.value('@TipoCuenta','varchar(3)'),
		nref.value('@NumeroCuenta','varchar(18)'),upper(nref.value('@TitularCuenta','varchar(60)')),upper(nref.value('@ReprCuenta','varchar(200)')),nref.value('@CodSwift','varchar(10)'),nref.value('@CodBENINT','varchar(10)'),
		nref.value('@CodABA','varchar(10)'),nref.value('@Principal','bit'),nref.value('@Estado','bit'),
		nref.value('@Provincia','varchar(10)'),
		upper(nref.value('@DirBancoExtranjero','varchar(250)')),
		upper(nref.value('@BancoExtranjero','varchar(250)')),
		nref.value('@FormaPago','varchar(10)')
		from @PI_ParamXML.nodes('/Root/SolBanco') R(nref)
END


IF @@TRANCOUNT > 0
			COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Pro_P_MantenimientoProveedor]'
END CATCH
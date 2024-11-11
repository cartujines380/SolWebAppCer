

CREATE PROCEDURE [Proveedor].[Pro_P_GrabaSolProveedor] 
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

select top 1  @IdProveedor=nref.value('@CodProveedor','varchar(10)')
from @PI_ParamXML.nodes('/Root/Contacto') AS R(nref) 

select top 1  @Identificacion=nref.value('@Identificacion','varchar(20)')
from @PI_ParamXML.nodes('/Root/Identificacion') AS R(nref) 

select top 1 @Ruc = ruc
from Proveedor.Pro_Proveedor where CodProveedor = @IdProveedor



select top 1  @Estado=nref.value('@Estado','varchar(20)')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 


select top 1  @Accion=nref.value('@ACCION','varchar(20)')
from @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 



select @CodProveedor=max(CodProveedor)+1
from Proveedor.Pro_Proveedor


if @IdProveedor is not NULL
begin

delete [Proveedor].[Pro_Contacto] where CodProveedor = @IdProveedor

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
	CodProveedor   =nref.value('@CodProveedor','varchar(10)'),
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
	Celular						=nref.value('@TelfMovil','varchar(35)'),
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
from @PI_ParamXML.nodes('/Root/Contacto') R(nref)

-- detalle proveedor
delete [Proveedor].[Pro_ProveedorDetalle] where IdProveedor = @IdProveedor

insert into [Proveedor].[Pro_ProveedorDetalle]
(IdProveedor, EsCritico, ProcesoBrindaSoporte, Sgs, TipoCalificacion, Calificacion, FecTermCalificacion, FechaCreacion
,CodActividadEconomica, TipoServicio, RelacionBanco, RelacionIdentificacion, RelacionNombres, RelacionArea
,PersonaExpuesta, EleccionPopular)

select distinct	
	IdProveedor              =nref.value('@IdProveedor','varchar(10)'),
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

else
begin
if exists	( SELECT top  1 	1
		FROM @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) where nref.value('@ACCION','VARCHAR(1)')='I')
		begin

		insert into [Proveedor].[Pro_SolProveedor](
						 IdEmpresa                      ,TipoSolicitud    ,TipoProveedor
						,CodSapProveedor       ,TipoIdentificacion  ,Identificacion   ,NomComercial
						,RazonSocial           ,FechaSRI            ,SectorComercial  ,Idioma
						,CodGrupoProveedor     ,GenDocElec          ,FechaSolicitud   ,Estado
						,GrupoTesoreria        ,CuentaAsociada      ,Autorizacion     ,TransfArticuProvAnterior
						,DepSolicitando        ,Responsable         ,Aprobacion       ,Comentario,ClaseContribuyente 
						,AnioConsti            ,LineaNegocio        ,princliente      ,totalventas,
						PlazoEntrega,DespachaProvincia,	GrupoCuenta,RetencionIva,RetencionIva2,RetencionFuente,RetencionFuente2,CondicionPago, GrupoCompra, GrupoEsquema )
						--, ActividadEconomica, TipoServicio, RelacionBanco, RelacionIdentificacion, RelacionNombres, RelacionArea, FechaCreacion )

		select  nref.value('@IdEmpresa','int'),nref.value('@TipoSolicitud','VARCHAR(10)'),nref.value('@TipoProveedor','VARCHAR(10)'),
				nref.value('@CodSapProveedor','VARCHAR(10)'),nref.value('@TipoIdentificacion','VARCHAR(10)'),nref.value('@Identificacion','VARCHAR(13)'),upper(nref.value('@NomComercial','VARCHAR(35)')),
				upper(nref.value('@RazonSocial','VARCHAR(100)')),CONVERT(DATETIME, nref.value('@FechaSRI','varchar(10)'),103),upper( nref.value('@SectorComercial','VARCHAR(3)')),nref.value('@Idioma','VARCHAR(2)'),
				nref.value('@CodGrupoProveedor','VARCHAR(13)'),case when nref.value('@GenDocElec','VARCHAR(5)')='True' then '01' else ''end,GETDATE(),nref.value('@Estado','VARCHAR(10)'),
				nref.value('@GrupoTesoreria','VARCHAR(10)'),nref.value('@CuentaAsociada','VARCHAR(20)'),
				nref.value('@Autorizacion','VARCHAR(10)'),nref.value('@TransfArticuProvAnterior','VARCHAR(13)'),
				nref.value('@DepSolicitando','VARCHAR(100)'),upper(nref.value('@Responsable','varchar(100)')),nref.value('@Aprobacion','VARCHAR(100)'),
				upper(nref.value('@Comentario','VARCHAR(500)')),nref.value('@ClaseContribuyente','VARCHAR(10)'),
				nref.value('@AnioConsti','VARCHAR(4)'),
				nref.value('@LineaNegocio','VARCHAR(10)'),upper(nref.value('@princliente','VARCHAR(500)')),
				nref.value('@totalventas','VARCHAR(10)'),
				nref.value('@PlazoEntrega','bigint'),
				nref.value('@DespachaProvincia','VARCHAR(10)'),
				nref.value('@GrupoCuenta','VARCHAR(10)'),
				nref.value('@RetencionIva','VARCHAR(10)'),
				nref.value('@RetencionIva2','VARCHAR(10)'),
				nref.value('@RetencionFuente','VARCHAR(10)'),
				nref.value('@RetencionFuente2','VARCHAR(10)'),
				nref.value('@CondicionPago','VARCHAR(10)'),
				nref.value('@GrupoCompra','VARCHAR(3)'),
				nref.value('@GrupoEsquema','VARCHAR(2)')
		FROM @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) where nref.value('@ACCION','VARCHAR(1)')='I'

	select @IdSolicitud=@@IDENTITY



end 

update a set

	TipoSolicitud            =nref.value('@TipoSolicitud','VARCHAR(10)'),
	TipoProveedor            =nref.value('@TipoProveedor','VARCHAR(10)'),
	CodSapProveedor          =nref.value('@CodSapProveedor','VARCHAR(10)'),
	TipoIdentificacion       =nref.value('@TipoIdentificacion','VARCHAR(10)'),
	Identificacion           =nref.value('@Identificacion','VARCHAR(13)'),
	NomComercial             =upper(nref.value('@NomComercial','VARCHAR(35)')),
	RazonSocial              =upper(nref.value('@RazonSocial','VARCHAR(100)')),
	FechaSRI                 =CONVERT(DATETIME, nref.value('@FechaSRI','varchar(10)'),103), 
	SectorComercial          =nref.value('@SectorComercial','VARCHAR(3)'),
	Idioma                   =nref.value('@Idioma','VARCHAR(2)'),
	CodGrupoProveedor        =nref.value('@CodGrupoProveedor','VARCHAR(13)'),
	GenDocElec               =case when nref.value('@GenDocElec','VARCHAR(5)')='True' then '01' else ''end,
	Estado                   =nref.value('@Estado','VARCHAR(10)'),
	GrupoTesoreria           =nref.value('@GrupoTesoreria','VARCHAR(10)'),
	CuentaAsociada           =nref.value('@CuentaAsociada','VARCHAR(20)') , 
	Autorizacion             =nref.value('@Autorizacion','VARCHAR(10)'),
	TransfArticuProvAnterior =nref.value('@TransfArticuProvAnterior','VARCHAR(13)'),
	DepSolicitando           =nref.value('@DepSolicitando','VARCHAR(100)'),
	Responsable              =upper(nref.value('@Responsable','varchar(100)')),
	Aprobacion               =nref.value('@Aprobacion','VARCHAR(100)'),
	Comentario               =upper(nref.value('@Comentario','VARCHAR(500)')),
	AnioConsti               =nref.value('@AnioConsti','VARCHAR(4)'),
	LineaNegocio             =nref.value('@LineaNegocio','VARCHAR(10)'),
	princliente              =upper(nref.value('@princliente','VARCHAR(500)')),
	totalventas              =nref.value('@totalventas','VARCHAR(10)'),
	PlazoEntrega             =nref.value('@PlazoEntrega','bigint'),
	DespachaProvincia        =nref.value('@DespachaProvincia','VARCHAR(10)'),
	GrupoCuenta              =nref.value('@GrupoCuenta','VARCHAR(10)'),
	RetencionIva             =nref.value('@RetencionIva','VARCHAR(10)'),
	RetencionIva2            =nref.value('@RetencionIva2','VARCHAR(10)'),
	RetencionFuente          =nref.value('@RetencionFuente','VARCHAR(10)'),
	RetencionFuente2         =nref.value('@RetencionFuente2','VARCHAR(10)'),
	CondicionPago            =nref.value('@CondicionPago','VARCHAR(10)'),		
	ClaseContribuyente       =nref.value('@ClaseContribuyente','VARCHAR(10)'),
	GrupoCompra              =nref.value('@GrupoCompra','VARCHAR(3)'),
	GrupoEsquema             =nref.value('@GrupoEsquema','VARCHAR(2)')	
	
from [Proveedor].[Pro_SolProveedor] a 
inner join  @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) on  nref.value('@ACCION','VARCHAR(1)')='U' and a.IdSolicitud= nref.value('@IdSolicitud','bigint')




if((@Estado='AP' OR @Estado='AC')  AND @Accion='U')
BEGIN
print 'insertar proveedor'
	insert into Proveedor.Pro_Proveedor
	(
		CodProveedor,
		Ruc,
		TipoProveedor,
		NomComercial,
		DirCalleNum,
		DirPisoEdificio,
		DirCallePrinc,
		DirDistrito,
		DirCodPostal,
		Poblacion,
		Pais,
		Region,
		Idioma,
		Telefono,
		Movil,
		Fax,
		CorreoE,
		GenDocElec,
		FechaCertifica,
		IndMinoria,
		ApoderadoNom,
		ApoderadoApe,
		ApoderadoIdFiscal,
		PlazoEntregaPrev,
		FechaMod,
		CodClaseContribuyente,
		Extension
	)
	select	@CodProveedor, 
			nref.value('@Identificacion','VARCHAR(13)'),
			nref.value('@GrupoCuenta','VARCHAR(10)'),
			upper(nref.value('@RazonSocial','VARCHAR(100)')),
			nrefT.value('@CallePrincipal','VARCHAR(100)'),
			nrefT.value('@PisoEdificio','VARCHAR(100)'),
			nrefT.value('@CalleSecundaria','VARCHAR(100)'),			
			null,
			nrefT.value('@CodPostal','varchar(30)'),
			'GUAYAQUIL',
			nrefT.value('@Pais','VARCHAR(100)'),
			'02',
			'S',
			nref.value('@TelfFijo','VARCHAR(40)'),
			'',
			'',
			'',
			null,
			getdate(),
			'',
			'',
			'',
			nref.value('@Identificacion','VARCHAR(13)'),
			null,
			getdate(),
			nref.value('@ClaseContribuyente','VARCHAR(10)'),
			nref.value('@TelfFijoEXT','VARCHAR(4)')
		FROM @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) 
		INNER JOIN  @PI_ParamXML.nodes('/Root/SolDireccion') AS T(nrefT) 
		ON nref.value('@IdSolicitud','VARCHAR(10)')=nrefT.value('@IdSolicitud','VARCHAR(10)')
		where nref.value('@ACCION','VARCHAR(1)')='U'
		
		print 'insertar contacto'

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
	@CodProveedor,
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

update [Proveedor].[Pro_SolProveedor] set CodSapProveedor = @CodProveedor where IdSolicitud = @IdSolicitud


insert into [Proveedor].[Pro_ContactoProveedor]
(
   CodProveedor  ,TipoIdentificacion ,Identificacion  ,Nombre2,  Nombre1
  ,Apellido2     ,Apellido1          ,PreFijo         ,Estado   ,TelfFijo
  ,TelfFijoEXT,   TelfMovil          ,email           ,NotElectronica
  ,NotTransBancaria,  RecActas,       RepLegal,        FechaRegistro )
select distinct
	@CodProveedor,
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

insert into [Proveedor].[Pro_ContactoDepartamento]
(IdContacto, Identificacion, CodDepartamento, CodFuncion, Estado)
select distinct
IdContacto           =a.IdContacto,
Identificacion       = nref.value('@Identificacion','varchar(13)'),
Departamento         =nref.value('@Departamento','varchar(4)'),
Funcion              =nref.value('@Funcion','varchar(2)'),
Estado               ='1'
from [Proveedor].Pro_ContactoProveedor a
inner  join @PI_ParamXML.nodes('/Root/SolContacto')  R(nref) on a.CodProveedor= @CodProveedor and a.Identificacion= nref.value('@Identificacion','varchar(13)') 

print 'insertar detalle'

insert into [Proveedor].[Pro_ProveedorDetalle]
(IdProveedor, EsCritico, ProcesoBrindaSoporte, Sgs, TipoCalificacion, Calificacion, FecTermCalificacion, FechaCreacion
,CodActividadEconomica, TipoServicio, RelacionBanco, RelacionIdentificacion, RelacionNombres, RelacionArea
,PersonaExpuesta, EleccionPopular)
select distinct	
	@CodProveedor,
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

insert into [Proveedor].Pro_Direccion
(
CodProveedor,     Pais,          Provincia,   Ciudad, CallePrincipal,
CalleSecundaria, PisoEdificio,  CodPostal,   Solar,  Estado)
select distinct
	@CodProveedor,
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

insert into [Proveedor].[Pro_DocAdjunto]
	(CodProveedor, CodDocumento, NomArchivo,Archivo,FechaCarga,Estado)
	select 
		distinct
		@CodProveedor,
		CodDocumento =nref.value('@CodDocumento','varchar(10)'),
	    NomArchivo   =nref.value('@NomArchivo','varchar(255)'),
	    Archivo      =nref.value('@Archivo','varchar(255)'),
	    FechaCarga   =getdate(),
	    Estado       =nref.value('@Estado','bit')
	from @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref)
	


insert into [Proveedor].[Pro_MedioContacto]
	(CodProveedor,IdContacto,TipMedioContacto,ValorMedioContacto,Estado)
	select distinct
		@CodProveedor,
		null,
	    TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
	    ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado                  =nref.value('@Estado','bit')	
	from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
	where nref.value('@Contacto','varchar(1)')='N'  and
	not exists(select top 1 1 from [Proveedor].[Pro_MedioContacto] M where m.CodProveedor=@CodProveedor and  isnull(m.IdContacto,0)=0 and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))

	insert into [Proveedor].[Pro_MedioContacto]
	(CodProveedor,IdContacto,TipMedioContacto,ValorMedioContacto,Estado)
	select distinct
		@CodProveedor,
		c.Id,
	    TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
	    ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado                  =nref.value('@Estado','bit')	
	from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
	  inner join Proveedor.Pro_Contacto c on  nref.value('@Identificacion','varchar(13)')= c.Identificacion and  c.CodProveedor=@CodProveedor
	where nref.value('@Contacto','varchar(1)')='S' and
	not exists(select top 1 1 from [Proveedor].[Pro_MedioContacto] M where m.CodProveedor=c.CodProveedor and  c.Id=m.IdContacto and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))


END
/*************************Banco*************************************************/

delete a  from [Proveedor].[Pro_SolBanco] a
where a.IdSolicitud=@IdSolicitud


Insert into [Proveedor].[Pro_SolBanco]
(IdSolicitud,  Extrangera     ,CodSapBanco   ,Pais      ,TipoCuenta
,NumeroCuenta, TitularCuenta,  ReprCuenta    ,CodSwift , CodBENINT
,CodABA        ,Principal     ,Estado,Provincia,DirBancoExtranjero,BancoExtranjero)

select distinct
(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
nref.value('@Extrangera','bit'),  nref.value('@CodSapBanco','varchar(15)'), nref.value('@Pais','varchar(10)'), nref.value('@TipoCuenta','varchar(3)'),
nref.value('@NumeroCuenta','varchar(18)'),upper(nref.value('@TitularCuenta','varchar(60)')),upper(nref.value('@ReprCuenta','varchar(200)')),nref.value('@CodSwift','varchar(10)'),nref.value('@CodBENINT','varchar(10)'),
nref.value('@CodABA','varchar(10)'),nref.value('@Principal','bit'),nref.value('@Estado','bit'),
nref.value('@Provincia','varchar(10)'),
upper(nref.value('@DirBancoExtranjero','varchar(250)')),
upper(nref.value('@BancoExtranjero','varchar(250)'))

from @PI_ParamXML.nodes('/Root/SolBanco') R(nref)

/*************************Proveedor Detalle*************************/
delete a from [Proveedor].[Pro_SolProveedorDetalle] a where a.IdSolicitud = @IdSolicitud

insert into [Proveedor].[Pro_SolProveedorDetalle]
(IdSolicitud, EsCritico, ProcesoBrindaSoporte, Sgs, TipoCalificacion, Calificacion, FecTermCalificacion, FechaCreacion
,CodActividadEconomica, TipoServicio, RelacionBanco, RelacionIdentificacion, RelacionNombres, RelacionArea
,PersonaExpuesta, EleccionPopular)

select distinct
	(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
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

/*****************************Contacto******************************/

delete 
Proveedor.Pro_SolMedioContacto where IdSolicitud=@IdSolicitud and IdSolContacto is not null

delete a  from [Proveedor].[Pro_SolContacto] a
where a.idsolicitud=@IdSolicitud

insert into [Proveedor].[Pro_SolContacto]
(
 IdSolicitud,   TipoIdentificacion, Identificacion,   Nombre2, Nombre1
,Apellido2     ,Apellido1          ,CodSapContacto   ,PreFijo, DepCliente
,Departamento  ,Funcion            ,RepLegal         ,Estado,NotElectronica, NotTransBancaria--)
, FechaNacimiento, Nacionalidad, PaisResidencia, EstadoCivil, ConyugeTipoIdentificacion
, ConyugeIdentificacion, ConyugeNombres, RegimenMatrimonial, RelacionDependencia
, AntiguedadLaboral, TipoIngreso, IngresoMensual, ConyugeApellidos, ConyugeFechaNac, ConyugeNacionalidad
, TipoParticipante)

select distinct
	(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
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

/*****************************Direccion******************************/
--delete a  from [Proveedor].[Pro_SolDireccion] a
--inner  join @PI_ParamXML.nodes('/Root/SolDireccion')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdDireccion= nref.value('@IdDireccion','bigint') and
--  nref.value('@ACCION','VARCHAR(1)')='D'


--update a set
--	Pais                =nref.value('@Pais','varchar(10)'),
--	Provincia			=nref.value('@Provincia','varchar(10)'),
--	Ciudad				=nref.value('@Ciudad','varchar(12)'),
--	CallePrincipal		=upper(nref.value('@CallePrincipal','varchar(100)')),
--	CalleSecundaria		=upper(nref.value('@CalleSecundaria','varchar(100)')),
--	PisoEdificio		=nref.value('@PisoEdificio','varchar(10)'),
--	CodPostal			=nref.value('@CodPostal','varchar(30)'),
--	Solar				=nref.value('@Solar','varchar(30)'),	
--	Estado				=nref.value('@Estado','bit')
--from [Proveedor].Pro_SolDireccion a
--inner  join @PI_ParamXML.nodes('/Root/SolDireccion')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdDireccion= nref.value('@IdDireccion','bigint') and
-- nref.value('@ACCION','VARCHAR(1)')='U'

delete from [Proveedor].[Pro_SolDireccion] where IdSolicitud=@IdSolicitud and IdDireccion is not null 

insert into [Proveedor].Pro_SolDireccion
(
IdSolicitud,     Pais,          Provincia,   Ciudad, CallePrincipal,
CalleSecundaria, PisoEdificio,  CodPostal,   Solar,  Estado)


select distinct
	(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
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
--where nref.value('@ACCION','VARCHAR(1)')='I'



/*****************************SolZona******************************/
	delete a  from [Proveedor].[Pro_SolZona] a
	inner  join @PI_ParamXML.nodes('/Root/SolZona')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.CodZona= nref.value('@CodZona','varchar(12)') and
	  nref.value('@ACCION','VARCHAR(1)')='D'


	update a set
	
		Estado				=nref.value('@Estado','bit')
	from [Proveedor].[Pro_SolZona] a
	inner  join @PI_ParamXML.nodes('/Root/SolZona')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.CodZona= nref.value('@CodZona','varchar(12)') and
	 nref.value('@ACCION','VARCHAR(1)')='U'

	insert into [Proveedor].Pro_SolZona
	(IdSolicitud,     CodZona,             Estado)


	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		CodZona                =nref.value('@CodZona','varchar(12)'),
		Estado				   =nref.value('@Estado','bit')
	from @PI_ParamXML.nodes('/Root/SolZona') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'

/*****************************SolLineaNegocio******************************/
	delete a  from [Proveedor].[Pro_SolLineaNegocio] a
	inner  join @PI_ParamXML.nodes('/Root/SolLineaNegocio')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdLIneNegocio= nref.value('@IdLIneNegocio','bigint') and
	  nref.value('@ACCION','VARCHAR(1)')='D'


	update a set
		CodigoSociedad =nref.value('@CodigoSociedad','varchar(10)'),
		CodigoSeccion =nref.value('@CodigoSeccion','varchar(3)')
	from [Proveedor].[Pro_SolLineaNegocio] a
	inner  join @PI_ParamXML.nodes('/Root/SolLineaNegocio')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdLIneNegocio= nref.value('@IdLIneNegocio','bigint') and
	 nref.value('@ACCION','VARCHAR(1)')='U'

	insert into [Proveedor].Pro_SolLineaNegocio
	(IdSolicitud,     CodigoSociedad,          CodigoSeccion)


	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		CodigoSociedad =nref.value('@CodigoSociedad','varchar(10)'),
		CodigoSeccion =nref.value('@CodigoSeccion','varchar(3)')
	from @PI_ParamXML.nodes('/Root/SolLineaNegocio') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'

/*****************************SolDocAdjunto******************************/

	;WITH cte_SolDocAdjunto 
	as (
			SELECT 
				a.IdSolDocAdjunto
			FROM [Proveedor].[Pro_SolDocAdjunto] as a
			WHERE a.IdSolicitud = @IdSolicitud
		),cte_SolDocAdjuntoEliminar 
		as (
			SELECT a.IdSolDocAdjunto
			FROM cte_SolDocAdjunto as a
			WHERE NOT EXISTS (SELECT nref.value('@IdSolDocAdjunto','bigint')
							  FROM @PI_ParamXML.nodes('/Root/SolDocAdjunto') as R(nref)
							  where nref.value('@IdSolDocAdjunto','bigint') = a.IdSolDocAdjunto)
			)

	DELETE da
	FROM [Proveedor].[Pro_SolDocAdjunto] da
	WHERE da.IdSolDocAdjunto in (select *from cte_SolDocAdjuntoEliminar)

	update a set
		CodDocumento =nref.value('@CodDocumento','varchar(10)'),
	    NomArchivo   =nref.value('@NomArchivo','varchar(255)'),
	    Archivo      =nref.value('@Archivo','varchar(255)'),
	    FechaCarga   =getdate(),
	    Estado       =nref.value('@Estado','bit')
	from [Proveedor].[Pro_SolDocAdjunto] a
	inner join @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref) 
		on a.IdSolicitud= nref.value('@IdSolicitud','bigint') 
		and a.IdSolDocAdjunto= nref.value('@IdSolDocAdjunto','bigint') 
		and nref.value('@ACCION','VARCHAR(1)')='U'

	declare @w_cantidadad int
	select 
		@w_cantidadad=count(*) 
	from [Proveedor].[Pro_SolDocAdjunto] as a
	inner join @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref) 
		on a.IdSolicitud=@IdSolicitud
	where nref.value('@ACCION','VARCHAR(1)')='I'

	if(@w_cantidadad > 0)
	begin
		delete a  
		from [Proveedor].[Pro_SolDocAdjunto] as a
		where a.idsolicitud=@IdSolicitud
		and a.IdSolDocAdjunto not in(select nref.value('@IdSolDocAdjunto','bigint') 
									 from @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref) 
									 where nref.value('@ACCION','VARCHAR(1)')='U')
	end

	insert into [Proveedor].[Pro_SolDocAdjunto]
	(IdSolicitud, CodDocumento, NomArchivo,Archivo,FechaCarga,Estado)
	select 
		distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		CodDocumento =nref.value('@CodDocumento','varchar(10)'),
	    NomArchivo   =nref.value('@NomArchivo','varchar(255)'),
	    Archivo      =nref.value('@Archivo','varchar(255)'),
	    FechaCarga   =getdate(),
	    Estado       =nref.value('@Estado','bit')
	from @PI_ParamXML.nodes('/Root/SolDocAdjunto') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'


/*****************************ProveedorLineaNegocio******************************/

IF EXISTS (SELECT top 1 1
		   FROM @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) WHERE nref.value('@Estado','VARCHAR(10)')='AP')
BEGIN
	DELETE FROM Proveedor.CateProveedor 
	WHERE CodProveedor = (SELECT top 1 pr.CodProveedor 
							FROM Proveedor.Pro_Proveedor pr 
							inner join Proveedor.Pro_SolProveedor pl on pr.Ruc = pl.Identificacion
							WHERE pl.IdSolicitud =  @IdSolicitud)

	INSERT INTO Proveedor.CateProveedor
	(CodProveedor, codCategoria)
	SELECT DISTINCT		
		CodProveedor = (SELECT top 1 pr.CodProveedor 
					FROM Proveedor.Pro_Proveedor pr 
						inner join Proveedor.Pro_SolProveedor pl on pr.Ruc = pl.Identificacion
					WHERE pl.IdSolicitud =  @IdSolicitud),
		LineaNegocio =  upper(nref.value('@Codigo','varchar(2)'))
	FROM @PI_ParamXML.nodes('/Root/SolLineasNegocios') R(nref)
END

/*****************************ProveedorLineaNegocio******************************/


/*****************************SolLineaNegocios******************************/

 delete from Proveedor.Pro_SolicitudLineaNegocio where idSolicitud = @IdSolicitud
  
  insert into Proveedor.Pro_SolicitudLineaNegocio
	(IdSolicitud, LineaNegocio,Principal)
	select distinct
		
		IdSolicitud                 =@IdSolicitud,
	    LineaNegocio            =  upper(nref.value('@Codigo','varchar(10)')),
	    Principal                =nref.value('@Principal','varchar(5)')
	from @PI_ParamXML.nodes('/Root/SolLineasNegocios') R(nref)
	
	if exists	( SELECT top  1 	1
		FROM @PI_ParamXML.nodes('/Root/SolProveedor') AS R(nref) where nref.value('@Estado','VARCHAR(10)')in('AP', 'AC'))
		begin
		    
			delete from Proveedor.Pro_LineaNegocio where CodProveedor             = (select top 1 pr.CodProveedor 
											from Proveedor.Pro_Proveedor pr 
											   inner join Proveedor.Pro_SolProveedor pl on pr.Ruc = pl.Identificacion
											where pl.IdSolicitud =  @IdSolicitud)

		    insert into Proveedor.Pro_LineaNegocio
	        (CodProveedor, LineaNegocio,Principal)

			select distinct		
				CodProveedor             = (select top 1 pr.CodProveedor 
											from Proveedor.Pro_Proveedor pr 
											   inner join Proveedor.Pro_SolProveedor pl on pr.Ruc = pl.Identificacion
											where pl.IdSolicitud =  @IdSolicitud),
				LineaNegocio             =  upper(nref.value('@Codigo','varchar(2)')),
				Principal                =nref.value('@Principal','varchar(5)')
			from @PI_ParamXML.nodes('/Root/SolLineasNegocios') R(nref)
		end


/*****************************SolProvHistEstado******************************/

	delete a  from [Proveedor].Pro_SolProvHistEstado a
	inner  join @PI_ParamXML.nodes('/Root/SolProvHistEstado')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdObservacion= nref.value('@IdObservacion','bigint') and
	  nref.value('@ACCION','VARCHAR(1)')='D'


	update a set
		Motivo                 =nref.value('@Motivo','varchar(10)'),
	    Observacion            =upper(nref.value('@Observacion','varchar(500)')),
	    Usuario                =nref.value('@Usuario','varchar(100)'),
	    EstadoSolicitud        =nref.value('@EstadoSolicitud','varchar(10)')
	from [Proveedor].Pro_SolProvHistEstado a
	inner  join @PI_ParamXML.nodes('/Root/SolProvHistEstado')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdObservacion= nref.value('@IdObservacion','bigint') and
	 nref.value('@ACCION','VARCHAR(1)')='U'

	insert into [Proveedor].[Pro_SolProvHistEstado]
	(IdSolicitud, Motivo,Observacion,Usuario,Fecha,EstadoSolicitud)

	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		Motivo                 =nref.value('@Motivo','varchar(10)'),
	    Observacion            =upper(nref.value('@Observacion','varchar(500)')),
	    Usuario                =nref.value('@Usuario','varchar(100)'),
		getdate(),
	    EstadoSolicitud        =nref.value('@EstadoSolicitud','varchar(10)')	
	from @PI_ParamXML.nodes('/Root/SolProvHistEstado') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'


/*****************************SolMedioContactor*****************************/

	delete a  from [Proveedor].Pro_SolMedioContacto a
	inner  join @PI_ParamXML.nodes('/Root/SolMedioContacto')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdMedioContacto= nref.value('@IdMedioContacto','bigint') and
	  nref.value('@ACCION','VARCHAR(1)')='D'


	update a set
		ValorMedioContacto =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado             =nref.value('@Estado','bit')
	from [Proveedor].Pro_SolMedioContacto a
	inner  join @PI_ParamXML.nodes('/Root/SolMedioContacto')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.TipMedioContacto= nref.value('@TipMedioContacto','varchar(10)') and
	nref.value('@Contacto','varchar(1)')='N' and nref.value('@IdSolContacto','bigint') =0

	update a set
		ValorMedioContacto =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado             =nref.value('@Estado','bit')
	from [Proveedor].Pro_SolMedioContacto a
		inner  join @PI_ParamXML.nodes('/Root/SolMedioContacto')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.TipMedioContacto= nref.value('@TipMedioContacto','varchar(10)') and

	nref.value('@Contacto','varchar(1)')='S' and isnull(a.IdSolContacto,0)>0 and nref.value('@IdSolContacto','bigint')= a.IdSolContacto
	

	insert into [Proveedor].[Pro_SolMedioContacto]
	(IdSolicitud,IdSolContacto,TipMedioContacto,ValorMedioContacto,Estado)
	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		null,
	    TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
	    ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado                  =nref.value('@Estado','bit')	
	from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
	where nref.value('@Contacto','varchar(1)')='N'  and
	not exists(select top 1 1 from [Proveedor].[Pro_SolMedioContacto] M where m.IdSolicitud=@IdSolicitud and  isnull(m.IdSolContacto,0)=0 and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))

	insert into [Proveedor].[Pro_SolMedioContacto]
	(IdSolicitud,IdSolContacto,TipMedioContacto,ValorMedioContacto,Estado)
	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		c.IdSolContacto,
	    TipMedioContacto        =nref.value('@TipMedioContacto','varchar(10)'),
	    ValorMedioContacto      =upper(nref.value('@ValorMedioContacto','varchar(100)')),
		Estado                  =nref.value('@Estado','bit')	
	from @PI_ParamXML.nodes('/Root/SolMedioContacto') R(nref)
	  inner join Proveedor.Pro_SolContacto c on  nref.value('@Identificacion','varchar(13)')= c.Identificacion and  c.IdSolicitud=@IdSolicitud
	where nref.value('@Contacto','varchar(1)')='S' and
	not exists(select top 1 1 from [Proveedor].[Pro_SolMedioContacto] M where m.IdSolicitud=c.IdSolicitud and  c.IdSolContacto=m.IdSolContacto and m.TipMedioContacto=nref.value('@TipMedioContacto','varchar(10)'))

	/*****************************SolViapago******************************/

	;WITH cte_Pro_SolViapago 
	as (
			SELECT 
				a.IdVia
			FROM [Proveedor].[Pro_SolViapago] as a
			WHERE a.IdSolicitud = @IdSolicitud
		),cte_Pro_SolViapagoEliminar 
		as (
			SELECT a.IdVia
			FROM cte_Pro_SolViapago as a
			WHERE NOT EXISTS (SELECT nref.value('@IdVia','bigint')
							  FROM @PI_ParamXML.nodes('/Root/SolViapago') as R(nref)
							  where nref.value('@IdVia','bigint') = a.IdVia)
			)

	DELETE da
	FROM [Proveedor].[Pro_SolViapago] da
	WHERE da.IdVia in (select *from cte_Pro_SolViapagoEliminar)
		
	update a set
		[CodVia]                 =nref.value('@CodVia','varchar(10)'),
	    [Estado]                =nref.value('@Estado','bit')

	from [Proveedor].[Pro_SolViapago] a
	inner  join @PI_ParamXML.nodes('/Root/SolViapago')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdVia= nref.value('@IdVia','bigint') and
	 nref.value('@ACCION','VARCHAR(1)')='U'

	insert into [Proveedor].Pro_SolViapago
	(IdSolicitud, CodVia,Estado)

	select distinct
	(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		CodVia                 =nref.value('@CodVia','varchar(10)'),
	    Estado        =nref.value('@Estado','bit')	
	from @PI_ParamXML.nodes('/Root/SolViapago') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'


/*****************************SolRamo******************************/

	delete a  from [Proveedor].Pro_SolRamo a
	inner  join @PI_ParamXML.nodes('/Root/SolRamo')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdRamo= nref.value('@IdRamo','bigint') and
	  nref.value('@ACCION','VARCHAR(1)')='D'


	update a set
		[CodRAmo]                 =nref.value('@CodRAmo','varchar(10)'),
	    [Estado]                =nref.value('@Estado','bit'),
		[Principal]                =nref.value('@Principal','bit')

	from [Proveedor].Pro_SolRamo a
	inner  join @PI_ParamXML.nodes('/Root/SolRamo')  R(nref) on a.IdSolicitud= nref.value('@IdSolicitud','bigint') and a.IdRamo= nref.value('@IdRamo','bigint') and
	 nref.value('@ACCION','VARCHAR(1)')='U'

	insert into [Proveedor].Pro_SolRamo
	(IdSolicitud, CodRAmo,Estado,Principal)


	select distinct
		(case when isnull(nref.value('@IdSolicitud','bigint'),0)=0 then @IdSolicitud else nref.value('@IdSolicitud','bigint') end),
		CodRAmo                 =nref.value('@CodRAmo','varchar(10)'),
	    Estado        =nref.value('@Estado','bit'),	
		Principal        =nref.value('@Principal','bit')
	from @PI_ParamXML.nodes('/Root/SolRamo') R(nref)
	where nref.value('@ACCION','VARCHAR(1)')='I'

	select IdSolicitud=@IdSolicitud,
	       CodigoSap = @CodProveedor
	end
IF @@TRANCOUNT > 0
			COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Pro_P_GrabaSolProveedor]'
END CATCH


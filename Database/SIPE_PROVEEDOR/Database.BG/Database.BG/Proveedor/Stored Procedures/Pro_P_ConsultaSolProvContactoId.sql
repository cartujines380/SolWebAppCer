
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaSolProvContactoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	 declare    @CodProveedor			varchar(10)
	 Declare @Identificacion varchar(20)
	 Declare @Ruc varchar(13)
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	SELECT
		@CodProveedor		= nref.value('@CodProveedor','bigint')
	FROM @PI_ParamXML.nodes('/Root/Contacto') AS R(nref)
	select top 1  @Identificacion=nref.value('@Identificacion','varchar(20)')
    from @PI_ParamXML.nodes('/Root/Identificacion') AS R(nref) 

	select top 1 @Ruc = ruc
    from Proveedor.Pro_Proveedor where CodProveedor = @CodProveedor

	Declare @TipoIdentificacion table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoMedioContacto table ( codigo varchar(25), Detalle varchar(64))
	Declare @funcion table ( codigo varchar(25), Detalle varchar(64))
	Declare @Departamento table ( codigo varchar(25), Detalle varchar(64))




	insert into @TipoIdentificacion (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_tipoIdentificacion' 
	where a.Estado=b.Estado and a.Estado='A'
	
	
	insert into @TipoMedioContacto (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoMedioContacto' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @funcion (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_FuncionContacto' 
	where a.Estado=b.Estado and a.Estado='A'


	insert into @Departamento (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_DepartaContacto' 
	where a.Estado=b.Estado and a.Estado='A'

	if @CodProveedor  is not null
	begin 
		--Consultar Zonas
		if @Identificacion is not null
			begin
			  select  c.Codigo Id  from Seguridad.Seg_UsuarioZona a
			   LEFT JOIN Proveedor.Pro_Catalogo c ON (Convert(int,a.Zona) = Convert(int,c.Codigo) AND c.Tabla = 1031)  
			   where Ruc = @Ruc and Usuario = @Identificacion
			end
		else
		begin
		    
			Select  Id ,  TipoIdentificacion ,b.Detalle DescTipoIdentificacion, Identificacion , Nombre2
				   ,Nombre1,       Apellido2,     Apellido1,          CodSapContacto,  PreFijo
				   ,DepCliente    ,Departamento,  Funcion,            RepLegal,        Estado,
				  Telefono   TelfFijo,
				   Ext TelfFijoEXT,
					Celular  TelfMovil,
					Correo     EMAIL,
					c.Detalle DescFuncion,
					d.Detalle DescDepartamento,
					a.NotElectronica,
					a.NotTransBancaria,
					a.FechaNacimiento,
					a.Nacionalidad,
					a.PaisResidencia,
					a.EstadoCivil,
					a.RegimenMatrimonial,
					a.ConyugeTipoIdentificacion,
					a.ConyugeIdentificacion,
					a.ConyugeNombres,
					a.ConyugeApellidos,
					isnull(a.ConyugeFechaNac, getdate()) as ConyugeFechaNac,
					a.ConyugeNacionalidad,
					a.RelacionDependencia,
					a.AntiguedadLaboral,
					a.TipoIngreso,
					a.IngresoMensual,
					a.TipoParticipante
			from [Proveedor].[Pro_Contacto] a with(nolock)
				 left join @TipoIdentificacion b on a.TipoIdentificacion=b.codigo
				  left join @funcion c on a.Funcion=c.codigo
				  left join @Departamento d on a.Departamento=d.codigo
			where a.CodProveedor=@CodProveedor
		end
	end
	else
	begin 
	Select  IdSolContacto, IdSolicitud ,  TipoIdentificacion ,b.Detalle DescTipoIdentificacion, Identificacion , Nombre2
           ,Nombre1,       Apellido2,     Apellido1,          CodSapContacto,  PreFijo
		   ,DepCliente    ,Departamento,  Funcion,            RepLegal,        Estado,
		    [Proveedor].[Pro_F_SolMedioContacto](IdSolicitud ,IdSolContacto,'TLFFIJO')    TelfFijo,
            [Proveedor].[Pro_F_SolMedioContacto](IdSolicitud ,IdSolContacto,'TLFFIJOEXT') TelfFijoEXT,
		    [Proveedor].[Pro_F_SolMedioContacto](IdSolicitud ,IdSolContacto,'TLFMOVIL')   TelfMovil,
		    [Proveedor].[Pro_F_SolMedioContacto](IdSolicitud ,IdSolContacto,'EMAIL')      EMAIL,
			c.Detalle DescFuncion,
			d.Detalle DescDepartamento,
			a.NotElectronica,
			a.NotTransBancaria,
			a.FechaNacimiento,
			a.Nacionalidad,
			a.PaisResidencia,
			a.EstadoCivil,
			a.RegimenMatrimonial,
			a.ConyugeTipoIdentificacion,
			a.ConyugeIdentificacion,
			a.ConyugeNombres,
			a.ConyugeApellidos,
		    isnull(a.ConyugeFechaNac, getdate()) as ConyugeFechaNac,
			a.ConyugeNacionalidad,
			a.RelacionDependencia,
			a.AntiguedadLaboral,
			a.TipoIngreso,
			a.IngresoMensual,
			a.TipoParticipante
	from [Proveedor].[Pro_SolContacto] a with(nolock)
		 left join @TipoIdentificacion b on a.TipoIdentificacion=b.codigo
		  left join @funcion c on a.Funcion=c.codigo
		  left join @Departamento d on a.Departamento=d.codigo
	where a.IdSolicitud=@IdSolicitud

	end


END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvContactoId]'
END CATCH


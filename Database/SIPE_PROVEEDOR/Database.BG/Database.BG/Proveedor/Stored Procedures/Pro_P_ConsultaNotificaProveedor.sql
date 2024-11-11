CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaNotificaProveedor]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IDNIVEL	 VARCHAR(10)
		,@IDLINEA	 VARCHAR(10)
		,@IDMODULO   varchar(10)

	SELECT
		@IDNIVEL	  = nref.value('@IDNIVEL',   'VARCHAR(10)'),
		@IDLINEA      = nref.value('@IDLINEA',   'VARCHAR(10)'),
		@IDMODULO     = nref.value('@IDMODULO','VARCHAR(10)')
	
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)



    Declare @LineaNegocio       table ( codigo varchar(25), Detalle varchar(64))
	Declare @Modulo             table ( codigo varchar(25), Detalle varchar(64))
	Declare @Nivel              table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoIdentificacion table ( codigo varchar(25), Detalle varchar(64))

	insert into @LineaNegocio (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_LineaNegocio' 
	where a.Estado=b.Estado and a.Estado='A'


	insert into @Modulo (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_modulo' 
	where a.Estado=b.Estado and a.Estado='A'


	insert into @Nivel (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_nivel' 
	where a.Estado=b.Estado and a.Estado='A'

	
	insert into @TipoIdentificacion (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_tipoIdentificacionSys' 
	where a.Estado=b.Estado and a.Estado='A'

	
	IF ISNULL(@IDLINEA,'') <>''
	BEGIN

		SELECT     B.IdEmpresa, 	 A.Modulo,               D.Detalle DesModulo,	 A.Nivel, E.Detalle DEsNivel, 	 A.Ruc,	A.Usuario, C.Linea, G.Detalle DescLinea,
			  B.Apellido1,  B.Nombre1,  B.Nombre2, B.CorreoE,B.Cargo,B.TipoIdent, f.Detalle DesTipoIdentificacion, b.Apellido2
	FROM Seguridad.Seg_AprobacionNivel         A    with(nolock)
		inner join Seguridad.[Seg_Empleado]    B    with(nolock) on A.IdEmpresa=B.IdEmpresa AND A.Ruc=B.Ruc and a.Usuario=b.Usuario
		left join  Seguridad.Seg_EmpleadoLinea C    with(nolock) on c.IdEmpresa=a.IdEmpresa and c.Ruc=a.Ruc and c.Usuario=a.Usuario
		left join  @Modulo                     D                 on a.Modulo                = d.codigo
		left join  @Nivel                      E                 on a.Nivel                 = e.codigo
		LEFT JOIN  @TipoIdentificacion         F                 on B.TipoIdent             = f.codigo
		left join  @LineaNegocio               G                 ON C.Linea					= G.codigo        
		where a.Nivel=@IDNIVEL and A.Modulo=@IDMODULO and c.Linea= @IDLINEA

	END
	ELSE
	BEGIN
	
			SELECT     B.IdEmpresa, 	 A.Modulo,               D.Detalle DesModulo,	 A.Nivel, E.Detalle DEsNivel, 	 A.Ruc,	A.Usuario, '' Linea, '' DescLinea,
			  B.Apellido1,  B.Nombre1,  B.Nombre2, B.CorreoE,B.Cargo,B.TipoIdent, f.Detalle DesTipoIdentificacion, b.Apellido2
	FROM Seguridad.Seg_AprobacionNivel         A    with(nolock)
		inner join Seguridad.[Seg_Empleado]    B    with(nolock) on A.IdEmpresa=B.IdEmpresa AND A.Ruc=B.Ruc and a.Usuario=b.Usuario
		--left join  Seguridad.Seg_EmpleadoLinea C    with(nolock) on c.IdEmpresa=a.IdEmpresa and c.Ruc=a.Ruc and c.Usuario=a.Usuario
		left join  @Modulo                     D                 on a.Modulo                = d.codigo
		left join  @Nivel                      E                 on a.Nivel                 = e.codigo
		LEFT JOIN  @TipoIdentificacion         F                 on B.TipoIdent             = f.codigo
		--left join  @LineaNegocio               G                 ON C.Linea					= G.codigo        
		where a.Nivel=@IDNIVEL and A.Modulo=@IDMODULO --and c.Linea= isnull(@IDLINEA,c.Linea)
		
	END

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaNotificaProveedor]'
END CATCH



 

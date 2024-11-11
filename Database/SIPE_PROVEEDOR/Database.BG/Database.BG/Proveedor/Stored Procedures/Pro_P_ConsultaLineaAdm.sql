
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaLineaAdm]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IDNIVEL	 VARCHAR(10)
		,@IDLINEA	 VARCHAR(10)
		,@IDMODULO   varchar(10)
		,@IDUSUARIO   varchar(10)

	SELECT
		@IDNIVEL	  = nref.value('@IDNIVEL',   'VARCHAR(10)'),
		@IDLINEA      = nref.value('@IDLINEA',   'VARCHAR(10)'),
		@IDMODULO     = nref.value('@IDMODULO','VARCHAR(10)'),
		@IDUSUARIO     = nref.value('@IDUSUARIO','VARCHAR(20)')
	
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)



    Declare @LineaNegocio       table ( codigo varchar(25), Detalle varchar(64))
	Declare @Modulo             table ( codigo varchar(25), Detalle varchar(64))
	Declare @Nivel              table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoIdentificacion table ( codigo varchar(25), Detalle varchar(64))

	insert into @LineaNegocio (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_LineaNegocio' 
	where a.Estado=b.Estado and a.Estado='A'

	
	IF ISNULL(@IDNIVEL,'') ='APR'
	BEGIN

			IF isnull(@IDLINEA,'')<>''
			begin
				SELECT    g.codigo Linea, G.Detalle DescLinea
				FROM Seguridad.Seg_AprobacionNivel         A    with(nolock)
					inner join Seguridad.[Seg_Empleado]    B    with(nolock) on A.IdEmpresa=B.IdEmpresa AND A.Ruc=B.Ruc and a.Usuario=b.Usuario
					inner join  Seguridad.Seg_EmpleadoLinea C    with(nolock) on c.IdEmpresa=a.IdEmpresa and c.Ruc=a.Ruc and c.Usuario=a.Usuario
					inner join  @LineaNegocio               G                 ON C.Linea					= G.codigo        
					where a.Nivel=@IDNIVEL and A.Modulo=@IDMODULO 
					and c.Linea= @IDLINEA 
					--and a.Usuario=@IDUSUARIO
			end
			else
			begin
			SELECT    g.codigo Linea, G.Detalle DescLinea
				FROM Seguridad.Seg_AprobacionNivel         A    with(nolock)
					inner join Seguridad.[Seg_Empleado]    B    with(nolock) on A.IdEmpresa=B.IdEmpresa AND A.Ruc=B.Ruc and a.Usuario=b.Usuario
					inner join  Seguridad.Seg_EmpleadoLinea C    with(nolock) on c.IdEmpresa=a.IdEmpresa and c.Ruc=a.Ruc and c.Usuario=a.Usuario
					inner join  @LineaNegocio               G                 ON C.Linea					= G.codigo        
					where a.Nivel=@IDNIVEL 
					and A.Modulo=@IDMODULO 
					--and a.Usuario=@IDUSUARIO
			end
	END
	ELSE
	BEGIN
	
			SELECT    g.codigo Linea, G.Detalle DescLinea
				FROM   @LineaNegocio               G             
	
	END

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaLineaAdm]'
END CATCH



 

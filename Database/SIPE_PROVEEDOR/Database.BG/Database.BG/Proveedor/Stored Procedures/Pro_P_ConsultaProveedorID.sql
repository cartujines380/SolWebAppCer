
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaProveedorID]
	@PI_ParamXML xml
AS

SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdEmpresa			INT
		,@CodProveedor		VARCHAR(10)
		,@Ruc   			VARCHAR(13)
		,@FechaDesde		DATETIME
		,@FechaHasta		DATETIME
		,@Tipo				VARCHAR(10)

	DECLARE @ESTADOS TABLE (ESTADO VARCHAR(10) PRIMARY KEY)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@CodProveedor		= nref.value('@CodProveedor','VARCHAR(10)'),
		@Ruc			    = nref.value('@Ruc','VARCHAR(13)'),
		@FechaDesde			= CASE WHEN nref.value('@FechaDesde','VARCHAR(10)') = '' THEN NULL ELSE CONVERT(DATETIME, nref.value('@FechaDesde','VARCHAR(10)'), 103) END,
		@FechaHasta			= CASE WHEN nref.value('@FechaHasta','VARCHAR(10)') = '' THEN NULL ELSE CONVERT(DATETIME, nref.value('@FechaHasta','VARCHAR(10)'), 103) END,
		@Tipo               = nref.value('@Tipo','VARCHAR(13)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	IF (@FechaDesde IS NULL)
	BEGIN
		SELECT @FechaDesde = CONVERT(DATETIME, '01-01-1900', 103)
	END

	IF (@FechaHasta IS NULL)
	BEGIN
		SELECT @FechaHasta = CONVERT(DATETIME, '31-12-2999 23:59', 103)
	END
	--IF (@Ruc IS NULL)
	--BEGIN
	--	SELECT @Ruc = '%'
	--END

	--INSERT INTO @ESTADOS
	--SELECT nref.value('@id','VARCHAR(10)')
	--FROM @PI_ParamXML.nodes('/Root/Est') AS R(nref)
	--WHERE NOT nref.value('@id','VARCHAR(10)') IS NULL

	--IF NOT EXISTS(SELECT TOP 1 1 FROM  @ESTADOS)
	--BEGIN
	--	INSERT INTO @ESTADOS
	--	SELECT Codigo
	--	FROM [Proveedor].[Pro_Catalogo]
	--	WHERE Tabla = 1008
	--END

if ISNULL(@Tipo, '') <> '' and ISNULL(@Tipo, '') = 'C'
begin
	Declare @TipoIdentificacion        table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoMedioContacto         table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoProveedor             table ( codigo varchar(25), Detalle varchar(64))
	Declare @SectorComercial           table ( codigo varchar(25), Detalle varchar(64))
	Declare @Idioma                    table ( codigo varchar(25), Detalle varchar(64))
	Declare @CuentaAsociada            table ( codigo varchar(25), Detalle varchar(64))
	Declare @GrupoTesoreria            table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoSolicitudProveedor    table ( codigo varchar(25), Detalle varchar(64))
	Declare @EstadosSolicitudProveedor table ( codigo varchar(25), Detalle varchar(64))
    Declare @ClaseContribuyente        table ( codigo varchar(25), Detalle varchar(64))
	Declare @LineaNegocio              table ( codigo varchar(25), Detalle varchar(64))
	Declare @RetencionIva              table ( codigo varchar(25), Detalle varchar(64))
	Declare @RetencionIva2             table ( codigo varchar(25), Detalle varchar(64))
	Declare @RetencionFuente           table ( codigo varchar(25), Detalle varchar(64))
	Declare @RetencionFuente2          table ( codigo varchar(25), Detalle varchar(64))
	Declare @CondicionPago             table ( codigo varchar(25), Detalle varchar(64))
    Declare @GrupoCuenta               table ( codigo varchar(25), Detalle varchar(64))
	Declare @DespachaProvincia         table ( codigo varchar(25), Detalle varchar(64))

	insert into @CondicionPago (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_CondicionPago' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @GrupoCuenta (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_GrupoCuenta' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @DespachaProvincia (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_DespachaProvincia' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @RetencionIva (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_RetencionIva' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @RetencionIva2 (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_RetencionIva2' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @RetencionFuente (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_RetencionFuente' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @RetencionFuente2 (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_RetencionFuente2' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @LineaNegocio (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_LineaNegocio' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @TipoSolicitudProveedor (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoSolicitudProveedor' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @ClaseContribuyente (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_ClaseImpuesto' 
	where a.Estado=b.Estado and a.Estado='A'


	insert into @TipoIdentificacion (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_tipoIdentificacionSys' 
	where a.Estado=b.Estado and a.Estado='A'
	
	
	insert into @TipoMedioContacto (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoMedioContacto' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @TipoProveedor (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoProveedor' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @SectorComercial (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_SectorComercial' 
	where a.Estado=b.Estado and a.Estado='A'

	insert into @Idioma (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_Idioma' 
	where a.Estado=b.Estado and a.Estado='A'


	insert into @CuentaAsociada (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_CuentaAsociada' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @GrupoTesoreria (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_GrupoTesoreria' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @EstadosSolicitudProveedor (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_EstadosSolicitudProveedor' 
	where a.Estado=b.Estado and a.Estado='A'
	
	
	
	SELECT 1 as IdEmpresa                 ,x.[IdSolicitud]                ,x.[TipoSolicitud]                   ,h.Detalle DescTipoSolicitud,         a.[TipoProveedor] 
	      ,c.Detalle DescProveedor,       a.[CodProveedor] as CodSapProveedor           ,'RC' as TipoIdentificacion               ,b.Detalle DEscTipoIndentificacion    ,a.[Ruc] as Identificacion  ,
		   a.[NomComercial],
		   a.[NomComercial] as RazonSocial
		   --,a.[FechaSRI]   
		    ,CONVERT(VARCHAR(15),GETDATE() ,103) as FechaSRI                 
		   ,'' as SectorComercial                 ,d.Detalle DescSectorComercial ,a.[Idioma], 
		   e.detalle DescIdioma,          '' as CodGrupoProveedor          ,case when a.[GenDocElec]='01' then 'true' else 'false'end GenDocElec                      
		   --,a.[FechaSolicitud]            
		    ,CONVERT(VARCHAR(15),x.FechaSolicitud ,103) as [FechaSolicitud]
		   ,x.[Estado]
		   ,i.Detalle DescEstado
		  ,x.[GrupoTesoreria]            ,f.Detalle DescGrupoTesoreria   ,x.[CuentaAsociada]                  ,g.Detalle DescCuentaAsociada  ,x.[Autorizacion]
		  ,x.[TransfArticuProvAnterior]  ,x.[DepSolicitando]             ,x.[Responsable]                     ,x.[Aprobacion]                ,x.[Comentario]
		  ,a.CodClaseContribuyente as ClaseContribuyente          , j.Detalle DescClaseContribuyente , 
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'TLFFIJO')    TelfFijo,
           [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'TLFFIJOEXT') TelfFijoEXT,
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'TLFMOVIL')   TelfMovil,
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'FAX')        TelfFax,
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'FAXEXT')     TelfFaxEXT,
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'EMAILCORP')  EMAILCorp,
		   [Proveedor].[Pro_F_MedioContacto](a.CodProveedor ,null,'EMAILSRI')   EMAILSRI,
		   
            AnioConsti,   l.LineaNegocio,	l1.Detalle DescLineaNegocio,      totalventas,
            princliente,
			PlazoEntrega,
            DespachaProvincia,
			O.Detalle  DescDespachaProvincia,
            GrupoCuenta,
			N.Detalle  DescGrupoCuenta,
		    RetencionIva,
			p.Detalle  DescRetencionIva,
		    RetencionIva2 ,
			q.Detalle  DescRetencionIva2,
		    RetencionFuente,
			r.Detalle  DescRetencionFuente,
		    RetencionFuente2,
			r.Detalle  DescRetencionFuente2,
            CondicionPago,
			M.Detalle	 DescCondicionPago  ,
			x.GrupoCompra,
			x.GrupoEsquema,
			x.Ramo,
			t.CodActividadEconomica as ActividadEconomica,
			t.TipoServicio,
			isnull(t.RelacionBanco, 0) as RelacionBanco,
			t.RelacionIdentificacion,
			t.RelacionNombres,
			t.RelacionArea,
			isnull(t.FechaCreacion, getdate()) as FechaCreacion,
			t.EsCritico,
			t.ProcesoBrindaSoporte,
			t.Sgs,
			t.TipoCalificacion,
			t.Calificacion,
			isnull(CONVERT (datetime, NULLIF(t.FecTermCalificacion, '')), GETDATE()) as FecTermCalificacion,
			isnull(t.PersonaExpuesta, 0) as PersonaExpuesta,
			isnull(t.EleccionPopular, 0) as EleccionPopular
	from [Proveedor].[Pro_Proveedor] a with(nolock)
	     left join [Proveedor].[Pro_SolProveedor] x on a.CodProveedor = x.CodSapProveedor
		 left join @TipoIdentificacion        b  on x.TipoIdentificacion  = b.codigo
		 left join @TipoProveedor             c  on a.TipoProveedor       = c.codigo
         left join @SectorComercial           d  on x.SectorComercial     = d.codigo
		 left join @Idioma                    e  on a.Idioma              = e.codigo
		 left join @GrupoTesoreria            f  on x.GrupoTesoreria      = f.codigo
		 left join @CuentaAsociada            g  on x.CuentaAsociada      = g.codigo
		 left join @TipoSolicitudProveedor    h  on x.TipoSolicitud       = h.codigo
		 left join @EstadosSolicitudProveedor i  on x.Estado              = i.codigo
		 left join @ClaseContribuyente        j  on a.CodClaseContribuyente  = j.codigo		 
		 left join [Proveedor].[Pro_LineaNegocio] l on a.CodProveedor       = l.CodProveedor and l.Principal = 1
		 left join @LineaNegocio              l1  on l.LineaNegocio        = l1.codigo
		 left join @CondicionPago             M  on x.CondicionPago       = M.codigo
		 left join @GrupoCuenta               N  on x.GrupoCuenta         = N.codigo
		 left join @DespachaProvincia         O   on x.DespachaProvincia  = O.codigo
		 left join @RetencionIva              p  on x.RetencionIva        = p.codigo
  	     left join @RetencionIva2             q  on x.RetencionIva2       = q.codigo
		 left join @RetencionFuente           r  on x.RetencionFuente     = r.codigo
		 left join @RetencionFuente2          s  on x.RetencionFuente2    = s.codigo
		 left join [Proveedor].[Pro_ProveedorDetalle] t on a.CodProveedor = t.IdProveedor
	where a.CodProveedor=@CodProveedor
	      


	return 0;
end
	
if isnull(@CodProveedor,'')<>''
begin
		SELECT CodProveedor,		Ruc,             TipoProveedor,     NomComercial,
			   DirCalleNum,         DirPisoEdificio, DirCallePrinc,     DirDistrito,
               DirCodPostal,        Poblacion,       Pais,              Region,
               Idioma,              Telefono,        Movil,             Fax,
               CorreoE,             GenDocElec,     convert(varchar(10), FechaCertifica, 103) FechaCertifica,     IndMinoria,
               ApoderadoNom,        ApoderadoApe,    ApoderadoIdFiscal, PlazoEntregaPrev,
               FechaMod,IndMinoria,GenDocElec
		FROM [Proveedor].[Pro_Proveedor] with(nolock)
		WHERE CodProveedor = @CodProveedor --AND FechaCertifica BETWEEN @FechaDesde AND @FechaHasta
end

if isnull(@CodProveedor,'')='' and isnull(@Ruc,'') <>''
begin
		SELECT CodProveedor,		Ruc,             TipoProveedor,     NomComercial,
			   DirCalleNum,         DirPisoEdificio, DirCallePrinc,     DirDistrito,
               DirCodPostal,        Poblacion,       Pais,              Region,
               Idioma,              Telefono,        Movil,             Fax,
               CorreoE,             GenDocElec,      convert(varchar(10), FechaCertifica, 103) FechaCertifica,    IndMinoria,
               ApoderadoNom,        ApoderadoApe,    ApoderadoIdFiscal, PlazoEntregaPrev,
               FechaMod,IndMinoria,GenDocElec
		FROM [Proveedor].[Pro_Proveedor] with(nolock)
		WHERE Ruc = @Ruc --AND FechaCertifica BETWEEN @FechaDesde AND @FechaHasta
end	
	
		--select CodProveedor,
		--CodBanco,
		--CuentaNum,
		--CuentaTitular
		--from [Proveedor].[Pro_ProveedorBanco]
		--where CodProveedor = @CodProveedor

		--select 
		--		CodProveedor,	CodContacto,  Tratamiento, NomPila,   Nombre,
		--		DepCliente,     Departamento, Funcion,     Telefono1, Telefono2,
		--		CorreoE
		--from 	[Proveedor].[Pro_ProveedorContacto]
		--where CodProveedor = @CodProveedor

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaProveedorID]'
END CATCH


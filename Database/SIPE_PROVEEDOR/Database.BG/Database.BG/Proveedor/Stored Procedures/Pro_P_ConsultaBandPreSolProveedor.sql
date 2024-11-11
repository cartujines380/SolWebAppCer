
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaBandPreSolProveedor]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		@Ruc         varchar(13), @FechaDesde  varchar(10), @FechaHasta  varchar(10), @Estado      varchar(10), @Pantalla  varchar(10),@Opcion varchar(10),
		@IDLINEA     varchar(10), @IDUSUARIO   varchar(20)

		declare @TEstado table(Codigo varchar(10))
	
	SELECT
		@Ruc		       = nref.value('@Ruc','varchar(13)'),
		@FechaDesde		   = nref.value('@FechaDesde','varchar(10)'),
		@FechaHasta		   = nref.value('@FechaHasta','varchar(10)'),
		@Estado            = nref.value('@Estado','varchar(10)'),
		@Pantalla          = nref.value('@Pantalla','varchar(10)'),
		@Opcion            = nref.value('@Opcion','varchar(10)'),
		@IDLINEA           = nref.value('@IDLINEA','varchar(10)'),
		@IDUSUARIO         = nref.value('@IDUSUARIO','varchar(20)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)


	select	@Ruc=isnull(@Ruc,'%'),
			@FechaDesde=isnull(@FechaDesde,'1900/01/01'),
			@FechaHasta=isnull(@FechaHasta,'2999/01/01'),
			@Estado=isnull(@Estado,'')

	Declare @TipoIdentificacion        table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoMedioContacto         table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoProveedor             table ( codigo varchar(25), Detalle varchar(64))
	Declare @SectorComercial           table ( codigo varchar(25), Detalle varchar(64))
	Declare @Idioma                    table ( codigo varchar(25), Detalle varchar(64))
	Declare @CuentaAsociada            table ( codigo varchar(25), Detalle varchar(64))
	Declare @GrupoTesoreria            table ( codigo varchar(25), Detalle varchar(64))
	Declare @TipoSolicitudProveedor    table ( codigo varchar(25), Detalle varchar(64))
	Declare @EstadosSolicitudProveedor table ( codigo varchar(25), Detalle varchar(64))
	Declare @ClaseContribuyente      table ( codigo varchar(25), Detalle varchar(64))
	Declare @LineaNegocio            table ( codigo varchar(25), Detalle varchar(64))

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

	insert into @GrupoTesoreria (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_GrupoTesoreria' 
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
	
	if @Pantalla='PRE' and @Estado=''
	begin
		insert into @TEstado(codigo)
		values('PR')
		insert into @TEstado(codigo)
		values('PA')
		insert into @TEstado(codigo)
		values('EP')
	end
  
	if @Pantalla='PRE' and @Estado<>''
	begin
		insert into @TEstado(codigo)
		values(@Estado)
	end

	if @Pantalla='NU' and @Estado=''
	begin
		--Bandeja de compras
		if @Opcion ='APR'
		begin
			insert into @TEstado(codigo)
			values('DP')
			insert into @TEstado(codigo)
			values('RE')
			/*Adicionado segun nuevo requerimiento */
			--insert into @TEstado(codigo)
			--values('RA')
			--insert into @TEstado(codigo)
			--values('EN')
			--insert into @TEstado(codigo)			
			--values('RC')
			--insert into @TEstado(codigo)
			--values('DM')
			--insert into @TEstado(codigo)
			--values('AC')
			--insert into @TEstado(codigo)
			--values('RV')
		end

		--Bandeja de seguridad integral
		if @Opcion ='APS'
		begin
			insert into @TEstado(codigo)
			values('EN')
			insert into @TEstado(codigo)
			values('AI')
			
		end

		--Bandeja de seguridad de la información
		if @Opcion ='API'
		begin
			insert into @TEstado(codigo)
			values('EN')
			insert into @TEstado(codigo)
			values('AS')
			
		end

		--if @Opcion ='APG'
		--begin
		--	insert into @TEstado(codigo)
		--	values('RV')
		--	insert into @TEstado(codigo)
		--	values('RC')
		--	insert into @TEstado(codigo)
		--	values('AC')
		--end

		--if @Opcion ='APM'
		--begin
		--	insert into @TEstado(codigo)
		--	values('DM')
		--	insert into @TEstado(codigo)
		--	values('AC')
		--	insert into @TEstado(codigo)
		--	values('AP')
		--	insert into @TEstado(codigo)
		--	values('RD')
		--end
	end

    if @Pantalla='NU' and @Estado<>''
	begin
		insert into @TEstado(codigo)
		values(@Estado)
	end

	IF @Ruc=''
	BEGIN
		SELECT @Ruc='%'
	END

	--if isnull(@IDLINEA,'')='' and @Opcion ='APR'
	--begin
	--	delete D
	--	from @LineaNegocio D
	--	where not exists(SELECT top 1 1
	--	FROM Seguridad.Seg_AprobacionNivel  A with(nolock)
	--	inner join Seguridad.[Seg_Empleado] B with(nolock) 
	--		on A.IdEmpresa=B.IdEmpresa and A.Ruc=B.Ruc and a.Usuario=b.Usuario
	--	inner join  Seguridad.Seg_EmpleadoLinea C with(nolock) 
	--		on c.IdEmpresa=a.IdEmpresa and c.Ruc=a.Ruc and c.Usuario=a.Usuario
	--	where 
	--		a.Nivel=@Opcion and A.Modulo='PRV'  
	--		--and a.Usuario=@IDUSUARIO 
	--		and d.codigo=c.Linea)
	--end

	if isnull(@IDLINEA,'')<>'' 
	begin
		delete from @LineaNegocio
		where codigo <> @IDLINEA
	end
	
	SELECT 
	distinct a.[IdEmpresa]                 ,a.[IdSolicitud]                ,a.[TipoSolicitud]                   ,
	h.Detalle DescTipoSolicitud,         
	a.[TipoProveedor] 
	      ,c.Detalle DescProveedor,       a.[CodSapProveedor]           ,a.[TipoIdentificacion]               ,b.Detalle DEscTipoIndentificacion    ,a.[Identificacion]            ,
		   a.[NomComercial],
		   a.[RazonSocial]               
		     ,CONVERT(VARCHAR(15),a.[FechaSRI] ,103) as [FechaSRI]
		    ,a.[SectorComercial]                 ,d.Detalle DescSectorComercial ,a.[Idioma], 
		   e.detalle DescIdioma,          a.[CodGrupoProveedor]          ,case when a.[GenDocElec]='01' then 'true' else 'false'end GenDocElec                     
			   ,CONVERT(VARCHAR(15),a.FechaSolicitud ,103) as [FechaSolicitud]
			 ,a.[Estado]
		   ,i.Detalle DescEstado
		  ,a.[GrupoTesoreria]            ,f.Detalle DescGrupoTesoreria   ,a.[CuentaAsociada]                  ,g.Detalle DescCuentaAsociada  ,a.[Autorizacion]
		  ,a.[TransfArticuProvAnterior]  ,a.[DepSolicitando]             ,a.[Responsable]                     ,a.[Aprobacion]                ,a.[Comentario]
		   ,a.ClaseContribuyente          , k.Detalle DescClaseContribuyente , 
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'TLFFIJO')    TelfFijo,
           [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'TLFFIJOEXT') TelfFijoEXT,
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'TLFMOVIL')   TelfMovil,
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'FAX')        TelfFax,
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'FAXEXT')     TelfFaxEXT,
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'EMAILCORP')  EMAILCorp,
		   [Proveedor].[Pro_F_SolMedioContacto](a.IdSolicitud ,null,'EMAILSRI')   EMAILSRI,
            AnioConsti,
            LineaNegocio,
			l.Detalle DescLineaNegocio,
            totalventas,
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
			M.Detalle	 DescCondicionPago,
			a.GrupoCompra,
			a.GrupoEsquema,
			a.Ramo,
		    t.CodActividadEconomica as ActividadEconomica,
			t.TipoServicio,
			isnull(t.RelacionBanco, 0) as RelacionBanco,
			t.RelacionIdentificacion,
			t.RelacionNombres,
			t.RelacionArea,
			isnull(t.FechaCreacion, GETDATE()) as FechaCreacion,
			t.EsCritico,
			t.ProcesoBrindaSoporte,
			t.Sgs,
			t.TipoCalificacion,
			t.Calificacion,
			isnull(CONVERT (datetime, NULLIF(t.FecTermCalificacion, '')), GETDATE()) as FecTermCalificacion,
			isnull(t.PersonaExpuesta, 0) as PersonaExpuesta,
			isnull(t.EleccionPopular, 0) as EleccionPopular
	from [Proveedor].[Pro_SolProveedor]        a with(nolock)
		 left join @TipoIdentificacion        b  on a.TipoIdentificacion = b.codigo
		 left join @TipoProveedor             c  on a.TipoProveedor      = c.codigo
         left join @SectorComercial           d  on a.SectorComercial    = d.codigo
		 left join @Idioma                    e  on a.Idioma             = e.codigo
		 left join @GrupoTesoreria            f  on a.GrupoTesoreria     = f.codigo
		 left join @CuentaAsociada            g  on a.CuentaAsociada     = g.codigo
		 left join @TipoSolicitudProveedor    h  on a.TipoSolicitud      = h.codigo
		 left join @EstadosSolicitudProveedor i  on a.Estado             = i.codigo 
		 INNER JOIN @TEstado                  j  on a.Estado             = j.codigo
		 left join @ClaseContribuyente        k  on a.ClaseContribuyente = k.codigo 
		 inner join @LineaNegocio              l  on a.LineaNegocio = l.codigo 
		 left join @CondicionPago             M  on a.CondicionPago       = M.codigo 
		 left join @GrupoCuenta               N  on a.GrupoCuenta         = N.codigo 
		 left join @DespachaProvincia         O   on a.DespachaProvincia  = O.codigo 
		 left join @RetencionIva              p  on a.RetencionIva        = p.codigo 
  	     left join @RetencionIva2             q  on a.RetencionIva2       = q.codigo 
		 left join @RetencionFuente           r  on a.RetencionFuente     = r.codigo 
		 left join @RetencionFuente2          s  on a.RetencionFuente2    = s.codigo 
		 left join [Proveedor].[Pro_SolProveedorDetalle]  t on a.IdSolicitud = t.IdSolicitud
	WHERE Identificacion LIKE @Ruc AND convert(date,FechaSolicitud) BETWEEN  cast(@FechaDesde as date) AND cast(@FechaHasta as date)
	AND Identificacion NOT IN( select Ruc
	from [Proveedor].[Pro_Proveedor])
--execute [Proveedor].[Pro_P_ConsultaBandPreSolProveedor] '<Root Estado="EP" Pantalla="PRE" Opcion="PRE" IDUSUARIO="cgarcia" />'
	
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaBandPreSolProveedor]'
END CATCH


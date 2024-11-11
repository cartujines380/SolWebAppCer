
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaULtSolProveedor]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdEmpresa			 INT
		,@CodSapProveedor	 VARCHAR(10)
		,@IdSolicitud        bigint
		,@Identificacion     varchar(13)	
		,@STipoIdentificacion varchar(10)
	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@CodSapProveedor    = nref.value('@CodSapProveedor','VARCHAR(10)'),
		@Identificacion   	= nref.value('@Identificacion','VARCHAR(13)'),
		@STipoIdentificacion = nref.value('@TipoIdentificacion','VARCHAR(10)'),
		@IdSolicitud   		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)


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

		insert into @ClaseContribuyente (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_ClaseImpuesto' 
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

		insert into @TipoSolicitudProveedor (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoSolicitudProveedor' 
	where a.Estado=b.Estado and a.Estado='A'








	
if isnull(@IdSolicitud,0)=0
begin
	if ISNULL(@CodSapProveedor,'')<>''
   Begin
	select @IdSolicitud=(SELECT max(IdSolicitud)  FROM [Proveedor].Pro_SolProveedor a with(nolock)
	WHERE a.CodSapProveedor  = @CodSapProveedor)		
   End
   
	if ISNULL(@Identificacion,'')<>''
	begin
		select @IdSolicitud=(SELECT max(IdSolicitud)  FROM [Proveedor].Pro_SolProveedor a with(nolock)
		WHERE a.Identificacion = @Identificacion /*and a.TipoIdentificacion=@STipoIdentificacion*/)		
	end
end
		if (isnull(@IdSolicitud,0)>0)
		begin 
		SELECT a.[IdEmpresa]                 ,a.[IdSolicitud]                ,a.[TipoSolicitud]                   ,h.Detalle DescTipoSolicitud,         a.[TipoProveedor] 
	      ,c.Detalle DescProveedor,       a.[CodSapProveedor]           ,a.[TipoIdentificacion]               ,b.codigo DEscTipoIndentificacion    ,a.[Identificacion]            ,
		   a.[NomComercial],
		   a.[RazonSocial]               
		   --,a.[FechaSRI]                   
		   ,CONVERT(VARCHAR(15),a.[FechaSRI] ,103) as [FechaSRI] 
		   ,a.[SectorComercial]                 ,d.Detalle DescSectorComercial ,a.[Idioma], 
		   e.detalle DescIdioma,          a.[CodGrupoProveedor]          ,a.[GenDocElec]                      
		   --,a.[FechaSolicitud]            
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
			a.GrupoCompra ,
			a.GrupoEsquema,
			a.Ramo,
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
	from [Proveedor].[Pro_SolProveedor]        a with(nolock)
		 left join @TipoIdentificacion        b  on a.TipoIdentificacion = b.codigo
		 left join @TipoProveedor             c  on a.TipoProveedor      = c.codigo
		 left join @SectorComercial           d  on a.SectorComercial    = d.codigo
		  left join @Idioma                    e  on a.Idioma             = e.codigo
		  left join @GrupoTesoreria            f  on a.GrupoTesoreria     = f.codigo
		  left join @CuentaAsociada            g  on a.CuentaAsociada     = g.codigo
		  left join @TipoSolicitudProveedor    h  on a.TipoSolicitud      = h.codigo
		  left join @EstadosSolicitudProveedor i  on a.Estado             = i.codigo 
		  left join @ClaseContribuyente        k  on a.ClaseContribuyente = k.codigo
		  left join @LineaNegocio              l  on a.LineaNegocio = l.codigo 
		  left join @CondicionPago             M  on a.CondicionPago       = M.codigo 
		  left join @GrupoCuenta               N  on a.GrupoCuenta         = N.codigo 
		  left join @DespachaProvincia         O   on a.DespachaProvincia  = O.codigo 
		  left join @RetencionIva              p  on a.RetencionIva        = p.codigo 
  	      left join @RetencionIva2             q  on a.RetencionIva2       = q.codigo 
		  left join @RetencionFuente           r  on a.RetencionFuente     = r.codigo 
		  left join @RetencionFuente2          s  on a.RetencionFuente2    = s.codigo
		  left join [Proveedor].[Pro_SolProveedorDetalle]  t on a.IdSolicitud = t.IdSolicitud
	where a.IdSolicitud=@IdSolicitud
		end
	
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaULtSolProveedor]'
END CATCH



 

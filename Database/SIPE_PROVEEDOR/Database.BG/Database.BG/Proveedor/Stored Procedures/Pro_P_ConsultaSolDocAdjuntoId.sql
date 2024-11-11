
--exec SIPE_PROVEEDOR.Proveedor.[Pro_P_ConsultaSolDocAdjuntoId] @PI_ParamXML='<Root IdSolicitud="13" />'
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaSolDocAdjuntoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT,
		 @TipoPersona           VARCHAR(10),
		 @ProcesoSoporte		VARCHAR(10),
		 @CodProveedor			VARCHAR(10)
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint'),
		@CodProveedor		= nref.value('@CodProveedor', 'varchar(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Documento table ( codigo varchar(25), Detalle varchar(200))
    
	--insert into @Documento (codigo, Detalle)
	--select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	--inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_DocuGeneralAdjunto' 
	--where a.Estado=b.Estado and a.Estado='A'
	
	if(@CodProveedor is not null)
	begin

	select top 1 
	@TipoPersona = CodClaseContribuyente,
	@ProcesoSoporte = d.ProcesoBrindaSoporte
	from [Proveedor].[Pro_Proveedor] p inner join [Proveedor].[Pro_ProveedorDetalle] d on d.IdProveedor = p.CodProveedor where p.CodProveedor=@CodProveedor 

	if @TipoPersona is not null and @TipoPersona <> 'PN' and @TipoPersona <> 'PJ'
			set @TipoPersona = 'PJ'
	
	insert into @Documento (codigo, Detalle)
	select Codigo, Descripcion from [Proveedor].[Pro_Documentos] d	
	where d.Estado='A' and (d.CodTipoPersona = @TipoPersona or d.Codigo = case @ProcesoSoporte when 'P3' then 'EH'
		                                                           when 'P4' then 'CV'
																   end);

	SELECT [CodProveedor]
		  ,[IdDocAdjunto]
		  ,[CodDocumento]
		  ,b.Detalle DescDocumento
		  ,[NomArchivo]
		  ,[Archivo]
		  --,[FechaCarga]
		   ,CONVERT(VARCHAR(15),[FechaCarga],103) as [FechaCarga]
		  ,[Estado]
	  FROM [Proveedor].[Pro_DocAdjunto] a
	  inner join @Documento b on a.CodDocumento=b.codigo
	  where a.CodProveedor=@CodProveedor
	end
	else
	begin

	select top 1 
	@TipoPersona = ClaseContribuyente,
	@ProcesoSoporte = d.ProcesoBrindaSoporte
	from [Proveedor].[Pro_SolProveedor] p inner join [Proveedor].[Pro_SolProveedorDetalle] d on d.IdSolicitud = p.IdSolicitud where p.idSolicitud=@IdSolicitud 
	
	if @TipoPersona is not null and @TipoPersona <> 'PN' and @TipoPersona <> 'PJ'
			set @TipoPersona = 'PJ'
	
	insert into @Documento (codigo, Detalle)
	select Codigo, Descripcion from [Proveedor].[Pro_Documentos] d	
	where d.Estado='A' and (d.CodTipoPersona = @TipoPersona or d.Codigo = case @ProcesoSoporte when 'P3' then 'EH'
		                                                           when 'P4' then 'CV'
																   end);

	SELECT [IdSolicitud]
		  ,[IdSolDocAdjunto]
		  ,[CodDocumento]
		  ,b.Detalle DescDocumento
		  ,[NomArchivo]
		  ,[Archivo]
		  --,[FechaCarga]
		   ,CONVERT(VARCHAR(15),[FechaCarga],103) as [FechaCarga]
		  ,[Estado]
	  FROM [Proveedor].[Pro_SolDocAdjunto] a
	  inner join @Documento b on a.CodDocumento=b.codigo
	  where a.IdSolicitud=@IdSolicitud
	  end
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolDocAdjuntoId]'
END CATCH


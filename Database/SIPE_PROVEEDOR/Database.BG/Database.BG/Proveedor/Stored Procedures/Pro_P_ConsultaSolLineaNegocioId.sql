Create PROCEDURE [Proveedor].[Pro_P_ConsultaSolLineaNegocioId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Sociedad table ( codigo varchar(25), Detalle varchar(64))
    Declare @Linea table ( codigo varchar(25), Detalle varchar(64))



	insert into @Sociedad (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Sociedad' 
	where a.Estado=b.Estado and a.Estado='A'

	
	insert into @Linea (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_LineaNegocio' 
	where a.Estado=b.Estado and a.Estado='A'
	





SELECT  [IdSolicitud]             ,[CodigoSociedad]	  ,b.Detalle DescSociedad      ,[CodigoSeccion]
	   ,c.Detalle DescSeccion     ,[IdLIneNegocio]
  FROM [Proveedor].[Pro_SolLineaNegocio] a
  inner join @Sociedad  b on a.CodigoSociedad=b.codigo
  inner join @Linea  c on a.CodigoSeccion=c.codigo
  where a.IdSolicitud=@IdSolicitud
  	
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolLineaNegocioId]'
END CATCH


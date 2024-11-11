
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaSolZonaId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Zona table ( codigo varchar(25), Detalle varchar(64))
	
	insert into @Zona (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Ciudad' 
	where a.Estado=b.Estado and a.Estado='A'

	
	--Declare @Region table ( codigo varchar(10), Detalle varchar(64))
	

	--insert into @Pais (codigo, Detalle)
	--select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	--inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Pais' 
	--where a.Estado=b.Estado and a.Estado='A'
	
	--insert into @Region (codigo, Detalle)
	--select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	--inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Region' 
	--where a.Estado=b.Estado and a.Estado='A'

	


	--SELECT IdDireccion,  IdSolicitud, Pais, b.Detalle DescPais, Provincia, c.Detalle DescRegion, Ciudad, CallePrincipal ,CalleSecundaria
 --         ,PisoEdificio, CodPostal,   Solar,Estado
	--from [Proveedor].[Pro_SolDireccion] a
	--	 inner join @Pais b on a.Pais=b.codigo
	--	 inner join @Region c on a.Provincia=c.codigo
	--where a.IdSolicitud=@IdSolicitud


SELECT  [IdSolicitud]
      ,[CodZona]
	  ,b.Detalle DescZona
        ,[Estado]
		,Idzona
  FROM [Proveedor].[Pro_SolZona] a
  left join @Zona b on a.CodZona=b.codigo
  	where a.IdSolicitud=@IdSolicitud
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolZonaId]'
END CATCH



CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaSolProvDireccionId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT,
		 @CodProveedor			VARCHAR(10)
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint'),
		@CodProveedor		= nref.value('@CodProveedor', 'varchar(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Pais table ( codigo varchar(10), Detalle varchar(64))
	Declare @Region table ( codigo varchar(10), Detalle varchar(64))
	

	insert into @Pais (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Pais' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @Region (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Region' 
	where a.Estado=b.Estado and a.Estado='A'

	
	if(@CodProveedor is null)
	begin 
		SELECT IdDireccion,  IdSolicitud, Pais, b.Detalle DescPais, Provincia, c.Detalle DescRegion, Ciudad, CallePrincipal ,CalleSecundaria
          ,PisoEdificio, CodPostal,   Solar,Estado
	from [Proveedor].[Pro_SolDireccion] a
		 left join @Pais b on a.Pais=b.codigo
		 left join @Region c on a.Provincia=c.codigo
	where a.IdSolicitud=@IdSolicitud
	end
	else
	begin
		SELECT IdDireccion,  CodProveedor, Pais, b.Detalle DescPais, Provincia, c.Detalle DescRegion, Ciudad, CallePrincipal ,CalleSecundaria
          ,PisoEdificio, CodPostal,   Solar,Estado
	from [Proveedor].[Pro_Direccion] a
		 left join @Pais b on a.Pais=b.codigo
		 left join @Region c on a.Provincia=c.codigo
	where a.CodProveedor=@CodProveedor
	end

	
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvDireccionId]'
END CATCH




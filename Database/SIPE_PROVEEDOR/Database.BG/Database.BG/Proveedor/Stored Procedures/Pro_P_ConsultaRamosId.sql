
Create PROCEDURE [Proveedor].[Pro_P_ConsultaRamosId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Ramo table ( codigo varchar(25), Detalle varchar(64))

	insert into @Ramo (codigo, Detalle)
		select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_Ramo' 
	where a.Estado=b.Estado and a.Estado='A'




	 SELECT  IdSolicitud,IdRamo, CodRAmo, b.Detalle DescRamo,  Estado,
	 Principal
	 FROM [Proveedor].[Pro_SolRamo] a
	 INNER JOIN @Ramo b on a.CodRAmo=b.codigo
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaRamosId]'
END CATCH


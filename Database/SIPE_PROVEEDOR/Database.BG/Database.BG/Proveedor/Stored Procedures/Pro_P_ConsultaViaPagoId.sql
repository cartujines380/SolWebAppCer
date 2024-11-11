
CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaViaPagoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @Via table ( codigo varchar(25), Detalle varchar(64))

	insert into @Via (codigo, Detalle)
		select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a
	inner join Proveedor.Pro_Catalogo b on a.Tabla=b.Tabla and a.TablaNombre='tbl_ViaPago' 
	where a.Estado=b.Estado and a.Estado='A'




	 SELECT  	 IdSolicitud,CodVia,
b.Detalle DescVia,
Estado,
IdVia

	 FROM [Proveedor].[Pro_SolViapago] a
	 INNER JOIN @Via b on a.CodVia=b.codigo
	 where a.IdSolicitud=isnull(@IdSolicitud,a.IdSolicitud)
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaViaPagoId]'
END CATCH


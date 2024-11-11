
Create PROCEDURE [Proveedor].[Pro_P_ConsultaSolProvBancoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud			= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @TipoCuenta  table ( codigo varchar(25), Detalle varchar(64))
	Declare @Pais        table ( codigo varchar(25), Detalle varchar(64))
	Declare @Provincia   table ( codigo varchar(25), Detalle varchar(64))

	insert into @TipoCuenta (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_TipoCuenta' 
	where a.Estado=b.Estado and a.Estado='A'
	
	insert into @Pais (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_Pais' 
	where a.Estado=b.Estado and a.Estado='A'
	

		insert into @Provincia (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_Region' 
	where a.Estado=b.Estado and a.Estado='A'


	Select IdSolBanco , IdSolicitud,  Extrangera,    CodSapBanco, Ba.NomBanco, Pais, c.Detalle DescPAis
          ,TipoCuenta, b.Detalle DesCuenta , NumeroCuenta, TitularCuenta, ReprCuenta, a.CodSwift
          ,CodBENINT,   CodABA,       Principal,      Estado, a.Provincia, d.Detalle DescProvincia, a.DirBancoExtranjero,BancoExtranjero
	from Proveedor.[Pro_SolBanco] a with(nolock)
		 inner join @TipoCuenta         b   on a.TipoCuenta  =b.codigo
		 left join  @Pais               c   on a.Pais        =c.codigo
		 left join  @Provincia          d   on a.Provincia   =d.codigo
		 left join  Proveedor.Pro_Banco Ba  on BA.CodBanco   =a.CodSapBanco
	where a.IdSolicitud=@IdSolicitud
             


END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvBancoId]'
END CATCH


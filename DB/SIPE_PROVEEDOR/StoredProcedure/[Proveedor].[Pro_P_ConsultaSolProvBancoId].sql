USE [SIPE_PROVEEDOR]
GO
/****** Object:  StoredProcedure [Proveedor].[Pro_P_ConsultaSolProvBancoId]    Script Date: 28/7/2022 16:56:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Proveedor].[Pro_P_ConsultaSolProvBancoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT,
         @CodProveedor			varchar(10)

	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint'),
		@CodProveedor		= nref.value('@CodProveedor', 'varchar(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	Declare @TipoCuenta  table ( codigo varchar(25), Detalle varchar(64))
	Declare @Pais        table ( codigo varchar(25), Detalle varchar(64))
	Declare @Provincia   table ( codigo varchar(25), Detalle varchar(64))
	Declare @FormaDePago table ( codigo varchar(25), Detalle varchar(64))

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

	insert into @FormaDePago (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_FPagoContrato' 
	where a.Estado=b.Estado and a.Estado='A'

	print 'ooo'

	if @CodProveedor  is not null
	begin
		print 'cdaa'
		Select IdProveedorPago , 0 IdSolicitud,  Extrangera,    CodSapBanco, Ba.NomBanco, Pais, c.Detalle DescPAis
          ,TipoCuenta, b.Detalle DesCuenta , NumeroCuenta, TitularCuenta, ReprCuenta, a.CodSwift
          ,CodBENINT,   CodABA,       Principal,      Estado, a.Provincia, d.Detalle DescProvincia, a.DirBancoExtranjero,BancoExtranjero, FormaPago, e.Detalle DesFormaPago
	from Proveedor.[Pro_ProveedorFrmPago] a with(nolock)
		 inner join @TipoCuenta         b   on a.TipoCuenta  =b.codigo
		 left join  @Pais               c   on a.Pais        =c.codigo
		 left join  @Provincia          d   on a.Provincia   =d.codigo
		 left join  Proveedor.Pro_Banco Ba  on BA.CodBanco   =a.CodSapBanco
		 left join @FormaDePago         e   on a.FormaPago   =e.codigo
	where a.CodProveedor=@CodProveedor
             
	end
	else
	begin
		Select IdSolBanco , IdSolicitud,  Extrangera,    CodSapBanco, Ba.NomBanco, Pais, c.Detalle DescPAis
          ,TipoCuenta, b.Detalle DesCuenta , NumeroCuenta, TitularCuenta, ReprCuenta, a.CodSwift
          ,CodBENINT,   CodABA,       Principal,      Estado, a.Provincia, d.Detalle DescProvincia, a.DirBancoExtranjero,BancoExtranjero, FormaPago, e.Detalle DesFormaPago
	from Proveedor.[Pro_SolBanco] a with(nolock)
		 inner join @TipoCuenta         b   on a.TipoCuenta  =b.codigo
		 left join  @Pais               c   on a.Pais        =c.codigo
		 left join  @Provincia          d   on a.Provincia   =d.codigo
		 left join  Proveedor.Pro_Banco Ba  on BA.CodBanco   =a.CodSapBanco
		 left join @FormaDePago         e   on a.FormaPago   =e.codigo
	where a.IdSolicitud=@IdSolicitud
             
	end
	


END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvBancoId]'
END CATCH


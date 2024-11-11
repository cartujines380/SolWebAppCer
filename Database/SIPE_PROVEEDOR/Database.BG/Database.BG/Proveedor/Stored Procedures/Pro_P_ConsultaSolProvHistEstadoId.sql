
Create PROCEDURE [Proveedor].[Pro_P_ConsultaSolProvHistEstadoId]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
	
	Declare @Motivo table ( codigo varchar(25), Detalle varchar(64))
	Declare @EstadosSolicitudProveedor table ( codigo varchar(25), Detalle varchar(64))	

	insert into @EstadosSolicitudProveedor (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_EstadosSolicitudProveedor' 
	where a.Estado=b.Estado and a.Estado='A'




	insert into @Motivo (codigo, Detalle)
	select b.Codigo,b.Detalle from Proveedor.Pro_Tabla a with(nolock)
	inner join Proveedor.Pro_Catalogo b with(nolock) on a.Tabla=b.Tabla and a.TablaNombre='tbl_MotivoRechazoProveedor' 
	where a.Estado=b.Estado and a.Estado='A'
	
	
SELECT  [IdObservacion]       ,[IdSolicitud]      ,[Motivo]	  ,b.Detalle DesMotivo
        ,[Observacion]        ,[Usuario]          ,[Fecha]    ,[EstadoSolicitud], i.Detalle DesEstadoSolicitud
  FROM [Proveedor].[Pro_SolProvHistEstado] a with(nolock)
  inner join @Motivo  b on a.Motivo=b.codigo
   left join @EstadosSolicitudProveedor i  on a.EstadoSolicitud             = i.codigo 
  where a.IdSolicitud=@IdSolicitud
  	
 
END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolProvHistEstadoId]'
END CATCH


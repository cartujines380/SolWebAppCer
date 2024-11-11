



CREATE PROCEDURE [Notificacion].[Not_ConsultaProveedores_Not]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @Ruc varchar(20)
	Declare @CodigoSol int
    select  @Ruc=nref.value('@Ruc','VARCHAR(20)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @CodigoSol=nref.value('@CodigoSol','int')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	

begin
   
   IF (@CodigoSol > 0)
   BEGIN
	   SELECT
		   a.CodProveedor
		  ,a.Ruc
		  ,a.NomComercial
		  ,a.CorreoE
		    ,(select detalle from Proveedor.Pro_Catalogo where tabla = 9922 and codigo in
			   (SELECT TOP 1  Motivo FROM Articulo.Art_SolHistorialEstado
			    WHERE IdSolicitud = b.IdSolicitud AND IdDetalle IS NULL 
				ORDER BY Fecha DESC)) Motivo
	  FROM [Proveedor].[Pro_Proveedor] a,
	       [Articulo].[Art_Solicitud] b
		  where b.IdSolicitud = @CodigoSol 
		  and a.CodProveedor = b.CodProveedor


	  END
   ELSE
   BEGIN
	   SELECT
		   [CodProveedor]
		  ,[Ruc]
		  ,[NomComercial]
	  FROM [Proveedor].[Pro_Proveedor]
		  where [Ruc] = @Ruc 
	  END
 
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaProveedores_Not]'
END CATCH









CREATE PROCEDURE [Proveedor].[Pro_P_ConsApoderado] 
   @PI_ParamXML xml
AS
BEGIN
	
	DECLARE
		 @CodProveedor			varchar(10)
	
	SELECT
		@CodProveedor		= nref.value('@CodProveedor','varchar(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	select case p.ApoderadoIdFiscal 
	        when '' then p.Ruc else isnull(p.ApoderadoIdFiscal,p.Ruc) end as Apoderado , 
           p.NomComercial 
    from proveedor.Pro_Proveedor p
	where p.CodProveedor = @CodProveedor
END


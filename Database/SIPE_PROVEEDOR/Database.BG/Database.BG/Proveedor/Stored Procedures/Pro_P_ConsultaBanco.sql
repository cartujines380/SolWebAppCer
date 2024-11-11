create PROCEDURE [Proveedor].[Pro_P_ConsultaBanco]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 
		@CodPais		VARCHAR(3)
		
		
	
	SELECT
		
		@CodPais		= nref.value('@CodPais','VARCHAR(3)')
		

	FROM @PI_ParamXML.nodes('/Root') AS R(nref)
	
		select 
		         CodPais, CodBanco,  NomBanco,  Region,
				Direcion,	Poblacion,CodSWIFT,GrupoBancario,IndGiroCajapost,IndBorrado,
				CodBancario
		from 	[Proveedor].[Pro_Banco] a with(nolock)
		where a.CodPais=@CodPais

END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaBanco]'
END CATCH



CREATE PROCEDURE [Seguridad].[Seg_P_consDescAutorizacion]
@PI_IdAutorizacion int
AS
	
	Select * from Seguridad.Seg_V_Autorizacion
	  where IdAutorizacion = @PI_IdAutorizacion






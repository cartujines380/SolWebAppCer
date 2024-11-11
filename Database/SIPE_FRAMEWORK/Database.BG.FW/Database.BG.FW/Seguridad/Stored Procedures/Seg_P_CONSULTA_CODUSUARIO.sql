
create procedure [Seguridad].[Seg_P_CONSULTA_CODUSUARIO] 
@PV_Codusuario      VARCHAR(20),
@PO_Nombre    varchar(50) output
AS
 
         SELECT @PO_Nombre = Nombre
         FROM Seguridad.Seg_V_Usuario
         WHERE UPPER(IDUSUARIO) = UPPER(@PV_Codusuario)           
return






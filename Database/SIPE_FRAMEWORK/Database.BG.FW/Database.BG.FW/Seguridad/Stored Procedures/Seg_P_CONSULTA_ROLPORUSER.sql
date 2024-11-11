



create procedure [Seguridad].[Seg_P_CONSULTA_ROLPORUSER] 
@PV_Codusuario      VARCHAR(20 )
AS
         SELECT distinct r.idrol, r.descripcion
         FROM Seguridad.Seg_RolUsuario ur, Seguridad.Seg_Rol r
         WHERE ur.idusuario = @PV_Codusuario
               and r.idrol = ur.idrol
        
  






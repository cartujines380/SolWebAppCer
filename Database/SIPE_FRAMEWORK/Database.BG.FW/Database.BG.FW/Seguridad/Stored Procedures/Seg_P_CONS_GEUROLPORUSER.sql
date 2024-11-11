

CREATE procedure [Seguridad].[Seg_P_CONS_GEUROLPORUSER] 
@PV_Codusuario      VARCHAR(20 )
AS
        SELECT distinct '1' Sec, r.descripcion DesRol, convert(char,ur.fechainicial, 110) as FechaInicial, convert(char,ur.fechafinal, 105 )as FechaFinal,
                ur.usrreemplazo UsuarioReemp, re.nombre "o-NomreempUser", r.idrol "o-CodRol" 
         FROM Seguridad.Seg_RolUsuario ur, Seguridad.Seg_Rol r,
              Seguridad.Seg_Usuario re
         WHERE ur.idusuario = @PV_Codusuario
               and r.idrol = ur.idrol
               and re.idusuario = ur.usrreemplazo
        
  






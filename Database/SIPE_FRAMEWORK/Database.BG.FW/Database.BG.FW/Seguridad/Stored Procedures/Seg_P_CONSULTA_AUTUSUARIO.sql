
CREATE procedure [Seguridad].[Seg_P_CONSULTA_AUTUSUARIO] 
@PV_Codusuario      VARCHAR( 20)
AS
                    
  select distinct 1 as Sec, 
        au.IdAutorizacion, 
        convert(char,au.fechainicio,110) as  FechaInicio,
        convert(char,au.fechafin, 110) as  FechaFin, 
        au.IdUsuarioAutorizador,
        au.NumAutorizacion,
        au.Valor	        
  from  Seguridad.Seg_AutorizacionUsuario au,
              Seguridad.Seg_Autorizacion a
 where au.idusuario = @PV_Codusuario and
      a.idautorizacion = au.idautorizacion
          






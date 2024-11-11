

CREATE procedure [Seguridad].[Seg_P_CONSULTA_USUARIO] 
@PV_Codusuario      VARCHAR(20)
AS
         SELECT distinct u.idusuario, u.nombre, u.tipousuario,
         u.idempresa, u.descripcion, u.notificacion,
         u.correo, u.fax, u.estado, convert(varchar,u.fechaexpira,110) as fechaExpira,
         u.tiempoexpira, u.ruc, u.cheqequipo, u.oficial,
         u.nivelseg, u.nivelaut, u.fechaact, e.Nombre as DescEmpresa
         FROM Seguridad.Seg_Usuario u left outer join Seguridad.Seg_V_EMPRESA e
				ON u.Idempresa = e.IdEmpresa
         WHERE u.idusuario =@PV_Codusuario
               
         
  







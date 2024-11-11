


create procedure [Seguridad].[Seg_P_CONS_IDAUTORIZACION]
@PV_idAutorizacion     int
AS
     select IdAutorizacion, IdTransaccion, IdOpcion, a.IdOrganizacion, IdHorario, Parametro, Operador, ValorAutorizado, Descripcion
     from Seguridad.Seg_Autorizacion a, Seguridad.Seg_Organizacion o
     where a.IdAutorizacion=@PV_idAutorizacion and
           a.IdOrganizacion=o.IdOrganizacion
  






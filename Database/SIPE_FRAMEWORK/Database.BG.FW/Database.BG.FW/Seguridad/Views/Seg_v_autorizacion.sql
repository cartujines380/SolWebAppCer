
CREATE  view [Seguridad].[Seg_v_autorizacion] as
select IdAutorizacion , Parametro, Operador, ValorAutorizado, IdOrganizacion, IdTransaccion 
from Seguridad.Seg_Autorizacion 
 





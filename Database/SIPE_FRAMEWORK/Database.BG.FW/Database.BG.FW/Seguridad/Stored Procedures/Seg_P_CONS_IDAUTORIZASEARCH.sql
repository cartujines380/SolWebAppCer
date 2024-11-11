



create procedure [Seguridad].[Seg_P_CONS_IDAUTORIZASEARCH]
@PV_idAutorizacion     int
AS
     select IdAutorizacion, PARAMETRO
     from Seguridad.Seg_Autorizacion
    -- where IdAutorizacion=@PV_idAutorizacion
     where IdAutorizacion >=@PV_idAutorizacion
     
 






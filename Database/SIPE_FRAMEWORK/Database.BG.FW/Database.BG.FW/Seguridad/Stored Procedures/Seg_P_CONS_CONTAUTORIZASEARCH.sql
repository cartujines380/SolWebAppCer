



create procedure [Seguridad].[Seg_P_CONS_CONTAUTORIZASEARCH]
@PV_valor     VARCHAR(100 )
AS
     select IdAutorizacion, PARAMETRO
     from Seguridad.Seg_Autorizacion
     where UPPER(PARAMETRO) like '%' + UPPER(@PV_valor) + '%'   
   







CREATE Procedure [Seguridad].[Seg_P_consAplicacionesDatagParams]
AS

Select IdAplicacion, IdEmpresa, Nombre, Descripcion, TipoServidor, Datagrama, Link
 From Seguridad.Seg_Aplicacion a
 Where IdAplicacion <> 1 
 --and ( Datagrama <> '' or
 --exists (Select 1 From Seguridad.Seg_ParamAplicacion p Where p.IdAplicacion=a.IdAplicacion ))

Select IdAplicacion, Parametro, Valor
 From Seguridad.Seg_ParamAplicacion
 --Where IdAplicacion <> 1




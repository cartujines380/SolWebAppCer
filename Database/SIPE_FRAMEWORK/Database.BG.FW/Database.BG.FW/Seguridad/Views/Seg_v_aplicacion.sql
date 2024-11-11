


CREATE  view [Seguridad].[Seg_v_aplicacion] as
select IdAplicacion,
         Nombre, TipoServidor AS IdServidor,
         Catalogo.Ctl_F_ConCatalogo(100,TipoServidor) AS Servidor
    from Seguridad.Seg_APLICACION




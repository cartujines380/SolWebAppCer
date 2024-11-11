


create procedure [Seguridad].[Seg_P_CONS_EQUIPODES]
@PV_desEquipo     VARCHAR(100 )
AS
     select IDIDENTIFICACION1, Nombre
     from Seguridad.Seg_Equipo 
     where IDIDENTIFICACION1 like @PV_desEquipo + '%'
     
  






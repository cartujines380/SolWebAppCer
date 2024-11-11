



create procedure [Seguridad].[Seg_P_CONS_EQUIPOID]
@PV_idEquipo     int
AS
     select IDIDENTIFICACION1, Nombre
     from Seguridad.Seg_Equipo
     where IdEquipo = @PV_idEquipo
     
   






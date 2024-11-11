



create procedure [Seguridad].[Seg_P_CONS_AREADES]
@PV_desArea     VARCHAR(100 )
AS
     select Area, NOMBRE
     from Seguridad.Seg_Equipo
     where Area like  @PV_desArea + '%'
     
 









create procedure [Seguridad].[Seg_P_CONS_IDEQUIPO] 
@PV_idEquipo           int

AS 

  select e.*, em.nombre as "EMPRESA", s.nombre as "SUCURSAL", o.Nombre as "OFICINA"
  from Seguridad.Seg_Equipo e,Seguridad.Seg_V_EMPRESA em, Seguridad.Seg_Sucursal s, Seguridad.Seg_V_OFICINA o
  where e.idequipo=@PV_idEquipo and
         e.idempresa= em.idempresa  and
         e.idsucursal= s.idsucursal  and
         e.idoficina = o.idoficina 
  






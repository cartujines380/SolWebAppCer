



create procedure [Seguridad].[Seg_P_MAXIMA_OPCION] 
@PV_idTransaccion  int,
  @PV_idOrganizacion int
AS

 DECLARE @In_Count               int
 
  SET  @In_Count=0
   select @In_Count = ISNULL(max(idOpcion),0) +1 
     from Seguridad.Seg_Opciontrans
   where IdTransaccion= @PV_idTransaccion and
         IdOrganizacion= @PV_idOrganizacion
   if @In_Count=1 
     SElect 0
   else
       SElect @In_Count -1
  







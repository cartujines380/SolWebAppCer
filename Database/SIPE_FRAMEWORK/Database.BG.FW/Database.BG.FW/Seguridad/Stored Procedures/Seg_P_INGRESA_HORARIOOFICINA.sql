


/*==============================================================*/
/* MANTENIMIENTO: HORARIOOFICINA  */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_HORARIOOFICINA]
@PV_idHorario     int,
        @PV_idOficina     int
AS
   DECLARE @In_count int
    SET @In_count=0
     SELECT @In_count = count(*)
	   FROM Seguridad.Seg_HORARIOOFICINA
   WHERE  IDHORARIO=@PV_idHorario 
	AND IDOFICINA=@PV_idOficina
     
    IF @In_count = 0 
	BEGIN
     INSERT INTO Seguridad.Seg_HORARIOOFICINA(idhorario,idoficina)
            VALUES(@PV_idHorario,@PV_idOficina)
     --COMMIT
     END
 






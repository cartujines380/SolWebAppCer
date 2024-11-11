



create procedure [Seguridad].[Seg_P_ELIMINA_EQUIPO]
@PV_idEquipo              int
AS 
       DELETE FROM Seguridad.Seg_EQUIPO
       WHERE idequipo=@PV_idEquipo
       --COMMIT







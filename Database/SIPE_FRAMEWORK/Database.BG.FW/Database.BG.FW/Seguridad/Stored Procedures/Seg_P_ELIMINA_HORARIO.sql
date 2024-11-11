
create procedure [Seguridad].[Seg_P_ELIMINA_HORARIO]
@PV_idHorario           int
AS
	if not exists(SELECT 1
		FROM  Seguridad.Seg_HorarioTrans 
		WHERE IdHorario = @PV_idHorario)
	
	   and not exists(SELECT 1
		FROM  Seguridad.Seg_RolUsuario 
		WHERE IdHorario = @PV_idHorario)

	  Begin
		DELETE FROM Seguridad.Seg_HorarioDia
	     	WHERE idhorario=@PV_idHorario

		DELETE FROM Seguridad.Seg_HORARIO
	     	WHERE idhorario=@PV_idHorario
	  End			
	else
	     raiserror ('Error: No puede eliminar Horario, porque esta siendo referenciado',16,1)






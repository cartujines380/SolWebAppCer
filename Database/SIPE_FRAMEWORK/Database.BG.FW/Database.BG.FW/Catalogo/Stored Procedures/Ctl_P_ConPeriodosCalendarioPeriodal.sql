create PROCEDURE [Catalogo].[Ctl_P_ConPeriodosCalendarioPeriodal] 
	@PI_Año smallint = NULL 
AS 
	IF ( @PI_Año is null ) 
		Select @PI_Año = isnull( Catalogo.Ctl_F_conAño( getdate() ), YEAR(getdate()) ) 

	SELECT Periodo FROM Catalogo.Ctl_Periodo 
	WHERE Año = @PI_Año 






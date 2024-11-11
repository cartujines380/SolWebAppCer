create PROCEDURE [Catalogo].[Ctl_P_conFechasCalendarioPeriodal] ( 
	@PI_Año smallint = NULL, @PI_Periodo tinyint, @PI_Semana tinyint 
 ) 
AS 
  set @PI_Año = isnull( @PI_Año, year(getdate()) ) 
  Select 	
	FechaInicio = Catalogo.Ctl_F_Fecha( 
	 Catalogo.Ctl_F_conFechaCalendarioPeriodal ( @PI_Año, @PI_Periodo, @PI_Semana, 1 )
	), 
	FechaFin = Catalogo.Ctl_F_Fecha( 
	 Catalogo.Ctl_F_conFechaCalendarioPeriodal ( @PI_Año, @PI_Periodo, @PI_Semana, 0 ) 
	) 






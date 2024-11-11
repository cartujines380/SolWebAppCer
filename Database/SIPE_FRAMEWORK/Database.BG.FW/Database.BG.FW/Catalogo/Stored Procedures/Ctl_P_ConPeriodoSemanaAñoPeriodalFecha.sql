create PROCEDURE [Catalogo].[Ctl_P_ConPeriodoSemanaAñoPeriodalFecha] ( @PI_Fecha varchar(10) ) 
AS
	DECLARE @VL_Fecha as datetime 
	SET @VL_Fecha =  @PI_Fecha 
	Select Periodo = Catalogo.Ctl_F_conPeriodo ( @VL_Fecha ), 
		Semana = Catalogo.Ctl_F_conSemana ( @VL_Fecha ), 
		Año = Catalogo.Ctl_F_conAño ( @VL_Fecha ) 





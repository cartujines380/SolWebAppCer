create view [Catalogo].[Ctl_V_ParametroParticipante] ( 
	IdParametro ,
	Descripcion)  
AS
	Select convert(int,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'ParametroParticipante'
		AND t.IdTabla = c.IdTabla 






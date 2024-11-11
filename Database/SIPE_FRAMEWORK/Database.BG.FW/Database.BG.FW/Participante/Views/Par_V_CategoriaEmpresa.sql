create   VIEW [Participante].[Par_V_CategoriaEmpresa]
AS
	SELECT IdCategoriaEmpresa as Codigo,
    	Descripcion, Nivel ,
    	IdCategoriaEmpPadre as "CODIGOPADRE"
    FROM Participante.Par_CategoriaEmpresa








create  procedure [Participante].[Par_P_ConCategoriaEmpresaId] 
@PI_IdCategoria int
AS
	
	select c.Descripcion,
	c.IdCategoriaEmpPadre,c.Nivel,
	cp.Descripcion as DescCategoriaEmpPadre
	FROM Participante.Par_CategoriaEmpresa c, Participante.Par_CategoriaEmpresa cp
	where   c.IdCategoriaEmpresa = @PI_IdCategoria
		and c.IdCategoriaEmpPadre = cp.IdCategoriaEmpresa
		





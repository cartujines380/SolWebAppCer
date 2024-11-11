CREATE PROCEDURE [Catalogo].[Ctl_P_conZonaEmpresa]
@IdEmpresa int
AS
SELECT Codigo, Catalogo.Ctl_F_conCatalogo(IdTabla,Codigo) as Descripcion
	FROM Catalogo.Ctl_CatalogoEmpresa
	WHERE IdEmpresa = @IdEmpresa AND IdTabla = 1 -- 1 = Zonas





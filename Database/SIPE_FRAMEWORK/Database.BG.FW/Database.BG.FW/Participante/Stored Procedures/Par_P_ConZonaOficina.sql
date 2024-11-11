
CREATE PROCEDURE [Participante].[Par_P_ConZonaOficina]
	@PI_IdEmpresa  int,
	@PI_IdOficina  int
AS
	SELECT o.IdZona, Catalogo.Ctl_F_conCatalogo(1,o.IdZona) Descripcion
	FROM   Participante.Par_Oficina o
	WHERE  o.IdEmpresa = @PI_IdEmpresa 
	  	AND o.IdOficina = @PI_IdOficina 

	







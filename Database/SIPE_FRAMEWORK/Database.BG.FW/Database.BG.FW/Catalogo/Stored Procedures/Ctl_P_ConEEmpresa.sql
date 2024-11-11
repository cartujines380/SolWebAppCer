CREATE PROC  [Catalogo].[Ctl_P_ConEEmpresa] 
	@PI_IdModulo int, 
	@PI_IdEmpresa int 
AS 

SELECT 
	IdModulo, 
	IdEmpresaPadre, 
	IdEmpresaHija, 
	  EmpresaHija = Participante.Par_F_getNombreParticipante(IdEmpresaHija) 
  FROM .Catalogo.Ctl_EEmpresa 
  WHERE IdModulo = @PI_IdModulo 
	AND IdEmpresaPadre = @PI_IdEmpresa 
  ORDER BY EmpresaHija 






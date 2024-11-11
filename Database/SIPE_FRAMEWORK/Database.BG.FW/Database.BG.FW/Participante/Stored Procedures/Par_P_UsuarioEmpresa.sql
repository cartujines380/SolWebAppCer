CREATE PROCEDURE [Participante].[Par_P_UsuarioEmpresa] 
@PI_IdEmpresa int
AS
if @PI_IdEmpresa = 1

Select Idusuario, Nombre
from Participante.Par_V_Empleado
else

Select Idusuario, Nombre
from Participante.Par_V_Empleado
where
IdEmpresa=@PI_IdEmpresa




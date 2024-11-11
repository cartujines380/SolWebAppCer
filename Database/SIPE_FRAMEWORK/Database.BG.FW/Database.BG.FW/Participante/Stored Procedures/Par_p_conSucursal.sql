


CREATE   procedure [Participante].[Par_p_conSucursal]
@IdEmpresa int
as

select IdSucursal, Nombre 
from Sucursal where 
IdEmpresa = @IdEmpresa 









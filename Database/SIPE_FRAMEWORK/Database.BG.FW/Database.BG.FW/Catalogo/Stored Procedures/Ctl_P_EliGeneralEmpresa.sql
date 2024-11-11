create proc [Catalogo].[Ctl_P_EliGeneralEmpresa]
(
    @PI_IdEmpresa int
)
as

if exists (select top 1 1 from Catalogo.Ctl_ParametroGEmpresa
            where IdEmpresa = @PI_IdEmpresa)
begin
    delete Catalogo.Ctl_ParametroGEmpresa
    where IdEmpresa = @PI_IdEmpresa
    if @@error <> 0
        raiserror ('Error de Eliminacion de Parametro General de Empresa',16,1)
end
else
    raiserror('Parametro General de Empresa no ha sido registrado',16,1)
return





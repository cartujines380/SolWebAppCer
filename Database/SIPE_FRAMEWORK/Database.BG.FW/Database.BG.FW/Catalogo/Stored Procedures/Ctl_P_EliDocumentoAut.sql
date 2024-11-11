create proc [Catalogo].[Ctl_P_EliDocumentoAut]
(
    @PI_IdEmpresa int,
    @PI_IdOficina int,
    @PI_IdDocumento varchar(20),
    @PI_PuntoVenta varchar(5)
)
as

if exists (select top 1 1 from Catalogo.Ctl_DocumentoAutorizado
            where IdEmpresa = @PI_IdEmpresa and 
                  IdOficina = @PI_IdOficina and
                  IdDocumento = @PI_IdDocumento and
                  PuntoVenta = @PI_PuntoVenta)
begin
    begin tran
        delete Catalogo.Ctl_DocumentoAutorizado
        where IdEmpresa = @PI_IdEmpresa and 
              IdOficina = @PI_IdOficina and
              IdDocumento = @PI_IdDocumento and
              PuntoVenta = @PI_PuntoVenta
        if @@error <> 0
        begin
            rollback
            return
        end
                

    commit tran
end
else
    raiserror('Documento Autorizado no ha sido registrado',16,1)
return





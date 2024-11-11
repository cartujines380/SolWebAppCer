create proc [Catalogo].[Ctl_P_SigDocAutorizado]
(
    @PI_IdEmpresa int,
    @PI_IdOficina int,
    @PI_IdDocumento varchar(20),
    @PI_PuntoVenta varchar(5),
    @PO_Siguiente int output, 
    @PO_SigDocAut varchar(50) = '' output
)
as
SET NOCOUNT ON

declare @VL_Siguiente int,
        @VL_SecFin int,
        @VL_Doc varchar(20),
        @VL_OficinaRef varchar(5),
        @VL_PuntoVenta varchar(5),
        @VL_SigDoc varchar(6)

    SET @VL_Siguiente = 0

    begin tran
        select 
            @VL_Doc = IdDocumento,
            @VL_Siguiente = isnull(SecuenciaInicial,0) + 1,
            @VL_SecFin = isnull(SecuenciaFinal,0),
            @VL_OficinaRef = right(replicate('0', convert(tinyint,NumDigitosOficina))+ convert(varchar,OficinaRef), convert(tinyint,NumDigitosOficina)),
            @VL_PuntoVenta = PuntoVenta,
            @VL_SigDoc = right(replicate('0', convert(tinyint, NumDigitos)) + convert(varchar, SecuenciaInicial + 1), convert(tinyint, NumDigitos))
        from Catalogo.Ctl_DocumentoAutorizado
        where 
            IdEmpresa = @PI_IdEmpresa and 
            IdOficina = @PI_IdOficina and 
            IdDocumento = @PI_IdDocumento and
            PuntoVenta = @PI_PuntoVenta
        if @@rowcount = 0
        begin
            rollback
            raiserror(55006, 16, 1, @PI_IdDocumento)
            return -1
        end        
        if @VL_Siguiente > @VL_SecFin
        begin
            rollback
            raiserror(55007,16,1,@PI_IdDocumento)
            return -1
        end
        
        update Catalogo.Ctl_DocumentoAutorizado
        set
            SecuenciaInicial = @VL_Siguiente
        where 
            IdEmpresa = @PI_IdEmpresa and 
            IdOficina = @PI_IdOficina and 
            IdDocumento = @PI_IdDocumento and
            PuntoVenta = @PI_PuntoVenta

    commit tran
  
    select @PO_Siguiente = @VL_Siguiente  
    select @PO_SigDocAut = @VL_Doc + '-' + @VL_OficinaRef + '-' + @VL_PuntoVenta + '-' + @VL_SigDoc

return 0






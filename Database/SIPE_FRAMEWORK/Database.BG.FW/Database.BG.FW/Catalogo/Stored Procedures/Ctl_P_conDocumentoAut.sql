create proc [Catalogo].[Ctl_P_conDocumentoAut]
(
    @PI_IdEmpresa int,
    @PI_IdOficina int,
    @PI_IdDocumento varchar(20),
    @PI_PuntoVenta varchar(5)
)
as

SELECT 
    NumAutorizacion, 
    Descripcion, 
    convert(varchar(10),FechaAutorizacion, 101) as FechaAutorizacion,
    Vigencia, 
    TipoVigencia, 
    OficinaRef,
    PuntoVenta, 
    NumDigitos, 
    NumDigitosOficina, 
    SecuenciaInicial, 
    SecuenciaFinal 
FROM Catalogo.Ctl_DocumentoAutorizado
WHERE 
    IdEmpresa = @PI_IdEmpresa and
    IdOficina = @PI_IdOficina and
    IdDocumento = @PI_IdDocumento and
    PuntoVenta = @PI_PuntoVenta and
    Estado = 'A'

return





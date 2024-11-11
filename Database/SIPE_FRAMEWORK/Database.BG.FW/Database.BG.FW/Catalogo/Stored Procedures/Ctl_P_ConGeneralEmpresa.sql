create proc [Catalogo].[Ctl_P_ConGeneralEmpresa]
(
    @PI_IdEmpresa int
)
as
SELECT 
    Dig_CostoU, 
    Dig_PvpU, 
    Dig_Cantidad, 
    Num_OChqVou, 
    Multimoneda, 
    MonedaDef 
FROM Catalogo.Ctl_ParametroGEmpresa
WHERE 
    IdEmpresa = @PI_IdEmpresa

return





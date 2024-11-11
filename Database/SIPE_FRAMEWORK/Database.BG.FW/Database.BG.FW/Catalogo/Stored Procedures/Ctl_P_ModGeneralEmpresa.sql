create proc [Catalogo].[Ctl_P_ModGeneralEmpresa]
(
    @PI_docXML varchar(8000)
)
as
declare 
    @VL_IdXML int,
    @VL_IdEmpresa int

SET NOCOUNT ON

exec sp_xml_preparedocument @VL_IdXML OUTPUT, @PI_docXML

select 
    @VL_IdEmpresa = IdEmpresa
from openxml(@VL_IdXML,'/Usuario/ResultSet/General') 
    WITH Catalogo.Ctl_ParametroGEmpresa

if exists (select top 1 1 from Catalogo.Ctl_ParametroGEmpresa
            where IdEmpresa = @VL_IdEmpresa)
begin
    UPDATE Catalogo.Ctl_ParametroGEmpresa
    SET 
        Dig_CostoU=doc.Dig_CostoU,
        Dig_PvpU=doc.Dig_PvpU,
        Dig_Cantidad=doc.Dig_Cantidad,
        Num_OChqVou=doc.Num_OChqVou,
        Multimoneda=doc.Multimoneda,
        MonedaDef=doc.MonedaDef
    FROM 
        Catalogo.Ctl_ParametroGEmpresa Ctl,
        OPENXML (@VL_IdXML, '/Usuario/ResultSet/General') 
            WITH Catalogo.Ctl_ParametroGEmpresa doc
    WHERE 
        Ctl.IdEmpresa = doc.IdEmpresa

    if @@error <> 0
        raiserror ('Error de Actualizacion de Parametro General de Empresa',16,1)   
end
else
begin
    raiserror('Parametro General de Empresa no ha sido registrado',16,1)
end

EXEC sp_xml_removedocument @VL_IdXML

return





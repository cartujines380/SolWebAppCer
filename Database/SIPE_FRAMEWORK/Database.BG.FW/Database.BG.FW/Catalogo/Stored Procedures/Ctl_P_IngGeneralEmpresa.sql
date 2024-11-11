create proc [Catalogo].[Ctl_P_IngGeneralEmpresa]
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


if not exists (select top 1 1 from Catalogo.Ctl_ParametroGEmpresa
            where IdEmpresa = @VL_IdEmpresa)
begin

    INSERT INTO Catalogo.Ctl_ParametroGEmpresa
        (
        IdEmpresa, Dig_CostoU, Dig_PvpU, Dig_Cantidad, Num_OChqVou, Multimoneda, MonedaDef
        )
    SELECT IdEmpresa, Dig_CostoU, Dig_PvpU, Dig_Cantidad, Num_OChqVou, Multimoneda, MonedaDef 
	FROM OPENXML (@VL_IdXML, '/Usuario/ResultSet/General')
    	WITH Catalogo.Ctl_ParametroGEmpresa

    if @@error<> 0
        raiserror ('Error de insercion de parametros Generales por Empresa',16,1)
end
else
begin
    raiserror('Parametro General de Empresa ya ha sido registrado',16,1)
end

EXEC sp_xml_removedocument @VL_IdXML

return





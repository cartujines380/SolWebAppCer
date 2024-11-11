
--exec Catalogo.Ctl_P_consDocumentoMotivo 2,3,2,'I'
CREATE proc [Catalogo].[Ctl_P_consDocumentoMotivo]
(
    @PI_IdEmpresa    int,
    @PI_IdModulo     int,
    @PI_IdMotivo     int,
    @PI_IdTipoMotivo char(1)=null    -- E=Egreso, I=Ingreso -->"No utlizado actualmente"
)
as
select ca.DescAlterno
from Sige_contabilidad.con_Motivo mo , Catalogo.Ctl_Catalogo ca
where 

	mo.IdTabla=ca.IdTabla and
	mo.IdTabla=8 and -- 8 relacionado a los tipos documentos en la tabla Catalogo
	mo.IdDocumento=ca.Codigo and
    mo.IdEmpresa = @PI_IdEmpresa and
    mo.IdModulo =  @PI_IdModulo and
    mo.IdMotivo = @PI_IdMotivo



if @@rowcount = 0
    select 'N/A' as DescAlterno


--//***************************************** ANTES
--as
--select IdenDocumento
--from Catalogo.Ctl_MotivoDocumento
--where 
--    IdEmpresa = @PI_IdEmpresa and
--    IdModulo =  @PI_IdModulo and
--    IdMotivo = @PI_IdMotivo and
--    TipoMotivo = @PI_IdTipoMotivo
--if @@rowcount = 0
--    select 'N/A' as IdenDocumento

-- Catalogo.Ctl_P_consDocumentoMotivo 2,3,2
--//****************************************







CREATE PROC  [Catalogo].[Ctl_P_ConsultaCatalogo] 
@PI_CodigoTabla int
AS
       --SELECT Codigo, Descripcion, DescAlterno, Estado = case Estado when 'A' then 'HABILITADO' when 'I' then 'INHABILITADO' end
       SELECT Codigo, Descripcion, DescAlterno, Estado
       FROM Catalogo.Ctl_Catalogo
       Where IdTabla = @PI_CodigoTabla






CREATE PROC [Catalogo].[Ctl_P_ConsultaTabla] 
@PI_Codigo int
AS
       --DECLARE @VL_Sec int
       --SELECT @VL_SEc = isnull(max(c.Codigo),0)
       --FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
       --WHERE t.IdTabla = c.IdTabla and t.IdTabla = @PI_Codigo
       SELECT Nombre, Descripcion --, @VL_SEc as MaxSec
       FROM Catalogo.Ctl_Tabla 
       Where IdTabla = @PI_Codigo





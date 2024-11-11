CREATE PROC  [Catalogo].[Ctl_P_ConsCatalogoNombre]
@PI_Nombre varchar(64)
AS
       SELECT Catalogo.Ctl_Catalogo.Codigo , Catalogo.Ctl_Catalogo.Descripcion , Catalogo.Ctl_Catalogo.DescAlterno 
       FROM Catalogo.Ctl_Catalogo, Catalogo.Ctl_Tabla
       Where Catalogo.Ctl_Tabla.idTabla = Catalogo.Ctl_Catalogo.idTabla and
             Catalogo.Ctl_Tabla.Nombre = @PI_Nombre and Catalogo.Ctl_Catalogo.Estado = 'A'






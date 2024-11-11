CREATE PROC  [Catalogo].[Ctl_P_ConsCatalogoUsuario]
@PI_Nombre varchar(50),
@PI_Alterno char(1),
@PI_Usuario varchar(20)

AS
declare @VL_IdParticipante int

	select @VL_IdParticipante = IdParticipante
	from .Participante.Par_Participante where IdUsuario = @PI_Usuario

        SELECT distinct Catalogo.Ctl_Catalogo.Codigo , Catalogo.Ctl_Catalogo.Descripcion  
        FROM Catalogo.Ctl_Catalogo, Catalogo.Ctl_Tabla, .Participante.Par_Producto p
        Where Catalogo.Ctl_Tabla.idTabla = Catalogo.Ctl_Catalogo.idTabla and
             Catalogo.Ctl_Tabla.Nombre = @PI_Nombre and Catalogo.Ctl_Catalogo.Estado = 'A'
	     and Catalogo.Ctl_Catalogo.DescAlterno = @PI_Alterno
	     and Catalogo.Ctl_Catalogo.codigo = p.TipoProducto
	     and p.IdParticipante = @VL_IdParticipante
	     and p.EstadoProducto = 1




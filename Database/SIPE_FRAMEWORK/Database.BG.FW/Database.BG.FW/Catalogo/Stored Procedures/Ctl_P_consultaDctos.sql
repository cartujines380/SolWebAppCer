create proc [Catalogo].[Ctl_P_consultaDctos]
as
select * from .Catalogo.Ctl_Catalogo
where IdTabla=8 order by Descripcion




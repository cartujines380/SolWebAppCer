USE SIPE_FRAMEWORK
go

select  GETDATE() INICIO
go
declare @v_idapl int
set @v_idapl  = 27

delete from Seguridad.Seg_ParamAplicacion 
where IdAplicacion = @v_idapl 
and Parametro in ('sftpServerBG','sftpUserBG','sftpPassBG','sftpRutaBG')

insert into Seguridad.Seg_ParamAplicacion(IdAplicacion,Parametro,Valor,Encriptado) 
values
(@v_idapl,'sftpServerBG','10.2.7.239',0),
(@v_idapl,'sftpUserBG','usrsftp',0),
(@v_idapl,'sftpPassBG','password',0),
(@v_idapl,'sftpRutaBG','\',0)
go

select  GETDATE() FIN
go


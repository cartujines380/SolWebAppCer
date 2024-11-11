
use SIPE_FRAMEWORK
go

declare @v_trx int 
set @v_trx = 2380

insert into Seguridad.Seg_Transaccion
(IdOrganizacion,IdTransaccion,Descripcion,Estado,Parametros,Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,Menu,Monitor)
values(39, @v_trx, 'Mantenimientos', 'A', '<SP></SP>', 'N',0, '','',0,1,0)
insert Seguridad.Seg_OpcionTrans (IdOrganizacion, IdTransaccion, IdOpcion, Descripcion, Nivel)
values (39,@v_trx,'1', 'default','0')
insert Seguridad.Seg_OpcionTransRol (IdRol, IdOrganizacion, IdTransaccion, IdOpcion)
values 
(22,39,@v_trx,'1') ,
(23,39,@v_trx,'1') ,
(24,39,@v_trx,'1') ,
(28,39,@v_trx,'1') 
insert Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
values (39,@v_trx,'1','1')

set @v_trx = 2381
insert into Seguridad.Seg_Transaccion
(IdOrganizacion,IdTransaccion,Descripcion,Estado,Parametros,Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,Menu,Monitor)
values(39, @v_trx, 'Documentos adjuntos matriculación', 'A', '<SP></SP>', 'N',0, '','',0,1,0)
insert Seguridad.Seg_OpcionTrans (IdOrganizacion, IdTransaccion, IdOpcion, Descripcion, Nivel)
values (39,@v_trx,'1', 'default','0')
insert Seguridad.Seg_OpcionTransRol (IdRol, IdOrganizacion, IdTransaccion, IdOpcion)
values 
(22,39,@v_trx,'1') ,
(23,39,@v_trx,'1') ,
(24,39,@v_trx,'1') ,
(28,39,@v_trx,'1') 
insert Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
values (39,@v_trx,'1','1')


go



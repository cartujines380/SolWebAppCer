
use SIPE_FRAMEWORK
go


declare @v_trx int ,
        @v_rol int
set @v_trx = 2260
set @v_rol = 34

insert into SIPE_FRAMEWORK.Seguridad.Seg_Rol values(@v_rol,2,0,'Seguridad Integral', 'ACTIVE', 'Seguridad Integral', 'SEG');

insert into Seguridad.Seg_Transaccion
(IdOrganizacion,IdTransaccion,Descripcion,Estado,Parametros,Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,Menu,Monitor)
values(39, @v_trx, 'Solicitudes de nuevos (SE)', 'A', '<SP></SP>', 'N',0, '','',0,1,0)
insert Seguridad.Seg_OpcionTrans (IdOrganizacion, IdTransaccion, IdOpcion, Descripcion, Nivel)
values (39,@v_trx,'1', 'default','0')
insert Seguridad.Seg_OpcionTransRol (IdRol, IdOrganizacion, IdTransaccion, IdOpcion)
values 
(22,39,@v_trx,'1') ,
(23,39,@v_trx,'1') ,
(24,39,@v_trx,'1') ,
(@v_rol,39,@v_trx,'1') 
insert Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
values (39,@v_trx,'1','1')

set @v_trx = 2265
set @v_rol = 35

insert into SIPE_FRAMEWORK.Seguridad.Seg_Rol values(@v_rol,2,0,'Seguridad de la Informacion', 'ACTIVE', 'Seguridad de la Informacion', 'SDI');

insert into Seguridad.Seg_Transaccion
(IdOrganizacion,IdTransaccion,Descripcion,Estado,Parametros,Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,Menu,Monitor)
values(39, @v_trx, 'Solicitudes de nuevos (SI)', 'A', '<SP></SP>', 'N',0, '','',0,1,0)
insert Seguridad.Seg_OpcionTrans (IdOrganizacion, IdTransaccion, IdOpcion, Descripcion, Nivel)
values (39,@v_trx,'1', 'default','0')
insert Seguridad.Seg_OpcionTransRol (IdRol, IdOrganizacion, IdTransaccion, IdOpcion)
values 
(22,39,@v_trx,'1') ,
(23,39,@v_trx,'1') ,
(24,39,@v_trx,'1') ,
(@v_rol,39,@v_trx,'1') 
insert Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
values (39,@v_trx,'1','1')


go



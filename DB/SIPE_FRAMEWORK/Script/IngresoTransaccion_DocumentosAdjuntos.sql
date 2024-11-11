use [SIPE_FRAMEWORK]
go

declare @idTran int, 
		@idOrg  int

select	@idTran= 106,
		@idOrg = 39

if not exists (select 1 
	from [SIPE_FRAMEWORK].[Seguridad].[Seg_Transaccion]
	where IdTransaccion = @idTran and IdOrganizacion = @idOrg
)
begin
	insert into [SIPE_FRAMEWORK].[Seguridad].[Seg_Transaccion]
	values(
	@idOrg,@idTran,'Mantenimineto Doc Adjuntos','A',null,
	'<SP nombre=''SIPE_PROVEEDOR.Proveedor.Pro_P_DocumentosAdjuntos''>  
	<Param nombre=''@PI_ParamXML'' tipo=''xml'' longitud=''-1'' posicion=''0'' direccion=''input'' />  	  
	</SP>',
	'S',null,null,27,'SIPE_PROVEEDOR','Proveedor.Pro_P_DocumentosAdjuntos',27,
	0,0,'','',''
	)
end

if not exists(
	select 1 
	from SIPE_FRAMEWORK.Seguridad.Seg_OpcionTrans
	where IdTransaccion = @idTran and IdOrganizacion = @idOrg
)
begin
	insert into SIPE_FRAMEWORK.Seguridad.Seg_OpcionTrans
	values(@idOrg,@idTran,1,'default',0)
end

if not exists(select 1
	from SIPE_FRAMEWORK.Seguridad.Seg_OpcionTransRol
	where IdTransaccion = @idTran and IdOrganizacion = @idOrg
)
begin
	insert into SIPE_FRAMEWORK.Seguridad.Seg_OpcionTransRol
	values(24,@idOrg,@idTran,1)
end

if not exists(select 1
	from SIPE_FRAMEWORK.Seguridad.Seg_HorarioTrans
	where IdTransaccion = @idTran and IdOrganizacion = @idOrg
)
begin
	insert into SIPE_FRAMEWORK.Seguridad.Seg_HorarioTrans
	values(@idOrg, @idTran, 1, 1)
end
go



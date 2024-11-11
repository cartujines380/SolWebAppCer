 create   Proc [MTRA].[Mtr_P_Cli_BandejaRecep]
(
  @PI_IdUsuario varchar(50)
)
AS
	declare @lv_IdParticipante int
	declare @lv_FechaInicioDia datetime	
	declare @tb_Bandeja 
	table (
			IdTarea int,
			Identificador int,
			NombreTarea varchar(100),			
			Cli_Encriptado bit,
			Cli_Compresion bit,
			TipoArchivo varchar(50),
			Archivo varchar(100),			
			Correo varchar(100),
			MetodoTransmision varchar(50),
			DescMetodoTransmision varchar(100),
			IdParticipante int,
			TipoUsuario varchar(50),						
			horaIni datetime,
			horaFin datetime,
			horaProgramada datetime,				   
		    TipoProceso char(1),
		    EstatusTarea varchar(50),
			DescEstatusTarea varchar(50),
			IdServidorFuente int,
			RutaServidorFuente	 varchar(500),
			Compresion bit,
			Encriptado bit,
			esBinario bit,
			EnviarMailCliente bit,
            MailCliente varchar(500),
            RenombreArchivo varchar(100)

															    
	)
  
Set @lv_FechaInicioDia = convert(datetime,convert(char(10),getdate(),103),103)
select @lv_IdParticipante = IdParticipante from SIPE_FrameWork.Participante.Par_Participante
where IdUsuario  = @PI_IdUsuario

insert into @tb_Bandeja 
(
-- columnas TR tareaResumen
IdTarea,Identificador,horaIni,horaFin,horaProgramada,EstatusTarea,
TipoProceso,MetodoTransmision,
-- columnas T Tarea
NombreTarea,Cli_compresion,cli_encriptado,
TipoArchivo,Archivo,correo,IdParticipante,TipoUsuario,IdServidorFuente,RutaServidorFuente
,Compresion,Encriptado,esBinario,
			EnviarMailCliente,
            MailCliente,
            RenombreArchivo
-- Columnas TS Schedule
)
select 
-- columnas TR tareaResumen
tr.IdTarea,tr.Identificador,tr.Fecha_IniTransmision,tr.Fecha_finTransmision,
tr.Fecha_Ejecucion,tr.EstatusTarea,tr.TipoProceso,tr.MetodoTransmision,
-- columnas T Tarea
t.nombreTarea,t.Cli_compresion,t.cli_encriptado,
t.TipoArchivo,t.Archivo,t.correo,t.IdUsuario,t.TipoUsuario,t.IdServidorFuente,
t.RutaServidorFuente, t.Compresion,t.Encriptado,t.esBinario,
t.EnviarMailCliente,t.MailCliente,t.RenombreArchivo
-- Columnas TS Schedule

from mtra.mtr_TareaResumen tr
	 inner join mtra.mtr_Tarea t
	 on  t.idtarea = tr.idTarea
	 and t.idUsuario  = @lv_IdParticipante
where tr.Fecha_Ejecucion > @lv_FechaInicioDia
and tr.TipoProceso = 'T'


update @tb_Bandeja
set DescEstatusTarea = SIPE_FrameWork.Catalogo.Ctl_F_conCatalogo(228, EstatusTarea),
	DescMetodoTransmision = SIPE_FrameWork.Catalogo.Ctl_F_conCatalogo(225, MetodoTransmision) 

select *,
case when MetodoTransmision in (3)
     Then '1'
     else '0' end Ejecutar
from @tb_Bandeja
order by horaProgramada


/*
set rowcount 1
select * from mtra.mtr_tarea
select * from mtra.mtr_tareaschedule
select * from mtra.mtr_tarearesumen
set rowcount 0
*/







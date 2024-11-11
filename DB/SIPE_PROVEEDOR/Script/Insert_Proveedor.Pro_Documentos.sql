/****** Script for SelectTopNRows command from SSMS  ******/
use SIPE_PROVEEDOR

delete from Proveedor.Pro_Documentos
  
  insert into Proveedor.Pro_Documentos values
  (	'PN'	,	'ID'	,	'C�dula de Identidad o Pasaporte en caso de ser extranjero'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'RU'	,	'RUC actualizado'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'SB'	,	'Copia de la planilla de luz, agua o tel�fono'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'RC'	,	'Referencias comerciales y bancarias'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'EF'	,	'Estados financieros (�ltimos actualizados) y/o Declaraci�n del impuesto a la renta de los tres �ltimos a�os'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'PI'	,	'Copia del pago de la �ltima planilla del IESS',	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'FS'	,	'Formulario de solicitud proveedor, debidamente firmado o formulario(s) entregado(s) por el tercero contratado para efectuar la evaluaci�n'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'RU'	,	'RUC Registro �nico de contribuyentes'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'EC'	,	'Escrituras de Constituci�n, Estatutos y Reformas vigentes'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'RL'	,	'Nombramiento del representante legal inscrito en el registro mercantil'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'ID'	,	'C�dula de Identidad del representante legal o copia de pasaporte en caso de ser extranjero'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'NA'	,	'N�mina de Accionistas'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'SB'	,	'Copia de la planilla de luz, agua o tel�fono'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'RC'	,	'Referencias comerciales y Bancarias'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'EF'	,	'Estados financieros (�ltimos actualizados) y/o Declaraci�n del impuesto a la renta de los tres �ltimos a�os, debidamente sellado en la Superintendencia de Compa��as, Valores y Seguros'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'PI'	,	'Copia del pago de la �ltima planilla del IESS'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PN'	,	'SGS'	,	'Certificado de calificaci�n de SGS'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'PJ'	,	'SGS'	,	'Certificado de calificaci�n de SGS'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'CO'	,	'CV'	,	'Certificado Vigente ISO 27001'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	),
(	'CO'	,	'EH'	,	'Resumen ejecutivo actualizado de Ethical Hacking o escaneos de vulnerabilidades (interno y externo) sin hallazgos de severidad cr�ticas o altas pendientes de remediar.'	,	'S'	,	getdate()	,	'ADM'	,	NULL	,	NULL	,	'A'	)


select * from Proveedor.Pro_Documentos
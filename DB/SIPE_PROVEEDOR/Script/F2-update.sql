USE SIPE_PROVEEDOR
GO

update Proveedor.Pro_Sociedad 
set RepresentanteLegal = 'Francisco Jaramillo',
	RucSociedad = '1712546738001',
	CodActividadEconomica = 'SFI',
	Direccion = 'Pichincha y P.Icaza',
	Locacion = 'Guayaquil',
	Correo = 'sipeprueba2@yopmail.com',
	Telefono = '043730100'
where Activar='1'
and IdSociedad = 7777

update Proveedor.Pro_Catalogo
set DescAlterno ='01|ADQUIRIR BIENES DEL PROVEEDOR'
where Tabla = 9899 and Codigo = 'CONTIPBIEN'

update Proveedor.Pro_Catalogo
set DescAlterno ='02|LOS SERVICIOS'
where Tabla = 9899 and Codigo = 'CONTIPSERV'

update Proveedor.Pro_Catalogo
set DescAlterno ='QUE TIENE POR OBJETO'
where Tabla = 1032 and Codigo = 'PJ'

update Proveedor.Pro_Catalogo
set DescAlterno ='QUE SE DEDICA A'
where Tabla = 1032 and Codigo = 'PN'

update Licitacion.Lic_RolesWorkflow 
set Nombre = 'APODERADO2'
where IdRol = 4


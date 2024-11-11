declare @v_max int
select @v_max = MAX(Tabla)+1 from Proveedor.Pro_Tabla where Estado = 'A'

if not exists(select 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_FormaPagoBG') 
begin
	insert into Proveedor.Pro_Tabla values(@v_max,'tbl_FormaPagoBG','A')
	
	insert into Proveedor.Pro_Catalogo 
	values(@v_max,'CC','Crédito a Cuenta','A','Valor Acreditado y disponible'),
		(@v_max,'CA','Crédito Bancos Locales','A','Valor Acreditado y disponible'),
		(@v_max,'GE','Pago con Cheque','A','Cheque disponible en Ventanilla'),
		(@v_max,'TJ','Crédito a Tarjeta','A','Valor Disponible en 48 horas - No Se Incluyen Las Comisiones, Ni Retenciones'),
		(@v_max,'SP','Crédito Interbancaria','A','Valor Disponible en 48 horas')

end
	
go

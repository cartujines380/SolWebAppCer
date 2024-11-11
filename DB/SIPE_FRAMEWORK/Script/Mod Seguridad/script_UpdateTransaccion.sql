USE [SIPE_FRAMEWORK]
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET [Auditable] = 'S'
 WHERE  [Menu] = 1
 AND IDORGANIZACION = 39
 AND estado = 'A'
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Contingencia'    
 WHERE IdTransaccion = 2900
  AND IDORGANIZACION = 39

GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consulta/Envío Pedidos'    
 WHERE IdTransaccion = 2905
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consulta de Órdenes'    
 WHERE IdTransaccion = 2910
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Seguimiento Distribución de pedidos Cross-Docking'    
 WHERE IdTransaccion = 2915
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Notificaciones'    
 WHERE IdTransaccion = 2400
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Generar notificaciones'    
 WHERE IdTransaccion = 2410
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Aprobación'    
 WHERE IdTransaccion = 2415
 AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Agrupación'    
 WHERE IdTransaccion = 2420
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Acceso de Usuarios'    
 WHERE IdTransaccion = 2600
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Administradores'    
 WHERE IdTransaccion = 2610
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Gestión Usuarios'    
 WHERE IdTransaccion = 2615
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Asignación de Líneas de Negocio'    
 WHERE IdTransaccion = 2620
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Asignación de Líneas de Negocio Proveedor'    
 WHERE IdTransaccion = 3516
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Proveedores'    
 WHERE IdTransaccion = 2200
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consulta de Proveedores'    
 WHERE IdTransaccion = 2250
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Precalificación'    
 WHERE IdTransaccion = 2215
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Calificación - C'    
 WHERE IdTransaccion = 2220
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Calificación - SEG'    
 WHERE IdTransaccion = 2260
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Calificación - SEG I'    
 WHERE IdTransaccion = 2265
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Porcentaje de Tolerancia de Distribución'    
 WHERE IdTransaccion = 2255
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Artículos'    
 WHERE IdTransaccion = 2100
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consulta de Artículos'    
 WHERE IdTransaccion = 2150
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Artículos'    
 WHERE IdTransaccion = 2120
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Artículos - GC'    
 WHERE IdTransaccion = 2125
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitudes de Artículos'    
 WHERE IdTransaccion = 2130
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Logística'    
 WHERE IdTransaccion = 2300
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Envío de Actas'    
 WHERE IdTransaccion = 2365
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consulta de Actas'    
 WHERE IdTransaccion = 2375
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Actualización de Actas'    
 WHERE IdTransaccion = 2370
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Modificación de Choferes'    
 WHERE IdTransaccion = 2330
  AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Modificación de Vehículos'    
 WHERE IdTransaccion = 2335
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Cancelación Citas'    
 WHERE IdTransaccion = 2340
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Solicitud de Cita Rápida'    
 WHERE IdTransaccion = 2345
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Consolidación de OC'    
 WHERE IdTransaccion = 2350
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Reporte Tabular Cita'    
 WHERE IdTransaccion = 2360
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Reportes'    
 WHERE IdTransaccion = 3500
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Usuarios sin Ingreso por Primera Vez'    
 WHERE IdTransaccion = 3505
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Proveedores con OC sin Descargar'    
 WHERE IdTransaccion = 3510
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Proveedores que no han Actualizado sus Datos'    
 WHERE IdTransaccion = 3515
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Etiquetas'    
 WHERE IdTransaccion = 3517
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Asignación Proveedor/Etiqueta'    
 WHERE IdTransaccion = 3518
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Reporte Solicitud Etiqueta'    
 WHERE IdTransaccion = 3519
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Asignación de Requisición'    
 WHERE IdTransaccion = 4102
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Licitación'    
 WHERE IdTransaccion = 4200
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Licitación'    
 WHERE IdTransaccion = 4201
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Licitaciones Adjudicadas'    
 WHERE IdTransaccion = 4202
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Licitaciones por Adjudicar'    
 WHERE IdTransaccion = 4203
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Mantenimientos'    
 WHERE IdTransaccion = 2380
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Documentos adjuntos matriculación'    
 WHERE IdTransaccion = 2381
   AND IDORGANIZACION = 39
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET
      [Descripcion] = 'Catalogo'    
 WHERE IdTransaccion = 2410
   AND IDORGANIZACION = 39
GO


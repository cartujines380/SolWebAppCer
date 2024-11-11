using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace eComp.PDF
{
    /// <summary>
    /// Clase que sobrecarga los métodos de la clase para generar el PDF para redondear los bordes
    /// </summary>
    public class RoundRectangle : IPdfPCellEvent
    {
        public void CellLayout(
          PdfPCell cell, Rectangle rect, PdfContentByte[] canvas
        )
        {
            PdfContentByte cb = canvas[PdfPTable.LINECANVAS];
            cb.RoundRectangle(
              rect.Left,
              rect.Bottom,
              rect.Width,
              rect.Height,
              6 // change to adjust how "round" corner is displayed
            );
            //cb.SetLineWidth(1f);
            cb.SetCMYKColorStrokeF(0f, 0f, 0f, 1f);
            cb.Stroke();
        }
    }
}

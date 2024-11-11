using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace eComp.PDF
{
    /// <summary>
    /// Clase para la generación de los códigos de barra de 128bits en los PDF del RIDE
    /// </summary>
    public class BarcodeHelper
    {
        /// <summary>
        /// Obtiene el barcode128.
        /// </summary>
        /// <param name="pdfContentByte">El contenido del PDF en bytes.</param>
        /// <param name="code">El código a ser generado.</param>
        /// <param name="extended">Si está seteado <c>true</c> [extended].</param>
        /// <param name="codeType">Tipo del código.</param>
        /// <returns>Imagen del código de barras.</returns>
        public static Image GetBarcode128(PdfContentByte pdfContentByte, string code, bool extended, int codeType)
        {
            Barcode128 code128 = new Barcode128 { Code = code, Extended = extended, CodeType = codeType };
            return code128.CreateImageWithBarcode(pdfContentByte, null, null);
        }
    }
}

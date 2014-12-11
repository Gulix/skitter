using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Skitter.ViewModel.Fonctionnel
{
    static class ExportExcel
    {
        public static void CopierDonneesExcelVersPressePapier(string sAvecTab)
        {
            DataObject dataObject = new System.Windows.DataObject();

            dataObject.SetText(sAvecTab);
            // Convert the CSV text to a UTF-8 byte stream before adding it to the container object.
            string sAvecVirgule = sAvecTab.Replace('\t', ',');
            var bytes = System.Text.Encoding.UTF8.GetBytes(sAvecVirgule);
            var stream = new System.IO.MemoryStream(bytes);
            dataObject.SetData(System.Windows.DataFormats.CommaSeparatedValue, stream);

            // Copy the container object to the clipboard.
            System.Windows.Clipboard.SetDataObject(dataObject, true);
        }
    }
}

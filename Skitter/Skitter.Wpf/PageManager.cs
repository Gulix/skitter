using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.Wpf
{
    public static class PageManager
    {
        static List<IPage> _lsPages;

        public static void AjouterPage(IPage page)
        {
            if (_lsPages == null)
                _lsPages = new List<IPage>();
            _lsPages.Add(page);
        }

        public static void ReinitialiserToutesPages()
        {
            ReinitialiserToutesPages(null);
        }

        public static void ReinitialiserToutesPages(IPage exceptThis)
        {
            if (_lsPages == null)
                return;

            foreach (IPage page in _lsPages)
            {
                if (page != exceptThis)
                    page.ReinitialiserPage();
            }
        }
        
    }

    public interface IPage
    {
        void ReinitialiserPage();
    }
}

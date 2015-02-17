using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;

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

        public static void ReinitialiserToutesPages(bool bRafraichirMenus)
        {
            ReinitialiserToutesPages(null, bRafraichirMenus);
        }

        public static void ReinitialiserToutesPages(IPage exceptThis, bool bRafraichirMenus)
        {
            if (_lsPages == null)
                return;

            foreach (IPage page in _lsPages)
            {
                if (page != exceptThis)
                    page.ReinitialiserPage();
            }



            if (bRafraichirMenus)
            {
                ModernWindow wnd = Application.Current.MainWindow as ModernWindow;

                if (wnd != null)
                {
                    wnd.MenuLinkGroups = ListeMenus;
                }
            }
        }

        #region Gestion des menus
        static MenuManager _menu;
        public static LinkGroupCollection ListeMenus
        {
            get
            {
                if (_menu == null)
                    _menu = new MenuManager();
                return _menu.ListeMenus;
            }
        }
        #endregion

        
    }

    public interface IPage
    {
        void ReinitialiserPage();
    }
}

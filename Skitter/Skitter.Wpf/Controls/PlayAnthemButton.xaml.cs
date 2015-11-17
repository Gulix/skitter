using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.Wpf.Controls
{
    /// <summary>
    /// Button that plays the associated Anthem (MP3 File)
    /// </summary>
    public partial class PlayAnthemButton : Button
    {
        public PlayAnthemButton()
        {
            InitializeComponent();

            this.Click += PlayAnthemButton_Click;
        }

        void PlayAnthemButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Anthem))
            {
                Mp3Player.Play(Anthem);
            }
        }

        #region Anthem Property

        /// <summary>
        /// Anthem property
        /// </summary>
        public static readonly DependencyProperty AnthemProperty = 
            DependencyProperty.Register("Anthem", typeof(String), typeof(PlayAnthemButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets and Sets the Anthem to be played
        /// </summary>
        public string Anthem
        {
            get { return (string)GetValue(AnthemProperty); }
            set { SetValue(AnthemProperty, value); }
        }

        #endregion
    }
}

using Hearthstone_Deck_Tracker.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeckTrackerBroadcastMode
{
    /// <summary>
    /// Interação lógica para BroadcastOverlayView.xam
    /// </summary>
    public partial class BroadcastOverlayView : UserControl
    {
        public bool isLocal { get; }
        
        public BroadcastOverlayView(bool isLocal)
        {
            this.isLocal = isLocal;
            InitializeComponent();
            UpdatePosition();
          
        }

        public void UpdateText(string text)
        {

            this.CardRemainingCounter.Text = text;
            UpdatePosition();
        }

        public void SetFatigueOverlay()
        {
            var newImageUri = new Uri("pack://application:,,,/DeckTrackerBroadcastMode;component/Ressources/fatigue.png");
            this.OverlayImage.ImageSource = new BitmapImage(newImageUri);
            
        }

        public void SetCardCounterOverlay()
        {
            var newImageUri = new Uri("pack://application:,,,/DeckTrackerBroadcastMode;component/Ressources/cardsRemainingPlaceholder.png");
            this.OverlayImage.ImageSource = new BitmapImage(newImageUri);

        }

        private double ScreenRatio => (4.0 / 3.0) / (Core.OverlayCanvas.Width / Core.OverlayCanvas.Height);
        public void UpdatePosition()
        {

            if (!isLocal)
            {
                Canvas.SetRight(this, Hearthstone_Deck_Tracker.Helper.GetScaledXPos(3.2 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio));

                Canvas.SetTop(this, Core.OverlayCanvas.Height * 18 / 100);
            }
            else
            {
                Canvas.SetRight(this, Hearthstone_Deck_Tracker.Helper.GetScaledXPos(2.5 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio));

                Canvas.SetBottom(this, Core.OverlayCanvas.Height * 26 / 100);
            }
        }

        public void Show()
        {
            
            this.Visibility = Visibility.Visible;
            this.Opacity = 100;
        }

        public void Hide()
        {
            this.Opacity = 0;
            this.Visibility = Visibility.Hidden;
        }


    }
}

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace DeckTrackerBroadcastMode
{

    internal class BroadcastEventsHandler
    {
        private BroadcastOverlayView view;
        private bool isCountingCards = true;
        private bool isLocal;
        
        
        private Player player => isLocal ? CoreAPI.Game.Player : CoreAPI.Game.Opponent;
        private Entity hero => player.Board.FirstOrDefault(x => x.IsHero);

        public BroadcastEventsHandler(BroadcastOverlayView view, bool isLocal)
        
        {

            this.view = view;
            this.isLocal = isLocal;
            
        }

    
        internal void GameStart()
        {
            
            if (Core.Game.Spectator)
            {
                view.Show();
                view.UpdateText("30");
            }
            

        }
        internal void InMenu()
        {
          
            view.Hide();
        }

        internal void Update()
        {
            updateBroadcastOverlay();
        }

        private void updateBroadcastOverlay()
        {
            if (this.player == null || this.hero == null || !Core.Game.Spectator )
            {
                
                view.Hide();
                return;
            }

           
            if (this.player.DeckCount > 0)
            {
                if (!isCountingCards)
                {
                    view.SetCardCounterOverlay();
                }
                isCountingCards = true;
                int playerCardsRemaining = this.player.DeckCount;
                view.UpdateText(playerCardsRemaining.ToString());
            }
            else
            {
                if (isCountingCards)
                {
                    view.SetFatigueOverlay();
                }
                isCountingCards = false;
                

                int fatigueCounter = this.player.Fatigue + 1;
                view.UpdateText(fatigueCounter.ToString());

            }
 
            view.Show();
        }

    }
}

using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DeckTrackerBroadcastMode
{
    public class BroadcastModePlugin : IPlugin
    {
        public string Name => "Broadcast Mode";
        public string Description => "This plugin displays the number of cards in each player's deck";

        public MenuItem MenuItem => null;

        public string ButtonText => "Close";

        public string Author => "vRestrita";

        Version IPlugin.Version => new Version(1, 0, 2);

        private List<UIElement> _displayElements;
        private List<BroadcastEventsHandler> _items;
        private HashSet<UIElement> _hiddenElements;
        private bool _hidingCardMark = false;

        void IPlugin.OnButtonPress()
        {
            /*NOP*/
           
        }

        private void hideDeckTracker()
        {
            if (!Config.Instance.HideOpponentCardMarks)
            {
                _hidingCardMark = true;
                Config.Instance.HideOpponentCardMarks = true;
                Config.Instance.HideOpponentCardAge = true;
                Config.Save();
                
                
            }

            foreach (UIElement element in Core.OverlayCanvas.Children)
            {
                if (element.Opacity > 0)
                {
                    element.Opacity = 0;
                    _hiddenElements.Add(element);
                }
            }

        }

        

        void IPlugin.OnLoad()
        {
            
            _displayElements = new List<UIElement>();
            _items = new List<BroadcastEventsHandler>();
            _hiddenElements = new HashSet<UIElement>();
            GameEvents.OnGameStart.Add(OnGameStart);
            GameEvents.OnInMenu.Add(OnInMenuOrGameEnd);
            GameEvents.OnGameEnd.Add(OnInMenuOrGameEnd);
        }

        private void OnGameStart()
        {
            
            if (!Core.Game.Spectator) {
                OnInMenuOrGameEnd();
                return;
            }

            hideDeckTracker();
            BuildDeckCounter(true);
            BuildDeckCounter(false);

            foreach (BroadcastEventsHandler handler in _items)
            {
                handler.GameStart();
            }
            
        }

        private void OnInMenuOrGameEnd()
        {
            ShowDeckTracker();
            foreach (BroadcastEventsHandler handler in _items)
            {
                handler.InMenu();
            }
        }

        
        private void ShowDeckTracker()
        {
            if (_hidingCardMark)
            {
                Config.Instance.HideOpponentCardMarks = false;
                Config.Instance.HideOpponentCardAge = false;
                Config.Save();
                _hidingCardMark = false;
            }

            foreach (UIElement element in _hiddenElements)
            {
                element.Opacity = 100;
            }
            _hiddenElements.Clear();
        }

        private void BuildDeckCounter(bool isPlayer)
        {

            BroadcastOverlayView view = new BroadcastOverlayView(isPlayer);

            _displayElements.Add(view);
            BroadcastEventsHandler handler = new BroadcastEventsHandler(view, isPlayer);
            _items.Add(handler);

            Core.OverlayCanvas.Children.Add(view);
            
        }

        

        void IPlugin.OnUnload()
        {
            ShowDeckTracker();
            foreach (UIElement element in _displayElements)
            {
                Core.OverlayCanvas.Children.Remove(element);
    
            }
            
            _displayElements.Clear();
            _items.Clear();
        }

     

        void IPlugin.OnUpdate()
        {
            foreach (BroadcastEventsHandler item in _items)
            {
                item.Update();
            }
        }


    }
}

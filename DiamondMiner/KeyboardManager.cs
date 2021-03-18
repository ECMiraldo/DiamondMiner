using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MonoGameKeyModule
{
    /*
     * MonoGame - Default:
     *      Keyboard.IsDown(tecla) -> bool
     *      Keyboard.IsUp(tecla) -> bool
     *  
     *  
     * IPCA.KeyboardManager:
     *      Keyboard.IsKeyDown(tecla) -> bool
     *      Keyboard.IsKeyUp(tecla) -> bool
     *      keyboard.GoingDown(tecla) -> bool
     *      Keyboard.GoingUp(tecla) -> bool
     */

    public enum KeysState
    {
        Up, Down, GoingUp, GoingDown
    }
    public class KeyboardManager : GameComponent
    {
        public static KeyboardManager _instance;
        private Dictionary<Keys, KeysState> _KeyboardState;
        private Dictionary<Keys, Dictionary<KeysState, List<Action>>> actions;

        /*  Actions:
         *  Keys.A => {
         *              KeyState.GoingUp => [action1, action2]
         *              KeyState.Down => [action1, action2]
         *            }
         *            
         *  Keys.B => {
         *              KeyState.GoingUp => [action1, action2]
         *              KeyState.Down => [action1, action2]
         *            }
         */
        public KeyboardManager(Game game) : base(game)
        {
            if (_instance != null) throw new Exception("KeyboardManager constructor called twice");
            _instance = this; //a unica instancia é guardada
            _KeyboardState = new Dictionary<Keys, KeysState>();
            actions = new Dictionary<Keys, Dictionary<KeysState, List<Action>>>();
            game.Components.Add(this); //auto instalavel
           
        }

        public static bool IsKeyDown(Keys k) => _instance._KeyboardState.ContainsKey(k) && _instance._KeyboardState[k] == KeysState.Down;
        public static bool IsKeyUp(Keys k) => _instance._KeyboardState.ContainsKey(k) && _instance._KeyboardState[k] == KeysState.Up;
        public static bool IsGoingDown(Keys k) => _instance._KeyboardState.ContainsKey(k) && _instance._KeyboardState[k] == KeysState.GoingDown;
        public static bool IsGoingUp(Keys k) => _instance._KeyboardState.ContainsKey(k) && _instance._KeyboardState[k] == KeysState.GoingUp;

        public override void Update(GameTime gametime)
        {
            KeyboardState state = Keyboard.GetState(); //Updates de state of the keyboard
            List<Keys> pressedKeys = state.GetPressedKeys().ToList(); //Gets any and all presed keys and adds to list 

            foreach(Keys key in pressedKeys) //then, foreach key in said list
            {
                //If we didnt know anything about this key, then probably it wasnt pressed before
                if (!_KeyboardState.ContainsKey(key)) _KeyboardState[key] = KeysState.Up;

                //what was the previous state, and decide what is our next state
                switch (_KeyboardState[key])
                {
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _KeyboardState[key] = KeysState.Down; //If key is down or if key is going down, then KeyState = down
                        break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _KeyboardState[key] = KeysState.GoingDown; //If If key is up or if key is going up, then KeyState = going down
                        break;
                }
            }

            //Now we process released keys
            //
            foreach(Keys key in _KeyboardState.Keys.Except(pressedKeys).ToArray())
            {
                switch (_KeyboardState[key])
                {
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _KeyboardState[key] = KeysState.GoingUp;
                        break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _KeyboardState[key] = KeysState.Up;
                        break;

                }
            }


            //Invocar as funcoes registadas
            foreach (Keys key in actions.Keys)
            {
                if (_KeyboardState.ContainsKey(key))
                {
                    KeysState Kstate = _KeyboardState[key];
                    if (actions[key].ContainsKey(Kstate))
                    {
                        foreach (Action action in actions[key][Kstate])
                        {
                            action();
                        }
                    }

                }
            }


        }
        //Recebe:
        //  Tecla a analisar
        // O estado a considerar para essa tecla
        // Funcao a ser executada quando essa tecla atinge o estado
        //
        // Action é uma referencia a uma funcao void f(void)
        public static void Register(Keys key, KeysState ks, Action action)
        {
            //Do we have the key already in this dictionary?
            if (!_instance.actions.ContainsKey(key)) _instance.actions[key] = new Dictionary<KeysState, List<Action>>();
            //For this key, do we have that state created?
            if (!_instance.actions[key].ContainsKey(ks)) _instance.actions[key][ks] = new List<Action>();
            //Add the code to the key/keystate pair
            _instance.actions[key][ks].Add(action);

            //Add the key to the keyboard state dictionary
            _instance._KeyboardState[key] = KeysState.Up;
            
        }

    


    }
}

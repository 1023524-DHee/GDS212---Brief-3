using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public static class PlayerSettings
    {
        private static bool _isInitialised;
        
        public static bool continuousMovementEnabled;
        public static bool teleportMovementEnabled;
        public static bool continuousTurnEnabled;
        public static bool snapTurnEnabled;

        public static void InitializeValues(bool contMove, bool teleMove, bool contTurn, bool snapTurn)
        {
            if (_isInitialised) return;

            _isInitialised = true;
            continuousMovementEnabled = contMove;
            teleportMovementEnabled = teleMove;
            continuousTurnEnabled = contTurn;
            snapTurnEnabled = snapTurn;
        }
    }
}

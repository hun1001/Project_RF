using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_RF_Server.Session
{
    public class PlayerSession : Session
    {
        private float _hp;
        private TankTransformInformation _tankTransformInformation;

        public PlayerSession(float hp, TankTransformInformation tankTransformInformation)
        {
            _hp = hp;
            _tankTransformInformation = tankTransformInformation;
        }
    }
}

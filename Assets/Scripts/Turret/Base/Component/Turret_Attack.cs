using Sound;
using UnityEngine;
using Util;

namespace Turret
{
    public class Turret_Attack : Base.CustomComponent<Turret>
    {
        private Transform _firePoint = null;

        //private AttackCancel _attackCancel = null;

        private float _shellSpeed = 1f;

        private float _range = 10f;

        private float _fireRate = 1f;

        private float _nextFire = 0f;

        private bool _isReload = false;

        protected void Awake()
        {

            _firePoint = Instance.FirePoint;

            _shellSpeed = Instance.TurretSO.shellSpeed;

            _range = Instance.TurretSO.attackRange;

            _fireRate = Instance.TurretSO.reloadSpeed;
        }

        protected virtual void Update()
        {
            if (_nextFire > 0)
            {
                _nextFire -= Time.deltaTime;
            }
            if (_isReload == true && _nextFire < Instance._reloadSound.length - 0.5f)
            {
                _isReload = false;
                SoundManager.Instance.PlaySound(Instance._reloadSound, SoundType.SFX, 0.5f);
            }
        }

        public virtual void Fire()
        {
            // if (_attackCancel.IsCancelAttack == true)
            // {
            //     _attackCancel.CancelAttackReset();
            //     return;
            // }

            if (_nextFire > 0)
            {
                return;
            }

            _nextFire = _fireRate;
            _isReload = true;

            SoundManager.Instance.PlaySound(Instance._fireSound, SoundType.SFX, 0.7f);
            var shell = PoolManager.Instance.Get("Shell", _firePoint.position, _firePoint.rotation);
            shell.SendMessage("SetSpeed", _shellSpeed);
            shell.SendMessage("SetRange", _range);
            Invoke("ShellDropSoundPlay", 0.6f);
        }

        private void ShellDropSoundPlay()
        {
            SoundManager.Instance.PlaySound(Instance._shellDropSound, SoundType.SFX, 0.5f);
        }

        public float Range => _range;
        public float NextFire => _nextFire;
        public float FireRate => _fireRate;
    }
}

using System.Threading;
using TrueGearSDK;

namespace MyTrueGear
{
    public class TrueGearMod
    {
        private static TrueGearPlayer _player = null;

        private static ManualResetEvent grabStringMRE = new ManualResetEvent(false);
        private static ManualResetEvent leftHandLoadingPowerGloveMRE = new ManualResetEvent(false);
        private static ManualResetEvent rightHandLoadingPowerGloveMRE = new ManualResetEvent(false);
        private static ManualResetEvent leftHandFlamethrowerMRE = new ManualResetEvent(false);
        private static ManualResetEvent rightHandFlamethrowerMRE = new ManualResetEvent(false);

        public void GrabString()
        {
            while(true)
            {
                grabStringMRE.WaitOne();
                _player.SendPlay("BowStringPull");
                Thread.Sleep(150);
            }            
        }
        public void LeftHandLoadingPowerGlove()
        {
            while (true)
            {
                leftHandLoadingPowerGloveMRE.WaitOne();
                _player.SendPlay("LeftHandLoadingPowerGlove");
                Thread.Sleep(150);
            }
        }
        public void RightHandLoadingPowerGlove()
        {
            while (true)
            {
                rightHandLoadingPowerGloveMRE.WaitOne();
                _player.SendPlay("RightHandLoadingPowerGlove");
                Thread.Sleep(150);
            }
        }
        public void LeftHandFlamethrower()
        {
            while (true)
            {
                leftHandFlamethrowerMRE.WaitOne();
                _player.SendPlay("LeftHandFlamethrower");
                Thread.Sleep(190);
            }
        }
        public void RightHandFlamethrower()
        {
            while (true)
            {
                rightHandFlamethrowerMRE.WaitOne();
                _player.SendPlay("RightHandFlamethrower");
                Thread.Sleep(190);
            }
        }

        public TrueGearMod() 
        {
            _player = new TrueGearPlayer("2401230","Clone Drone in the Hyperdome");
            _player.Start();
            new Thread(new ThreadStart(this.GrabString)).Start();
            new Thread(new ThreadStart(this.LeftHandLoadingPowerGlove)).Start();
            new Thread(new ThreadStart(this.RightHandLoadingPowerGlove)).Start();
            new Thread(new ThreadStart(this.LeftHandFlamethrower)).Start();
            new Thread(new ThreadStart(this.RightHandFlamethrower)).Start();
        }    

        public void Play(string Event)
        { 
            _player.SendPlay(Event);
        }

        public void StartGrabString()
        {
            grabStringMRE.Set();
        }

        public void StopGrabString()
        {
            grabStringMRE.Reset();
        }

        public void StartLeftHandLoadingPowerGlove()
        {
            leftHandLoadingPowerGloveMRE.Set();
        }

        public void StopLeftHandLoadingPowerGlove()
        {
            leftHandLoadingPowerGloveMRE.Reset();
        }

        public void StartRightHandLoadingPowerGlove()
        {
            rightHandLoadingPowerGloveMRE.Set();
        }

        public void StopRightHandLoadingPowerGlove()
        {
            rightHandLoadingPowerGloveMRE.Reset();
        }

        public void StartLeftHandFlamethrower()
        {
            leftHandFlamethrowerMRE.Set();
        }

        public void StopLeftHandFlamethrower()
        {
            leftHandFlamethrowerMRE.Reset();
        }

        public void StartRightHandFlamethrower()
        {
            rightHandFlamethrowerMRE.Set();
        }

        public void StopRightHandFlamethrower()
        {
            rightHandFlamethrowerMRE.Reset();
        }

    }
}

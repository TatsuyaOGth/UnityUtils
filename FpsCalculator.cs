using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Ogsn.Utils
{
    public class FpsCalculator
    {
        public double Interval { get; set; }

        public float Fps { get; protected set; } = 0;

        int _frameCount = 0;
        DateTime _prevTime;

        public FpsCalculator()
        {
            Interval = 0.5f;
            _prevTime = DateTime.Now;
        }

        public FpsCalculator(float interval)
        {
            Interval = interval;
            _prevTime = DateTime.Now;
        }

        public void AppendFrame()
        {
            ++_frameCount;
        }

        public void Update()
        {
            var elasTime = (DateTime.Now - _prevTime).TotalSeconds;
            if (elasTime >= Interval)
            {
                double fps = _frameCount / elasTime;
                _frameCount = 0;
                _prevTime = DateTime.Now;
                Fps = (float)fps;
            }
        }

        public float GetFps(bool dontUpdate = false)
        {
            if (!dontUpdate)
            {
                AppendFrame();
                Update();
            }
            return Fps;
        }
    }
}

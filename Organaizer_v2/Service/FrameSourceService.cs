using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organaizer_v2.Service
{
    public class FrameSourceService
    {
        public event Action<Page> FrameChangeEvent;
        public void ChangeFrame(Page frame) => FrameChangeEvent?.Invoke(frame);
    }
}

using Cc.Anba.PhantomOs.VirtualMachine;

namespace Cc.Anba.PhantomOs.Apps.VmUi.ViewModels
{
    public class MainViewModel
    {
        private PvmRoot _pvmRoot;

        public MainViewModel()
        {
            _pvmRoot= new PvmRoot();
        }
    }
}


namespace @internal
{
    public class bootstrap
    {
        public void sleep(@int msec) { }

        public @string[] getKernelEnvironment() { return new @string[2]; }

        public void consolePrint(@string s) { }

        public @class loadClass(@string name) { return new @class(); }

        public binary createBinary(@int sizeBytes) { return new binary(); }

        public void setOsInterface(phantom.osimpl oi) { }

        public void startThread(@object o) { }

        public void setScreenBackground(bitmap bg) { }

        public void setClassLoader(@object cl) { }
    }
}

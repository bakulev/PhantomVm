
using @internal;

namespace ru.dz.phantom.system
{
    public class boot
    {
        public @internal.io.tty console;
        public ru.dz.phantom.system.regression_tests reg_test;
        public @internal.@int debug;
        public @internal.bootstrap boot_object;
        //public loader_class;
        //public loader;
        //public reg_test_class;
        //public shell_name;
        //public run;
        //public windows;
        //public shell_class;

        public void sleep()
        {
            // int const "1000" : int const
            boot_object.sleep(new @int(1000)); // Method no. 21

        }

        // Method no. 16
        public void print(@string input)
        {
            boot_object.consolePrint(input); // Method no. 16
        }

        // Method no. 8
        public @internal.@class load_class(@string name)
        {
            return boot_object.loadClass(name); // Method no. 8
        }

        public void startup(@internal.bootstrap _boot_object)
        {
            try
            {
                do_startup(_boot_object);
            }
            catch (System.Exception e)
            {
                print(new @string("Exception: '"));
                print(new @string(e.ToString()));
                print(new @string("'"));
            }
        }

        public void do_startup(@internal.bootstrap _boot_object)
        {

        }
    }
}

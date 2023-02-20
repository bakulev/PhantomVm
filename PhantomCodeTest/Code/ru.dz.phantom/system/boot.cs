
using @internal;
using static System.Net.Mime.MediaTypeNames;

namespace ru.dz.phantom.system
{
    public class boot
    {
        // Ord no. 6
        public @internal.io.tty console;
        // Ord no. 5
        public ru.dz.phantom.system.regression_tests reg_test;
        // Ord no. 0
        public @internal.@int debug;
        // Ord no. 1
        public @internal.bootstrap boot_object;
        // Ord no. 2
        //public loader_class;
        // Ord no. 3
        //public loader;
        // Ord no. 4
        //public reg_test_class;
        // Ord no. 9
        //public shell_name;
        // Ord no. 8
        //public run;
        // Ord no. 7
        //public windows;
        // Ord no. 10
        //public shell_class;

        // Method no. 16
        public void sleep()
        {
            // int const "1000" : int const
            boot_object.sleep(new @int(1000)); // Method no. 21

        }

        // Method no. 17
        public void print(@string input)
        {
            boot_object.consolePrint(input); // Method no. 16
        }

        // Method no. 18
        public void setScreenBackgroud()
        {
        }

        // Method no. 19
        public @internal.@class load_class(@string name)
        {
            return boot_object.loadClass(name); // Method no. 8
        }

        // Method no. 8
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

        // Method no. 20
        //public void get_os_interface_object()

        // Method no. 21
        public void do_startup(@internal.bootstrap _boot_object)
        {
            @int debug = new @int(1);
            @internal.bootstrap boot_object = _boot_object;
            print(new @string("Phantom System Envirinment Setup is running\n"));
            if (debug == new @int(1))
                print(new @string("loading loader class\n"));
            var loader_class = load_class(new @string(".ru.dz.phantom.system.class_loader"));
            if (debug == new @int(1))
                print(new @string("creating loader object"));
            var loader = new loader_class();
            if (debug == new @int(1))
                print(new @string("initializing loader\n"));
            loader.init(boot_object);
            if (debug == new @int(1))
                print(new @string("registering loader\n"));
            boot_object.mtd17(loader); // Method no. 17

            var reg_test = new ru.dz.phantom.system.regression_tests();
            print(new @string("Starting compiler regression tests\n"));
            reg_test.run(boot_object);
            print(new @string("Finished compiler regression tests\n"));

            var console = new @internal.io.tty();
            phantom.osimpl oi = get_os_interface_object();
            boot_object.mtd22(oi); // Method no. 22

            var shell_name = oi.getKernelEnvironmentValue(new @string("root.shell"));
            print(new @string("Env root.shell="));
            print(new @string(shell_name));
            print(new @string("\n"));
            print(new @string("Env root.init="));
            print(new @string(oi.getKernelEnvironmentValue(new @string("root.init"))));
            print(new @string("\n"));

            console.putws(new @string("\n\n\n\n\n"));
            console.putws(new @string("Env root.shell="));
            console.putws(new @string(shell_name));
            console.putws(new @string("\n"));
            console.putws(new @string("Env root.init="));
            console.putws(new @string(oi.getKernelEnvironmentValue(new @string("root.init")))));
            console.putws(new @string("\n"));

            console.putws(new @string("Creating root window...\n"));
            var windows = new ru.dz.windows.root();
            console.putws(new @string(" initing root window...\n"));
            windows.init(console, boot_object);
            console.putws(new @string(" starting windows test...\n"));
            windows.test();
            print(new @string("Finished windows tests\n"));

            print(new @string("Run Java tests\n"));
            var pp = new test.toPhantom.PhantomPrinter();
            pp.init(console);
            var ar 
        }

        // Method no. 22
        //public void newBinary(@int nbytes)

        public void load_class(@internal.@object _boot_object)
        {
        }
    }
}

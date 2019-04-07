This is a simple app that output the dependencies of a given type. This can be useful if you need to figure out what you need to register with an IoC container.

If a constructor takes an interface, the app will look for all implementations of that interface within the assembly that the interface is located in.
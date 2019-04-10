# Purpose

This is a simple app that outputs the types required to instantiate a given type. This can be useful if you need to figure out what you need to register with an IoC container at startup.

# Details

- If a constructor takes an interface, the app will look for all implementations of that interface within the assembly that the interface is located in, and display all found implementations in the output.

# Future Improvements

- Cache already traversed types
- Allow user to specify other assemblies that should be scanned to find interface implementations
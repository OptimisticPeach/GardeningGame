# Gardening Game 3D

Gardening Game 3D is a low poly gardening simulator. It will be developed solely for practice with MonoGame (The framework it is created with) and SharpDX.

There are a few "noteworthy" (not really) features:
  
   * Water is done all in the shader with the help of a geometry shader that is injected into the context before the draw call is run.    
   * There are changes to the MonoGame library to expose the right Constant Buffers to inject the right buffer into the context. This will allow for use to use the MonoGame effect parameter loading and not have to do it ourselves.  
   * A cylindrical camera is used for the movement in the level selection scene.

# License
This code is distributed under the terms of a dual MIT and Apache 2.0 license, and the user is free to choose either.

See the files named LICENSE-MIT and LICENSE-APACHE2 relative to the root directory of this 
project for more details.

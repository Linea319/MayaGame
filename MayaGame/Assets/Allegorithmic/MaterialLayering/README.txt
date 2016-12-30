Substance Painter Material layering Unity import scripts

Requirements:
 - Substance Painter 2.2
 - Unity 5.3.5f1 or later
 - DX11+ or OpenGL Core 4.1+

Quick howto:
 - Import 3D model in Unity and drag it in the scene
 - Select the material you wish to configure
 - Select "Allegorithmic/Material layering" shader
 - Click "Load json" choose the json file exported by Substance Painter
 - You can then assign the mesh normal map and adjust shader parameters

Common pitfalls:
 - In your Substance Painter project, use the provided pbr-material-layering shader
 - When exporting from Substance Painter, be sure to use the "Document channels + Normal + AO" configuration and check "Export shaders parameters"
 - Make sure .sbsar(s) used by your Substance Painter project are also imported in Unity
 - Make sure your Unity materials are named after the textureset names in your Substance Painter project
 - Depending on your hardware, the shader might hit the image unit count limit (especially on Mac OS). If the shader does not compile, try using the Deferred rendering path as a workaround

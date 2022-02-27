# Triangle.NET

Triangle.NET generates 2D (constrained) Delaunay triangulations and high-quality meshes of point sets or planar straight line graphs. It is a C# port of Jonathan Shewchuk's [Triangle](https://www.cs.cmu.edu/~quake/triangle.html) software.

## Features

* Constrained Delaunay triangulation of planar straight line graphs
* Incremental, sweepline and divide & conquer Delaunay triangulation algorithms
* High-quality triangular meshes with minimum/maximum angle constraints
* Mesh refinement
* Mesh smoothing using centroidal Voronoi tessellation (CVT)
* Node renumbering (Cuthill-McKee)
* Read and write Triangle format files (.node, .poly, .ele)

## Usage

Please refer to the [Examples](https://github.com/wo80/Triangle.NET/tree/master/src/Triangle.Examples) project. [Geri Borbás](https://github.com/Geri-Borbas) has prepared a "Release" branch containing the main project source only for submodule usage.

## License

The original C code published by Jonathan Shewchuk comes with a proprietary license (see [Triangle README](https://github.com/wo80/Triangle/blob/master/src/Triangle/README)) which, unfortunately, isn't very clear about how a derived work like Triangle.NET should be handled. Though Triangle.NET was published on Codeplex (https://triangle.codeplex.com, no longer available) under the MIT license in 2012, I recommend not using this code in a commercial context. Due to the unclear licensing situation, there will also be no Nuget package release.

For further discussion, please refer to the open issue [License Confusion](https://github.com/wo80/Triangle.NET/issues/6).

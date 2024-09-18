# Triangle.NET

Triangle.NET generates 2D (constrained) Delaunay triangulations and high-quality meshes of point sets or planar straight line graphs. The code is based on Jonathan Richard Shewchuk's [Triangle](https://www.cs.cmu.edu/~quake/triangle.html) project, see references below.

## Features

* Constrained Delaunay triangulation of planar straight line graphs
* Incremental, sweepline and divide & conquer Delaunay triangulation algorithms
* High-quality triangular meshes with minimum/maximum angle constraints
* Mesh refinement
* Mesh smoothing using centroidal Voronoi tessellation (CVT)
* Node renumbering (Cuthill-McKee)
* Read and write Triangle format files (.node, .poly, .ele)

To get started, take a look at the [wiki](https://github.com/wo80/Triangle.NET/wiki). There's also an extensive list of examples in the [Triangle.Examples](https://github.com/wo80/Triangle.NET/tree/master/src/Triangle.Examples) project (see [overview](https://github.com/wo80/Triangle.NET/wiki/Examples) in the wiki).

## License

The original C code published by Jonathan Shewchuk comes with a proprietary license (see [Triangle README](https://github.com/wo80/Triangle/blob/master/src/Triangle/README)) which, unfortunately, isn't very clear about how a derived work like Triangle.NET should be handled. Though Triangle.NET was published on Codeplex under the MIT license in 2012 ([triangle.codeplex.com](https://triangle.codeplex.com), no longer available), I recommend not using this code in a commercial context. This restriction only applies to the *Triangle* project and specifically those files containing a copyright header pointing to Jonathan Richard Shewchuk. The code contained in the other projects (like *Triangle.Rendering* or *Triangle.Viewer*) is released under MIT license.

Due to the unclear licensing situation, there will also be no Nuget package release. For further discussion, please refer to the [License Confusion](https://github.com/wo80/Triangle.NET/discussions/50) topic in the discussions section.

## References

If you want to learn about the algorithms used in Triangle.NET, I recommend taking a look at the following papers:
* Jonathan Richard Shewchuk, *Triangle: Engineering a 2D Quality Mesh Generator and Delaunay Triangulator* ([free PDF](https://duckduckgo.com/?q=Triangle+Engineering+a+2D+Quality+Mesh+Generator+and+Delaunay+Triangulator+filetype%3Apdf&ia=web))
* Jonathan Richard Shewchuk, *Delaunay Refinement Algorithms for Triangular Mesh Generation* ([free PDF](https://duckduckgo.com/?q=Delaunay+Refinement+Algorithms+for+Triangular+Mesh+Generation+filetype%3Apdf&ia=web))
* Hale Erten & Alper Üngör, *Triangulations with Locally Optimal Steiner Points* ([free PDF](https://duckduckgo.com/?q=Triangulations+with+Locally+Optimal+Steiner+Points+filetype%3Apdf&ia=web))

# sandbox
Various little samples as I continue exploring various paradigms, patterns and practices.

# Mediator Sample
A small solution to demonstarte how to setup reusable Pipeline Behaviors utilizing [MediatR v6.0.0](https://github.com/jbogard/MediatR)

# Snippets
Usually incomplete projects and/or a place to put files I use over in various projects and have been too lazy to put into a Nuget for my own use.

**NOTE**:  The folders within the root space are each a top level solution or snippet folder.  The following notes apply within each of these folders (to a point).

# Notes and project layout 
(stolen from [David Fowler](https://gist.github.com/davidfowl/ed7564297c61fe9ab814))
```
$/
  artifacts/
  build/
  docs/
  lib/
  packages/
  samples/
  src/
  tests/
  .editorconfig
  .gitignore
  .gitattributes
  build.cmd
  build.sh
  LICENSE
  NuGet.Config
  README.md
  {solution}.sln
```


- `src` - Main projects (the product code)
- `tests` - Test projects
- `docs` - Documentation stuff, markdown files, help files etc.
- `samples` (optional) - Sample projects
- `lib` - Things that can **NEVER** exist in a nuget package
- `artifacts` - Build outputs go here. Doing a build.cmd/build.sh generates artifacts here (nupkgs, dlls, pdbs, etc.)
- `packages` - NuGet packages
- `build` - Build customizations (custom msbuild files/psake/fake/albacore/etc) scripts
- `build.cmd` - Bootstrap the build for windows
- `build.sh` - Bootstrap the build for *nix
- `global.json` - ASP.NET vNext only

## .gitignore
```
[Oo]bj/
[Bb]in/
.nuget/
_ReSharper.*
packages/
artifacts/
*.user
*.suo
*.userprefs
*DS_Store
*.sln.ide
```

There's probably more things that go in the ignore file.

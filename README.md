# HACC

C# DotNet 6 HTML ANSI Console Canvas.

* Contains a virtual terminal character buffer with text and appearance kept separately.
* Contains a HTML component that renders the character buffer contents.
* Contains driver code to create a System.Console compatible ANSI Console on an HTML5 Canvas.
  - Primary driver [Gui.cs](https://github.com/migueldeicaza/gui.cs)
  - Secondary driver (in progress) for [Spectre.Console](https://github.com/spectreconsole/spectre.console)
* Was formerly using the [BlazorExtensions/Canvas](https://github.com/BlazorExtensions/Canvas) project, which has been somewhat stagnent.
  - We absorbed the library, also under MIT license, and brought it up to .net 6 and newer typescript, webpack and other npm libraries.
  - We're about to bring in some of the other impovements other forks had developed but not merged into the stagnant project.  

## Beta!
![Right Click works!](https://user-images.githubusercontent.com/3766240/169219917-215267bc-87c1-4323-a3ca-321c6875240a.png)

![ezgif com-gif-maker (1)](https://user-images.githubusercontent.com/3766240/169589558-01748bbf-154b-42f8-bbf6-2f306e28a889.gif)

### Features
- Builtin PNG frame-grabber
  - Can be triggered manually in console mode with window.canvasToPNG() which returns a data string with the frame at the time.
  - Can be integrated into an app with JSInterop calls such that the console application can have a "save to png" option, etc
  - ![image](https://user-images.githubusercontent.com/3766240/170335937-37b4b461-665c-497f-8538-1a4d8255289a.png)


## Tests


- https://github.com/Blazor-Console/HACC.Tests

## Development

* https://github.com/Blazor-Console/HACC/wiki/Developing

## Future

* https://github.com/JessicaMulein/PlayZMachine will be based on this
* A possible BBS with door games like ZMachine and a sea-faring game I'm writing in a private repo, (top secret!) which
  used to have the only test harness solution containing both the source and test repos.

# About
This project was built and is maintained by the [Digital Defiance](https://digitaldefiance.org) ([GitHub page](https://github.com/Digital-Defiance)) - a group of like-minded engineers working together to improve the world and have fun in the process.

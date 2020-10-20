# Xamarin

This has been my first encounter with Xamarin.

So let's start with the good parts. Coming from a mainly Typescript and PHP background,
it is easy to get into C#. But that's when the fun stops happening.

For all the hate npm gets. NuGet is singlehandely the worst package ecosystem i've encountered.

Let me refer you to this issue on github [#1886](https://github.com/xamarin/Xamarin.Forms/issues/1886)
`modernhttpclient-updated` Really? This just reminds me of the dark days of PHP with `real_escape_string`.
Packages should be deprecated in a much more visible manner.

.NET runs everywhere, right? Well.. It's complicated, as we have

+ .NET for Windows
+ .NET Core
+ .NET Standard (this one is crossplatform too?)

And that's just .NET - then we have Xamarin in itself, which is another runtime layer on top.

Xamarin.Android, Xamarin.iOS, Xamarin.TVOS and so on.

It just feels like the whole ecosystem is scattered, and everyone is fumbling around trying to get their existing code
not to break when xamarin updates.

I really want to like it. Because when it works, it's down right awesome.
But being so many layers away from the native api's of Android and iOS, makes it a pain to extend upon.

Sure it's doable. But you have to write HEAPS of boilerplate, to get it running.

## Bugs, bugs and XAML

I was lucky enough to stumble upon  

+ [XamarinCommunityToolkit/issues #399](https://github.com/xamarin/XamarinCommunityToolkit/issues/399)
+ [XamarinCommunityToolkit/issues #415](https://github.com/xamarin/XamarinCommunityToolkit/issues/415)
+ [Xamarin.Forms/issues #5172](https://github.com/xamarin/Xamarin.Forms/issues/5172)
+ [Xamarin.Forms/issues #11101](https://github.com/xamarin/Xamarin.Forms/issues/11101)

Besides those. F#§! XAML and their context bindings.

The lack of updated documentation made me almost ragequit, and just let it figure it out at runtime.

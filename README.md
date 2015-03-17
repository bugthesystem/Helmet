# helmet.net
Middlewares to help secure your apps

[![Build status](https://ci.appveyor.com/api/projects/status/032t00oscffq1jmd?svg=true)](https://ci.appveyor.com/project/ziyasal/helmet-net)

To install Helmet.Net,  run the following command in the NuGet [Package Manager Console](http://docs.nuget.org/consume/package-manager-console)

```sh
Install-Package Helmet.Net
```


##Middlewares

* [X-XSS-Protection middleware](#x-xss-protection-middleware)
* ["Don't infer the MIME type" middleware](#dont-infer-the-mime-type-middleware)
* [Middleware to turn off caching](#middleware-to-turn-off-caching)
* [IE, restrict untrusted HTML](#ie-restrict-untrusted-html)
* [Frameguard middleware](#frameguard-middleware)
* [Hide powered by](#hide-powered-by)


## X-XSS-Protection middleware

**Trying to prevent:** Cross-site scripting attacks (XSS), a subset of the above.

**How we mitigate this:** The ```X-XSS-Protection``` HTTP header is a basic protection against XSS. It was originally [by Microsoft](http://blogs.msdn.com/b/ieinternals/archive/2011/01/31/controlling-the-internet-explorer-xss-filter-with-the-x-xss-protection-http-header.aspx) but Chrome has since adopted it as well. To use it:

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
    appBuilder.Use<XssFilterMiddleware>();
    //...
  }
}
```

This sets the ```X-XSS-Protection``` header. On modern browsers, it will set the value to ```1; mode=block```. On old versions of Internet Explorer, this creates a vulnerability (see [here](http://hackademix.net/2009/11/21/ies-xss-filter-creates-xss-vulnerabilities/) and [here](http://technet.microsoft.com/en-us/security/bulletin/MS10-002)), and so the header is set to ```0``` to disable it. To force the header on all versions of IE, add the option:

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
    appBuilder.Use<XssFilterMiddleware>(new XssFilterOptions
    {
      SetOnOldIE = true
    });
    // This has some security problems for old IE!
    //...
  }
}

```

**Limitations:** This isn't anywhere near as thorough as ```Content Security Policy```. It's only properly supported on IE9+ and Chrome; no other major browsers support it at this time. Old versions of IE support it in a buggy way, which we disable by default.


## "Don't infer the MIME type" middleware
Some browsers will try to "sniff" mimetypes. For example, if my server serves file.txt with a text/plain content-type, some browsers can still run that file with ```<script src="file.txt"></script>```. Many browsers will allow file.js to be run even if the content-type isn't for JavaScript. There are [some other vulnerabilities](https://miki.it/blog/2014/7/8/abusing-jsonp-with-rosetta-flash/), too.

This middleware to keep Chrome, Opera, and IE from doing this sniffing ([and Firefox soon](https://bugzilla.mozilla.org/show_bug.cgi?id=471020)). The following example sets the ```X-Content-Type-Options``` header to its only option, ```nosniff```:

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
     appBuilder.Use<DontSniffMimetypeMiddleware>();
    //...
  }
}
```

[MSDN has a good description](https://msdn.microsoft.com/en-us/library/gg622941(v=vs.85).aspx) of how browsers behave when this header is sent.

This only prevents against a certain kind of attack.


## Middleware to turn off caching

It's possible that you've got bugs in an old HTML or JavaScript file, and with a cache, some users will be stuck with those old versions. This will (try to) abolish all client-side caching.
```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
     appBuilder.Use<NoCacheMiddleware>();
    //...
  }
}
```
This will set ```Cache-Control``` and ```Pragma``` headers to stop caching. It will also set an Expires header of 0, effectively saying "this has already expired."

If you want to crush the ```ETag``` header as well, you can:
```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
     appBuilder.Use<NoCacheMiddleware>(new NoCacheOptions 
     { 
       NoEtag = true 
     });
    //...
  }
}
```
Caching has some real benefits, and you lose them here. Browsers won't cache resources with this enabled, although _some_ performance is retained if you keep ETag support. It's also possible that you'll introduce new bugs and you'll wish people had old resources cached, but that's less likely.

## IE, restrict untrusted HTML
This middleware sets the ```X-Download-Options``` header to ```noopen``` to prevent IE users from executing downloads in your site's context.

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    //omitted for brevity
    appBuilder.Use<IeNoOpenMiddleware>();
    //...
  }
}
```

Some web applications will serve untrusted HTML for download. By default, some versions of IE will allow you to open those HTML files in the _context of your site_, which means that an untrusted HTML page could start doing bad things in the context of your pages. For more, see [this MSDN blog post](http://blogs.msdn.com/b/ie/archive/2008/07/02/ie8-security-part-v-comprehensive-protection.aspx).

This is pretty obscure, fixing a small bug on IE only. No real drawbacks other than performance/bandwidth of setting the headers, though.

## Frameguard middleware

**Trying to prevent:** Your page being put in a `<frame>` or `<iframe>` without your consent. This helps to prevent things like [clickjacking attacks](https://en.wikipedia.org/wiki/Clickjacking).

**How do we mitigate this:** The `X-Frame-Options` HTTP header restricts who can put your site in a frame. It has three modes: `DENY`, `SAMEORIGIN`, and `ALLOW-FROM`. If your app does not need to be framed (and most don't) you can use the default `DENY`. If your site can be in frames from the same origin, you can set it to `SAMEORIGIN`. If you want to allow it from a specific URL, you can allow that with `ALLOW-FROM` and a URL.

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    // Don't allow me to be in ANY frames:
     appBuilder.Use<FrameGuardMiddleware>("deny");
    //...
  }
}
```

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
     // Only let me be framed by people of the same origin:
     appBuilder.Use<FrameGuardMiddleware>("SAMEORIGIN");
    //...
  }
}
```

```csharp
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    // Allow from a specific host:
     appBuilder.Use<FrameGuardMiddleware>("allow-from","http://example.com");
    //...
  }
}
```

**Limitations:** This has pretty good (but not 100%) browser support: IE8+, Opera 10.50+, Safari 4+, Chrome 4.1+, and Firefox 3.6.9+. It only prevents against a certain class of attack, but does so pretty well. It also prevents your site from being framed, which you might want for legitimate reasons.


## Hide powered by

Simple middleware to remove the `X-Powered-By` HTTP header if it's set.

Hackers can exploit known vulnerabilities in .net web apps if they see that your site is powered by .net web apps (or whichever framework you use). For example, `X-Powered-By: aspnet/mvc` is sent in every HTTP request coming from .net, by default.

The `hidePoweredBy` middleware will remove the `X-Powered-By` header if it is set.

```c#
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
     
     appBuilder.Use<HidePoweredByHeaderMiddleware>();
    //...
  }
}
```
You can also explicitly set the header to something else, if you want. This could throw people off:

```c#
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
     
     appBuilder.Use<HidePoweredByHeaderMiddleware>(new HidePoweredOptions { SetTo = "steampowered" });
    //...
  }
}

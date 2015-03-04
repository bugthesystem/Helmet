# helmet.net
Middlewares to help secure your apps

[![Build status](https://ci.appveyor.com/api/projects/status/032t00oscffq1jmd?svg=true)](https://ci.appveyor.com/project/ziyasal/helmet-net)

##Middlewares

* [X-XSS-Protection middleware](#x-xss-protection-middleware)
* ["Don't infer the MIME type" middleware](#dont-infer-the-mime-type-middleware)


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
